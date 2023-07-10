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

namespace Moviewer.Tube.Core
{
    public class TubeTagViewModel : BindableBase
    {
        public TubeTagViewModel(string tag)
        {
            Tag = tag;

            AddDisposed((sender, e) =>
            {
                OnClickTag.TryDispose();
            });
        }

        public string Tag { get; private set; }

        public ICommand OnClickTag => _OnClickTag = _OnClickTag ?? RelayCommand.Create(_ =>
        {
            //MainViewModel.Instance.Current = new NicoSearchViewModel(Tag, NicoSearchType.Tag);
        });
        private ICommand _OnClickTag;

    }
}