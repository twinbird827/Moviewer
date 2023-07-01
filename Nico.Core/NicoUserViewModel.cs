using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Workspaces;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoUserViewModel : BindableBase
    {
        public NicoUserViewModel()
        {

        }

        public NicoUserViewModel(NicoUserModel m)
        {
            SetModel(m);
        }

        public void SetModel(NicoUserModel m)
        {
            Userid = m.Userid;

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Username = m.Username;
            }, nameof(m.Username), true);

            SetThumbnail(m.ThumbnailUrl);

            AddDisposed((sender, e) =>
            {
                Thumbnail = null;
                OnClickUsername.TryDispose();
            });
        }

        private async void SetThumbnail(string url)
        {
            await VideoUtil
                .GetThumnailAsync(url, NicoUtil.NicoBlankUserUrl)
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
            MainViewModel.Instance.Current = new NicoSearchViewModel(Userid, NicoSearchType.User);
        });
        private ICommand _OnClickUsername;

    }
}