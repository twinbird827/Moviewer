using Moviewer.Core;
using Moviewer.Core.Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoVideoViewModel : NicoViewModel
    {
        public NicoVideoViewModel(WorkspaceViewModel parent, NicoVideoModel m)
        {
            Parent = parent;
            Source = m;

            VideoUrl = $"http://nico.ms/{m.ContentId}";

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Title = m.Title;
            }, nameof(m.Title), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Description = m.Description;
            }, nameof(m.Description), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                ViewCounter = m.ViewCounter;
            }, nameof(m.ViewCounter), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistCounter = m.MylistCounter;
            }, nameof(m.MylistCounter), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                CommentCounter = m.CommentCounter;
            }, nameof(m.CommentCounter), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                StartTime = m.StartTime;
            }, nameof(m.StartTime), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                TempTime = m.TempTime;
                ExistTempTime = TempTime != default(DateTime);
            }, nameof(m.TempTime), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                LengthToSeconds = TimeSpan.FromSeconds(m.LengthSeconds);
            }, nameof(m.LengthSeconds), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                if (m.UserInfo != null) UserInfo = new NicoUserViewModel(m.UserInfo);
            }, nameof(m.UserInfo), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Status = m.Status;
            }, nameof(m.Status), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                if (m.Tags == null) return;

                Tags = new ObservableCollection<NicoVideoTagViewModel>(
                    m.Tags.Split(' ').Select(x => new NicoVideoTagViewModel(x))
                );
            }, nameof(m.Tags), true);

            AddDisposed((sender, e) =>
            {
                UserInfo.TryDispose();

                if (Tags != null)
                {
                    Tags.ForEach(x => x.Dispose());
                    Tags.Clear();
                }

                Loaded.TryDispose();
                OnDoubleClick.TryDispose();
                OnDownload.TryDispose();
                OnKeyDown.TryDispose();
                OnTemporaryAdd.TryDispose();
                OnTemporaryDel.TryDispose();

                Source = null;
                Parent = null;
            });
        }

        public WorkspaceViewModel Parent { get; private set; }

        public NicoVideoModel Source { get; private set; }

        public string VideoUrl
        {
            get => _VideoUrl;
            set => SetProperty(ref _VideoUrl, value);
        }
        private string _VideoUrl;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        private string _Title;

        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }
        private string _Description;

        public BitmapImage Thumbnail
        {
            get => _Thumbnail;
            set => SetProperty(ref _Thumbnail, value);
        }
        private BitmapImage _Thumbnail;

        public long ViewCounter
        {
            get => _ViewCounter;
            set => SetProperty(ref _ViewCounter, value);
        }
        private long _ViewCounter;

        public long MylistCounter
        {
            get => _MylistCounter;
            set => SetProperty(ref _MylistCounter, value);
        }
        private long _MylistCounter;

        public long CommentCounter
        {
            get => _CommentCounter;
            set => SetProperty(ref _CommentCounter, value);
        }
        private long _CommentCounter;

        public DateTime StartTime
        {
            get => _StartTime;
            set => SetProperty(ref _StartTime, value);
        }
        private DateTime _StartTime;

        public DateTime TempTime
        {
            get => _TempTime;
            set => SetProperty(ref _TempTime, value);
        }
        private DateTime _TempTime;

        public bool ExistTempTime
        {
            get => _ExistTempTime;
            set => SetProperty(ref _ExistTempTime, value);
        }
        private bool _ExistTempTime;

        public TimeSpan LengthToSeconds
        {
            get => _LengthToSeconds;
            private set => SetProperty(ref _LengthToSeconds, value);
        }
        private TimeSpan _LengthToSeconds;

        public NicoUserViewModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private NicoUserViewModel _UserInfo;

        public VideoStatus Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }
        private VideoStatus _Status = VideoStatus.None;

        public ObservableCollection<NicoVideoTagViewModel> Tags
        {
            get => _Tags;
            set => SetProperty(ref _Tags, value);
        }
        private ObservableCollection<NicoVideoTagViewModel> _Tags = null;

        public ICommand OnDoubleClick => _OnDoubleClick = _OnDoubleClick ?? RelayCommand.Create(_ =>
        {
            // TODO 子画面出して追加するかどうかを決めたい
            // TODO ﾘﾝｸも抽出したい
            // TODO smだけじゃなくてsoとかも抽出したい
            foreach (var videoid in Regex.Matches(Description, @"(?<id>sm[\d]+)").OfType<Match>()
                    .Select(m => m.Groups["id"].Value)
                    .Where(tmp => !NicoModel.Histories.Any(x => x.ContentId == tmp))
                )
            {
                NicoModel.AddTemporary(videoid);
            }

            // 視聴ﾘｽﾄに追加
            NicoModel.AddHistory(Source.ContentId);

            // ｽﾃｰﾀｽ更新
            Source.RefreshStatus();

            // ﾌﾞﾗｳｻﾞ起動
            Process.Start(AppSetting.Instance.BrowserPath, VideoUrl);
        });
        private ICommand _OnDoubleClick;

        public ICommand OnKeyDown => _OnKeyDown = _OnKeyDown ?? RelayCommand.Create<KeyEventArgs>(e =>
        {
            if (e.Key == Key.Enter)
            {
                OnDoubleClick.Execute(null);
            }
            else if (e.Key == Key.Delete && Parent is INicoVideoParentViewModel parent)
            {
                parent.NicoVideoOnDelete(this);
            }
        });
        private ICommand _OnKeyDown;

        public ICommand OnTemporaryAdd => _OnTemporaryAdd = _OnTemporaryAdd ?? RelayCommand.Create(_ =>
        {
            NicoModel.AddTemporary(Source.ContentId);
            Source.RefreshStatus();
        });
        private ICommand _OnTemporaryAdd;

        public ICommand OnTemporaryDel => _OnTemporaryDel = _OnTemporaryDel ?? RelayCommand.Create(_ =>
        {
            NicoModel.DelTemporary(Source.ContentId);
            Source.RefreshStatus();
        });
        private ICommand _OnTemporaryDel;

        public ICommand OnDownload => _OnDownload = _OnDownload ?? RelayCommand.Create(_ =>
        {
            MessageService.Info("OnDownload");
            //            NicoModel.AddTemporary(Source.ContentId);
        });
        private ICommand _OnDownload;

        public override IRelayCommand Loaded => RelayCommand.Create(async async =>
        {
            await Source.RefreshProperties();

            if (Thumbnail != null) return;

            if (!string.IsNullOrEmpty(Source.ThumbnailUrl))
            {
                await SetThumnailAsync(Source.ThumbnailUrl);
            }
            else
            {
                Source.AddOnPropertyChanged(this, async (sender, e) =>
                {
                    await SetThumnailAsync(Source.ThumbnailUrl);
                }, nameof(Source.ThumbnailUrl), true);
            }
        });

        private async Task SetThumnailAsync(string url)
        {
            var urls = Arr(".L", ".M", "")
                .Select(x => $"{url}{x}")
                .ToArray();

            await VideoUtil
                .GetThumnailAsync(urls)
                .ContinueWith(x => Thumbnail = x.IsFaulted ? null : x.Result);
        }
    }
}