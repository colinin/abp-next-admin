using Dapr.Actors;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Dapr.Actors
{
    [RemoteService(Name = "Test")]
    public interface ITestActor : IActor
    {
        Task<ListResultDto<NameValue>> GetAsync();

        Task<NameValue> UpdateAsync();

        Task ClearAsync();
    }
}
