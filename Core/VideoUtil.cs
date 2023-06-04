using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TBird.Core;

namespace Moviewer.Core
{
    public static class VideoUtil
    {
        public static string Url2Id(string url)
        {
            return CoreUtil.Nvl(url).Split('/').Last().Split('?').First();
        }

        public static async Task<BitmapImage> GetThumnailAsync(params string[] urls)
        {
            foreach (var url in urls)
            {
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }
                else if (_dic.ContainsKey(url))
                {
                    return _dic[url];
                }

                var bytes = await WebUtil.GetThumnailBytes(url);

                if (bytes == null) continue;

                using (WrappingStream stream = new WrappingStream(new MemoryStream(bytes)))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    //bitmap.DecodePixelWidth = 160 + 48 * 0;
                    //bitmap.DecodePixelHeight = 120 + 36 * 0;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    if (bitmap.CanFreeze)
                    {
                        bitmap.Freeze();
                    }
                    return _dic[url] = bitmap;
                }
            }

            return null;
        }
        private static Dictionary<string, BitmapImage> _dic = new Dictionary<string, BitmapImage>();

    }
}
