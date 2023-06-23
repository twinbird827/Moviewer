﻿using Moviewer.Core;
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
            Sources = new ObservableSynchronizedCollection<NicoVideoModel>();

            Videos = Sources.ToSyncedSynchronizationContextCollection(
                x => new NicoVideoViewModel(this, x),
                WpfUtil.GetContext()
            );

            Orderby = new ComboboxViewModel(NicoUtil.GetCombos("order_by"));
            Orderby.SelectedItem = Orderby.GetItemNotNull(NicoSetting.Instance.NicoSearchOrderby);

            Histories = NicoModel.SearchHistories
                .ToSyncedSortedObservableCollection(x => x.Date, isDescending: true)
                .ToSyncedObservableSynchronizedCollection(x => new NicoSearchHistoryViewModel(this, x))
                .ToSyncedSynchronizationContextCollection(x => x, WpfUtil.GetContext());

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

        public ObservableSynchronizedCollection<NicoVideoModel> Sources { get; private set; }

        public SynchronizationContextCollection<NicoVideoViewModel> Videos { get; private set; }

        public SynchronizationContextCollection<NicoSearchHistoryViewModel> Histories { get; private set; }

        public ICommand OnSearch => _OnSearch = _OnSearch ?? RelayCommand.Create<NicoSearchType>(async t =>
        {
            await GetSources(t).ContinueOnUI(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });

            NicoModel.AddSearchHistory(Word, t);
        });
        private ICommand _OnSearch;
    
        private Task<IEnumerable<NicoVideoModel>> GetSources(NicoSearchType t)
        {
            switch (t)
            {
                case NicoSearchType.User:
                    return NicoUtil.GetVideosByNicouser(Word, Orderby.SelectedItem.Value);
                case NicoSearchType.Tag:
                    return NicoUtil.GetVideosByTag(Word, Orderby.SelectedItem.Value);
                case NicoSearchType.Mylist:
                    return NicoUtil.GetVideosByMylist(Word, NicoUtil.GetComboDisplay("oyder_by_mylist", Orderby.SelectedItem.Value));
                //case NicoSearchType.Word:
                default:
                    return NicoUtil.GetVideosByWord(Word, Orderby.SelectedItem.Value);
            }
        }

        public void NicoSearchHistoryDoubleClick(NicoSearchHistoryViewModel vm)
        {
            Word = vm.Word;
            OnSearch.Execute(vm.Type);
        }
    }
}
