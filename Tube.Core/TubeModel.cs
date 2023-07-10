using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf.Collections;

namespace Moviewer.Tube.Core
{
    public class TubeModel
    {
        private TubeModel()
        {

        }

        private static TubeModel Instance
        {
            get => _Instance = _Instance ?? new TubeModel();
        }
        private static TubeModel _Instance;

        public static void Save()
        {
            TubeSetting.Instance.Users = Users.ToArray();
            TubeSetting.Instance.Save();
        }

        // **************************************************
        // Users

        public static BindableCollection<TubeUserModel> Users
        {
            get => Instance._Users = Instance._Users ?? new BindableCollection<TubeUserModel>(TubeSetting.Instance.Users);
        }
        private BindableCollection<TubeUserModel> _Users;

        public static TubeUserModel GetUser(string id)
        {
            return Users.FirstOrDefault(x => x.ChannelId == id);
        }

        public static void AddUser(TubeUserModel m, bool issave = true)
        {
            var tmp = Users.FirstOrDefault(x => x == m);
            if (tmp == null)
            {
                Users.Add(m);
            }
            if (issave) Save();
        }

        public static void DelUser(TubeUserModel m, bool issave = true)
        {
            var tmp = Users.FirstOrDefault(x => x == m);
            if (tmp != null)
            {
                Users.Remove(tmp);
                if (issave) Save();
            }
        }

        // **************************************************
        // Temporaries

        public static BindableCollection<TubeVideoHistoryModel> Temporaries
        {
            get => Instance._Temporaries = Instance._Temporaries ?? new BindableCollection<TubeVideoHistoryModel>(TubeSetting.Instance.Temporaries);
        }
        private BindableCollection<TubeVideoHistoryModel> _Temporaries;

        public static void AddTemporary(string id, bool issave = true)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == id);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            if (tmp == null)
            {
                Temporaries.Add(new TubeVideoHistoryModel(id));
            }
            if (issave) Save();
        }

        public static void DelTemporary(string id, bool issave = true)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == id);
            if (tmp != null)
            {
                Temporaries.Remove(tmp);
                if (issave) Save();
            }
        }

        // **************************************************
        // Histories

        public static BindableCollection<TubeVideoHistoryModel> Histories
        {
            get => Instance._Histories = Instance._Histories ?? new BindableCollection<TubeVideoHistoryModel>(TubeSetting.Instance.Histories);
        }
        private BindableCollection<TubeVideoHistoryModel> _Histories;

        public static void AddHistory(string id, bool issave = true)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == id);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            if (tmp == null)
            {
                Histories.Add(new TubeVideoHistoryModel(id));
            }
            if (issave) Save();
        }

        public static void DelHistory(string id, bool issave = true)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == id);
            if (tmp != null)
            {
                Histories.Remove(tmp);
                if (issave) Save();
            }
        }

    }
}