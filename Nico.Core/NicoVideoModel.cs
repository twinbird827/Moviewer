using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoVideoModel : BindableBase
    {
        public NicoVideoModel SetFromXml(XElement xml)
        {
            xml = xml.Descendants("thumb").First();
            ContentId = VideoUtil.Url2Id(xml.ElementS("watch_url"));
            Title = xml.ElementS("title");
            Description = xml.ElementS("description");
            ThumbnailUrl = xml.ElementS("thumbnail_url");
            ViewCounter = xml.ElementL("view_counter");
            CommentCounter = xml.ElementL("comment_num");
            MylistCounter = xml.ElementL("mylist_counter");
            StartTime = DateTime.Parse(xml.ElementS("first_retrieve"));
            LengthSeconds = ToLengthSeconds(xml.ElementS("length"));
            Tags = xml.Descendants("tags").First().Descendants("tag").Select(tag => (string)tag).GetString(" ");
            UserInfo = new NicoUserModel(xml.ElementS("user_id"), xml.ElementS("user_nickname"));

            return this;
        }

        public NicoVideoModel SetFromXml(XElement item, string view, string mylist, string comment)
        {
            try
            {
                // 明細部読み込み
                var descriptionXml = GetDescriptionXml(item);

                ContentId = VideoUtil.Url2Id(item.ElementS("link"));
                Title = item.Element("title").Value;
                Description = (string)descriptionXml.Descendants("p").FirstOrDefault(x => x.AttributeS("class") == "nico-description");
                ThumbnailUrl = descriptionXml.Descendants("img").First().AttributeS("src");
                ViewCounter = ToCounter(descriptionXml, view);
                CommentCounter = ToCounter(descriptionXml, comment);
                MylistCounter = ToCounter(descriptionXml, mylist);
                StartTime = ToRankingDatetime(descriptionXml, "nico-info-date");
                LengthSeconds = ToLengthSeconds(descriptionXml);
                
                // 取得できない項目は非同期で設定する。
                SetFromVideo();
            }
            catch
            {
                Status = VideoStatus.Delete;
            }

            return this;
        }

        private async void SetFromVideo()
        {
            var video = await NicoUtil.GetVideo(ContentId);
            if (video.Status == VideoStatus.Delete) return;

            Title = CoreUtil.Nvl(Title, video.Title);
            Description = CoreUtil.Nvl(Description, video.Description);
            ThumbnailUrl = CoreUtil.Nvl(ThumbnailUrl, video.ThumbnailUrl);
            Tags = CoreUtil.Nvl(Tags, video.Tags);
            UserInfo = UserInfo ?? video.UserInfo;
        }

        private XElement GetDescriptionXml(XElement item)
        {
            var descriptionString = item.Element("description").Value;
            descriptionString = descriptionString.Replace("&nbsp;", "&#x20;");
            //descriptionString = HttpUtility.HtmlDecode(descriptionString);
            descriptionString = descriptionString.Replace("&", "&amp;");
            descriptionString = descriptionString.Replace("'", "&apos;");
            return WebUtil.ToXml($"<root>{descriptionString}</root>");
        }

        private long ToLengthSeconds(string lengthSecondsStr)
        {
            var lengthSecondsIndex = 0;
            var lengthSeconds = lengthSecondsStr
                    .Split(':')
                    .Reverse()
                    .Sum(s => int.Parse(s) * Math.Pow(60, lengthSecondsIndex++));
            return (long)lengthSeconds;
        }

        private long ToLengthSeconds(XElement xml)
        {
            var lengthSecondsStr = (string)xml
                .Descendants("strong")
                .Where(x => (string)x.Attribute("class") == "nico-info-length")
                .First();

            return ToLengthSeconds(lengthSecondsStr);
        }

        private string GetData(XElement e, string name)
        {
            return (string)e
                .Descendants("strong")
                .Where(x => (string)x.Attribute("class") == name)
                .FirstOrDefault();
        }

        private long ToCounter(XElement e, string name)
        {
            var s = string.IsNullOrEmpty(name) ? null : GetData(e, name);

            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                return long.Parse(s.Replace(",", ""));
            }
        }

        private DateTime ToRankingDatetime(XElement e, string name)
        {
            // 2018年02月27日 20：00：00
            var s = GetData(e, name);

            return DateTime.ParseExact(s,
                "yyyy年MM月dd日 HH：mm：ss",
                System.Globalization.DateTimeFormatInfo.InvariantInfo,
                System.Globalization.DateTimeStyles.None
            );
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