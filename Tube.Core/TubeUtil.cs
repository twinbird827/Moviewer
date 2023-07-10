using Codeplex.Data;
using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TBird.Core;
using TBird.Wpf;
using TBird.Wpf.Controls;

namespace Moviewer.Tube.Core
{
    public static class TubeUtil
    {
        private const string TubeComboPath = @"lib\tube-combo-setting.xml";

        public static ComboboxModel[] Combos { get; private set; }

        public static void Initialize()
        {
            Combos = XDocument.Load(FileUtil.RelativePathToAbsolutePath(TubeComboPath))
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

        public static IEnumerable<ComboboxItemModel> GetCombos(string group)
        {
            return Combos.Where(x => x.Group == group).SelectMany(x => x.Items);
        }

        public static string GetComboDisplay(string group, string value)
        {
            return GetCombos(group).FirstOrDefault(x => x.Value == value)?.Display;
        }

        public static string GetAPIKEY()
        {
            if (string.IsNullOrEmpty(TubeSetting.Instance.APIKEY))
            {
                using (var vm = new WpfMessageInputViewModel(AppConst.H_InputAPIKEY, AppConst.M_InputAPIKEY, AppConst.L_APIKEY, true))
                {
                    if (vm.ShowDialog(() => new WpfMessageInputWindow()))
                    {
                        TubeSetting.Instance.APIKEY = vm.Value;
                        TubeSetting.Instance.Save();
                    }
                }
            }
            return TubeSetting.Instance.APIKEY;
        }

        private static async Task<dynamic> GetResponse(string url, Dictionary<string, string> dic)
        {
            var urlparameter = dic.Select(x => $"{x.Key}={x.Value}").GetString("&");
            var body = await WebUtil.GetStringAsync(url + urlparameter);
            var json = DynamicJson.Parse(body);
            return json;
        }

        private static async Task<IEnumerable<TubeVideoModel>> GetUserThumnailUrlInVideos(IEnumerable<TubeVideoModel> videos)
        {
            var dic = new Dictionary<string, string>()
            {
                { "part", "statistics" },
                { "id", videos.Select(x => x.UserInfo).Where(x => string.IsNullOrEmpty(x.ThumbnailUrl)).Select(x => x.ChannelId).GetString(",") },
                { "key", GetAPIKEY() },
            };
            var json = await GetResponse("https://www.googleapis.com/youtube/v3/channels?", dic);

            foreach (var item in json.items)
            {
                var id = DynamicUtil.S(item.id);
                var url = CoreUtil.Nvl(
                    DynamicUtil.S(item.snippet.thumbnails.standard.url),
                    DynamicUtil.S(item.snippet.thumbnails.high.url),
                    DynamicUtil.S(item.snippet.thumbnails.medium.url)
                );
                var info = videos.FirstOrDefault(x => x.UserInfo.ChannelId == id);
                if (info != null) info.UserInfo.ThumbnailUrl = url;
            }
            videos.ForEach(x => TubeModel.AddUser(x.UserInfo));

            return videos;
        }

        private static IEnumerable<TubeVideoModel> GetVideosByJson(dynamic json)
        {
            foreach (var item in json.items)
            {
                yield return new TubeVideoModel().SetFromJson(item);
            }
        }

        private static async Task<IEnumerable<TubeVideoModel>> GetVideosByUrl(string url, Dictionary<string, string> dic)
        {
            return GetUserThumnailUrlInVideos(GetVideosByJson(await GetResponse(url, dic)));
        }

        public static async Task<IEnumerable<TubeVideoModel>> GetVideosByIds(params string[] ids)
        {
            var dic = new Dictionary<string, string>()
            {
                { "part", "id,snippet,statistics" },
                { "id", ids.GetString(",") },
                { "key", GetAPIKEY() },
            };

            return await GetVideosByUrl("https://www.googleapis.com/youtube/v3/videos?", dic);
        }

        public static async Task<IEnumerable<TubeVideoModel>> GetVideosByPopular(string category)
        {
            var dic = new Dictionary<string, string>()
            {
                { "part", "id,snippet,statistics" },
                { "chart", "mostPopular" },
                { "maxResults", "50" },
                { "videoCategoryId", CoreUtil.Nvl(category, "0") },
                { "key", GetAPIKEY() },
            };

            return await GetVideosByUrl("https://www.googleapis.com/youtube/v3/videos?", dic);
        }
    }
}