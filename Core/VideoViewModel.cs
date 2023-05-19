using System.Windows.Media.Imaging;
using TBird.Wpf;

namespace Moviewer.Core
{
    public class VideoViewModel : BindableBase
    {
        public VideoViewModel(VideoModel video)
        {

        }

        public BitmapImage Thumbnail
        {
            get => _Thumbnail;
            set => SetProperty(ref _Thumbnail, value);
        }
        private BitmapImage _Thumbnail;
    }
}