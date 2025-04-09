using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtectionManagement;

public interface ISubjectStrategyAppService : IApplicationService
{
    Task<SubjectStrategyDto> GetAsync(SubjectStrategyGetInput input);

    Task<SubjectStrategyDto> SetAsync(SubjectStrategySetInput input);
}
