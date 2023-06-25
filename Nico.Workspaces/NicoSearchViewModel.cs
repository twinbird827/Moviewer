using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TBird.Core.Stateful;
using TBird.Wpf;
using TBird.Core;
using System.Windows.Controls;
using TBird.Wpf.Collections;

namespace Moviewer.Nico.Workspaces
{
    public class NicoSearchViewModel : WorkspaceViewModel, INicoSearchHistoryParentViewModel
    {
        public override MenuType Type => MenuType.NicoSearch;

        public NicoSearchViewModel(string word, NicoSearchType type) : this()
        {
            Word = word;
            OnSearch.Execute(type);
        }

        public NicoSearchViewModel()
        {
            Sources = new BindableCollection<NicoVideoModel>();

            Videos = Sources
                .ToBindableConvertCollection(x => new NicoVideoViewModel(this, x))
                .ToBindableContextCollection();

            Orderby = new ComboboxViewModel(NicoUtil.GetCombos("order_by"));
            Orderby.SelectedItem = Orderby.GetItemNotNull(NicoSetting.Instance.NicoSearchOrderby);

            Histories = NicoModel.SearchHistories
                .ToBindableSortedCollection(x => x.Date, true)
                .ToBindableConvertCollection(x => new NicoSearchHistoryViewModel(this, x))
                .ToBindableContextCollection();

            AddDisposed((sender, e) =>
            {
                NicoSetting.Instance.NicoSearchOrderby = Orderby.SelectedItem.Value;
                NicoSetting.Instance.Save();

                Orderby.Dispose();
                Videos.Dispose();
            });
        }

        public ComboboxViewModel Orderby { get; private set; }

        public string Word
        {
            get => _Word;
            set => SetProperty(ref _Word, value);
        }
        public string _Word;

        public BindableCollection<NicoVideoModel> Sources { get; private set; }

        public BindableContextCollection<NicoVideoViewModel> Videos { get; private set; }

        public BindableContextCollection<NicoSearchHistoryViewModel> Histories { get; private set; }

        public ICommand OnSearch => _OnSearch = _OnSearch ?? RelayCommand.Create<NicoSearchType>(async t =>
        {
            await NicoUtil.GetVideoBySearchType(Word, t, Orderby.SelectedItem.Value).ContinueWith(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });

            NicoModel.AddSearchHistory(Word, t);
        });
        private ICommand _OnSearch;
    
        public void NicoSearchHistoryOnDoubleClick(NicoSearchHistoryViewModel vm)
        {
            Word = vm.Word;
            OnSearch.Execute(vm.Type);
        }

        public void NicoSearchHistoryOnDelete(NicoSearchHistoryViewModel vm)
        {
            NicoModel.DelSearchHistory(vm.Word, vm.Type);
        }
    }
}
