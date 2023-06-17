using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoUserViewModel : BindableBase
    {
        public NicoUserViewModel(NicoUserModel m)
        {
            Userid = m.Userid;
            Username = m.Username ?? Userid;
            SetThumbnail(m.ThumbnailUrl);
        }

        private async void SetThumbnail(string url)
        {
            await VideoUtil
                .GetThumnailAsync(url)
                .ContinueWith(x => Thumbnail = x.IsFaulted ? null : x.Result);
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

        public BitmapImage Thumbnail
        {
            get => _Thumbnail;
            set => SetProperty(ref _Thumbnail, value);
        }
        private BitmapImage _Thumbnail;

        public ICommand OnClickUsername => _OnClickUsername = _OnClickUsername ?? RelayCommand.Create(_ =>
        {
            //MainViewModel.Instance.Current = new NicoSearchViewModel(Tag, NicoSearchModel.TagModel.Type);
        });
        private ICommand _OnClickUsername;

    }
}
