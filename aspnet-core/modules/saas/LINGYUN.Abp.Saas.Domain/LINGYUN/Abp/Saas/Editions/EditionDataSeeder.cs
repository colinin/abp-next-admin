using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace LINGYUN.Abp.Saas.Editions;

public class EditionDataSeeder : IEditionDataSeeder, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; }
    protected IEditionRepository EditionRepository { get; }

    public EditionDataSeeder(
        IGuidGenerator guidGenerator,
        IEditionRepository editionRepository)
    {
        GuidGenerator = guidGenerator;
        EditionRepository = editionRepository;
    }

    public async virtual Task SeedDefaultEditionsAsync()
    {
        await AddEditionIfNotExistsAsync("Free");
        await AddEditionIfNotExistsAsync("Standard");
        await AddEditionIfNotExistsAsync("Professional");
        await AddEditionIfNotExistsAsync("Enterprise");
    }

    protected async virtual Task AddEditionIfNotExistsAsync(string displayName)
    {
        if (await EditionRepository.FindByDisplayNameAsync(displayName) != null)
        {
            return;
        }

        await EditionRepository.InsertAsync(
            new Edition(
                GuidGenerator.Create(),
                displayName
            )
        );
    }
}
