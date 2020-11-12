namespace LINGYUN.Abp.Account
{
    /// <summary>
    /// 定义账户系统Url
    /// </summary>
    /// <remarks>
    /// 定义在领域共享层,可以方便其他应用程序调用
    /// </remarks>
    public static class AccountUrlNames
    {
        /// <summary>
        /// 邮件登录验证地址
        /// </summary>
        public static string MailLoginVerify { get; set; } = "";
    }
}
