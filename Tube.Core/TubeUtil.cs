﻿using Codeplex.Data;
using Moviewer.Core;
using Moviewer.Tube.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static string Url2Id(string url)
        {
            return CoreUtil.Nvl(url).Split('/').Last().Split("v=").First();
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

        public static async Task<TubeVideoModel> GetVideo(string id)
        {
            return await GetVideosByIds(id).ContinueWith(x => x.Result.FirstOrDefault());
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

        public static async Task<IEnumerable<TubeVideoModel>> GetVideosByHome()
        {
            var htmlString = await WebUtil.GetStringAsync(@"https://www.youtube.com/?hl=ja&gl=JP");
            var jsonMatch = Regex.Match(htmlString, @"(?<=var ytInitialData =)[^;]+");
            if (!jsonMatch.Success) return Enumerable.Empty<TubeVideoModel>();

            dynamic json = DynamicJson.Parse(jsonMatch.Value);
            var ids = GetVideosByHome(json);
            return await GetVideosByHome(ids);
        }

        private static IEnumerable<string> GetVideosByHome(dynamic json)
        {
            foreach (dynamic tab in json.contents.twoColumnBrowseResultsRenderer.tabs)
            {
                foreach (dynamic tmp in tab.tabRenderer.content.richGridRenderer.contents)
                {
                    var id = DynamicUtil.S(tmp, "richItemRenderer.content.videoRenderer.videoId");
                    if (string.IsNullOrEmpty(id)) continue;
                    yield return id;
                }
            }
        }

        private static async Task<IEnumerable<TubeVideoModel>> GetVideosByHome(IEnumerable<string> arr)
        {
            return await arr.Chunk(50)
                .Select(ids => GetVideosByIds(ids.ToArray()))
                .WhenAll()
                .ContinueWith(x => x.Result.SelectMany(x => x));
        }

    }
}