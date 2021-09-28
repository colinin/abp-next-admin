namespace LINGYUN.Abp.IM.Messages
{
    public class MessageSendResult
    {
        public bool Success { get; }
        public string Error { get; }
        public int Code { get; }
        public string Form { get; }
        public string To { get; }
        public string Content { get; }
        public static MessageSendResult Successed(string form, string to, string content)
        {
            return new MessageSendResult(form, to, content);
        }

        public static MessageSendResult Failed(int code, string error, string form, string to, string content)
        {
            return new MessageSendResult(code, error, form, to, content);
        }
        private MessageSendResult(
            int code,
            string error,
            string form,
            string to,
            string content)
        {
            Code = code;
            Error = error;
            Form = form;
            To = to;
            Success = false;
        }

        private MessageSendResult(
            string form,
            string to,
            string content)
        {
            Form = form;
            To = to;
            Content = content;
            Success = true;
        }
    }
}
