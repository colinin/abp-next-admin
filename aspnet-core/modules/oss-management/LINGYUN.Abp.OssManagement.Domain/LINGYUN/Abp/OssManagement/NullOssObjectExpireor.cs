using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OssManagement;

[Dependency(TryRegister = true)]
public class NullOssObjectExpireor : IOssObjectExpireor, ISingletonDependency
{
    public readonly static IOssObjectExpireor Instance = new NullOssObjectExpireor();

    public Task ExpireAsync(ExprieOssObjectRequest request)
    {
        return Task.CompletedTask;
    }
}
