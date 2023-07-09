using Microsoft.WindowsAPICodePack.PortableDevices.CommandSystem.Object;
using Moviewer.Core;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TBird.Core;
using TBird.Wpf;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Moviewer.Tube.Core
{
    public class TubeVideoViewModel : BindableBase
    {
        public TubeVideoViewModel(TubeVideoModel m)
        {
            VideoUrl = $"http://nico.ms/{m.ContentId}";

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Title = m.Title;
            }, nameof(m.Title), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Description = m.Description;
            }, nameof(m.Description), true);

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
                ExistTempTime = TempTime != default(DateTime);
            }, nameof(m.TempTime), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                LengthToSeconds = TimeSpan.FromSeconds(m.LengthSeconds);
            }, nameof(m.LengthSeconds), true);

            UserInfo = new NicoUserViewModel();
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                if (m.UserInfo != null) UserInfo.SetModel(m.UserInfo);
            }, nameof(m.UserInfo), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Status = m.Status;
            }, nameof(m.Status), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                if (m.Tags == null) return;

                Tags = new ObservableCollection<NicoVideoTagViewModel>(
                    m.Tags.Split(' ').Select(x => new NicoVideoTagViewModel(x))
                );
            }, nameof(m.Tags), true);

            AddDisposed((sender, e) =>
            {
                UserInfo.TryDispose();

                if (Tags != null)
                {
                    Tags.ForEach(x => x.Dispose());
                    Tags.Clear();
                }

                Loaded.TryDispose();
                OnDoubleClick.TryDispose();
                OnDownload.TryDispose();
                OnKeyDown.TryDispose();
                OnTemporaryAdd.TryDispose();
                OnTemporaryDel.TryDispose();

                Source = null;
                Parent = null;
            });
        }

        public string VideoUrl
        {
            get => _VideoUrl;
            set => SetProperty(ref _VideoUrl, value);
        }
        private string _VideoUrl;

        public string ContentId
        {
            get => _ContentId;
            set => SetProperty(ref _ContentId, value);
        }
        private string _ContentId;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, HttpUtility.HtmlDecode(value));
        }
        private string _Title;

        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, HttpUtility.HtmlDecode(value));
        }
        private string _Description;

        public string ThumbnailUrl
        {
            get => _ThumbnailUrl;
            set => SetProperty(ref _ThumbnailUrl, value);
        }
        private string _ThumbnailUrl;

        public long ViewCounter
        {
            get => _ViewCounter;
            set => SetProperty(ref _ViewCounter, value);
        }
        private long _ViewCounter;

        public long LikeCounter
        {
            get => _LikeCounter;
            set => SetProperty(ref _LikeCounter, value);
        }
        private long _LikeCounter;

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

        public TimeSpan Duration
        {
            get => _Duration;
            private set => SetProperty(ref _Duration, value);
        }
        private TimeSpan _Duration;

        public TubeUserModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private TubeUserModel _UserInfo;

        public VideoStatus Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }
        private VideoStatus _Status = VideoStatus.None;

        public string[] Tags
        {
            get => _Tags;
            set => SetProperty(ref _Tags, value);
        }
        private string[] _Tags = null;

    }
}
