using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.Utils
{
    [Dependency(ServiceLifetime.Singleton, TryRegister = true)]
    [ExposeServices(typeof(ISnowflakeIdGenerator))]
    public class SnowflakeIdGenerator : ISnowflakeIdGenerator
    {
        public long Create()
        {
            return SnowflakeId.Default().NextId();
        }
    }
}
