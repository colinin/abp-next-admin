namespace LINGYUN.Abp.Account;
public class AuthenticatorDto
{
    public bool IsAuthenticated { get; set; }
    public string SharedKey { get; set; }
    public string AuthenticatorUri { get; set; }
}
