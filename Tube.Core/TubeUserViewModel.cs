using Moviewer.Core;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TBird.Wpf;

namespace Moviewer.Tube.Core
{
    public class TubeUserViewModel : BindableBase
    {
        public TubeUserViewModel()
        {

        }

        public TubeUserViewModel(TubeUserModel m)
        {

        }

        public void SetModel(TubeUserModel m)
        {
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                ChannelId = m.ChannelId;
            }, nameof(m.ChannelId), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Title = m.Title;
            }, nameof(m.Title), true);

            m.AddOnPropertyChanged(this, async (sender, e) =>
            {
                await VideoUtil
                    .GetThumnailAsync(ChannelId, m.ThumbnailUrl)
                    .ContinueWith(x => Thumbnail = x.IsFaulted ? null : x.Result);
            }, nameof(m.ThumbnailUrl), true);

            AddDisposed((sender, e) =>
            {
                Thumbnail = null;
            });
        }

        public string ChannelId
        {
            get => _ChannelId;
            set => SetProperty(ref _ChannelId, value);
        }
        private string _ChannelId = null;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        private string _Title = null;

        public BitmapImage Thumbnail
        {
            get => _Thumbnail;
            set => SetProperty(ref _Thumbnail, value);
        }
        private BitmapImage _Thumbnail;

    }
}
