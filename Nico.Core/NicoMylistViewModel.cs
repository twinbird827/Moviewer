using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoMylistViewModel : BindableBase
    {
        public NicoMylistViewModel(NicoMylistModel m)
        {
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistId = m.MylistId;
            }, nameof(MylistId), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistTitle = m.MylistTitle;
            }, nameof(MylistTitle), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistDescription = m.MylistDescription;
            }, nameof(MylistDescription), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                MylistDate = m.MylistDate;
            }, nameof(MylistDate), true);
            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                UserInfo = new NicoUserViewModel(m.UserInfo);
            }, nameof(UserInfo), true);

        }

        public string MylistId
        {
            get => _MylistId;
            set => SetProperty(ref _MylistId, value);
        }
        private string _MylistId;

        public string MylistTitle
        {
            get => _MylistTitle;
            set => SetProperty(ref _MylistTitle, value);
        }
        private string _MylistTitle;

        public string MylistDescription
        {
            get => _MylistDescription;
            set => SetProperty(ref _MylistDescription, value);
        }
        private string _MylistDescription = null;

        public DateTime MylistDate
        {
            get => _MylistDate;
            set => SetProperty(ref _MylistDate, value);
        }
        private DateTime _MylistDate;

        public NicoUserViewModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private NicoUserViewModel _UserInfo;

    }
}
