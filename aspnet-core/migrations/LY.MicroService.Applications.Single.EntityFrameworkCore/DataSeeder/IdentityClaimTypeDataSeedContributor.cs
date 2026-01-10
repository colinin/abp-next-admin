using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;
public class IdentityClaimTypeDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public ILogger<IdentityClaimTypeDataSeedContributor> Logger { protected get; set; }

    protected IGuidGenerator GuidGenerator { get; }
    protected IdentityClaimTypeManager IdentityClaimTypeManager { get; }
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }
    public IdentityClaimTypeDataSeedContributor(
        IGuidGenerator guidGenerator, 
        IdentityClaimTypeManager identityClaimTypeManager, 
        IIdentityClaimTypeRepository identityClaimTypeRepository)
    {
        GuidGenerator = guidGenerator;
        IdentityClaimTypeManager = identityClaimTypeManager;
        IdentityClaimTypeRepository = identityClaimTypeRepository;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId.HasValue)
        {
            return;
        }

        Logger.LogInformation("Seeding the default identity claim types...");

        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Address);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Birthdate, valueType: IdentityClaimValueType.DateTime);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Country);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Email);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.EmailVerified, valueType: IdentityClaimValueType.Boolean);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.FamilyName);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Gender);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.GivenName);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Locale);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Locality);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.MiddleName);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Name);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Nickname);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.PhoneNumber);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.PhoneNumberVerified, valueType: IdentityClaimValueType.Boolean);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Picture);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.PostalCode);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.PreferredUsername);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Profile);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Region);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Role);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.StreetAddress);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Username);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Website);
        await CreateIdentityClaimTypeAsync(OpenIddictConstants.Claims.Zoneinfo);

        Logger.LogInformation("Seeding default identity claim types completed.");
    }

    private async Task CreateIdentityClaimTypeAsync(
        [NotNull] string name,
        bool required = false,
        bool isStatic = false,
        [CanBeNull] string regex = null,
        [CanBeNull] string regexDescription = null,
        [CanBeNull] string description = null,
        IdentityClaimValueType valueType = IdentityClaimValueType.String)
    {
        if (!await IdentityClaimTypeRepository.AnyAsync(name))
        {
            await IdentityClaimTypeManager.CreateAsync(
                new IdentityClaimType(
                    GuidGenerator.Create(),
                    name,
                    required,
                    isStatic,
                    regex,
                    regexDescription,
                    description,
                    valueType));
        }
    }
}
