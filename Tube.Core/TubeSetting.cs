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
                Users = new TubeUserModel[] { };
                Histories = new TubeVideoHistoryModel[] { };
                Temporaries = new TubeVideoHistoryModel[] { };
            }
        }

        public string APIKEY
        {
            get => GetProperty(_APIKEY);
            set => SetProperty(ref _APIKEY, value);
        }
        private string _APIKEY;

        public string TubePopularCategory
        {
            get => GetProperty(_TubePopularCategory);
            set => SetProperty(ref _TubePopularCategory, value);
        }
        private string _TubePopularCategory;

        public TubeUserModel[] Users
        {
            get => GetProperty(_Users);
            set => SetProperty(ref _Users, value);
        }
        private TubeUserModel[] _Users;

        public TubeVideoHistoryModel[] Histories
        {
            get => GetProperty(_Histories);
            set => SetProperty(ref _Histories, value);
        }
        private TubeVideoHistoryModel[] _Histories;

        public TubeVideoHistoryModel[] Temporaries
        {
            get => GetProperty(_Temporaries);
            set => SetProperty(ref _Temporaries, value);
        }
        private TubeVideoHistoryModel[] _Temporaries;

    }
}