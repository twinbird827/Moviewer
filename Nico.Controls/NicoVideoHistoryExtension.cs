using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviewer.Nico.Controls
{
    public static class NicoVideoHistoryExtension
    {
        public static NicoVideoModel GetVideo(this VideoHistoryModel m)
        {
            var video = new NicoVideoModel(m.ContentId);

            m.AddOnPropertyChanged(video, (sender, e) =>
            {
                video.TempTime = m.Date;
                video.RefreshStatus();
            }, nameof(m.Date), true);

            video.AddOnPropertyChanged(m, (sender, e) =>
            {
                m.Date = video.TempTime;
                video.RefreshStatus();
            }, nameof(video.TempTime), false);

            return video;
        }
    }
}