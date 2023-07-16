using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TBird.Core;
using TBird.Web;
using TBird.Wpf;

namespace Moviewer.Core
{
    public static class VideoUtil
    {
        public static string Url2Id(string url)
        {
            return CoreUtil.Nvl(url).Split('/').Last().Split('?').First();
        }

        public static async Task<BitmapImage> GetThumnailAsync(string id, params string[] urls)
        {
            var bytes = await WebImageUtil.GetBytesAsync(id, urls);
            var image = bytes != null ? ControlUtil.GetImage(bytes) : null;
            return image;
        }
    }
}