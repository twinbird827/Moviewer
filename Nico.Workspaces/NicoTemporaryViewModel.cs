using Microsoft.WindowsAPICodePack.Win32Native.NamedPipe;
using Moviewer.Core;
using Moviewer.Core.Controls;
using Moviewer.Core.Windows;
using Moviewer.Nico.Controls;
using Moviewer.Nico.Core;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TBird.Wpf;
using TBird.Wpf.Collections;
using TBird.Wpf.Controls;

namespace Moviewer.Nico.Workspaces
{
    public class NicoTemporaryViewModel : WorkspaceViewModel, IVideoParentViewModel
    {
        public override MenuType Type => MenuType.NicoTemporary;

        public NicoTemporaryViewModel()
        {
            VideoType = new ComboboxViewModel(ComboUtil.GetNicos("video_type"));
            VideoType.SelectedItem = null;

            Sources = VideoUtil.Temporaries
                .ToBindableWhereCollection(x => x.Mode == MenuMode.Niconico)
                .ToBindableSelectCollection(NicoVideoModel.FromHistory)
                .ToBindableSelectCollection(x => new NicoVideoViewModel(this, x));

            Users = Sources
                .ToBindableSelectCollection(x => x.UserInfo)
                .ToBindableDistinctCollection(x => x.Userid, nameof(UserViewModel.Userid))
                .ToBindableContextCollection();

            Videos = Sources
                .ToBindableWhereCollection(x => 
                    (SelectedUser == null || x.UserInfo.Userid == SelectedUser.Userid) &&
                    (VideoType.SelectedItem == null || x.ContentId.StartsWith(VideoType.SelectedItem.Value))
                )
                .AddOnRefreshCollection(this, nameof(SelectedUser))
                .AddOnRefreshCollection(VideoType, nameof(VideoType.SelectedItem))
                .ToBindableSortedCollection(x => x.TempTime, true)
                .ToBindableContextCollection();

            AddDisposed((sender, e) =>
            {
                Videos.Dispose();
                Users.Dispose();
                Sources.Dispose();
            });
        }

        public ComboboxViewModel VideoType { get; private set; }

        public ICommand OnDeleteVideoType => _OnDeleteVideoType = _OnDeleteVideoType ?? RelayCommand.Create(_ =>
        {
            VideoType.SelectedItem = null;
        });
        private ICommand _OnDeleteVideoType;

        public BindableChildCollection<UserViewModel> Users
        {
            get => _Users;
            set => SetProperty(ref _Users, value);
        }
        public BindableChildCollection<UserViewModel> _Users;

        public UserViewModel SelectedUser
        {
            get => _SelectedUser;
            set => SetProperty(ref _SelectedUser, value);
        }
        public UserViewModel _SelectedUser;

        public BindableCollection<NicoVideoViewModel> Sources
        {
            get => _Sources;
            set => SetProperty(ref _Sources, value);
        }
        public BindableCollection<NicoVideoViewModel> _Sources;

        public BindableContextCollection<NicoVideoViewModel> Videos
        {
            get => _Videos;
            set => SetProperty(ref _Videos, value);
        }
        public BindableContextCollection<NicoVideoViewModel> _Videos;

        public ICommand OnDeleteSelectedUser => _OnDeleteSelectedUser = _OnDeleteSelectedUser ?? RelayCommand.Create(_ =>
        {
            SelectedUser = null;
        });
        private ICommand _OnDeleteSelectedUser;

        public ICommand OnTemporaryAdd => _OnTemporaryAdd = _OnTemporaryAdd ?? RelayCommand.Create(async _ =>
        {
            using (var vm = new WpfMessageInputViewModel(AppConst.L_AddTemporary, AppConst.M_AddTemporary, AppConst.L_UrlOrId, true))
            {
                if (!vm.ShowDialog(() => new WpfMessageInputWindow()))
                {
                    return;
                }

                await AddTemporary(vm.Value);
            }
        });
        private ICommand _OnTemporaryAdd;

        protected override async void OnDropProcess(string droptxt)
        {
            await AddTemporary(droptxt);
        }

        private async Task AddTemporary(string url)
        {
            var video = await NicoUtil.GetVideo(NicoUtil.Url2Id(url));
            if (video.Status != VideoStatus.Delete)
            {
                VideoUtil.AddTemporary(MenuMode.Niconico, video.ContentId);
            }
        }

        public void DeleteOnVideo(VideoViewModel vm)
        {
            VideoUtil.DelTemporary(MenuMode.Niconico, vm.Source.ContentId);
        }
    }
}