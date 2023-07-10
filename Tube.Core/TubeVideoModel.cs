using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Tube.Core
{
    public class TubeVideoModel : BindableBase
    {
        public TubeVideoModel SetFromJson(dynamic json)
        {
            ContentId = DynamicUtil.S(json.id);
            Title = DynamicUtil.S(json.snippet.title);
            Description = DynamicUtil.S(json.snippet.title.description);
            ThumbnailUrl = CoreUtil.Nvl(
                DynamicUtil.S(json.snippet.title.thumbnails.standard.url),
                DynamicUtil.S(json.snippet.title.thumbnails.high.url),
                DynamicUtil.S(json.snippet.title.thumbnails.medium.url),
                DynamicUtil.S(json.snippet.title.thumbnails["default"].url)
            );
            ViewCounter = DynamicUtil.L(json.statistics.viewCount);
            LikeCounter = DynamicUtil.L(json.statistics.likeCount);
            CommentCounter = DynamicUtil.L(json.statistics.commentCount);
            StartTime = DateTime.Parse(DynamicUtil.S(json.snippet.publishedAt));
            TempTime = default(DateTime);
            Duration = XmlConvert.ToTimeSpan(DynamicUtil.S(json.contentDetails.duration));
            Tags = GetTags(json).ToArray();
            UserInfo = new TubeUserModel(
                DynamicUtil.S(json.snippet.channelId),
                DynamicUtil.S(json.snippet.channelTitle)
            );

            RefreshStatus();

            return this;
        }

        private IEnumerable<string> GetTags(dynamic json)
        {
            foreach (var item in json.snippet.tags) yield return DynamicUtil.S(item);
        }

        public void RefreshStatus()
        {
            // Temporaryの有無でﾌﾟﾛﾊﾟﾃｨを変更
            if (TubeModel.Temporaries.FirstOrDefault(x => x.ContentId == ContentId) is TubeVideoHistoryModel tmp)
            {
                TempTime = tmp.Date;
            }

            Status = TubeModel.Histories.Any(x => x.ContentId == ContentId)
                ? VideoStatus.See
                : TubeModel.Temporaries.Any(x => x.ContentId == ContentId && MainViewModel.Instance.StartupTime < x.Date)
                ? VideoStatus.New
                : TubeModel.Temporaries.Any(x => x.ContentId == ContentId)
                ? VideoStatus.Temporary
                : VideoStatus.None;
        }

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