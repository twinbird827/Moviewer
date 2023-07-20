using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Core.Controls
{
    public class UserModel : ControlModel, IThumbnailUrl
    {
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

        public virtual void SetUserInfo(UserModel m)
        {
            Userid = CoreUtil.Nvl(m.Userid, Userid);
            Username = CoreUtil.Nvl(m.Username, Username);
            ThumbnailUrl = CoreUtil.Nvl(m.ThumbnailUrl, ThumbnailUrl);
        }
    }
}