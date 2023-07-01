using System;
using System.Linq;
using TBird.Wpf.Collections;

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

        public static BindableCollection<NicoVideoHistoryModel> Temporaries
        {
            get => Instance._Temporaries = Instance._Temporaries ?? new BindableCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Temporaries);
        }
        private BindableCollection<NicoVideoHistoryModel> _Temporaries;

        public static void AddTemporary(string contentid, bool issave = true)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                Temporaries.Add(new NicoVideoHistoryModel(contentid));
            }
            if (issave) Save();
        }

        public static void DelTemporary(string contentid, bool issave = true)
        {
            var tmp = Temporaries.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                Temporaries.Remove(tmp);
                if (issave) Save();
            }
        }

        // **************************************************
        // Histories

        public static BindableCollection<NicoVideoHistoryModel> Histories
        {
            get => Instance._Histories = Instance._Histories ?? new BindableCollection<NicoVideoHistoryModel>(NicoSetting.Instance.Histories);
        }
        private BindableCollection<NicoVideoHistoryModel> _Histories;

        public static void AddHistory(string contentid, bool issave = true)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                Histories.Add(new NicoVideoHistoryModel(contentid));
            }
            if (issave) Save();
        }

        public static void DelHistory(string contentid, bool issave = true)
        {
            var tmp = Histories.FirstOrDefault(x => x.ContentId == contentid);
            if (tmp != null)
            {
                Histories.Remove(tmp);
                if (issave) Save();
            }
        }

        // **************************************************
        // Search Histories

        public static BindableCollection<NicoSearchHistoryModel> SearchHistories
        {
            get => Instance._SearchHistories = Instance._SearchHistories ?? new BindableCollection<NicoSearchHistoryModel>(NicoSetting.Instance.SearchHistories);
        }
        private BindableCollection<NicoSearchHistoryModel> _SearchHistories;

        public static void AddSearchHistory(string word, NicoSearchType type, bool issave = true)
        {
            var tmp = SearchHistories.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                SearchHistories.Add(new NicoSearchHistoryModel(word, type));
            }
            if (issave) Save();
        }

        public static void DelSearchHistory(string word, NicoSearchType type, bool issave = true)
        {
            var tmp = SearchHistories.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                SearchHistories.Remove(tmp);
                if (issave) Save();
            }
        }

        // **************************************************
        // SearchFavorites

        public static BindableCollection<NicoSearchHistoryModel> SearchFavorites
        {
            get => Instance._SearchFavorites = Instance._SearchFavorites ?? new BindableCollection<NicoSearchHistoryModel>(NicoSetting.Instance.SearchFavorites);
        }
        private BindableCollection<NicoSearchHistoryModel> _SearchFavorites;

        public static void AddSearchFavorite(string word, NicoSearchType type, bool issave = true)
        {
            var tmp = SearchFavorites.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                tmp.Date = DateTime.Now;
            }
            else
            {
                SearchFavorites.Add(new NicoSearchHistoryModel(word, type));
            }
            if (issave) Save();
        }

        public static void DelSearchFavorite(string word, NicoSearchType type, bool issave = true)
        {
            var tmp = SearchFavorites.FirstOrDefault(x => x.Word == word && x.Type == type);
            if (tmp != null)
            {
                SearchFavorites.Remove(tmp);
                if (issave) Save();
            }
        }
    }
}