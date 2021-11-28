using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Dapr.ServiceInvocation
{
    public interface ITestAppService : IApplicationService
    {
        Task<ListResultDto<NameValue>> GetAsync();

        Task<NameValue> UpdateAsync();

        Task<TestNeedWrapObject> GetWrapedAsync(string name);
    }
}
