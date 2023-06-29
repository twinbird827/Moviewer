using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public abstract class NicoViewModel : BindableBase
    {
        public abstract IRelayCommand Loaded { get; }
    }
}