﻿using Moviewer.Core.Windows;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using TBird.Wpf;
using TBird.Wpf.Collections;

namespace Moviewer.Core.Controls
{
    public abstract class VideoModel : ControlModel, IThumbnailUrl
    {
        public VideoModel()
        {
            Tags = new BindableCollection<string>();
            Counters = new BindableCollection<CounterModel>();

            AddDisposed((sender, e) =>
            {
                Tags.Dispose();
                Counters.Dispose();
            });
        }

        public abstract MenuMode Mode { get; }

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

        public string ThumbnailUrl
        {
            get => _ThumbnailUrl;
            set => SetProperty(ref _ThumbnailUrl, value);
        }
        private string _ThumbnailUrl;

        public BindableCollection<CounterModel> Counters
        {
            get => _Counters;
            set => SetProperty(ref _Counters, value);
        }
        private BindableCollection<CounterModel> _Counters = null;

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

        public TimeSpan Duration
        {
            get => _Duration;
            set => SetProperty(ref _Duration, value);
        }
        private TimeSpan _Duration;

        public UserModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private UserModel _UserInfo;

        public BindableCollection<string> Tags
        {
            get => _Tags;
            set => SetProperty(ref _Tags, value);
        }
        private BindableCollection<string> _Tags = null;

        public VideoStatus Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }
        private VideoStatus _Status = VideoStatus.None;

        public virtual void RefreshStatus()
        {
            var temporary = VideoUtil.Temporaries.GetModel(this);
            var history = VideoUtil.Histories.GetModel(this);

            // Temporaryの有無でﾌﾟﾛﾊﾟﾃｨを変更
            if (temporary != null) TempTime = temporary.Date;

            Status = history != null
                ? VideoStatus.See
                : temporary != null && MainViewModel.Instance.StartupTime < temporary.Date
                ? VideoStatus.New
                : temporary != null
                ? VideoStatus.Temporary
                : VideoStatus.None;
        }
    }
}