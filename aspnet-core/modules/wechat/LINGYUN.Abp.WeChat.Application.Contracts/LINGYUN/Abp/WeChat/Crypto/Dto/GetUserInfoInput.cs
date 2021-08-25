namespace LINGYUN.Abp.WeChat.Crypto
{
    public class GetUserInfoInput
    {
        public string EncryptedData { get; set; }
        public string IV { get; set; }
        public string Code { get; set; }
    }
}
