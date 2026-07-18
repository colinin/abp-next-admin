using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.MultiTenancy;

#nullable enable
namespace LY.MicroService.AuthServer.Pages.Abp.MultiTenancy;

public class TenantSwitchModalModel : AbpPageModel
{
    [BindProperty]
    public TenantInfoModel Input { get; set; } = default!;

    public List<SelectListItem> AvailableTenants { get; set; } = new();

    protected ITenantStore TenantStore { get; }
    protected ITenantNormalizer TenantNormalizer { get; }
    protected AbpAspNetCoreMultiTenancyOptions Options { get; }

    public TenantSwitchModalModel(
        ITenantStore tenantStore,
        ITenantNormalizer tenantNormalizer,
        IOptions<AbpAspNetCoreMultiTenancyOptions> options)
    {
        TenantStore = tenantStore;
        TenantNormalizer = tenantNormalizer;
        Options = options.Value;
        LocalizationResourceType = typeof(AbpUiMultiTenancyResource);
    }

    public virtual async Task OnGetAsync()
    {
        Input = new TenantInfoModel();

        if (CurrentTenant.IsAvailable)
        {
            var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
            Input.Name = tenant?.Name;
        }

        await LoadAvailableTenants();
    }

    public virtual async Task OnPostAsync()
    {
        Guid? tenantId = null;
        if (!Input.Name.IsNullOrEmpty())
        {
            var tenant = await TenantStore.FindAsync(TenantNormalizer.NormalizeName(Input.Name!)!);
            if (tenant == null)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotExist", Input.Name!]);
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotAvailable", Input.Name!]);
            }

            tenantId = tenant.Id;
        }

        AbpMultiTenancyCookieHelper.SetTenantCookie(HttpContext, tenantId, Options.TenantKey);
    }
    private Task LoadAvailableTenants()
    {
        AvailableTenants = new List<SelectListItem>
        {
            new SelectListItem("Host", null),
            new SelectListItem("A租户", "A"),
            new SelectListItem("B租户", "B"),
            new SelectListItem("C租户", "C"),
            new SelectListItem("D租户", "D")
        };

        return Task.CompletedTask;
    }

    public class TenantInfoModel
    {
        public string? Name { get; set; }
    }

    public class TenantInfo
    {
        public string? Name { get; set; }
        public string DisplayName { get; set; }
        public TenantInfo(string? name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
#nullable disable
