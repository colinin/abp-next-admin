using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class CaptchaComponentDictionary : Dictionary<string, CaptchaComponent?>
{
    public const string DefaultName = "Default";

    public CaptchaComponent? Default {
        get => this.GetOrDefault(DefaultName);
        set => this[DefaultName] = value;
    }

    [NotNull]
    public CaptchaComponent GetComponentOrDefault(string name)
    {
        return this.GetOrDefault(name)
               ?? Default
               ?? throw new AbpException($"Captcha component '{name}' was not found and there is no default component.");
    }
}
