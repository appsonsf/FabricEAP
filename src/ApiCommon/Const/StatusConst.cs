using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public static class StatusConst
    {
        public const int SucessStatus = 0;
        public const int DefaultFailureStatus = -1;


        //custome statusCode

        /// <summary>
        /// 群组文件中校验md5不存在
        /// </summary>
        public const int FileNotFound = 10;

        /// <summary>
        /// 不在同一个联系人组
        /// </summary>
        public const int NotInSameContactGroup = 0x00A1;


        #region 工作台配置信息错误编码

        /// <summary>
        /// 未找到工作太配置
        /// </summary>
        public const int NotFoundWorkBenchConfig = 0x00B1;

        /// <summary>
        /// 工作台参数配置错误
        /// </summary>
        public const int WorkBenchConfigError = 0x00B2;

        /// <summary>
        /// 工作台内部网络请求错误
        /// </summary>
        public const int WorkBenchInnerError = 0x00B3;

        #endregion
    }
}
