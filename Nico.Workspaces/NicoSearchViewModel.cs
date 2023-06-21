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

namespace Moviewer.Nico.Workspaces
{
    public class NicoSearchViewModel : WorkspaceViewModel
    {
        public NicoSearchViewModel()
        {
            Sources = new ObservableSynchronizedCollection<NicoVideoModel>();

            Videos = Sources.ToSyncedSynchronizationContextCollection(
                x => new NicoVideoViewModel(this, x),
                WpfUtil.GetContext()
            );

            Orderby = new ComboboxViewModel(NicoUtil.GetCombos("order_by"));
            Orderby.SelectedItem = Orderby.GetItemNotNull(NicoSetting.Instance.NicoSearchOrderby);

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

        public ICommand OnSearch => _OnSearch = _OnSearch ?? RelayCommand.Create<NicoSearchType>(async t =>
        {
            await GetSources(t).ContinueOnUI(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });
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
                //case NicoSearchType.Word:
                default:
                    return NicoUtil.GetVideosByWord(Word, Orderby.SelectedItem.Value);
            }
        }
    }
}
