using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public class PrivateBlobPolicyCheckProvider : IBlobPolicyCheckProvider
{
    public const string ProviderName = "users";
    public string Name => ProviderName;

    public async virtual Task CheckAsync(BlobPolicyCheckContext context)
    {
        var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
        await authorizationService.CheckAsync(context.Blob, context.PolicyName);
    }
}
