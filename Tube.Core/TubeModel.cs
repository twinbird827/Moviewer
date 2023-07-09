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

    }
}
