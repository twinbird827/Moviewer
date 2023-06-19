using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core.Stateful;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public static class NicoModel
    {
        public static void Save()
        {
            Private.Instance.Save();
        }

        // **************************************************
        // Temporaries

        public static SynchronizationContextCollection<NicoVideoModel> Temporaries
        {
            get => _Temporaries = _Temporaries ?? Private.Instance.Temporaries.ToSyncedSynchronizationContextCollection(
                x => x.GetVideo(),
                WpfUtil.GetContext()
            );
        }
        public static SynchronizationContextCollection<NicoVideoModel> _Temporaries;

        public static void AddTemporary(string contentid)
        {
            Private.Instance.AddTemporary(contentid);
        }

        public static void DelTemporary(string contentid)
        {
            Private.Instance.DelTemporary(contentid);
        }

        // **************************************************
        // Histories

        public static ObservableCollection<NicoVideoHistoryModel> Histories => Private.Instance.Histories;

        public static void AddHistory(string contentid)
        {
            Private.Instance.AddHistory(contentid);
        }

        public static void DelHistory(string contentid)
        {
            Private.Instance.DelHistory(contentid);
        }

        public class Private : BindableBase
        {
            private Private()
            {
                Temporaries = new ObservableSynchronizedCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Temporaries);

                Histories = new ObservableCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Histories);
            }

            public static Private Instance
            {
                get => _Instance = _Instance ?? new Private();
            }
            private static Private _Instance;

            public void Save()
            {
                NicoSetting.Instance.Temporaries = Temporaries.ToArray();
                NicoSetting.Instance.Histories = Histories.ToArray();
                NicoSetting.Instance.Save();
            }

            // **************************************************
            // Temporaries

            public ObservableSynchronizedCollection<NicoVideoHistoryModel> Temporaries { get; private set; }

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
}
