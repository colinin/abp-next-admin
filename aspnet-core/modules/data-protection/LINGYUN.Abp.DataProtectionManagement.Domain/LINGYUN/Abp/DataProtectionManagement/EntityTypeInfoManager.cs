using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.DataProtectionManagement;
public class EntityTypeInfoManager : DomainService
{
    protected IEntityTypeInfoRepository EntityTypeInfoRepository { get; }
    protected IRoleEntityRuleRepository RoleEntityRuleRepository { get; }
    protected IOrganizationUnitEntityRuleRepository OrganizationUnitEntityRuleRepository { get; }

    public EntityTypeInfoManager(
        IEntityTypeInfoRepository entityTypeInfoRepository,
        IRoleEntityRuleRepository roleEntityRuleRepository, 
        IOrganizationUnitEntityRuleRepository organizationUnitEntityRuleRepository)
    {
        EntityTypeInfoRepository = entityTypeInfoRepository;
        RoleEntityRuleRepository = roleEntityRuleRepository;
        OrganizationUnitEntityRuleRepository = organizationUnitEntityRuleRepository;
    }

    [UnitOfWork]
    public async virtual Task<EntityTypeInfo> CreateAsync(EntityTypeInfo entityTypeInfo)
    {
        await ValidateEntityTypeInfoAsync(entityTypeInfo);

        return await EntityTypeInfoRepository.InsertAsync(entityTypeInfo);
    }

    [UnitOfWork]
    public async virtual Task<EntityTypeInfo> UpdateAsync(EntityTypeInfo entityTypeInfo)
    {
        await ValidateEntityTypeInfoAsync(entityTypeInfo);

        return await EntityTypeInfoRepository.UpdateAsync(entityTypeInfo);
    }

    public async virtual Task DeleteAsync(EntityTypeInfo entityTypeInfo)
    {
        await EntityTypeInfoRepository.DeleteAsync(entityTypeInfo);

        var roleEntityRules = await RoleEntityRuleRepository.GetListByEntityAsync(entityTypeInfo.TypeFullName);
        await RoleEntityRuleRepository.DeleteManyAsync(roleEntityRules);

        var orgEntityRules = await OrganizationUnitEntityRuleRepository.GetListByEntityAsync(entityTypeInfo.TypeFullName);
        await OrganizationUnitEntityRuleRepository.DeleteManyAsync(orgEntityRules);
    }

    protected async virtual Task ValidateEntityTypeInfoAsync(EntityTypeInfo entityTypeInfo)
    {
        if (await EntityTypeInfoRepository.FindByTypeAsync(entityTypeInfo.TypeFullName) != null)
        {
            throw new BusinessException(DataProtectionManagementErrorCodes.EntityTypeInfo.DuplicateTypeInfo)
                .WithData("Name", entityTypeInfo.TypeFullName);
        }
    }
}
