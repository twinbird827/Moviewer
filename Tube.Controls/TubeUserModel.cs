using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moviewer.Tube.Core;
using TBird.Wpf;

namespace Moviewer.Tube.Controls
{
    public class TubeUserModel : BindableBase
    {
        public TubeUserModel()
        {

        }

        public TubeUserModel(string id, string title)
        {
            ChannelId = id;
            Title = title;
            if (TubeModel.GetUser(id) is TubeUserModel m)
            {
                ThumbnailUrl = m.ThumbnailUrl;
            };
        }

        public string ChannelId
        {
            get => _ChannelId;
            set => SetProperty(ref _ChannelId, value);
        }
        private string _ChannelId = null;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        private string _Title = null;

        public string ThumbnailUrl
        {
            get => _ThumbnailUrl;
            set => SetProperty(ref _ThumbnailUrl, value);
        }
        private string _ThumbnailUrl;

    }
}
