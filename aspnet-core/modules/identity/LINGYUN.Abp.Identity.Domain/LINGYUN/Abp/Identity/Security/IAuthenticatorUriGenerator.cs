namespace LINGYUN.Abp.Identity.Security;
public interface IAuthenticatorUriGenerator
{
    string Generate(string email, string unformattedKey);
}
