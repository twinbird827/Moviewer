using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;
using TBird.Core;

namespace Moviewer.Nico.Core
{
    public class NicoUserModel : BindableBase
    {
        public NicoUserModel(string userid, string username, string url = null) 
        {
            Userid = userid;
            Username = username;
            ThumbnailUrl = CoreUtil.Nvl(url, GetThumbnailUrl(Userid));
            RefreshUsername();
        }

        public string Userid
        {
            get => _Userid;
            set => SetProperty(ref _Userid, value);
        }
        private string _Userid = null;

        public string Username
        {
            get => _Username;
            set => SetProperty(ref _Username, value);
        }
        private string _Username = null;

        public string ThumbnailUrl
        {
            get => _ThumbnailUrl;
            set => SetProperty(ref _ThumbnailUrl, value);
        }
        private string _ThumbnailUrl;

        private string GetThumbnailUrl(string value)
        {
            if (value == null) return null;
            var url1 = "https://secure-dcdn.cdn.nimg.jp/nicoaccount/usericon";
            var url2 = 4 < value.Length ? value.Left(value.Length - 4) : "0";
            var url3 = value;
            return $"{url1}/{url2}/{url3}.jpg";
        }

        private async void RefreshUsername()
        {
            if (string.IsNullOrEmpty(Userid) || !string.IsNullOrEmpty(Username)) return;

            Username = await GetNickname(Userid);
        }

        private async Task<string> GetNickname(string userid)
        {
            if (_userids.ContainsKey(userid)) return _userids[userid];

            try
            {
                var url = $"https://seiga.nicovideo.jp/api/user/info?id={userid}";
                var xml = await WebUtil.GetXmlAsync(url);
                return _userids[userid] = (string)xml.Descendants("user")
                    .SelectMany(x => x.Descendants("nickname"))
                    .FirstOrDefault();
            }
            catch
            {
                return userid;
            }
            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <response>
                <user>
                    <id>1</id>
                    <nickname>しんの</nickname>
                </user>
            </response>
            */
        }
        private static Dictionary<string, string> _userids = new Dictionary<string, string>();

    }
}
