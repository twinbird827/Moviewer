using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;
using TBird.Wpf.Controls;

namespace Moviewer.Core.Windows
{
    public class MainViewModel : MainViewModelBase
    {
        public MainViewModel()
        {
            Loaded.Add(() => MessageService.Info("TEST"));
        }
    }
}