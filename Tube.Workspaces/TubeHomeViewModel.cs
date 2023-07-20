using Moviewer.Core.Windows;
using Moviewer.Core;
using Moviewer.Tube.Controls;
using Moviewer.Tube.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf.Collections;
using TBird.Wpf;
using TBird.Core;

namespace Moviewer.Tube.Workspaces
{
    public class TubeHomeViewModel : WorkspaceViewModel
    {
        public override MenuType Type => MenuType.TubeRanking;

        public TubeHomeViewModel()
        {
            Sources = new BindableCollection<TubeVideoModel>();

            Videos = Sources
                .ToBindableSelectCollection(x => new TubeVideoViewModel(this, x))
                .ToBindableContextCollection();

            Loaded.Add(Reload);

            AddDisposed((sender, e) =>
            {
                Videos.Dispose();
                Sources.Dispose();
            });

        }

        public BindableCollection<TubeVideoModel> Sources { get; private set; }

        public BindableContextCollection<TubeVideoViewModel> Videos { get; private set; }

        private async Task Reload()
        {
            await TubeUtil.GetVideosByHome().ContinueWith(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });
        }
    }
}