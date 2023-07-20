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
            NicoSetting.Instance.SearchHistories = SearchHistories.ToArray();
            NicoSetting.Instance.SearchFavorites = SearchFavorites.ToArray();
            NicoSetting.Instance.Save();
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