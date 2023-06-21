using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TBird.Core.Stateful;
using TBird.Wpf;
using TBird.Wpf.Controls;

namespace Moviewer.Nico.Workspaces
{
    public class NicoTemporaryViewModel : WorkspaceViewModel
    {
        public NicoTemporaryViewModel()
        {
            Videos = NicoModel.Temporaries
                .ToSyncedSortedObservableCollection(x => x.TempTime, isDescending: true)
                .ToSyncedSynchronizationContextCollection(
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

        public ICommand OnTemporaryAdd => _OnTemporaryAdd = _OnTemporaryAdd ?? RelayCommand.Create(async _ =>
        {
            using (var vm = new WpfMessageInputViewModel(AppConst.L_AddTemporary, AppConst.MH_AddTemporary, AppConst.L_UrlOrId, true))
            {
                if (!(bool)vm.ShowDialog(() => new WpfMessageInputWindow()))
                {
                    return;
                }

                var video = await NicoUtil.GetVideo(VideoUtil.Url2Id(vm.Value));
                if (video.Status != VideoStatus.Delete)
                {
                    NicoModel.AddTemporary(video.ContentId);
                }
            }
        });
        private ICommand _OnTemporaryAdd;

        public ICommand OnDropInListbox => _OnDropInListbox = _OnDropInListbox ?? RelayCommand.Create<DragEventArgs>(async e =>
        {
            if (e.Data.GetData(DataFormats.Text) is string url)
            {
                var video = await NicoUtil.GetVideo(VideoUtil.Url2Id(url));
                if (video.Status != VideoStatus.Delete)
                {
                    NicoModel.AddTemporary(video.ContentId);
                }
            }
        });
        private ICommand _OnDropInListbox;

    }
}
