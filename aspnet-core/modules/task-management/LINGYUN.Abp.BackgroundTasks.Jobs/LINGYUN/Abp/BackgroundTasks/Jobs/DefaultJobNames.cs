namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public static class DefaultJobNames
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public const string SendEmailJob = "SendEmail";
    /// <summary>
    /// 发送短信
    /// </summary>
    public const string SendSmsJob = "SendSms";
    /// <summary>
    /// 控制台输出
    /// </summary>
    public const string ConsoleJob = "Console";
    /// <summary>
    /// 休眠
    /// </summary>
    public const string SleepJob = "Sleep";
    /// <summary>
    /// 服务间调用
    /// </summary>
    public const string ServiceInvocationJob = "ServiceInvocation";
    /// <summary>
    /// Http请求
    /// </summary>
    public const string HttpRequestJob = "HttpRequest";
}
