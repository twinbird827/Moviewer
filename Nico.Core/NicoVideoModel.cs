﻿using Moviewer.Core;
using Moviewer.Core.Windows;
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
        public NicoVideoModel SetFromContentId(string contentid)
        {
            ContentId = contentid;
            Status = VideoStatus.Delete;

            _isinitialize = true;
            return this;
        }

        public NicoVideoModel SetFromJsonVideo(dynamic json)
        {
            ContentId = (string)json.data.video.id;
            Title = (string)json.data.video.title;
            Description = (string)json.data.video.description;
            ThumbnailUrl = (string)json.data.video.thumbnail.url;
            ViewCounter = (long)json.data.video.count.view;
            CommentCounter = (long)json.data.video.count.comment;
            MylistCounter = (long)json.data.video.count.mylist;
            StartTime = DateTime.Parse((string)json.data.video.registeredAt);
            LengthSeconds = (long)json.data.video.duration;
            Tags = string.Join(' ', GetTags(json));
            UserInfo = json.data.channel == null
                ? new NicoUserModel($"{json.data.owner.id}", (string)json.data.owner.nickname, null, null)
                : new NicoUserModel(null, null, (string)json.data.channel.id, (string)json.data.channel.name);

            RefreshStatus();

            _isinitialize = false;
            return this;
        }

        private static IEnumerable<string> GetTags(dynamic json)
        {
            foreach (var item in json.data.tag.items)
            {
                yield return (string)item.name;
            }
        }

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
            UserInfo = new NicoUserModel(
                xml.ElementS("user_id"),
                xml.ElementS("user_nickname"),
                "ch" + xml.ElementS("ch_id"),
                xml.ElementS("ch_name")
            );
            RefreshStatus();

            _isinitialize = false;
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
                RefreshStatus();

                _isinitialize = true;
            }
            catch
            {
                Status = VideoStatus.Delete;
            }

            return this;
        }

        public NicoVideoModel SetFromJson(dynamic item)
        {
            try
            {
                ContentId = item["contentId"];
                Title = item["title"];
                Description = item["description"];
                ThumbnailUrl = item["thumbnailUrl"];
                ViewCounter = (long)item["viewCounter"];
                CommentCounter = (long)item["commentCounter"];
                MylistCounter = (long)item["mylistCounter"];
                StartTime = DateTimeOffset.Parse(item["startTime"]).DateTime;
                LengthSeconds = (long)item["lengthSeconds"];
                Tags = item["tags"];
                UserInfo = new NicoUserModel($"{item["userId"]}", null, $"ch{item["channelId"]}", null);
                RefreshStatus();

                _isinitialize = true;
            }
            catch
            {
                Status = VideoStatus.Delete;
            }

            return this;
        }

        public void RefreshStatus()
        {
            // Temporaryの有無でﾌﾟﾛﾊﾟﾃｨを変更
            if (NicoModel.Temporaries.FirstOrDefault(x => x.ContentId == ContentId) is NicoVideoHistoryModel tmp)
            {
                TempTime = tmp.Date;
            }

            Status = NicoModel.Histories.Any(x => x.ContentId == ContentId)
                ? VideoStatus.See
                : NicoModel.Temporaries.Any(x => x.ContentId == ContentId && MainViewModel.Instance.StartupTime < x.Date)
                ? VideoStatus.New
                : NicoModel.Temporaries.Any(x => x.ContentId == ContentId)
                ? VideoStatus.Temporary
                : VideoStatus.None;
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

        public async Task RefreshProperties()
        {
            if (!_isinitialize) return;

            var m = await NicoUtil.GetVideo(ContentId);
            if (m.Status == VideoStatus.Delete) return;

            _isinitialize = false;
            ContentId = CoreUtil.Nvl(ContentId, m.ContentId);
            Title = CoreUtil.Nvl(Title, m.Title);
            Description = CoreUtil.Nvl(m.Description, Description);
            ThumbnailUrl = CoreUtil.Nvl(ThumbnailUrl, m.ThumbnailUrl);
            ViewCounter = Arr(ViewCounter, m.ViewCounter).Max();
            CommentCounter = Arr(CommentCounter, m.CommentCounter).Max();
            MylistCounter = Arr(MylistCounter, m.MylistCounter).Max();
            StartTime = Arr(StartTime, m.StartTime).Max();
            LengthSeconds = Arr(LengthSeconds, m.LengthSeconds).Max();
            Tags = CoreUtil.Nvl(Tags, m.Tags);
            UserInfo = UserInfo != null ? UserInfo.SetUserInfo(m.UserInfo) : m.UserInfo;
            RefreshStatus();
        }

        private bool _isinitialize = false;

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