using Codeplex.Data;
using Moviewer.Core;
using Moviewer.Tube.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TBird.Core;
using TBird.Web;
using TBird.Wpf;
using TBird.Wpf.Controls;

namespace Moviewer.Tube.Core
{
    public static partial class TubeUtil
    {
        private const string TubeComboPath = @"lib\tube-combo-setting.xml";

        public static ComboboxModel[] Combos { get; private set; }

        public static void Initialize()
        {
            Combos = XmlUtil.Load(TubeComboPath)
                .Descendants("combo")
                .Select(x => new ComboboxModel(
                    x.AttributeS("group"),
                    x.Descendants("item").Select(i =>
                        new ComboboxItemModel(i.AttributeS("value"), i.AttributeS("display"))
                    )
                ))
                .ToArray();
        }

        public static IEnumerable<ComboboxItemModel> GetCombos(string group)
        {
            return Combos.Where(x => x.Group == group).SelectMany(x => x.Items);
        }

        public static string GetComboDisplay(string group, string value)
        {
            return GetCombos(group).FirstOrDefault(x => x.Value == value)?.Display;
        }

        private static async Task<dynamic> GetResponse(string url, Dictionary<string, string> dic)
        {
            dic.Add("key", GetAPIKEY());
            dic.Add("access_token", await GetAccessToken());
            var body = await WebUtil.GetStringAsync(WebUtil.GetUrl(url, dic));
            var json = DynamicJson.Parse(body);
            return json;
        }

        private static async Task<IEnumerable<TubeVideoModel>> GetUserThumnailUrlInVideos(IEnumerable<TubeVideoModel> videos)
        {
            var videoarr = videos.ToArray();
            if (!videoarr.Any()) return videoarr;

            var idcomma = videoarr.Select(x => x.UserInfo.Userid).GetString(",");

            var dic = new Dictionary<string, string>()
            {
                { "part", "snippet" },
                { "id", idcomma },
            };
            var json = await GetResponse("https://www.googleapis.com/youtube/v3/channels?", dic);

            foreach (var item in json.items)
            {
                var id = DynamicUtil.S(item, "id");
                var url = CoreUtil.Nvl(
                    DynamicUtil.S(item, "snippet.thumbnails.standard.url"),
                    DynamicUtil.S(item, "snippet.thumbnails.high.url"),
                    DynamicUtil.S(item, "snippet.thumbnails.medium.url")
                );
                var info = videoarr.FirstOrDefault(x => x.UserInfo.Userid == id);
                if (info != null) info.UserInfo.ThumbnailUrl = url;
            }

            return videoarr;
        }

        private static IEnumerable<TubeVideoModel> GetVideosByJson(dynamic json)
        {
            foreach (var item in json.items)
            {
                yield return new TubeVideoModel(item);
            }
        }

        private static async Task<IEnumerable<TubeVideoModel>> GetVideosByUrl(string url, Dictionary<string, string> dic)
        {
            return await GetUserThumnailUrlInVideos(GetVideosByJson(await GetResponse(url, dic)));
        }

        public static async Task<IEnumerable<TubeVideoModel>> GetVideosByIds(params string[] ids)
        {
            var dic = new Dictionary<string, string>()
            {
                { "part", "id,snippet,statistics,contentDetails" },
                { "id", ids.GetString(",") },
            };

            return await GetVideosByUrl("https://www.googleapis.com/youtube/v3/videos?", dic);
        }

        public static async Task<IEnumerable<TubeVideoModel>> GetVideosByPopular(string category)
        {
            var dic = new Dictionary<string, string>()
            {
                { "part", "id,snippet,statistics,contentDetails" },
                { "chart", "mostPopular" },
                { "maxResults", "50" },
                { "videoCategoryId", CoreUtil.Nvl(category, "0") },
                { "regionCode", "jp" },
            };

            return await GetVideosByUrl("https://www.googleapis.com/youtube/v3/videos?", dic);
        }

        public static async Task<IEnumerable<TubeVideoModel>> GetVideosByActivities(string category)
        {
            var dic = new Dictionary<string, string>()
            {
                { "part", "id,snippet,contentDetails" },
                { "chart", "mostPopular" },
                { "maxResults", "50" },
                { "videoCategoryId", CoreUtil.Nvl(category, "0") },
                { "regionCode", "jp" },
            };

            return await GetVideosByUrl("https://www.googleapis.com/youtube/v3/activities?", dic);
        }
    }
}