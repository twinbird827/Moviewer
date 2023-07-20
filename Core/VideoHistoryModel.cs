using Moviewer.Core.Controls;
using Moviewer.Nico.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Wpf;
using TBird.Wpf.Collections;

namespace Moviewer.Core
{
    public class VideoHistoryModel : BindableBase
    {
        public VideoHistoryModel()
        {

        }

        public VideoHistoryModel(MenuMode mode, string id)
        {
            Mode = mode;
            ContentId = id;
        }

        public MenuMode Mode
        {
            get => _Mode;
            set => SetProperty(ref _Mode, value);
        }
        private MenuMode _Mode;

        public string ContentId
        {
            get => _ContentId;
            set => SetProperty(ref _ContentId, value);
        }
        private string _ContentId = null;

        public DateTime Date
        {
            get => _Date;
            set => SetProperty(ref _Date, value);
        }
        private DateTime _Date;
    }

    public static class VideoHistoryModelExtension
    {
        public static VideoHistoryModel GetModel(this BindableCollection<VideoHistoryModel> arr, VideoModel m)
        {
            return arr.FirstOrDefault(x => x.Mode == m.Mode && x.ContentId == m.ContentId);
        }

        public static void AddModel(this BindableCollection<VideoHistoryModel> arr, MenuMode mode, string id)
        {
            var tmp = arr.FirstOrDefault(x => x.Mode == mode && x.ContentId == id);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                arr.Add(new VideoHistoryModel(mode, id));
            }
        }

        public static bool DelModel(this BindableCollection<VideoHistoryModel> arr, MenuMode mode, string id)
        {
            var tmp = arr.FirstOrDefault(x => x.Mode == mode && x.ContentId == id);
            if (tmp != null)
            {
                return arr.Remove(tmp);
            }
            else
            {
                return false;
            }
        }
    }
}