using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System.Windows;
using System.Windows.Input;
using TBird.Wpf;
using TBird.Wpf.Collections;
using TBird.Wpf.Controls;

namespace Moviewer.Nico.Workspaces
{
    public class NicoTemporaryViewModel : WorkspaceViewModel, INicoVideoParentViewModel
    {
        public override MenuType Type => MenuType.NicoTemporary;

        public NicoTemporaryViewModel()
        {
            Sources = NicoModel.Temporaries
                .ToBindableSelectCollection(x => x.GetVideo())
                .ToBindableSelectCollection(x => new NicoVideoViewModel(this, x));

            Users = Sources
                .ToBindableSelectCollection(x => x.UserInfo)
                .ToBindableDistinctCollection(x => x.Userid, "Userid")
                .ToBindableContextCollection();

            Videos = Sources
                .ToBindableWhereCollection(x => SelectedUser == null || x.UserInfo.Userid == SelectedUser.Userid)
                .AddOnRefreshCollection(this, nameof(SelectedUser))
                .ToBindableSortedCollection(x => x.TempTime, true)
                .ToBindableContextCollection();

            AddDisposed((sender, e) =>
            {
                Videos.Dispose();
                Users.Dispose();
                Sources.Dispose();
            });
        }

        public BindableChildCollection<NicoUserViewModel> Users
        {
            get => _Users;
            set => SetProperty(ref _Users, value);
        }
        public BindableChildCollection<NicoUserViewModel> _Users;

        public NicoUserViewModel SelectedUser
        {
            get => _SelectedUser;
            set => SetProperty(ref _SelectedUser, value);
        }
        public NicoUserViewModel _SelectedUser;

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
            using (var vm = new WpfMessageInputViewModel(AppConst.L_AddTemporary, AppConst.MH_AddTemporary, AppConst.L_UrlOrId, true))
            {
                if (!(bool)vm.ShowDialog(() => new WpfMessageInputWindow()))
                {
                    return;
                }

                var video = await NicoUtil.GetVideo(VideoUtil.Url2Id(vm.Value));
                if (video.Status != VideoStatus.Delete)
                {
                    NicoModel.AddTemporary(video.ContentId);
                }
            }
        });
        private ICommand _OnTemporaryAdd;

        public ICommand OnDrop => _OnDrop = _OnDrop ?? RelayCommand.Create<DragEventArgs>(async e =>
        {
            if (e.Data.GetData(DataFormats.Text) is string url)
            {
                var video = await NicoUtil.GetVideo(VideoUtil.Url2Id(url));
                if (video.Status != VideoStatus.Delete)
                {
                    NicoModel.AddTemporary(video.ContentId);
                }
            }
        });
        private ICommand _OnDrop;

        public void NicoVideoOnDelete(NicoVideoViewModel vm)
        {
            NicoModel.DelTemporary(vm.Source.ContentId);
        }
    }
}