using LINGYUN.Abp.DataProtection.Stores;
using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtectionManagement;

[Authorize(DataProtectionManagementPermissionNames.SubjectStrategy.Default)]
public class SubjectStrategyAppService : DataProtectionManagementApplicationServiceBase, ISubjectStrategyAppService
{
    private readonly ISubjectStrategyRepository _repository;
    private readonly IDataProtectedStrategyStateStore _strategyStateStore;

    public SubjectStrategyAppService(
        ISubjectStrategyRepository repository,
        IDataProtectedStrategyStateStore strategyStateStore)
    {
        _repository = repository;
        _strategyStateStore = strategyStateStore;
    }

    public async virtual Task<SubjectStrategyDto> GetAsync(SubjectStrategyGetInput input)
    {
        var subjectStrategy = await _repository.FindBySubjectAsync(input.SubjectName, input.SubjectId);

        return ObjectMapper.Map<SubjectStrategy, SubjectStrategyDto>(subjectStrategy);
    }

    [Authorize(DataProtectionManagementPermissionNames.SubjectStrategy.Change)]
    public async virtual Task<SubjectStrategyDto> SetAsync(SubjectStrategySetInput input)
    {
        var subjectStrategy = await _repository.FindBySubjectAsync(input.SubjectName, input.SubjectId);
        if (subjectStrategy == null)
        {
            subjectStrategy = new SubjectStrategy(
                GuidGenerator.Create(),
                input.SubjectName,
                input.SubjectId,
                input.Strategy,
                CurrentTenant.Id)
            {
                IsEnabled = input.IsEnabled
            };

            await _repository.InsertAsync(subjectStrategy);
        }
        else
        {
            subjectStrategy.IsEnabled = input.IsEnabled;
            subjectStrategy.Strategy = input.Strategy;

            await _repository.UpdateAsync(subjectStrategy);
        }

        await _strategyStateStore.SetAsync(
            new DataProtection.DataAccessStrategyState(
                subjectStrategy.SubjectName,
                new string[] { subjectStrategy .SubjectId},
                subjectStrategy.Strategy));

        return ObjectMapper.Map<SubjectStrategy, SubjectStrategyDto>(subjectStrategy);
    }
}
