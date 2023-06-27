using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoVideoHistoryModel : BindableBase
    {
        public NicoVideoHistoryModel()
        {

        }

        public NicoVideoHistoryModel(string contentid)
        {
            ContentId = contentid;
            Date = DateTime.Now; ;
        }

        public string ContentId
        {
            get => _ContentId;
            set => SetProperty(ref _ContentId, value);
        }
        private string _ContentId = null;

        public DateTime Date
        {
            get => _Date;
            set => SetProperty(ref _Date, value);
        }
        private DateTime _Date;

        public NicoVideoModel GetVideo()
        {
            var video = new NicoVideoModel().SetFromContentId(ContentId);

            AddOnPropertyChanged(video, (sender, e) =>
            {
                video.TempTime = Date;
            }, nameof(Date), true);
            video.AddOnPropertyChanged(this, (sender, e) =>
            {
                Date = video.TempTime;
                video.RefreshStatus();
            }, nameof(video.TempTime), false);

            return video;
        }
    }
}
