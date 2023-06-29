using TBird.Core;

namespace Moviewer.Core
{
    public static class AppConst
    {
        /// <summary>お気に入り追加</summary>
        public static string L_AddFavorite { get; } = Lang.Instance.Get();

        /// <summary>ﾃﾝﾎﾟﾗﾘ追加</summary>
        public static string L_AddTemporary { get; } = Lang.Instance.Get();

        /// <summary>お気に入り削除</summary>
        public static string L_DelFavorite { get; } = Lang.Instance.Get();

        /// <summary>URL or ID</summary>
        public static string L_UrlOrId { get; } = Lang.Instance.Get();

        /// <summary>ﾃﾝﾎﾟﾗﾘに追加する情報を入力してください。</summary>
        public static string MH_AddTemporary { get; } = Lang.Instance.Get();

    }
}