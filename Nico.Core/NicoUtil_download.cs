using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviewer.Nico.Core
{
    public static partial class NicoUtil
    {
        private static string ToDmcContent(dynamic n)
        {
            Func<dynamic, string[]> toarrstr = arr => ((object[])arr).Select(x => x.ToString()).ToArray();

            var src_id_to_mux = new
            {
                video_src_ids = toarrstr(n.videos),
                audio_src_ids = toarrstr(n.audios)
            };

            var content_src_ids0 = new
            {
                src_id_to_mux = src_id_to_mux
            };

            var content_src_ids = new[] { content_src_ids0 };

            var content_src_id_sets0 = new
            {
                content_src_ids = content_src_ids
            };

            var content_src_id_sets = new[] { content_src_id_sets0 };

            var heartbeat = new
            {
                lifetime = n.heartbeatLifetime
            };

            var keep_method = new
            {
                heartbeat = heartbeat
            };

            var http_output_download_parameters = new
            {
                use_well_known_port = "yes",
                use_ssl = "yes",
                transfer_preset = ""
            };

            var http_parameters_parameters = new
            {
                http_output_download_parameters = http_output_download_parameters
            };

            var http_parameters = new
            {
                parameters = http_parameters_parameters
            };

            var parameters = new
            {
                http_parameters = http_parameters
            };

            var protocol = new
            {
                name = "http",
                parameters = parameters
            };

            var session_operation_auth_by_signature = new
            {
                token = n.token,
                signature = n.signature
            };

            var session_operation_auth = new
            {
                session_operation_auth_by_signature = session_operation_auth_by_signature
            };

            var content_auth = new
            {
                auth_type = "ht2",
                content_key_timeout = n.contentKeyTimeout,
                service_id = "nicovideo",
                service_user_id = n.serviceUserId
            };

            var client_info = new
            {
                player_id = n.playerId
            };

            var session = new
            {
                recipe_id = n.recipeId,
                content_id = n.contentId,
                content_type = "movie",
                content_src_id_sets = content_src_id_sets,
                timing_constraint = "unlimited",
                keep_method = keep_method,
                protocol = protocol,
                content_uri = "",
                session_operation_auth = session_operation_auth,
                content_auth = content_auth,
                client_info = client_info,
                priority = n.priority
            };

            var jsonroot = new
            {
                session = session
            };

            //var url = $"{n.urls[0].url}?_format=json";
            //var req = new HttpRequestMessage(HttpMethod.Post, url);
            //req.Headers.Add("Accept", "application/json");
            //req.Headers.Add("Host", "api.dmc.nico");
            //req.Headers.Add("Connection", "keep-alive");
            //req.Headers.Add("sec-ch-ua-mobile", "?0");
            //req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36 Edg/93.0.961.38");
            //req.Headers.Add("sec-ch-ua-platform", "Windows");
            //req.Headers.Add("Origin", "https://www.nicovideo.jp");
            ////req.Headers.Add("sec-ch-ua", "\"Microsoft Edge\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
            //req.Headers.Add("Sec-Fetch-Site", "cross-site");
            //req.Headers.Add("Sec-Fetch-Mode", "cors");
            //req.Headers.Add("Sec-Fetch-Dest", "empty");
            //req.Headers.Add("Referer", "https://www.nicovideo.jp/");
            //req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            //req.Headers.Add("Accept-Language", "ja,en;q=0.9,en-GB;q=0.8,en-US;q=0.7");
            //req.Content = content;

            return DynamicJson.Serialize(jsonroot);
        }

    }
}