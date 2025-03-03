using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;

namespace LINGYUN.Platform.Portal;

[Authorize(PlatformPermissions.Enterprise.Default)]
public class EnterpriseAppService :
    PlatformApplicationCurdAppServiceBase<
        Enterprise,
        EnterpriseDto,
        Guid,
        EnterpriseGetListInput,
        EnterpriseCreateDto,
        EnterpriseUpdateDto>,
    IEnterpriseAppService
{
    protected IEnterpriseRepository EnterpriseRepository { get; }

    public EnterpriseAppService(IEnterpriseRepository repository) 
        : base(repository)
    {
        EnterpriseRepository = repository;

        CreatePolicyName = PlatformPermissions.Enterprise.Create;
        UpdatePolicyName = PlatformPermissions.Enterprise.Update;
        DeletePolicyName = PlatformPermissions.Enterprise.Delete;
        GetListPolicyName = PlatformPermissions.Enterprise.Default;
        GetPolicyName = PlatformPermissions.Enterprise.Default;
    }

    protected async override Task<Enterprise> MapToEntityAsync(EnterpriseCreateDto createInput)
    {
        if (await EnterpriseRepository.FindByNameAsync(createInput.Name) != null)
        {
            throw new BusinessException(PlatformErrorCodes.DuplicateEnterpriseName)
                .WithData("Name", createInput.Name);
        }

        if (!createInput.EnglishName.IsNullOrWhiteSpace())
        {
            if (await EnterpriseRepository.FindByNameAsync(createInput.EnglishName) != null)
            {
                throw new BusinessException(PlatformErrorCodes.DuplicateEnterpriseName)
                    .WithData("Name", createInput.EnglishName);
            }
        }

        var enterprise = new Enterprise(
            GuidGenerator.Create(),
            createInput.Name,
            createInput.Address,
            createInput.TaxCode,
            createInput.OrganizationCode,
            createInput.RegistrationCode,
            createInput.RegistrationDate,
            createInput.ExpirationDate,
            createInput.TenantId);

        UpdateByInput(enterprise, createInput);

        return enterprise;
    }

    protected async override Task MapToEntityAsync(EnterpriseUpdateDto updateInput, Enterprise entity)
    {
        if (!string.Equals(entity.EnglishName, updateInput.EnglishName, StringComparison.InvariantCultureIgnoreCase))
        {
            if (await EnterpriseRepository.FindByNameAsync(updateInput.EnglishName) != null)
            {
                throw new BusinessException(PlatformErrorCodes.DuplicateEnterpriseName)
                    .WithData("Name", updateInput.EnglishName);
            }
        }

        UpdateByInput(entity, updateInput);

        entity.SetConcurrencyStampIfNotNull(updateInput.ConcurrencyStamp);
    }

    protected override void TryToSetTenantId(Enterprise entity)
    {
        // ignore tenant
    }

    protected async override Task DeleteByIdAsync(Guid id)
    {
        var enterprise = await Repository.GetAsync(id);

        await Repository.DeleteAsync(enterprise);
    }

    protected virtual void UpdateByInput(Enterprise enterprise, EnterpriseCreateOrUpdateDto input)
    {
        if (!string.Equals(enterprise.EnglishName, input.EnglishName, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.EnglishName = input.EnglishName;
        }
        if (!string.Equals(enterprise.Logo, input.Logo, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.Logo = input.Logo;
        }
        if (!string.Equals(enterprise.Address, input.Address, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.Address = input.Address;
        }
        if (!string.Equals(enterprise.LegalMan, input.LegalMan, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.LegalMan = input.LegalMan;
        }
        if (!string.Equals(enterprise.TaxCode, input.TaxCode, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.TaxCode = input.TaxCode;
        }
        if (!string.Equals(enterprise.OrganizationCode, input.OrganizationCode, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.SetOrganization(input.OrganizationCode);
        }
        if (!string.Equals(enterprise.RegistrationCode, input.RegistrationCode, StringComparison.InvariantCultureIgnoreCase))
        {
            enterprise.SetRegistration(input.RegistrationCode, input.RegistrationDate, input.ExpirationDate);
        }
    }

    protected async override Task<IQueryable<Enterprise>> CreateFilteredQueryAsync(EnterpriseGetListInput input)
    {
        var filteredQuery = await base.CreateFilteredQueryAsync(input);

        Expression<Func<Enterprise, bool>> expression = _ => true;

        expression = expression.AndIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) ||
            x.EnglishName.Contains(input.Filter) || x.Address.Contains(input.Filter) || x.LegalMan.Contains(input.Filter) ||
            x.TaxCode.Contains(input.Filter)|| x.OrganizationCode.Contains(input.Filter) || x.RegistrationCode.Contains(input.Filter));

        expression = expression.AndIf(input.BeginRegistrationDate.HasValue, x => x.RegistrationDate >= input.BeginRegistrationDate);
        expression = expression.AndIf(input.EndRegistrationDate.HasValue, x => x.RegistrationDate <= input.EndRegistrationDate);
        expression = expression.AndIf(input.BeginExpirationDate.HasValue, x => x.ExpirationDate >= input.BeginExpirationDate);
        expression = expression.AndIf(input.EndExpirationDate.HasValue, x => x.ExpirationDate <= input.EndExpirationDate);

        return filteredQuery.Where(expression);
    }
}
