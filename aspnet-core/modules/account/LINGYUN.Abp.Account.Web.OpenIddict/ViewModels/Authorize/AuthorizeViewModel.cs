using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LINGYUN.Abp.Account.Web.OpenIddict.ViewModels.Authorize;

public class AuthorizeViewModel
{
    public string ApplicationName { get; set; }

    [HiddenInput]
    public string Scope { get; set; }

    public List<ScopeItemViewModel> AvailableScopes { get; set; }
}

public class ScopeItemViewModel
{
    public string Value { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; } 
    public bool IsRequired { get; set; }
    public bool IsChecked { get; set; }
}