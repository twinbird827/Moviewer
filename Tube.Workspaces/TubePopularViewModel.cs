using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using Moviewer.Tube.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;
using TBird.Wpf.Collections;
using TBird.Core;
using Moviewer.Tube.Controls;

namespace Moviewer.Tube.Workspaces
{
    public class TubePopularViewModel : WorkspaceViewModel
    {
        public override MenuType Type => MenuType.TubePopular;

        public TubePopularViewModel()
        {
            Sources = new BindableCollection<TubeVideoModel>();

            Videos = Sources
                .ToBindableSelectCollection(x => new TubeVideoViewModel(this, x))
                .ToBindableContextCollection();

            Categories = new ComboboxViewModel(ComboUtil.GetTubes("popular_category"));
            Categories.SelectedItem = Categories.GetItemNotNull(TubeSetting.Instance.TubePopularCategory);
            Categories.AddOnPropertyChanged(this, Reload, nameof(Categories.SelectedItem), true);

            AddDisposed((sender, e) =>
            {
                TubeSetting.Instance.TubePopularCategory = Categories.SelectedItem.Value;
                TubeSetting.Instance.Save();

                Categories.Dispose();
                Videos.Dispose();
                Sources.Dispose();
            });

        }

        public ComboboxViewModel Categories { get; private set; }

        public BindableCollection<TubeVideoModel> Sources { get; private set; }

        public BindableContextCollection<TubeVideoViewModel> Videos { get; private set; }

        private async void Reload(object sender, PropertyChangedEventArgs e)
        {
            await TubeUtil.GetVideosByPopular(Categories.SelectedItem.Value).ContinueWith(x =>
            {
                Sources.Clear();
                Sources.AddRange(x.Result);
            });
        }

    }
}