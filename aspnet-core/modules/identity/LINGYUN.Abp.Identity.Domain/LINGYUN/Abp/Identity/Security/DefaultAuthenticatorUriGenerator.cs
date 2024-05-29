using System.Text.Encodings.Web;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Security;
public class DefaultAuthenticatorUriGenerator : IAuthenticatorUriGenerator, ITransientDependency
{
    protected const string OTatpUrlFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";


    private readonly UrlEncoder _urlEncoder;
    private readonly IApplicationInfoAccessor _applicationInfoAccessor;

    public DefaultAuthenticatorUriGenerator(
        UrlEncoder urlEncoder, 
        IApplicationInfoAccessor applicationInfoAccessor)
    {
        _urlEncoder = urlEncoder;
        _applicationInfoAccessor = applicationInfoAccessor;
    }

    public virtual string Generate(string email, string unformattedKey)
    {
        var application = _urlEncoder.Encode(_applicationInfoAccessor.ApplicationName);
        var account = _urlEncoder.Encode(email);

        return string.Format(OTatpUrlFormat, application, account, unformattedKey, application);
    }
}
