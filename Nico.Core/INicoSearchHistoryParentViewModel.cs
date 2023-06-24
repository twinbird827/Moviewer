using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviewer.Nico.Core
{
    public interface INicoSearchHistoryParentViewModel
    {
        void NicoSearchHistoryOnDelete(NicoSearchHistoryViewModel vm);

        void NicoSearchHistoryOnDoubleClick(NicoSearchHistoryViewModel vm);
    }
}
