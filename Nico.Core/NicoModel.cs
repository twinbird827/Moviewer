using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoModel : BindableBase
    {
        public NicoModel()
        {
            Temporaries = new ObservableCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Temporaries);

            Histories = new ObservableCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Histories);
        }

        public void Save()
        {
            NicoSetting.Instance.Temporaries = Temporaries.ToArray();
            NicoSetting.Instance.Histories = Histories.ToArray();
            NicoSetting.Instance.Save();
        }

        // **************************************************
        // Temporaries

        public ObservableCollection<NicoVideoHistoryModel> Temporaries { get; private set; }

        public void AddTemporary(string contentid)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                tmp.RegistDate = DateTime.Now;
            }
            else
            {
                Temporaries.Add(new NicoVideoHistoryModel(contentid));
            }
        }

        public void DelTemporary(string contentid)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                Temporaries.Remove(tmp);
            }
        }

        // **************************************************
        // Histories

        public ObservableCollection<NicoVideoHistoryModel> Histories { get; private set; }

        public void AddHistory(string contentid)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                tmp.RegistDate = DateTime.Now;
            }
            else
            {
                Histories.Add(new NicoVideoHistoryModel(contentid));
            }
        }

        public void DelHistory(string contentid)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                Histories.Remove(tmp);
            }
        }

    }
}
