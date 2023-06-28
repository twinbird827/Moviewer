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
using TBird.Wpf.Collections;
using TBird.Wpf.Controls;

namespace Moviewer.Nico.Workspaces
{
    public class NicoTemporaryViewModel : WorkspaceViewModel, INicoVideoParentViewModel
    {
        public override MenuType Type => MenuType.NicoTemporary;

        public NicoTemporaryViewModel()
        {
            Videos = NicoModel.Temporaries
                .ToBindableSelectCollection(x => x.GetVideo())
                .ToBindableSortedCollection(x => x.TempTime, isDescending: true)
                .ToBindableSelectCollection(x => new NicoVideoViewModel(this, x))
                .ToBindableContextCollection();

            AddDisposed((sender, e) =>
            {
                Videos.Dispose();
            });
        }

        public BindableContextCollection<NicoVideoViewModel> Videos
        {
            get => _Videos;
            set => SetProperty(ref _Videos, value);
        }
        public BindableContextCollection<NicoVideoViewModel> _Videos;

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

        public ICommand OnDrop => _OnDrop = _OnDrop ?? RelayCommand.Create<DragEventArgs>(async e =>
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
        private ICommand _OnDrop;

        public void NicoVideoOnDelete(NicoVideoViewModel vm)
        {
            NicoModel.DelTemporary(vm.Source.ContentId);
        }
    }
}
