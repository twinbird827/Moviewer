using TBird.Core;

namespace Moviewer.Nico.Core
{
    public class NicoSetting : JsonBase<NicoSetting>
    {
        private const string _path = @"lib\nico-setting.json";

        public static NicoSetting Instance
        {
            get => _Instance = _Instance ?? new NicoSetting();
        }
        private static NicoSetting _Instance;

        public NicoSetting() : base(_path)
        {
            if (!Load())
            {
                Temporaries = new NicoVideoHistoryModel[] { };

                Histories = new NicoVideoHistoryModel[] { };

                SearchHistories = new NicoSearchHistoryModel[] { };

                SearchFavorites = new NicoSearchHistoryModel[] { };
            }
        }

        public string NicoRankingGenre
        {
            get => GetProperty(_NicoRankingGenre);
            set => SetProperty(ref _NicoRankingGenre, value);
        }
        private string _NicoRankingGenre;

        public string NicoRankingPeriod
        {
            get => GetProperty(_NicoRankingPeriod);
            set => SetProperty(ref _NicoRankingPeriod, value);
        }
        private string _NicoRankingPeriod;

        public string NicoSearchOrderby
        {
            get => GetProperty(_NicoSearchOrderby);
            set => SetProperty(ref _NicoSearchOrderby, value);
        }
        private string _NicoSearchOrderby;

        public string NicoFavoriteOrderby
        {
            get => GetProperty(_NicoFavoriteOrderby);
            set => SetProperty(ref _NicoFavoriteOrderby, value);
        }
        private string _NicoFavoriteOrderby;

        public NicoVideoHistoryModel[] Temporaries
        {
            get => GetProperty(_Temporaries);
            set => SetProperty(ref _Temporaries, value);
        }
        private NicoVideoHistoryModel[] _Temporaries;

        public NicoVideoHistoryModel[] Histories
        {
            get => GetProperty(_Histories);
            set => SetProperty(ref _Histories, value);
        }
        private NicoVideoHistoryModel[] _Histories;

        public NicoSearchHistoryModel[] SearchHistories
        {
            get => GetProperty(_SearchHistories);
            set => SetProperty(ref _SearchHistories, value);
        }
        private NicoSearchHistoryModel[] _SearchHistories;

        public NicoSearchHistoryModel[] SearchFavorites
        {
            get => GetProperty(_SearchFavorites);
            set => SetProperty(ref _SearchFavorites, value);
        }
        private NicoSearchHistoryModel[] _SearchFavorites;

    }
}