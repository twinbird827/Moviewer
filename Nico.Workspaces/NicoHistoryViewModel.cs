using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using TBird.Wpf.Collections;

namespace Moviewer.Nico.Workspaces
{
    public class NicoHistoryViewModel : WorkspaceViewModel, INicoVideoParentViewModel
    {
        public override MenuType Type => MenuType.NicoHistory;

        public NicoHistoryViewModel()
        {
            Videos = NicoModel.Histories
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

        public void NicoVideoOnDelete(NicoVideoViewModel vm)
        {
            NicoModel.DelHistory(vm.Source.ContentId);
        }
    }
}