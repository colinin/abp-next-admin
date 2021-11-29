using System;

namespace OpenApi
{
    [Serializable]
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() { }
        public ApiResponse(
            string code,
            string message,
            string details = null)
            : base(code, message, details)
        {
        }

        public ApiResponse(
            string code,
            object result,
            string message = "OK")
            : base(code, result, message)
        {
        }
    }
}
