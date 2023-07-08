using Codeplex.Data;
using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;
using TBird.Wpf.Controls;

namespace Moviewer.Tube.Core
{
    public static class TubeUtil
    {
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

        private static IEnumerable<TubeVideoModel> GetVideosByJson(dynamic json)
        {
            foreach (var item in json.items)
            {
                yield return new TubeVideoModel();
            }
        }

        private static async Task<IEnumerable<TubeVideoModel>> GetVideosByUrl(string url, Dictionary<string, string> dic)
        {
            var urlparameter = dic.Select(x => $"{x.Key}={x.Value}").GetString("&");
            var body = await WebUtil.GetStringAsync(url + urlparameter);
            var json = DynamicJson.Parse(body);

            return GetVideosByJson(json);
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