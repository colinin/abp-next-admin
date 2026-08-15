using System;

namespace LINGYUN.Abp.Account.Web.Models;

public class ExternalLoginProviderModel
{
    public Type ComponentType { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string AuthenticationScheme { get; set; } = default!;
}
