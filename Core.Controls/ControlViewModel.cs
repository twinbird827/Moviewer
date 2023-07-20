using Moviewer.Core.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Wpf;

namespace Moviewer.Core.Controls
{
    public class ControlViewModel : BindableBase
    {
        public ControlViewModel(ControlModel m)
        {
            if (m != null) Loaded.Add(m.OnLoadedModel);

            AddDisposed((sender, e) =>
            {
                Loaded.Dispose();
            });
        }

        public ICommand OnLoaded => _OnLoaded = _OnLoaded ?? RelayCommand.Create(async _ =>
        {
            if (ShowProgress) MainViewModel.Instance.ShowProgress = true;

            await Loaded.ExecuteAsync();

            if (ShowProgress) MainViewModel.Instance.ShowProgress = false;
        });
        private ICommand _OnLoaded;

        protected bool ShowProgress { get; set; } = false;

        protected BackgroundTaskManager Loaded { get; } = new BackgroundTaskManager();
    }
}