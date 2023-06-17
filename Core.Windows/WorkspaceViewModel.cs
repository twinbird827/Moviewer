using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Wpf;

namespace Moviewer.Core.Windows
{
    public class WorkspaceViewModel : BindableBase
    {
        public ICommand OnLoaded => _OnLoaded = _OnLoaded ?? RelayCommand.Create(async _ =>
        {
            MainViewModel.Instance.ShowProgress = true;

            await Loaded.ExecuteAsync();

            MainViewModel.Instance.ShowProgress = false;
        });
        private ICommand _OnLoaded;

        public BackgroundTaskManager Loaded { get; } = new BackgroundTaskManager();

    }
}
