using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core.Stateful;
using TBird.Wpf;

namespace Moviewer.Nico.Workspaces
{
    public class NicoTemporaryViewModel : WorkspaceViewModel
    {
        public NicoTemporaryViewModel()
        {
            Videos = NicoModel.Temporaries.ToSyncedSynchronizationContextCollection(
                x => new NicoVideoViewModel(this, x),
                WpfUtil.GetContext()
            );

            AddDisposed((sender, e) =>
            {
                Videos.Dispose();
            });
        }

        public SynchronizationContextCollection<NicoVideoViewModel> Videos
        {
            get => _Videos;
            set => SetProperty(ref _Videos, value);
        }
        public SynchronizationContextCollection<NicoVideoViewModel> _Videos;

    }
}
