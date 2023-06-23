using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core.Stateful;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoModel
    {
        private NicoModel()
        {

        }

        private static NicoModel Instance
        {
            get => _Instance = _Instance ?? new NicoModel();
        }
        private static NicoModel _Instance;

        public static void Save()
        {
            NicoSetting.Instance.Temporaries = Temporaries.ToArray();
            NicoSetting.Instance.Histories = Histories.ToArray();
            NicoSetting.Instance.SearchHistories = SearchHistories.ToArray();
            NicoSetting.Instance.SearchFavorites = SearchFavorites.ToArray();
            NicoSetting.Instance.Save();
        }

        // **************************************************
        // Temporaries

        public static ObservableSynchronizedCollection<NicoVideoHistoryModel> Temporaries
        {
            get => Instance._Temporaries = Instance._Temporaries ?? new ObservableSynchronizedCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Temporaries);
        }
        private ObservableSynchronizedCollection<NicoVideoHistoryModel> _Temporaries;

        public static void AddTemporary(string contentid)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                tmp.RegistDate = DateTime.Now;
            }
            else
            {
                Temporaries.Add(new NicoVideoHistoryModel(contentid));
                Save();
            }
        }

        public static void DelTemporary(string contentid)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                Temporaries.Remove(tmp);
                Save();
            }
        }

        // **************************************************
        // Histories

        public static ObservableSynchronizedCollection<NicoVideoHistoryModel> Histories
        {
            get => Instance._Histories = Instance._Histories ?? new ObservableSynchronizedCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Histories);
        }
        private ObservableSynchronizedCollection<NicoVideoHistoryModel> _Histories;

        public static void AddHistory(string contentid)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                tmp.RegistDate = DateTime.Now;
            }
            else
            {
                Histories.Add(new NicoVideoHistoryModel(contentid));
                Save();
            }
        }

        public static void DelHistory(string contentid)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                Histories.Remove(tmp);
                Save();
            }
        }

        // **************************************************
        // Search Histories

        public static ObservableSynchronizedCollection<NicoSearchHistoryModel> SearchHistories
        {
            get => Instance._SearchHistories = Instance._SearchHistories ?? new ObservableSynchronizedCollection<NicoSearchHistoryModel>(NicoSetting.Instance.SearchHistories);
        }
        private ObservableSynchronizedCollection<NicoSearchHistoryModel> _SearchHistories;

        public static void AddSearchHistory(string word, NicoSearchType type)
        {
            var tmp = SearchHistories.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                SearchHistories.Add(new NicoSearchHistoryModel(word, type));
                Save();
            }
        }

        public static void DelSearchHistory(string word, NicoSearchType type)
        {
            var tmp = SearchHistories.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                SearchHistories.Remove(tmp);
                Save();
            }
        }

        // **************************************************
        // SearchFavorites

        public static ObservableSynchronizedCollection<NicoSearchHistoryModel> SearchFavorites
        {
            get => Instance._SearchFavorites = Instance._SearchFavorites ?? new ObservableSynchronizedCollection<NicoSearchHistoryModel>(NicoSetting.Instance.SearchFavorites);
        }
        private ObservableSynchronizedCollection<NicoSearchHistoryModel> _SearchFavorites;

        public static void AddSearchFavorite(string word, NicoSearchType type)
        {
            var tmp = SearchFavorites.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                SearchFavorites.Add(new NicoSearchHistoryModel(word, type));
                Save();
            }
        }

        public static void DelSearchFavorite(string word, NicoSearchType type)
        {
            var tmp = SearchFavorites.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                SearchFavorites.Remove(tmp);
                Save();
            }
        }
    }
}
