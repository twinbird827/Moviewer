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
            RegistDate = DateTime.Now;
            UpdateDate = DateTime.MinValue;
        }

        public string ContentId
        {
            get => _ContentId;
            set => SetProperty(ref _ContentId, value);
        }
        private string _ContentId = null;

        public DateTime RegistDate
        {
            get => _RegistDate;
            set => SetProperty(ref _RegistDate, value);
        }
        private DateTime _RegistDate;

        public DateTime UpdateDate
        {
            get => _UpdateDate;
            set => SetProperty(ref _UpdateDate, value);
        }
        private DateTime _UpdateDate;

    }
}
