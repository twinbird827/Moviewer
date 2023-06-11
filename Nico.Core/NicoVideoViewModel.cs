using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoVideoViewModel : BindableBase
    {
        public NicoVideoViewModel(NicoVideoModel m)
        {
            Source = m;

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Title = m.Title;
            }, nameof(m.Title), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Description = m.Description;
            }, nameof(m.Description), true);

            m.AddOnPropertyChanged(this, async (sender, e) =>
            {
                var urls = Arr(".L", ".M", "")
                    .Select(x => $"{m.ThumbnailUrl}{x}")
                    .ToArray();

                await VideoUtil
                    .GetThumnailAsync(urls)
                    .ContinueWith(x => Thumbnail = x.IsFaulted ? null : x.Result);
            }, nameof(m.ThumbnailUrl), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                ViewCounter = m.ViewCounter;
            }, nameof(m.ViewCounter), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistCounter = m.MylistCounter;
            }, nameof(m.MylistCounter), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                CommentCounter = m.CommentCounter;
            }, nameof(m.CommentCounter), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                StartTime = m.StartTime;
            }, nameof(m.StartTime), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                TempTime = m.TempTime;
            }, nameof(m.TempTime), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                LengthSeconds = m.LengthSeconds;
            }, nameof(m.LengthSeconds), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                UserInfo = new NicoUserViewModel(m.UserInfo);
            }, nameof(m.UserInfo), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Status = m.Status;
            }, nameof(m.Status), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Tags = new ObservableCollection<NicoVideoTagViewModel>(
                    m.Tags.Split(' ').Select(x => new NicoVideoTagViewModel(x))
                );
            }, nameof(m.Tags), true);
        }

        public NicoVideoModel Source { get; private set; }

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        private string _Title;

        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }
        private string _Description;

        public BitmapImage Thumbnail
        {
            get => _Thumbnail;
            set => SetProperty(ref _Thumbnail, value);
        }
        private BitmapImage _Thumbnail;

        public long ViewCounter
        {
            get => _ViewCounter;
            set => SetProperty(ref _ViewCounter, value);
        }
        private long _ViewCounter;

        public long MylistCounter
        {
            get => _MylistCounter;
            set => SetProperty(ref _MylistCounter, value);
        }
        private long _MylistCounter;

        public long CommentCounter
        {
            get => _CommentCounter;
            set => SetProperty(ref _CommentCounter, value);
        }
        private long _CommentCounter;

        public DateTime StartTime
        {
            get => _StartTime;
            set => SetProperty(ref _StartTime, value);
        }
        private DateTime _StartTime;

        public DateTime TempTime
        {
            get => _TempTime;
            set => SetProperty(ref _TempTime, value);
        }
        private DateTime _TempTime;

        public long LengthSeconds
        {
            get => _LengthSeconds;
            private set => SetProperty(ref _LengthSeconds, value);
        }
        private long _LengthSeconds;

        public NicoUserViewModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private NicoUserViewModel _UserInfo;

        public VideoStatus Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }
        private VideoStatus _Status = VideoStatus.None;

        public ObservableCollection<NicoVideoTagViewModel> Tags
        {
            get => _Tags;
            set => SetProperty(ref _Tags, value);
        }
        private ObservableCollection<NicoVideoTagViewModel> _Tags = null;

    }
}
