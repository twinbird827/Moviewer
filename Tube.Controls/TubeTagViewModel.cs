﻿using Moviewer.Core.Controls;
using Moviewer.Nico.Core;
using Moviewer.Nico.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Tube.Controls
{
    public class TubeTagViewModel : TagViewModel
    {
        public TubeTagViewModel(string tag) : base(tag)
        {

        }

        protected override ICommand CreateOnClickTag()
        {
            return base.CreateOnClickTag();
        }
    }
}