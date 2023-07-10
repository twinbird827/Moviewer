using Microsoft.WindowsAPICodePack.PortableDevices.CommandSystem.Object;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using Moviewer.Core;
using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Tube.Core
{
    public class TubeVideoViewModel : TubeViewModel
    {
        public TubeVideoViewModel(TubeVideoModel m)
        {
            Source = m;

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                ContentId = m.ContentId;
                VideoUrl = $"http://nico.ms/{m.ContentId}";
            }, nameof(m.ContentId), true);

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
                LikeCounter = m.LikeCounter;
            }, nameof(m.LikeCounter), true);

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
                Duration = m.Duration;
            }, nameof(m.Duration), true);

            UserInfo = new TubeUserViewModel();
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                if (m.UserInfo != null) UserInfo.SetModel(m.UserInfo);
            }, nameof(m.UserInfo), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Status = m.Status;
            }, nameof(m.Status), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                if (m.Tags == null) return;

                Tags = new ObservableCollection<TubeTagViewModel>(
                    m.Tags.Select(x => new TubeTagViewModel(x))
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
            });
        }

        public TubeVideoModel Source { get; private set; }

        public string VideoUrl
        {
            get => _VideoUrl;
            set => SetProperty(ref _VideoUrl, value);
        }
        private string _VideoUrl;

        public string ContentId
        {
            get => _ContentId;
            set => SetProperty(ref _ContentId, value);
        }
        private string _ContentId;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, HttpUtility.HtmlDecode(value));
        }
        private string _Title;

        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, HttpUtility.HtmlDecode(value));
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

        public long LikeCounter
        {
            get => _LikeCounter;
            set => SetProperty(ref _LikeCounter, value);
        }
        private long _LikeCounter;

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

        public TimeSpan Duration
        {
            get => _Duration;
            private set => SetProperty(ref _Duration, value);
        }
        private TimeSpan _Duration;

        public TubeUserViewModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private TubeUserViewModel _UserInfo;

        public VideoStatus Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }
        private VideoStatus _Status = VideoStatus.None;

        public ObservableCollection<TubeTagViewModel> Tags
        {
            get => _Tags;
            set => SetProperty(ref _Tags, value);
        }
        private ObservableCollection<TubeTagViewModel> _Tags;

        public ICommand OnDoubleClick => _OnDoubleClick = _OnDoubleClick ?? RelayCommand.Create(_ =>
        {
            // TODO 子画面出して追加するかどうかを決めたい
            // TODO ﾘﾝｸも抽出したい
            // TODO smだけじゃなくてsoとかも抽出したい
            //foreach (var videoid in Regex.Matches(Description, @"(?<id>sm[\d]+)").OfType<Match>()
            //        .Select(m => m.Groups["id"].Value)
            //        .Where(tmp => !NicoModel.Histories.Any(x => x.ContentId == tmp))
            //    )
            //{
            //    NicoModel.AddTemporary(videoid);
            //}

            // 視聴ﾘｽﾄに追加
            NicoModel.AddHistory(ContentId);

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
            //else if (e.Key == Key.Delete && Parent is INicoVideoParentViewModel parent)
            //{
            //    parent.NicoVideoOnDelete(this);
            //}
        });
        private ICommand _OnKeyDown;

        public ICommand OnTemporaryAdd => _OnTemporaryAdd = _OnTemporaryAdd ?? RelayCommand.Create(_ =>
        {
            TubeModel.AddTemporary(Source.ContentId);
            Source.RefreshStatus();
        });
        private ICommand _OnTemporaryAdd;

        public ICommand OnTemporaryDel => _OnTemporaryDel = _OnTemporaryDel ?? RelayCommand.Create(_ =>
        {
            TubeModel.DelTemporary(Source.ContentId);
            Source.RefreshStatus();
        });
        private ICommand _OnTemporaryDel;

        public ICommand OnDownload => _OnDownload = _OnDownload ?? RelayCommand.Create(_ =>
        {
            //DownloadViewModel.Download(new NicoDownloadModel(Source));
        });
        private ICommand _OnDownload;

        public override IRelayCommand Loaded => RelayCommand.Create(async async =>
        {
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
            await VideoUtil
                .GetThumnailAsync(VideoUtil.Url2Id(VideoUrl), url)
                .ContinueWith(x => Thumbnail = x.IsFaulted ? null : x.Result);
        }

    }
}