using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;
using TBird.Core;

namespace Moviewer.Nico.Core
{
    public class NicoUserModel : BindableBase
    {
        public NicoUserModel(string userid, string username) 
        {
            Userid = userid;
            Username = username ?? Userid;
            ThumbnailUrl = GetThumbnailUrl(Userid);
        }

        public string Userid
        {
            get => _Userid;
            set => SetProperty(ref _Userid, value);
        }
        private string _Userid = null;

        public string Username
        {
            get => _Username;
            set => SetProperty(ref _Username, value);
        }
        private string _Username = null;

        public string ThumbnailUrl
        {
            get => _ThumbnailUrl;
            set => SetProperty(ref _ThumbnailUrl, value);
        }
        private string _ThumbnailUrl;

        private string GetThumbnailUrl(string value)
        {
            var url1 = "https://secure-dcdn.cdn.nimg.jp/nicoaccount/usericon";
            var url2 = 4 < value.Length ? value.Left(value.Length - 4) : "0";
            var url3 = value;
            return $"{url1}/{url2}/{url3}.jpg";
        }

    }
}
