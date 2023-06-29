using Moviewer.Core;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoMylistModel : BindableBase
    {
        public static async Task<NicoMylistModel> GetNicoMylistModel(string id)
        {
            var xml = await NicoUtil.GetXmlChannelAsync($"http://www.nicovideo.jp/mylist/{VideoUtil.Url2Id(id)}?rss=2.0&numbers=1&sort=0");
            return new NicoMylistModel(id, xml);
        }

        private NicoMylistModel(string id, XElement xml)
        {
            MylistId = id;
            MylistTitle = GetMylistTitle(xml.ElementS("title"));
            MylistDate = DateTime.Parse(xml.ElementS("lastBuildDate"));
            MylistDescription = xml.ElementS("description");

            UserInfo = new NicoUserModel(
                Regex.Match(xml.ElementS("link"), @"(?<=user\/)[\d]+").Value,          // user id
                xml.ElementS(XName.Get("creator", "http://purl.org/dc/elements/1.1/")) // creator name
            , null, null);

            UserInfo.AddOnPropertyChanged(this, (sender, e) =>
            {
                ThumbnailUrl = UserInfo.ThumbnailUrl;
            }, nameof(UserInfo.ThumbnailUrl), true);
        }

        private string GetMylistTitle(string value)
        {
            return NicoUtil.GetCombos("mylist_title_removes")
                .Aggregate(value, (s, c) => s.Replace(c.Display, ""));
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

        public DateTime MylistDate
        {
            get => _MylistDate;
            set => SetProperty(ref _MylistDate, value);
        }
        private DateTime _MylistDate;

        public string ThumbnailUrl
        {
            get => _ThumbnailUrl;
            set => SetProperty(ref _ThumbnailUrl, value);
        }
        private string _ThumbnailUrl;

        public NicoUserModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private NicoUserModel _UserInfo;
    }
}