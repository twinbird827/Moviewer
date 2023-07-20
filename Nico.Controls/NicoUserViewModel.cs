using Moviewer.Core.Controls;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using Moviewer.Nico.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviewer.Nico.Controls
{
    public class NicoUserViewModel : UserViewModel
    {
        public NicoUserViewModel(UserModel m) : base(m)
        {

        }

        protected override void OnClickUsernameCommand(object _)
        {
            MainViewModel.Instance.Current = new NicoSearchViewModel(Userid, NicoSearchType.User);
        }

        public override Task SetThumbnail(string url)
        {
            return this.SetThumbnail(Userid, url, NicoUtil.NicoBlankUserUrl);
        }
    }
}