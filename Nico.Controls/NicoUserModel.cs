using ControlzEx.Standard;
using Moviewer.Core.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;
using TBird.Web;

namespace Moviewer.Nico.Controls
{
    public class NicoUserModel : UserModel
    {
        public NicoUserModel()
        {
            AddOnPropertyChanged(this, async (sender, e) =>
            {
                if (string.IsNullOrEmpty(Userid)) return;

                if (!Userid.StartsWith("ch"))
                {
                    Username = await GetNickname(Userid);
                    var url0 = Userid;
                    var url1 = "https://secure-dcdn.cdn.nimg.jp/nicoaccount/usericon";
                    var url2 = 4 < url0.Length ? url0.Left(url0.Length - 4) : "0";
                    var url3 = url0;
                    ThumbnailUrl = $"{url1}/{url2}/{url3}.jpg";
                }
                else
                {
                    Username = Username ?? Userid;
                    ThumbnailUrl = $"https://secure-dcdn.cdn.nimg.jp/comch/channel-icon/128x128/{Userid}.jpg";
                }
            }, nameof(Userid), false);
        }

        private async Task<string> GetNickname(string userid)
        {
            using (await Locker.LockAsync(Lock))
            {
                if (_nicknames.ContainsKey(userid)) return _nicknames[userid];

                try
                {
                    var url = $"https://seiga.nicovideo.jp/api/user/info?id={userid}";
                    var xml = await WebUtil.GetXmlAsync(url);
                    return _nicknames[userid] = (string)xml.Descendants("user")
                        .SelectMany(x => x.Descendants("nickname"))
                        .FirstOrDefault();
                }
                catch
                {
                    return userid;
                }
            }
            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <response>
                <user>
                    <id>1</id>
                    <nickname>しんの</nickname>
                </user>
            </response>
            */
        }

        private static Dictionary<string, string> _nicknames = new Dictionary<string, string>();
    }
}