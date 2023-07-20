﻿using Microsoft.WindowsAPICodePack.PortableDevices.CommandSystem.Object;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using Moviewer.Core;
using Moviewer.Core.Controls;
using Moviewer.Core.Windows;
using Moviewer.Nico.Controls;
using Moviewer.Nico.Core;
using Moviewer.Tube.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TBird.Core;
using TBird.Web;
using TBird.Wpf;

namespace Moviewer.Tube.Controls
{
    public class TubeVideoViewModel : VideoViewModel
    {
        public TubeVideoViewModel(WorkspaceViewModel parent, TubeVideoModel m) : base(m)
        {
            Parent = parent;

            AddOnPropertyChanged(this, (sender, e) =>
            {
                ContentUrl = $"https://www.youtube.com/watch?v={ContentId}";
            }, nameof(ContentId), true);

            AddDisposed((sender, e) =>
            {
                Parent = null;
            });
        }

        public WorkspaceViewModel Parent { get; private set; }

        protected override UserViewModel CreateUserViewModel(UserModel m)
        {
            return new TubeUserViewModel(m);
        }

        protected override TagViewModel CreateTagViewModel(string tag)
        {
            return new TubeTagViewModel(tag);
        }

        protected override void DoubleClickCommand(object parameter)
        {
            //// TODO 子画面出して追加するかどうかを決めたい
            //// TODO ﾘﾝｸも抽出したい
            //// TODO smだけじゃなくてsoとかも抽出したい
            //foreach (var videoid in Regex.Matches(Description, @"(?<id>sm[\d]+)").OfType<Match>()
            //        .Select(m => m.Groups["id"].Value)
            //        .Where(tmp => VideoUtil.Histories.GetModel(Source) == null)
            //    )
            //{
            //    VideoUtil.AddTemporary(Source.Mode, videoid);
            //}

            base.DoubleClickCommand(parameter);
        }

        protected override void KeyDownCommand(KeyEventArgs e)
        {
            base.KeyDownCommand(e);

            //else if (e.Key == Key.Delete && Parent is INicoVideoParentViewModel parent)
            //{
            //    parent.NicoVideoOnDelete(this);
            //}
        }

        protected override DownloadModel GetDownloadModel()
        {
            throw new NotImplementedException();
            //return new NicoDownloadModel(Source as NicoVideoModel);
        }

    }
}