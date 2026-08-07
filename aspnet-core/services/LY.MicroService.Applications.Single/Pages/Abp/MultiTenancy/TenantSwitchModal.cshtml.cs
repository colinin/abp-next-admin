using LINGYUN.Platform.Portal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

#nullable enable
namespace LY.MicroService.Applications.Single.Pages.Abp.MultiTenancy;

public class TenantSwitchModalModel : AbpPageModel
{
    [BindProperty]
    public TenantInfoModel Input { get; set; } = default!;

    public List<SelectListItem> AvailableTenants { get; set; } = new();

    protected ITenantStore TenantStore { get; }
    protected ITenantNormalizer TenantNormalizer { get; }
    protected AbpAspNetCoreMultiTenancyOptions Options { get; }
    protected IEnterpriseRepository EnterpriseRepository { get; }
    public TenantSwitchModalModel(
        ITenantStore tenantStore,
        ITenantNormalizer tenantNormalizer,
        IOptions<AbpAspNetCoreMultiTenancyOptions> options,
        IEnterpriseRepository enterpriseRepository)
    {
        TenantStore = tenantStore;
        TenantNormalizer = tenantNormalizer;
        Options = options.Value;
        EnterpriseRepository = enterpriseRepository;

        LocalizationResourceType = typeof(AbpUiMultiTenancyResource);
    }

    public async virtual Task OnGetAsync()
    {
        await LoadAvailableTenants();
        if (AvailableTenants.Count > 0)
        {
            Input = new SelectTenantInfoModel
            {
                AvailableTenants = AvailableTenants
            };
        }
        else
        {
            Input = new TenantInfoModel();
        }

        if (CurrentTenant.IsAvailable)
        {
            var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
            Input.Name = tenant?.Name;
        }
    }

    public virtual async Task OnPostAsync()
    {
        Guid? tenantId = null;
        if (!Input.Name.IsNullOrEmpty())
        {
            var tenant = await TenantStore.FindAsync(TenantNormalizer.NormalizeName(Input.Name!)!);
            if (tenant == null && Guid.TryParse(Input.Name, out var id))
            {
                tenant = await TenantStore.FindAsync(id);
            }
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
    private async Task LoadAvailableTenants()
    {
        var enterprises = await EnterpriseRepository.GetEnterprisesInTenantListAsync(maxResultCount: 25);
        if (enterprises.Count > 0)
        {
            AvailableTenants = enterprises
                .Select(enterprise => 
                    new SelectListItem(
                        enterprise.Name, enterprise.TenantId?.ToString()))
                .Union([new SelectListItem("Default", "")])
                .ToList();
        }
    }

    public class TenantInfoModel
    {
        public virtual string? Name { get; set; }
    }

    public class SelectTenantInfoModel : TenantInfoModel
    {
        public List<SelectListItem> AvailableTenants { get; set; } = new();

        [SelectItems(nameof(AvailableTenants))]
        public override string? Name { get; set; }
    }
}
#nullable disable
