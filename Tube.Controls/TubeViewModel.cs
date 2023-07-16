using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;

namespace Moviewer.Tube.Controls
{
    public abstract class TubeViewModel : BindableBase
    {
        public abstract IRelayCommand Loaded { get; }
    }
}