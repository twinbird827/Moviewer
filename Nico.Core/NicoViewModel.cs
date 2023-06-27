using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public abstract class NicoViewModel : BindableBase
    {
        public abstract IRelayCommand Loaded { get; }
    }
}
