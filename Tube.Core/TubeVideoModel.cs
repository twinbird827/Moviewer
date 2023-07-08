using Moviewer.Core;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TBird.Wpf;

namespace Moviewer.Tube.Core
{
    public class TubeVideoModel : BindableBase
    {
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

        public NicoUserModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private NicoUserModel _UserInfo;

        public VideoStatus Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }
        private VideoStatus _Status = VideoStatus.None;

        public string Tags
        {
            get => _Tags;
            set => SetProperty(ref _Tags, value);
        }
        private string _Tags = null;

    }
}