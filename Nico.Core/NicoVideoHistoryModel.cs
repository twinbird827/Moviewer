using System;
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
    }
}