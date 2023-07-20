using Moviewer.Core;
using Moviewer.Core.Controls;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using Moviewer.Tube.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Tube.Controls
{
    public class TubeVideoModel : VideoModel
    {
        public override MenuMode Mode => MenuMode.Youtube;

        public TubeVideoModel()
        {
            Counters.AddRange(Arr(_ViewCount, _LikeCount, _CommentCount));
        }

        public TubeVideoModel(dynamic json)
        {
            ContentId = DynamicUtil.S(json, "id");
            Title = DynamicUtil.S(json, "snippet.title");
            Description = DynamicUtil.S(json, "snippet.description");
            ThumbnailUrl = CoreUtil.Nvl(
                DynamicUtil.S(json, "snippet.thumbnails.standard.url"),
                DynamicUtil.S(json, "snippet.thumbnails.high.url"),
                DynamicUtil.S(json, "snippet.thumbnails.medium.url")
            );
            ViewCount = DynamicUtil.L(json, "statistics.viewCount");
            LikeCount = DynamicUtil.L(json, "statistics.likeCount");
            CommentCount = DynamicUtil.L(json, "statistics.commentCount");
            StartTime = DateTime.Parse(DynamicUtil.S(json, "snippet.publishedAt"));
            TempTime = default;
            Duration = XmlConvert.ToTimeSpan(DynamicUtil.S(json, "contentDetails.duration"));
            Tags.AddRange((string[])DynamicUtil.T<string[]>(json, "snippet.tags"));
            UserInfo = new TubeUserModel(
                DynamicUtil.S(json, "snippet.channelId"),
                DynamicUtil.S(json, "snippet.channelTitle")
            );

            RefreshStatus();
        }

        public long ViewCount
        {
            get => _ViewCount.Count;
            set => _ViewCount.Count = value;
        }
        private CounterModel _ViewCount = new CounterModel(CounterType.View, 0);

        public long LikeCount
        {
            get => _LikeCount.Count;
            set => _LikeCount.Count = value;
        }
        private CounterModel _LikeCount = new CounterModel(CounterType.Like, 0);

        public long CommentCount
        {
            get => _CommentCount.Count;
            set => _CommentCount.Count = value;
        }
        private CounterModel _CommentCount = new CounterModel(CounterType.Comment, 0);

    }
}