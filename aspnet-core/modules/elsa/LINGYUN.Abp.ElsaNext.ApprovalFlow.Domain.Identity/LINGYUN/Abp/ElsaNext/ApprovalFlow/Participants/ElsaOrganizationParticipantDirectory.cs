using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IIdentityRoleRepository = LINGYUN.Abp.Identity.IIdentityRoleRepository;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;

public class ElsaOrganizationParticipantDirectory : IUserTaskParticipantDirectory, ITransientDependency
{
    protected const int MaxLimit = 200;

    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

    protected IIdentityUserRepository IdentityUserRepository { get; }

    protected IIdentityRoleRepository IdentityRoleRepository { get; }

    protected ILookupNormalizer LookupNormalizer { get; }

    public ElsaOrganizationParticipantDirectory(
        IOrganizationUnitRepository organizationUnitRepository,
        IIdentityUserRepository identityUserRepository,
        IIdentityRoleRepository identityRoleRepository,
        ILookupNormalizer lookupNormalizer)
    {
        OrganizationUnitRepository = organizationUnitRepository;
        IdentityUserRepository = identityUserRepository;
        IdentityRoleRepository = identityRoleRepository;
        LookupNormalizer = lookupNormalizer;
    }

    public virtual async Task<ParticipantSearchResult> SearchAsync(UserTaskParticipantQuery query, CancellationToken cancellationToken = default)
    {
        var limit = Math.Clamp(query.Limit <= 0 ? 50 : query.Limit, 1, MaxLimit);
        var items = new List<ParticipantReference>();

        if (query.Type != UserTaskParticipantType.User)
        {
            items.AddRange((await FindOrganizationUnitsAsync(query.Search, limit, cancellationToken))
                .Select(x => ParticipantReferenceFactory.Group(query.TenantId, x.Id, x.DisplayName)));

            var roles = await IdentityRoleRepository.GetListAsync(
                maxResultCount: limit,
                filter: query.Search,
                cancellationToken: cancellationToken);
            items.AddRange(roles.Select(x => ParticipantReferenceFactory.Role(query.TenantId, x.Name)));
        }

        if (query.Type != UserTaskParticipantType.Group)
        {
            var organizationUnitIds = (await FindOrganizationUnitsAsync(query.Search, MaxLimit, cancellationToken))
                .Select(x => x.Id)
                .ToList();
            if (organizationUnitIds.Count > 0)
            {
                items.AddRange((await IdentityUserRepository.GetUsersInOrganizationsListAsync(organizationUnitIds, query.Search, 0, limit, cancellationToken))
                    .Select(x => ParticipantReferenceFactory.User(query.TenantId, x.Id, ResolveDisplayName(x))));
            }
        }

        return new ParticipantSearchResult(items.Take(limit).ToArray(), null, items.Count);
    }

    public virtual async Task<IReadOnlyCollection<ParticipantReference>> ResolveDisplayNamesAsync(
        IReadOnlyCollection<ParticipantReference> participants,
        CancellationToken cancellationToken = default)
    {
        if (participants.Count == 0)
        {
            return Array.Empty<ParticipantReference>();
        }

        var resolved = new List<ParticipantReference>();
        var userIds = participants
            .Where(x => x.Type == UserTaskParticipantType.User && x.Provider == ParticipantReferenceFactory.UserProvider)
            .Select(ParticipantReferenceFactory.TryGetId)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Distinct()
            .ToList();

        var userNames = new Dictionary<Guid, string?>();
        if (userIds.Count > 0)
        {
            foreach (var user in await IdentityUserRepository.GetListByIdListAsync(userIds, includeDetails: false, cancellationToken))
            {
                userNames[user.Id] = ResolveDisplayName(user);
            }
        }

        foreach (var participant in participants)
        {
            if (participant.Type == UserTaskParticipantType.Group && participant.Provider == ParticipantReferenceFactory.GroupProvider)
            {
                var id = ParticipantReferenceFactory.TryGetId(participant);
                if (id.HasValue)
                {
                    var organizationUnit = await OrganizationUnitRepository.FindAsync(id.Value, true, cancellationToken);
                    if (organizationUnit != null)
                    {
                        resolved.Add(participant with { DisplayName = organizationUnit.DisplayName ?? participant.DisplayName });
                        continue;
                    }
                }
            }
            else if (participant.Type == UserTaskParticipantType.Group && participant.Provider == ParticipantReferenceFactory.RoleProvider)
            {
                resolved.Add(participant with { DisplayName = participant.Id });
                continue;
            }
            else
            {
                var id = ParticipantReferenceFactory.TryGetId(participant);
                if (id.HasValue && userNames.TryGetValue(id.Value, out var displayName))
                {
                    resolved.Add(participant with { DisplayName = displayName });
                    continue;
                }
            }

            resolved.Add(participant);
        }

        return resolved;
    }

    public virtual async Task<IReadOnlyCollection<ParticipantReference>> EnumerateGroupMembersAsync(
        ParticipantReference group,
        CancellationToken cancellationToken = default)
    {
        if (group.Provider == ParticipantReferenceFactory.RoleProvider)
        {
            var users = await IdentityUserRepository.GetListByNormalizedRoleNameAsync(
                LookupNormalizer.NormalizeName(group.Id),
                includeDetails: false,
                cancellationToken);

            return users
                .Select(x => ParticipantReferenceFactory.User(group.TenantId, x.Id, ResolveDisplayName(x)))
                .ToArray();
        }

        var organizationUnitId = ParticipantReferenceFactory.TryGetId(group);
        if (!organizationUnitId.HasValue)
        {
            return Array.Empty<ParticipantReference>();
        }

        return (await IdentityUserRepository.GetUsersInOrganizationUnitAsync(organizationUnitId.Value, null, 0, MaxLimit, cancellationToken))
            .Select(x => ParticipantReferenceFactory.User(group.TenantId, x.Id, ResolveDisplayName(x)))
            .ToArray();
    }

    protected virtual async Task<List<OrganizationUnit>> FindOrganizationUnitsAsync(string? search, int limit, CancellationToken cancellationToken)
    {
        var all = await OrganizationUnitRepository.GetListAsync(null, int.MaxValue, 0, true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
        {
            return all
                .Where(x => x.DisplayName != null && x.DisplayName.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Take(limit)
                .ToList();
        }

        return all.Take(limit).ToList();
    }

    private static string? ResolveDisplayName(IdentityUser user)
    {
        return string.IsNullOrWhiteSpace(user.Name) ? user.UserName : user.Name;
    }
}
