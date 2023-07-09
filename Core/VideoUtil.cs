using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static async Task<BitmapImage> GetThumnailAsync(string id, params string[] urls)
        {
            using (await Locker.LockAsync(_guid))
            {
                foreach (var url in urls)
                {
                    if (string.IsNullOrEmpty(url))
                    {
                        continue;
                    }

                    var bytes = GetThumnailFromFileAsync(id) ?? await WebUtil.GetThumnailBytes(url);

                    if (bytes == null) continue;

                    SaveFileBytes(id, bytes);

                    using (WrappingStream stream = new WrappingStream(new MemoryStream(bytes)))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        if (bitmap.CanFreeze)
                        {
                            bitmap.Freeze();
                        }
                        return bitmap;
                    }
                }
            }

            return null;
        }

        private static string _guid = Guid.NewGuid().ToString();

        private static void SaveFileBytes(string id, byte[] bytes)
        {
            Directory.CreateDirectory(_path);

            var urlpath = Path.Combine(_path, id);

            if (File.Exists(urlpath)) return;

            File.WriteAllBytes(urlpath, bytes);
        }

        private static byte[] GetThumnailFromFileAsync(string id)
        {
            Directory.CreateDirectory(_path);

            var urlpath = Path.Combine(_path, id);

            if (!File.Exists(urlpath)) return null;

            return File.ReadAllBytes(urlpath);
        }

        private const string _path = @"bytes\";
    }
}