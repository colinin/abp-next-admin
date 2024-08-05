using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionStore : IIdentitySessionStore, ITransientDependency
{
    protected IClock Clock { get; }
    protected ICurrentUser CurrentUser { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IIdentitySessionRepository IdentitySessionRepository { get; }

    public IdentitySessionStore(
        IClock clock,
        ICurrentUser currentUser,
        IGuidGenerator guidGenerator,
        IIdentitySessionRepository identitySessionRepository)
    {
        Clock = clock;
        CurrentUser = currentUser;
        GuidGenerator = guidGenerator;
        IdentitySessionRepository = identitySessionRepository;
    }

    public async virtual Task<IdentitySession> CreateAsync(
        string sessionId,
        string device,
        string deviceInfo,
        Guid userId,
        string clientId,
        string ipAddresses,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(sessionId, nameof(sessionId));
        Check.NotNullOrWhiteSpace(device, nameof(device));

        var identitySession = new IdentitySession(
            GuidGenerator.Create(),
            sessionId,
            device,
            deviceInfo,
            userId,
            tenantId,
            clientId,
            ipAddresses,
            Clock.Now,
            Clock.Now
        );

        identitySession = await IdentitySessionRepository.InsertAsync(identitySession, cancellationToken: cancellationToken);

        return identitySession;
    }

    public async virtual Task UpdateAsync(
        IdentitySession session,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.UpdateAsync(session, cancellationToken: cancellationToken);
    }

    public async virtual Task<IdentitySession> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await IdentitySessionRepository.GetAsync(id, cancellationToken: cancellationToken);
    }

    public async virtual Task<IdentitySession> FindAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await IdentitySessionRepository.FindAsync(id, cancellationToken: cancellationToken);
    }

    public async virtual Task<IdentitySession> GetAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        return await IdentitySessionRepository.GetAsync(sessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task<IdentitySession> FindAsync(
        string sessionId, 
        CancellationToken cancellationToken = default)
    {
        return await IdentitySessionRepository.FindAsync(sessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task<IdentitySession> FindLastAsync(
        Guid userId, 
        string device,
        CancellationToken cancellationToken = default)
    {
        return await IdentitySessionRepository.FindLastAsync(userId, device, cancellationToken: cancellationToken);
    }

    public async virtual Task<bool> ExistAsync(
        string sessionId, 
        CancellationToken cancellationToken = default)
    {
        return await IdentitySessionRepository.ExistAsync(sessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task RevokeAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.DeleteAsync(id, cancellationToken: cancellationToken);
    }

    public async virtual Task RevokeAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.DeleteAllAsync(sessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task RevokeAllAsync(
        Guid userId, 
        Guid? exceptSessionId = null,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.DeleteAllAsync(userId, exceptSessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task RevokeAllAsync(
        Guid userId, 
        string device, 
        Guid? exceptSessionId = null,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.DeleteAllAsync(userId, device, exceptSessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task RevokeAllAsync(
        TimeSpan inactiveTimeSpan,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.DeleteAllAsync(inactiveTimeSpan, cancellationToken);
    }

    public async virtual Task RevokeWithAsync(
        Guid userId, 
        string device = null, 
        Guid? exceptSessionId = null,
        int maxCount = 0,
        CancellationToken cancellationToken = default)
    {
        var revokeSelessions = await IdentitySessionRepository.GetListAsync(userId, device, exceptSessionId, maxCount, cancellationToken: cancellationToken);
        if (revokeSelessions.Count < maxCount)
        {
            return;
        }

        await IdentitySessionRepository.DeleteManyAsync(
            revokeSelessions.Skip(0).Take(revokeSelessions.Count - maxCount + 1),
            cancellationToken: cancellationToken);
    }

    public async virtual Task RevokeManyAsync(
        IEnumerable<IdentitySession> identitySessions,
        CancellationToken cancellationToken = default)
    {
        await IdentitySessionRepository.DeleteManyAsync(identitySessions, cancellationToken: cancellationToken);
    }
}
