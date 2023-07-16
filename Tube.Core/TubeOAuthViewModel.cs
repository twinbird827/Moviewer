using Codeplex.Data;
using Moviewer.Core;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TBird.Core;
using TBird.Web;
using TBird.Wpf;
using TBird.Wpf.Controls;

namespace Moviewer.Tube.Core
{
    public class TubeOAuthViewModel : DialogViewModel
    {
        public TubeOAuthViewModel()
        {
            ClientId = TubeSetting.Instance.ClientId;
            ClientSecret = TubeSetting.Instance.ClientSecret;

            AddDisposed((sender, e) =>
            {
                if (DialogResult.Value)
                {
                    TubeSetting.Instance.ClientId = ClientId;
                    TubeSetting.Instance.ClientSecret = ClientSecret;
                    TubeSetting.Instance.Save();
                }
            });
        }

        public string ClientId
        {
            get => _ClientId;
            set => SetProperty(ref _ClientId, value);
        }
        private string _ClientId = null;

        public string ClientSecret
        {
            get => _ClientSecret;
            set => SetProperty(ref _ClientSecret, value);
        }
        private string _ClientSecret = null;

        public ICommand OnDrop => _OnDrop = _OnDrop ?? RelayCommand.Create<DragEventArgs>(e =>
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] filepaths && filepaths.Length == 1)
            {
                var filepath = filepaths[0];
                dynamic json = DynamicJson.Parse(File.ReadAllText(filepath));
                ClientId = DynamicUtil.S(json, "installed.client_id");
                ClientSecret = DynamicUtil.S(json, "installed.client_secret");
            }
        });
        private ICommand _OnDrop;

        protected override ICommand GetOKCommand()
        {
            return RelayCommand.Create(async _ =>
            {
                if (string.IsNullOrEmpty(CoreUtil.Nvl(ClientId, ClientSecret)))
                {
                    MessageService.Error("client id or client secret is empty.");
                    return;
                }

                using (var listener = new WebListener())
                {
                    var oauthurl = "https://accounts.google.com/o/oauth2/v2/auth";
                    var dic = new Dictionary<string, string>()
                {
                    { "client_id", ClientId },
                    { "redirect_uri", $@"http://localhost:{listener.Port}" },
                    { "response_type", "code" },
                    { "scope", scopes.GetString(" ")},
                };
                    WebUtil.Browse(WebUtil.GetUrl(oauthurl, dic));

                    var context = await listener.GetContextAsync();
                    var request = context.Request;

                }
                //MainViewModel.Instance.Current = new NicoSearchViewModel(Tag, NicoSearchType.Tag);
            });
        }

        private static readonly string[] scopes = new string[]
        {
            "https://www.googleapis.com/auth/youtube",
            "https://www.googleapis.com/auth/youtube.channel-memberships.creator",
            "https://www.googleapis.com/auth/youtube.force-ssl",
            "https://www.googleapis.com/auth/youtube.readonly",
            "https://www.googleapis.com/auth/youtube.upload",
            "https://www.googleapis.com/auth/youtubepartner",
            "https://www.googleapis.com/auth/youtubepartner-channel-audit"
        };
    }
}