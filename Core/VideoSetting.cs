using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;

namespace Moviewer.Core
{
    public class VideoSetting : JsonBase<VideoSetting>
    {
        private const string _path = @"lib\video-setting.json";

        public static VideoSetting Instance
        {
            get => _Instance = _Instance ?? new VideoSetting();
        }
        private static VideoSetting _Instance;

        public VideoSetting() : base(_path)
        {
            if (!Load())
            {
                Histories = NicoSetting.Instance.Histories
                    .Select(x => new VideoHistoryModel(MenuMode.Niconico, x.ContentId) { Date = x.Date })
                    .ToArray();
                Temporaries = NicoSetting.Instance.Temporaries
                    .Select(x => new VideoHistoryModel(MenuMode.Niconico, x.ContentId) { Date = x.Date })
                    .ToArray();
            }
        }

        public VideoHistoryModel[] Histories
        {
            get => GetProperty(_Histories);
            set => SetProperty(ref _Histories, value);
        }
        private VideoHistoryModel[] _Histories;

        public VideoHistoryModel[] Temporaries
        {
            get => GetProperty(_Temporaries);
            set => SetProperty(ref _Temporaries, value);
        }
        private VideoHistoryModel[] _Temporaries;

    }
}