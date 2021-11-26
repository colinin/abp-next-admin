using System;

namespace LINGYUN.Abp.Wrapper
{
    [Serializable]
    public class WrapResult: WrapResult<object>
    {
        public WrapResult() { }
        public WrapResult(
            string code,
            string message,
            string details = null)
            : base(code, message, details)
        {
        }

        public WrapResult(
            string code,
            object result,
            string message = "OK")
            : base(code, result, message)
        {
        }
    }
}
