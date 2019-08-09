using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigMgmt
{
    [Serializable]
    public class InfoVisibility
    {
        /// <summary>
        /// 为True显示手机号，默认隐藏手机号
        /// </summary>
        public bool Mobile { get; set; }
    }
}
