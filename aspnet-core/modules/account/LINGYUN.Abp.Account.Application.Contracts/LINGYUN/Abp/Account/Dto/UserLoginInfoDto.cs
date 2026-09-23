namespace LINGYUN.Abp.Account;
public class UserLoginInfoDto
{
    public string LoginProvider { get; set; } = default!;
    public string ProviderKey { get; set; } = default!;
    public string? ProviderDisplayName { get; set; }
}
