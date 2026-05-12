using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.BlobManagement;

public static class IBlobContainerRepositoryExtenssions
{
    public async static Task<BlobContainer> GetByNameAsync(
        this IBlobContainerRepository repository,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await repository.FindByNameAsync(name, cancellationToken)
            ?? throw new BusinessException(
                BlobManagementErrorCodes.Container.NameNotFound,
                $"There is no blob container named {name}!")
                .WithData("Name", name);
    }
}
