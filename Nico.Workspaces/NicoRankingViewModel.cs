using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;
using TBird.Core;
using TBird.Core.Stateful;
using System.Threading;
using Moviewer.Core;
using TBird.Wpf.Collections;

namespace Moviewer.Nico.Workspaces
{
    public class NicoRankingViewModel : WorkspaceViewModel
    {
        public override MenuType Type => MenuType.NicoRanking;

        public NicoRankingViewModel()
        {
            Sources = new BindableCollection<NicoVideoModel>();

            Videos = Sources
                .ToBindableConvertCollection(x => new NicoVideoViewModel(this, x))
                .ToBindableContextCollection();

            Genre = new ComboboxViewModel(NicoUtil.GetCombos("rank_genre"));
            Genre.SelectedItem = Genre.GetItemNotNull(NicoSetting.Instance.NicoRankingGenre);
            Genre.AddOnPropertyChanged(this, Reload, nameof(Genre.SelectedItem), false);

            Period = new ComboboxViewModel(NicoUtil.GetCombos("rank_period"));
            Period.SelectedItem = Period.GetItemNotNull(NicoSetting.Instance.NicoRankingPeriod);
            Period.AddOnPropertyChanged(this, Reload, nameof(Period.SelectedItem), true);

            AddDisposed((sender, e) =>
            {
                NicoSetting.Instance.NicoRankingGenre = Genre.SelectedItem.Value;
                NicoSetting.Instance.NicoRankingPeriod = Period.SelectedItem.Value;
                NicoSetting.Instance.Save();

                Genre.Dispose();
                Period.Dispose();
                Videos.Dispose();
            });
        }

        public ComboboxViewModel Genre { get; private set; }

        public ComboboxViewModel Period { get; private set; }

        //public ObservableSynchronizedCollection<NicoVideoModel> Sources { get; private set; }

        //public SynchronizationContextCollection<NicoVideoViewModel> Videos { get; private set; }

        public BindableCollection<NicoVideoModel> Sources { get; private set; }

        public BindableContextCollection<NicoVideoViewModel> Videos { get; private set; }

        private async void Reload(object sender, PropertyChangedEventArgs e)
        {
            await NicoUtil.GetVideosByRanking(Genre.SelectedItem.Value, "all", Period.SelectedItem.Value).ContinueWith(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });
        }
    }
}
