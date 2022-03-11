using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.Saas.Editions;

public class EditionManager : DomainService
{
    protected IEditionRepository EditionRepository { get; }

    public EditionManager(IEditionRepository editionRepository)
    {
        EditionRepository = editionRepository;

    }

    public virtual async Task<Edition> CreateAsync(string displayName)
    {
        Check.NotNull(displayName, nameof(displayName));

        await ValidateDisplayNameAsync(displayName);
        return new Edition(GuidGenerator.Create(), displayName);
    }

    public virtual async Task ChangeDisplayNameAsync(Edition edition, string displayName)
    {
        Check.NotNull(edition, nameof(edition));
        Check.NotNull(displayName, nameof(displayName));

        await ValidateDisplayNameAsync(displayName, edition.Id);

        edition.SetDisplayName(displayName);
    }

    protected virtual async Task ValidateDisplayNameAsync(string displayName, Guid? expectedId = null)
    {
        var edition = await EditionRepository.FindByDisplayNameAsync(displayName);
        if (edition != null && edition.Id != expectedId)
        {
            throw new BusinessException("LINGYUN.Edition:010001")
                .WithData(nameof(Edition.DisplayName), displayName);
        }
    }
}
