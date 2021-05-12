using Dapr.Actors;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.Dapr.Actors
{
    [RemoteService(Name = "Test")]
    public interface ITestActor : IActor
    {
        Task<List<NameValue>> GetAsync();

        Task<NameValue> UpdateAsync();

        Task ClearAsync();
    }
}
