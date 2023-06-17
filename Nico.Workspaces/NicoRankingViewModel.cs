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

namespace Moviewer.Nico.Workspaces
{
    public class NicoRankingViewModel : WorkspaceViewModel
    {
        public NicoRankingViewModel()
        {
            Videos = new ObservableCollection<NicoVideoViewModel>();

            Genre = new ComboboxViewModel(
                NicoUtil.Combos.Where(x => x.Group == "rank_genre").SelectMany(x => x.Items)
            );
            Genre.SelectedItem = Genre.GetItemNotNull(NicoSetting.Instance.NicoRankingGenre);
            Genre.AddOnPropertyChanged(this, Reload, nameof(Genre.SelectedItem), false);

            Period = new ComboboxViewModel(
                NicoUtil.Combos.Where(x => x.Group == "rank_period").SelectMany(x => x.Items)
            );
            Period.SelectedItem = Period.GetItemNotNull(NicoSetting.Instance.NicoRankingPeriod);
            Period.AddOnPropertyChanged(this, Reload, nameof(Period.SelectedItem), true);

            AddDisposed((sender, e) =>
            {
                NicoSetting.Instance.NicoRankingGenre = Genre.SelectedItem.Value;
                NicoSetting.Instance.NicoRankingPeriod = Period.SelectedItem.Value;
                NicoSetting.Instance.Save();
            });
        }

        public ComboboxViewModel Genre { get; private set; }

        public ComboboxViewModel Period { get; private set; }

        public ObservableCollection<NicoVideoViewModel> Videos { get; private set; }

        private async void Reload(object sender, PropertyChangedEventArgs e)
        {
            var videos = await NicoUtil.GetVideosByRanking(Genre.SelectedItem.Value, "all", Period.SelectedItem.Value);

            WpfUtil.ExecuteOnUI(() =>
            {
                Videos.Clear();
                Videos.AddRange(videos.Select(x => new NicoVideoViewModel(x)));
            });
        }
    }
}
