﻿using Moviewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;

namespace Moviewer.Nico.Core
{
    internal class NicoSetting : JsonBase<NicoSetting>
    {
        private const string _path = @"lib\nico-setting.json";

        public static NicoSetting Instance
        {
            get => _Instance = _Instance ?? new NicoSetting();
        }
        private static NicoSetting _Instance;

        public NicoSetting() : base(_path)
        {
            if (!Load())
            {
                Temporaries = new NicoVideoHistoryModel[] { };

                Histories = new NicoVideoHistoryModel[] { };
            }
        }

        public NicoVideoHistoryModel[] Temporaries
        {
            get => GetProperty(_Temporaries);
            set => SetProperty(ref _Temporaries, value);
        }
        private NicoVideoHistoryModel[] _Temporaries;

        public NicoVideoHistoryModel[] Histories
        {
            get => GetProperty(_Histories);
            set => SetProperty(ref _Histories, value);
        }
        private NicoVideoHistoryModel[] _Histories;

    }
}