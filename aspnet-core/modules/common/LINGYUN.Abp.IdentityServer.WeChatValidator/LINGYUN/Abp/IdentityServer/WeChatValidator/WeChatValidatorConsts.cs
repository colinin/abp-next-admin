namespace LINGYUN.Abp.IdentityServer.WeChatValidator
{
    public class WeChatValidatorConsts
    {
        public const string WeChatValidatorClientName = "WeChatValidator";

        public const string WeChatValidatorGrantTypeName = "wechat";

        public const string WeChatValidatorTokenName = "code";

        public class ClaimTypes
        {
            public const string OpenId = "wechat-id";
        }

        public class AuthenticationMethods
        {
            public const string BasedWeChatAuthentication = "wca";
        }
    }
}
