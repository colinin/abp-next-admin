namespace LINGYUN.Abp.Idempotent;
public class AbpIdempotentOptions
{
    public bool IsEnabled { get; set; }
    public int DefaultTimeout { get; set; }
    public string IdempotentTokenName { get; set; }
    public int HttpStatusCode { get; set; }
    public AbpIdempotentOptions()
    {
        DefaultTimeout = 5000;
        IdempotentTokenName = "X-With-Idempotent-Token";
        // 默认使用 TooManyRequests(429)代码
        HttpStatusCode = 429;
    }
}
