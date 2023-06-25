using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoMylistViewModel : BindableBase
    {
        public NicoMylistViewModel(NicoMylistModel m)
        {
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistId = m.MylistId;
            }, nameof(MylistId), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistTitle = m.MylistTitle;
            }, nameof(MylistTitle), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistDescription = m.MylistDescription;
            }, nameof(MylistDescription), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistDate = m.MylistDate;
            }, nameof(MylistDate), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                SetThumbnail(m.ThumbnailUrl);
            }, nameof(m.ThumbnailUrl), true);
            m.UserInfo.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistUsername = m.UserInfo.Username;
            }, nameof(m.UserInfo.Username), true);
        }

        public string MylistId
        {
            get => _MylistId;
            set => SetProperty(ref _MylistId, value);
        }
        private string _MylistId;

        public string MylistTitle
        {
            get => _MylistTitle;
            set => SetProperty(ref _MylistTitle, value);
        }
        private string _MylistTitle;

        public string MylistDescription
        {
            get => _MylistDescription;
            set => SetProperty(ref _MylistDescription, value);
        }
        private string _MylistDescription = null;

        public string MylistUsername
        {
            get => _MylistUsername;
            set => SetProperty(ref _MylistUsername, value);
        }
        private string _MylistUsername = null;

        public DateTime MylistDate
        {
            get => _MylistDate;
            set => SetProperty(ref _MylistDate, value);
        }
        private DateTime _MylistDate;

        public BitmapImage Thumbnail
        {
            get => _Thumbnail;
            set => SetProperty(ref _Thumbnail, value);
        }
        private BitmapImage _Thumbnail;

        private async void SetThumbnail(string url)
        {
            await VideoUtil
                .GetThumnailAsync(url)
                .ContinueWith(x => Thumbnail = x.IsFaulted ? null : x.Result);
        }
    }
}
