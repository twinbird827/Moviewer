using Moviewer.Nico.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviewer.Core.Controls
{
    public interface IVideoParentViewModel
    {
        void DeleteOnVideo(VideoViewModel vm);
    }
}