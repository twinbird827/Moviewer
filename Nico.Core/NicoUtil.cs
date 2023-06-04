using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public static class NicoUtil
    {
        private const string NicoComboPath = @"lib\nico-combo-setting.xml";

        public static ComboboxModel[] Combos { get; private set; }

        public static void Initialize()
        {
            Combos = XDocument.Load(FileUtil.RelativePathToAbsolutePath(NicoComboPath))
                .Root
                .Descendants("combo")
                .Select(x => new ComboboxModel(
                    x.AttributeS("group"), 
                    x.Descendants("item").Select(i => 
                        new ComboboxItemModel(i.AttributeS("value"), i.AttributeS("display"))
                    )
                ))
                .ToArray();
        }

        public static async Task<NicoVideoModel> GetVideo(string videoid)
        {
            var video = new NicoVideoModel();

            try
            {
                var txt = await WebUtil.GetStringAsync($"http://ext.nicovideo.jp/api/getthumbinfo/{videoid}");
                var xml = WebUtil.ToXml(txt);

                if (xml == null || xml.AttributeS("status") == "fail")
                {
                    video.ContentId = videoid;
                    video.Status = VideoStatus.Delete;
                    return video;
                }

                return video.SetFromXml(xml);
            }
            catch
            {
                video.ContentId = videoid;
                video.Status = VideoStatus.Delete;
                return video;
            }
        }

        /// <summary>
        /// URLのchannelﾀｸﾞの内容をXml形式で取得します。
        /// </summary>
        /// <param name="url">URL</param>
        public static async Task<XElement> GetXmlChannelAsync(string url)
        {
            var xml = await WebUtil.GetXmlAsync(url);
            var tmp = xml.Descendants("channel");
            return tmp.First();
        }

        private static async Task<IEnumerable<NicoVideoModel>> GetVideosFromXmlUrl(string url, string view, string mylist, string comment)
        {
            var xml = await GetXmlChannelAsync(url);

            return xml
                .Descendants("item")
                .Select(item => new NicoVideoModel().SetFromXml(item, view, mylist, comment));
        }

        public static Task<IEnumerable<NicoVideoModel>> GetVideosByRanking(string genre, string tag, string term)
        {
            var url = $"https://www.nicovideo.jp/ranking/genre/{genre}?video_ranking_menu?tag={tag}&term={term}&rss=2.0&lang=ja-jp";

            return GetVideosFromXmlUrl(url,
                "nico-info-total-view",
                "nico-info-total-mylist",
                "nico-info-total-res"
            );
        }

        public static Task<IEnumerable<NicoVideoModel>> GetVideosByMylist(string mylistid, string orderby)
        {
            var url = $"http://www.nicovideo.jp/mylist/{mylistid}?rss=2.0&numbers=1&sort={orderby}";

            return GetVideosFromXmlUrl(url,
                "nico-numbers-view",
                "nico-numbers-mylist",
                "nico-numbers-res"
            );
        }

        public static Task<IEnumerable<NicoVideoModel>> GetVideosByNicouser(string userid, string key, string order)
        {
            var url = $"https://www.nicovideo.jp/user/{userid}/video?sortKey={key}&sortOrder={order}&rss=2.0";

            return GetVideosFromXmlUrl(url, null, null, null);
        }
    }
}
