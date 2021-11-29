using System;

namespace OpenApi
{
    [Serializable]
    public class ApiResponse<TResult>
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 补充消息
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public TResult Result { get; set; }
        public ApiResponse() { }
        public ApiResponse(
            string code,
            string message,
            string details = null)
        {
            Code = code;
            Message = message;
            Details = details;
        }

        public ApiResponse(
            string code,
            TResult result,
            string message = "OK")
        {
            Code = code;
            Result = result;
            Message = message;
        }
    }
}
