using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtection;

public interface IEntityTypeInfoAppService : IApplicationService
{
    /// <summary>
    /// 获取实体可访问规则
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<EntityTypeInfoDto> GetEntityRuleAsync(EntityTypeInfoGetInput input);
}
