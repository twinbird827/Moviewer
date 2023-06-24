using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Core.Stateful;
using TBird.Wpf;
using TBird.Core;
using Moviewer.Core;

namespace Moviewer.Nico.Workspaces
{
    public class NicoFavoriteViewModel : WorkspaceViewModel, INicoSearchHistoryParentViewModel
    {
        public override MenuType Type => MenuType.NicoFavorite;

        public NicoFavoriteViewModel()
        {
            Sources = new ObservableSynchronizedCollection<NicoVideoModel>();

            Videos = Sources.ToSyncedSynchronizationContextCollection(
                x => new NicoVideoViewModel(this, x),
                WpfUtil.GetContext()
            );

            Orderby = new ComboboxViewModel(NicoUtil.GetCombos("order_by"));
            Orderby.SelectedItem = Orderby.GetItemNotNull(NicoSetting.Instance.NicoFavoriteOrderby);

            Favorites = NicoModel.SearchFavorites
                .ToSyncedSortedObservableCollection(x => x.Date, isDescending: true)
                .ToSyncedObservableSynchronizedCollection(x => new NicoSearchHistoryViewModel(this, x))
                .ToSyncedSynchronizationContextCollection(x => x, WpfUtil.GetContext());

            AddDisposed((sender, e) =>
            {
                NicoSetting.Instance.NicoFavoriteOrderby = Orderby.SelectedItem.Value;
                NicoSetting.Instance.Save();

                Orderby.Dispose();
                Videos.Dispose();
            });
        }

        public ComboboxViewModel Orderby { get; private set; }

        public ObservableSynchronizedCollection<NicoVideoModel> Sources { get; private set; }

        public SynchronizationContextCollection<NicoVideoViewModel> Videos { get; private set; }

        public SynchronizationContextCollection<NicoSearchHistoryViewModel> Favorites { get; private set; }

        public ICommand OnSearch => _OnSearch = _OnSearch ?? RelayCommand.Create<NicoSearchHistoryViewModel>(async vm =>
        {
            await NicoUtil.GetVideoBySearchType(vm.Word, vm.Type, Orderby.SelectedItem.Value).ContinueOnUI(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });

            NicoModel.AddSearchFavorite(vm.Word, vm.Type);
        });
        private ICommand _OnSearch;

        public void NicoSearchHistoryOnDoubleClick(NicoSearchHistoryViewModel vm)
        {
            OnSearch.Execute(vm);
        }

        public void NicoSearchHistoryOnDelete(NicoSearchHistoryViewModel vm)
        {
            NicoModel.DelSearchFavorite(vm.Word, vm.Type);
        }
    }
}
