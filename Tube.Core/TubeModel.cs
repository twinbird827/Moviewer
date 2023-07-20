using Moviewer.Nico.Core;
using Moviewer.Tube.Controls;
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
            TubeSetting.Instance.Save();
        }

    }
}