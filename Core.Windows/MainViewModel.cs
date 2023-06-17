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

        public MenuMode MovieType
        {
            get => _MovieType;
            set => SetProperty(ref _MovieType, value);
        }
        private MenuMode _MovieType = MenuMode.Niconico;

        public string NicoTemporaryString => $"Temp (0)";

        /// <summary>
        /// ﾛｸﾞｵﾌﾀｲﾏｰ
        /// </summary>
        public DispatcherTimer CheckTimer { get; private set; }

        //public ObservableCollection<DownloadModel> Downloads
        //{
        //    get => _Downloads;
        //    set => SetProperty(ref _Downloads, value);
        //}
        //private ObservableCollection<DownloadModel> _Downloads;

        private void DoLoading()
        {
            //// 自動ﾀｲﾏｰ起動
            //CheckTimer = new DispatcherTimer();
            //CheckTimer.Interval = TimeSpan.FromMinutes(5);
            //CheckTimer.Tick += async (sender, e) =>
            //{
            //    await NicoUtil.PatrolFavorites();
            //};
            //CheckTimer.Start();

            //// TemporaryStringの更新ｲﾍﾞﾝﾄ関連付け
            //NicoUtil.Temporaries.CollectionChanged += (sender, e) =>
            //{
            //    OnPropertyChanged(nameof(NicoTemporaryString));
            //};
            //OnPropertyChanged(nameof(NicoTemporaryString));

            OnClickMenu.Execute(MenuType.NicoRanking);
        }

        private bool DoClosing()
        {
            //// TODO 設定ﾌｧｲﾙを閉じる
            //CheckTimer.Stop();

            //AppSetting.Instance.Save();

            return true;
        }

        public ICommand OnClickNiconico => _OnClickNiconico = _OnClickNiconico ?? RelayCommand.Create(_ =>
        {
            MovieType = MenuMode.Youtube;
        });
        private ICommand _OnClickNiconico;

        public ICommand OnClickYoutube => _OnClickYoutube = _OnClickYoutube ?? RelayCommand.Create(_ =>
        {
            MovieType = MenuMode.Niconico;
        });
        private ICommand _OnClickYoutube;

        public ICommand OnClickMenu => _OnClickMenu = _OnClickMenu ?? RelayCommand.Create<MenuType>(menu =>
        {
            MessageService.Info(menu.GetLabel());

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
            //[MenuType.NicoTemporary] = typeof(NicoTemporaryViewModel),
            //[MenuType.NicoFavorite] = typeof(NicoFavoriteViewModel),
            //[MenuType.NicoHistory] = typeof(NicoHistoryViewModel),
            //[MenuType.NicoSearch] = typeof(NicoSearchViewModel),
        };

    }
}