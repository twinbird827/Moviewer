using Moviewer.Core.Windows;
using Moviewer.Nico.Workspaces;
using System.Windows.Input;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoVideoTagViewModel : BindableBase
    {
        public NicoVideoTagViewModel(string tag)
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
            MainViewModel.Instance.Current = new NicoSearchViewModel(Tag, NicoSearchType.Tag);
        });
        private ICommand _OnClickTag;

    }
}