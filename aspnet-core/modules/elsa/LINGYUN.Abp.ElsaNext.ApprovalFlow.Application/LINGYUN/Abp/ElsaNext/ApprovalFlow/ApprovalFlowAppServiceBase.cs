using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Actors;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

public abstract class ApprovalFlowAppServiceBase : ApplicationService
{
    protected IApprovalActorAccessor ActorAccessor =>
        LazyServiceProvider.LazyGetRequiredService<IApprovalActorAccessor>();

    protected virtual async Task<UserTaskActor> GetRequiredActorAsync()
    {
        var actor = await ActorAccessor.GetAsync();
        if (actor == null)
        {
            throw new AbpAuthorizationException("The current user could not be resolved as an approval actor.");
        }

        return actor;
    }
}
