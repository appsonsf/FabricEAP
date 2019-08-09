using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 封装返回数据，一般在变更数据（非HTTPGET）的时候才使用
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        /// 状态 0 表示成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 构建成功的返回值
        /// </summary>
        /// <returns></returns>
        public static ResponseData BuildSuccessResponse(string message = "Success")
        {
            return new ResponseData
            {
                Status = StatusConst.SucessStatus,
                Message = message,
            };
        }

        public static ResponseData BuildFailedResponse(int status = StatusConst.DefaultFailureStatus, string message = "Failed")
        {
            return new ResponseData
            {
                Status = status,
                Message = message,
            };
        }
    }

    /// <summary>
    /// 请求返回数据模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseData<T>: ResponseData
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 构建成功的返回值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseData<T> BuildSuccessResponse(T data)
        {
            return new ResponseData<T>
            {
                Status = StatusConst.SucessStatus,
                Message = "Success",
                Data = data
            };
        }

        new public static ResponseData<T> BuildFailedResponse(int status = StatusConst.DefaultFailureStatus, string message = "Failed")
        {
            return new ResponseData<T>
            {
                Status = status,
                Message = message,
            };
        }
    }
}
