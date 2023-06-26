using Moviewer.Nico.Core;
using Moviewer.Nico.Workspaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TBird.Core;
using TBird.Wpf;
using TBird.Wpf.Controls;

namespace Moviewer.Core.Windows
{
    public class MainViewModel : MainViewModelBase
    {
        public static MainViewModel Instance { get; private set; }

        public MainViewModel()
        {
            if (Instance != null) throw new ApplicationException();

            Instance = this;

            Loaded.Add(DoLoading);

            Closing.Add(DoClosing);
        }

        public DateTime StartupTime { get; } = DateTime.Now;

        /// <summary>
        /// ｶﾚﾝﾄﾜｰｸｽﾍﾟｰｽ
        /// </summary>
        public WorkspaceViewModel Current
        {
            get => _Current;
            set => SetProperty(ref _Current, value, true);
        }
        private WorkspaceViewModel _Current;

        public MenuMode MenuMode
        {
            get => _MenuMode;
            set => SetProperty(ref _MenuMode, value);
        }
        private MenuMode _MenuMode = MenuMode.Niconico;

        public int NicoTemporaryCount => NicoModel.Temporaries.Count;

        /// <summary>
        /// お気に入り巡回ﾀｲﾏｰ
        /// </summary>
        public IntervalTimer FavoriteChecker { get; private set; }

        private async Task PatrolFavorites()
        {
            foreach (var m in NicoModel.SearchFavorites)
            {
                var arr = await NicoUtil.GetVideoBySearchType(m.Word, m.Type, "regdate-")
                    .ContinueWith(x => x.Result.Where(y => m.Date < y.StartTime));

                if (!arr.Any()) return;

                m.Date = arr.Max(x => x.StartTime);

                arr.ForEach(x => NicoModel.AddTemporary(x.ContentId));
            }
        }

        //public ObservableCollection<DownloadModel> Downloads
        //{
        //    get => _Downloads;
        //    set => SetProperty(ref _Downloads, value);
        //}
        //private ObservableCollection<DownloadModel> _Downloads;

        private void DoLoading()
        {
            NicoUtil.Initialize();

            AddCollectionChanged(NicoModel.Temporaries, (sender, e) =>
            {
                OnPropertyChanged(nameof(NicoTemporaryCount));
            });

            // お気に入り巡回ﾀｲﾏｰの起動
            FavoriteChecker = new IntervalTimer(PatrolFavorites);
            FavoriteChecker.Interval = TimeSpan.FromMinutes(10);
            FavoriteChecker.Start();

            OnClickMenu.Execute(MenuType.NicoRanking);
        }

        private bool DoClosing()
        {
            if (FavoriteChecker != null)
            {
                FavoriteChecker.Stop();
                FavoriteChecker.Dispose();
            }

            NicoModel.Save();

            return true;
        }

        public ICommand OnClickMode => _OnClickMode = _OnClickMode ?? RelayCommand.Create<MenuMode>(mode =>
        {
            MenuMode = mode;
        });
        private ICommand _OnClickMode;

        public ICommand OnClickMenu => _OnClickMenu = _OnClickMenu ?? RelayCommand.Create<MenuType>(menu =>
        {
            var newtype = _menu[menu];
            if (Current != null && newtype == Current.GetType())
            {
                return;
            }
            else
            {
                Current = Activator.CreateInstance(newtype) as WorkspaceViewModel;
            }

        });
        private ICommand _OnClickMenu;

        private Dictionary<MenuType, Type> _menu = new Dictionary<MenuType, Type>()
        {
            [MenuType.NicoRanking] = typeof(NicoRankingViewModel),
            [MenuType.NicoTemporary] = typeof(NicoTemporaryViewModel),
            [MenuType.NicoFavorite] = typeof(NicoFavoriteViewModel),
            //[MenuType.NicoHistory] = typeof(NicoHistoryViewModel),
            [MenuType.NicoSearch] = typeof(NicoSearchViewModel),
        };

    }
}