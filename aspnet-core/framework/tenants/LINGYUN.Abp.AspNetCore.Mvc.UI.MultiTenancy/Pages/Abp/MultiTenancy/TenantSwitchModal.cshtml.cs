using LINGYUN.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.MultiTenancy;

#nullable enable
namespace LINGYUN.Abp.MicroService.AuthServer.Pages.Abp.MultiTenancy;

public class TenantSwitchModalModel : AbpPageModel
{
    [BindProperty]
    public TenantInfoModel TenantInput { get; set; } = default!;

    [BindProperty]
    public SelectTenantInfoModel TenantSelect { get; set; } = default!;

    public List<SelectListItem> AvailableTenants { get; set; } = new();

    protected ITenantStore TenantStore { get; }
    protected ITenantNormalizer TenantNormalizer { get; }
    protected AbpAspNetCoreMultiTenancyOptions Options { get; }
    protected IAvailableTenantPickerStore AvailableTenantPickerStore { get; }
    public TenantSwitchModalModel(
        ITenantStore tenantStore,
        ITenantNormalizer tenantNormalizer,
        IOptions<AbpAspNetCoreMultiTenancyOptions> options,
        IAvailableTenantPickerStore availableTenantPickerStore)
    {
        TenantStore = tenantStore;
        TenantNormalizer = tenantNormalizer;
        Options = options.Value;
        AvailableTenantPickerStore = availableTenantPickerStore;

        LocalizationResourceType = typeof(AbpUiMultiTenancyResource);
    }

    public async virtual Task OnGetAsync()
    {
        TenantInput = new TenantInfoModel();
        TenantSelect = new SelectTenantInfoModel();

        await LoadAvailableTenants();
        if (AvailableTenants.Count > 0)
        {
            TenantSelect.AvailableTenants = AvailableTenants;
            TenantSelect.Name = "";
            if (CurrentTenant.IsAvailable)
            {
                var tenantId = CurrentTenant.GetId().ToString();
                TenantSelect.Name = tenantId;
            }
        }
        else
        {
            if (CurrentTenant.IsAvailable)
            {
                var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
                TenantInput.Name = tenant?.Name;
            }
        }
    }

    public async virtual Task OnPostTenantSwitchAsync()
    {
        Guid? tenantId = null;
        var tenantName = TenantSelect.Name;
        if (tenantName.IsNullOrWhiteSpace())
        {
            tenantName = TenantSelect.InputName;
        }
        if (!tenantName.IsNullOrEmpty())
        {
            var tenant = await TenantStore.FindAsync(TenantNormalizer.NormalizeName(tenantName)!);
            if (tenant == null && Guid.TryParse(tenantName, out var id))
            {
                tenant = await TenantStore.FindAsync(id);
            }
            if (tenant == null)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotExist", tenantName]);
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotAvailable", tenantName]);
            }

            tenantId = tenant.Id;
        }

        AbpMultiTenancyCookieHelper.SetTenantCookie(HttpContext, tenantId, Options.TenantKey);
    }

    public async virtual Task OnPostTenantInputAsync()
    {
        Guid? tenantId = null;
        if (!TenantInput.Name.IsNullOrEmpty())
        {
            var tenant = await TenantStore.FindAsync(TenantNormalizer.NormalizeName(TenantInput.Name!)!);
            if (tenant == null)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotExist", TenantInput.Name!]);
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotAvailable", TenantInput.Name!]);
            }

            tenantId = tenant.Id;
        }

        AbpMultiTenancyCookieHelper.SetTenantCookie(HttpContext, tenantId, Options.TenantKey);
    }

    private async Task LoadAvailableTenants()
    {
        var availableTenants = await AvailableTenantPickerStore.GetAvailableTenantsAsync();
        if (availableTenants.Count > 0)
        {
            AvailableTenants = availableTenants
                .Select(tenant => new SelectListItem(tenant.Name, tenant.Value))
                .Union([new SelectListItem(L["InputTenantName"], "")])
                .ToList();
        }
    }

    public class TenantInfoModel
    {
        [InputInfoText("SwitchTenantHint")]
        public string? Name { get; set; }
    }

    public class SelectTenantInfoModel
    {
        public List<SelectListItem> AvailableTenants { get; set; } = new();

        [InputInfoText("SelectTenantHint")]
        [SelectItems(nameof(AvailableTenants))]
        public string? Name { get; set; }

        [DisplayName("InputTenantName")]
        [InputInfoText("SwitchTenantHint")]
        public string? InputName { get; set; }
    }
}
#nullable disable
