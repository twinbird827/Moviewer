using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;

namespace Moviewer.Tube.Core
{
    public class TubeSetting : JsonBase<TubeSetting>
    {
        private const string _path = @"lib\tube-setting.json";

        public static TubeSetting Instance
        {
            get => _Instance = _Instance ?? new TubeSetting();
        }
        private static TubeSetting _Instance;

        public TubeSetting() : base(_path)
        {
            if (!Load())
            {

            }
        }

        public string APIKEY
        {
            get => GetProperty(_APIKEY);
            set => SetProperty(ref _APIKEY, value);
        }
        private string _APIKEY;

    }
}