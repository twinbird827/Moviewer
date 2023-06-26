using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Core.Windows
{
    public abstract class WorkspaceViewModel : BindableBase
    {
        public abstract MenuType Type { get; }

        public ICommand OnLoaded => _OnLoaded = _OnLoaded ?? RelayCommand.Create(async _ =>
        {
            MainViewModel.Instance.ShowProgress = true;

            await Loaded.ExecuteAsync();

            MainViewModel.Instance.ShowProgress = false;
        });
        private ICommand _OnLoaded;

        public BackgroundTaskManager Loaded { get; } = new BackgroundTaskManager();

        public string Title => $"Moviewer [{Type.GetLabel()}]";
    }
}
