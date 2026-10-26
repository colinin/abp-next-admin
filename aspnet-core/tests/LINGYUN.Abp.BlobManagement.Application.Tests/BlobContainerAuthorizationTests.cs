using System.Security.Claims;
using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Security.Claims;
using Volo.Abp.Specifications;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

public class BlobContainerAuthorizationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Anonymous_requests_are_rejected_before_container_access(bool isTenant)
    {
        using var application = await CreateApplicationAsync();
        var services = application.ServiceProvider;
        using var tenant = services.GetRequiredService<ICurrentTenant>().Change(isTenant ? Guid.NewGuid() : null);
        using var principal = services.GetRequiredService<ICurrentPrincipalAccessor>()
            .Change(new ClaimsPrincipal(new ClaimsIdentity()));
        var service = services.GetRequiredService<IBlobContainerAppService>();
        var state = services.GetRequiredService<ContainerAuthorizationState>();

        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.GetAsync(state.Container.Id));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.GetListAsync(new BlobContainerGetPagedListInput()));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.CreateAsync(new BlobContainerCreateDto { Name = "media" }));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.DeleteAsync(state.Container.Id));
        Assert.Empty(state.Repository.ReceivedCalls());
        Assert.Empty(state.Manager.ReceivedCalls());
        await application.ShutdownAsync();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Authenticated_users_without_permissions_cannot_read_or_write(bool isTenant)
    {
        using var application = await CreateApplicationAsync();
        var services = application.ServiceProvider;
        var tenantId = isTenant ? Guid.NewGuid() : (Guid?)null;
        using var tenant = services.GetRequiredService<ICurrentTenant>().Change(tenantId);
        using var principal = services.GetRequiredService<ICurrentPrincipalAccessor>().Change(Principal(tenantId));
        var service = services.GetRequiredService<IBlobContainerAppService>();
        var state = services.GetRequiredService<ContainerAuthorizationState>();

        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.GetAsync(state.Container.Id));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.GetListAsync(new BlobContainerGetPagedListInput()));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.CreateAsync(new BlobContainerCreateDto { Name = "media" }));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.DeleteAsync(state.Container.Id));
        Assert.Empty(state.Repository.ReceivedCalls());
        Assert.Empty(state.Manager.ReceivedCalls());
        await application.ShutdownAsync();
    }

    [Theory]
    [InlineData(false, BlobManagementPermissionNames.BlobContainer.Default, false)]
    [InlineData(true, BlobManagementPermissionNames.BlobContainer.Default, false)]
    [InlineData(false, BlobManagementPermissionNames.Blob.Default, true)]
    [InlineData(true, BlobManagementPermissionNames.Blob.Default, true)]
    public async Task Container_management_or_effective_file_permission_allows_reads(
        bool isTenant, string permission, bool blobEnabled)
    {
        using var application = await CreateApplicationAsync();
        var services = application.ServiceProvider;
        var tenantId = isTenant ? Guid.NewGuid() : (Guid?)null;
        using var tenant = services.GetRequiredService<ICurrentTenant>().Change(tenantId);
        using var principal = services.GetRequiredService<ICurrentPrincipalAccessor>().Change(Principal(tenantId));
        var service = services.GetRequiredService<IBlobContainerAppService>();
        var state = services.GetRequiredService<ContainerAuthorizationState>();
        state.Granted.Add(permission);
        state.BlobEnabled = blobEnabled;

        Assert.Equal(state.Container.Id, (await service.GetAsync(state.Container.Id)).Id);
        Assert.Single((await service.GetListAsync(new BlobContainerGetPagedListInput())).Items);
        Assert.Equal(tenantId, services.GetRequiredService<ICurrentTenant>().Id);
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.CreateAsync(new BlobContainerCreateDto { Name = "media" }));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.DeleteAsync(state.Container.Id));
        Assert.DoesNotContain(state.Manager.ReceivedCalls(), call => call.GetMethodInfo().Name != nameof(BlobManager.GetBlobProvider));
        await application.ShutdownAsync();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Disabled_file_feature_rejects_file_only_readers_before_repository_access(bool isTenant)
    {
        using var application = await CreateApplicationAsync();
        var services = application.ServiceProvider;
        var tenantId = isTenant ? Guid.NewGuid() : (Guid?)null;
        using var tenant = services.GetRequiredService<ICurrentTenant>().Change(tenantId);
        using var principal = services.GetRequiredService<ICurrentPrincipalAccessor>().Change(Principal(tenantId));
        var service = services.GetRequiredService<IBlobContainerAppService>();
        var state = services.GetRequiredService<ContainerAuthorizationState>();
        state.Granted.Add(BlobManagementPermissionNames.Blob.Default);
        state.BlobEnabled = false;

        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.GetAsync(state.Container.Id));
        await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.GetListAsync(new BlobContainerGetPagedListInput()));
        Assert.Empty(state.Repository.ReceivedCalls());
        await application.ShutdownAsync();
    }

    [Theory]
    [InlineData(false, BlobManagementPermissionNames.BlobContainer.Create)]
    [InlineData(true, BlobManagementPermissionNames.BlobContainer.Create)]
    [InlineData(false, BlobManagementPermissionNames.BlobContainer.Delete)]
    [InlineData(true, BlobManagementPermissionNames.BlobContainer.Delete)]
    public async Task Write_permissions_remain_independent(bool isTenant, string permission)
    {
        using var application = await CreateApplicationAsync();
        var services = application.ServiceProvider;
        var tenantId = isTenant ? Guid.NewGuid() : (Guid?)null;
        using var tenant = services.GetRequiredService<ICurrentTenant>().Change(tenantId);
        using var principal = services.GetRequiredService<ICurrentPrincipalAccessor>().Change(Principal(tenantId));
        var service = services.GetRequiredService<IBlobContainerAppService>();
        var state = services.GetRequiredService<ContainerAuthorizationState>();
        state.Granted.Add(permission);

        if (permission == BlobManagementPermissionNames.BlobContainer.Create)
        {
            Assert.Equal(state.Container.Id,
                (await service.CreateAsync(new BlobContainerCreateDto { Name = "media" })).Id);
            await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.DeleteAsync(state.Container.Id));
            await state.Manager.Received(1).CreateContainerAsync("media");
            await state.Manager.DidNotReceive().DeleteContainerAsync(Arg.Any<BlobContainer>());
        }
        else
        {
            await service.DeleteAsync(state.Container.Id);
            await Assert.ThrowsAsync<AbpAuthorizationException>(() => service.CreateAsync(new BlobContainerCreateDto { Name = "media" }));
            await state.Manager.Received(1).DeleteContainerAsync(state.Container);
            await state.Manager.DidNotReceive().CreateContainerAsync(Arg.Any<string>());
        }
        await application.ShutdownAsync();
    }

    [Fact]
    public async Task Authorized_delete_still_rejects_static_containers()
    {
        using var application = await CreateApplicationAsync();
        var services = application.ServiceProvider;
        using var principal = services.GetRequiredService<ICurrentPrincipalAccessor>().Change(Principal(null));
        var service = services.GetRequiredService<IBlobContainerAppService>();
        var state = services.GetRequiredService<ContainerAuthorizationState>();
        state.Granted.Add(BlobManagementPermissionNames.BlobContainer.Delete);
        services.GetRequiredService<IOptions<AbpBlobManagementOptions>>().Value.StaticContainers.Add(state.Container.Name);

        var error = await Assert.ThrowsAsync<BusinessException>(() => service.DeleteAsync(state.Container.Id));
        Assert.Equal(BlobManagementErrorCodes.Container.DeleteWithStatic, error.Code);
        await state.Manager.DidNotReceive().DeleteContainerAsync(Arg.Any<BlobContainer>());
        await application.ShutdownAsync();
    }

    private static async Task<IAbpApplicationWithInternalServiceProvider> CreateApplicationAsync()
    {
        var application = await AbpApplicationFactory.CreateAsync<ContainerAuthorizationTestModule>(options => options.UseAutofac());
        await application.InitializeAsync();
        return application;
    }

    private static ClaimsPrincipal Principal(Guid? tenantId)
    {
        var claims = new List<Claim> { new(AbpClaimTypes.UserId, Guid.NewGuid().ToString()) };
        if (tenantId.HasValue) claims.Add(new Claim(AbpClaimTypes.TenantId, tenantId.Value.ToString()));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }
}

public class ContainerAuthorizationState
{
    public HashSet<string> Granted { get; } = new(StringComparer.Ordinal);
    public bool BlobEnabled { get; set; } = true;
    public BlobContainer Container { get; } = new TestContainer();
    public IBlobContainerRepository Repository { get; } = Substitute.For<IBlobContainerRepository>();
    public BlobManager Manager { get; } = Substitute.For<BlobManager>(
        new object?[] { null, null, null, null, null, null, null, null, null, null, null });
    public IObjectMapper<AbpBlobManagementApplicationModule> Mapper { get; } =
        Substitute.For<IObjectMapper<AbpBlobManagementApplicationModule>>();

    public ContainerAuthorizationState()
    {
        Repository.GetAsync(Container.Id, Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(Container);
        Repository.GetCountAsync(Arg.Any<ISpecification<BlobContainer>>(), Arg.Any<CancellationToken>()).Returns(1);
        Repository.GetListAsync(Arg.Any<ISpecification<BlobContainer>>(), Arg.Any<string?>(),
            Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<BlobContainer> { Container });
        Manager.GetBlobProvider().Returns("test-provider");
        Manager.CreateContainerAsync(Arg.Any<string>()).Returns(Container);
        Mapper.Map<BlobContainer, BlobContainerDto>(Arg.Any<BlobContainer>())
            .Returns(call => new BlobContainerDto { Id = call.Arg<BlobContainer>().Id });
        Mapper.Map<List<BlobContainer>, List<BlobContainerDto>>(Arg.Any<List<BlobContainer>>())
            .Returns(new List<BlobContainerDto> { new() { Id = Container.Id } });
        Repository.ClearReceivedCalls();
        Manager.ClearReceivedCalls();
    }

    private sealed class TestContainer : BlobContainer
    {
        public TestContainer()
        {
            Id = Guid.NewGuid();
            Name = "media";
            SetProvider("test-provider");
        }
    }
}

[DependsOn(typeof(AbpAutofacModule), typeof(AbpAuthorizationModule), typeof(AbpFeaturesModule), typeof(AbpBlobManagementApplicationModule))]
public class ContainerAuthorizationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var state = new ContainerAuthorizationState();
        context.Services.AddSingleton(state);
        context.Services.AddSingleton(state.Repository);
        context.Services.AddSingleton(state.Manager);
        context.Services.AddSingleton(state.Mapper);
        context.Services.AddTransient<BlobManagementPermissionDefinitionProvider>();
        context.Services.AddTransient<BlobManagementFeatureDefinitionProvider>();
        var permissions = Substitute.For<IPermissionStore>();
        permissions.IsGrantedAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(call => state.Granted.Contains(call.ArgAt<string>(0)));
        permissions.IsGrantedAsync(Arg.Any<string[]>(), Arg.Any<string>(), Arg.Any<string>()).Returns(call =>
        {
            var result = new MultiplePermissionGrantResult();
            foreach (var name in call.ArgAt<string[]>(0))
                result.Result.Add(name, state.Granted.Contains(name) ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);
            return result;
        });
        context.Services.AddSingleton(permissions);
        var features = Substitute.For<IFeatureStore>();
        features.GetOrNullAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(call => call.ArgAt<string>(0) == BlobManagementFeatureNames.Blob.Enable ? state.BlobEnabled.ToString() : null);
        context.Services.AddSingleton(features);
        Configure<AbpPermissionOptions>(options =>
        {
            options.DefinitionProviders.Clear();
            options.DefinitionProviders.Add<BlobManagementPermissionDefinitionProvider>();
        });
        Configure<AbpFeatureOptions>(options =>
        {
            options.DefinitionProviders.Clear();
            options.DefinitionProviders.Add<BlobManagementFeatureDefinitionProvider>();
        });
    }
}
