using Elsa;
using Elsa.Authorization;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using Elsa.UserTasks.Options;
using Elsa.UserTasks.Permissions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;

/// <summary>
/// Maps an ABP principal to an Elsa user task actor. ABP roles are emitted as "role" groups so approval rules and
/// candidate groups can target roles, and permissions flow from the configured permission claim types.
/// </summary>
public class AbpUserTaskIdentityResolver : IUserTaskIdentityResolver, ITransientDependency
{
    private const string ManagerRole = "user-tasks:manager";

    protected IOptions<UserTasksOptions> Options { get; }

    public AbpUserTaskIdentityResolver(IOptions<UserTasksOptions> options)
    {
        Options = options;
    }

    public virtual ValueTask<UserTaskActor?> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var options = Options.Value;

        var subjectId = principal.FindFirst(options.SubjectClaimType)?.Value
                        ?? principal.FindFirst(AbpClaimTypes.UserId)?.Value
                        ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(subjectId))
        {
            return ValueTask.FromResult<UserTaskActor?>(null);
        }

        var tenantId = principal.FindFirst(options.TenantClaimType)?.Value
                       ?? principal.FindFirst(AbpClaimTypes.TenantId)?.Value
                       ?? options.DefaultTenantId;
        var provider = principal.FindFirst(options.ProviderClaimType)?.Value ?? ParticipantReferenceFactory.UserProvider;
        var displayName = principal.FindFirst(options.DisplayNameClaimType)?.Value
                          ?? principal.FindFirst(AbpClaimTypes.Name)?.Value
                          ?? principal.FindFirst(AbpClaimTypes.UserName)?.Value
                          ?? principal.Identity?.Name;

        var subject = ParticipantReferenceFactory.User(tenantId, subjectId, displayName, provider);

        var groups = new List<ParticipantReference>();
        groups.AddRange(principal.FindAll(ClaimTypes.Role)
            .Concat(principal.FindAll("role"))
            .Select(x => x.Value)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .Select(x => ParticipantReferenceFactory.Role(tenantId, x, x)));

        groups.AddRange(options.GroupClaimTypes
            .SelectMany(type => principal.FindAll(type))
            .Select(x => x.Value)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .Select(x => new ParticipantReference(tenantId, provider, UserTaskParticipantType.Group, x)));

        var permissions = options.PermissionClaimTypes
            .SelectMany(type => principal.FindAll(type))
            .Select(x => x.Value)
            .Where(x => !string.IsNullOrWhiteSpace(x) && !string.Equals(x, PermissionNames.None, StringComparison.Ordinal))
            .ToHashSet(StringComparer.Ordinal);

        // ABP access tokens do not carry Elsa user-task permission claims. Approval users need the base
        // view/claim/complete grants so the User Tasks access policy can build a query scope; candidate
        // matching still restricts which tasks each user can actually see and act on.
        permissions.Add($"{UserTasksResourcePermissions.UserTasks}:{CoreVerbs.View}");
        permissions.Add($"{UserTasksResourcePermissions.UserTasks}:{UserTaskVerbs.Claim}");
        permissions.Add($"{UserTasksResourcePermissions.UserTasks}:{UserTaskVerbs.Complete}");

        var actor = new UserTaskActor(subject, groups.Distinct().ToArray(), displayName)
        {
            Permissions = permissions,
        };

        var isManager = principal.IsInRole(ManagerRole)
                        || actor.HasPermission(UserTasksResourcePermissions.UserTasks, UserTaskVerbs.Supervise);
        if (isManager)
        {
            permissions.Add($"{UserTasksResourcePermissions.UserTasks}:{UserTaskVerbs.Supervise}");
        }

        return ValueTask.FromResult<UserTaskActor?>(actor with { IsManager = isManager });
    }
}
