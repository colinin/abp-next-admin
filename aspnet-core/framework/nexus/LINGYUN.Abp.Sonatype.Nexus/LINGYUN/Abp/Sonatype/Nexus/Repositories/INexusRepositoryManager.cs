using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sonatype.Nexus.Repositories;

public interface INexusRepositoryManager<TRepository, TRepositoryCreateArgs, TRepositoryUpdateArgs>
    where TRepository : NexusRepository
    where TRepositoryCreateArgs : NexusRepositoryCreateArgs
    where TRepositoryUpdateArgs: NexusRepositoryUpdateArgs
{
    Task CreateAsync(TRepositoryCreateArgs args, CancellationToken cancellationToken = default);

    Task<TRepository> GetAsync(string name, CancellationToken cancellationToken = default);

    Task UpdateAsync(string name, TRepositoryUpdateArgs args, CancellationToken cancellationToken = default);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);

    Task<List<NexusRepositoryListResult>> ListAsync(CancellationToken cancellationToken = default);
}

public interface INexusRepositoryManager<TRepository, TRepositoryCreateArgs>
    where TRepository : NexusRepository
    where TRepositoryCreateArgs : NexusRepositoryCreateArgs
{
    Task CreateAsync(TRepositoryCreateArgs args, CancellationToken cancellationToken = default);

    Task<TRepository> GetAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);

    Task<List<NexusRepositoryListResult>> ListAsync(CancellationToken cancellationToken = default);
}


public interface INexusRepositoryManager<TRepository>
    where TRepository : NexusRepository
{
    Task<TRepository> GetAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);

    Task<List<NexusRepositoryListResult>> ListAsync(CancellationToken cancellationToken = default);
}