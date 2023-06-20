﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBird.Core;

namespace Moviewer.Core
{
    public static class AppConst
    {
        /// <summary>ﾃﾝﾎﾟﾗﾘ追加</summary>
        public static string L_AddTemporary { get; } = Lang.Instance.Get();

        /// <summary>URL or ID</summary>
        public static string L_UrlOrId { get; } = Lang.Instance.Get();

        /// <summary>ﾃﾝﾎﾟﾗﾘに追加する情報を入力してください。</summary>
        public static string MH_AddTemporary { get; } = Lang.Instance.Get();

    }
}
