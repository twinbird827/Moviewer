using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;

namespace Moviewer.Tube.Core
{
    public class TubeVideoHistoryModel : BindableBase
    {
        public TubeVideoHistoryModel()
        {

        }

        public TubeVideoHistoryModel(string contentid)
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
    }
}