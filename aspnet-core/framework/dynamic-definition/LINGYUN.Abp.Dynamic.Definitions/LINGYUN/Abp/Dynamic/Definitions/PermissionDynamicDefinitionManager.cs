using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Dynamic.Definitions;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IPermissionDefinitionManager),
    typeof(PermissionDynamicDefinitionManager))]
public class PermissionDynamicDefinitionManager : 
    DynamicDefinitionManager<PermissionGroupDefinition, PermissionDefinition>, 
    IPermissionDefinitionManager, 
    ITransientDependency
{
    protected IStaticPermissionDefinitionStore StaticStore { get; }
    protected IDynamicPermissionDefinitionStore DynamicStore { get; }
    public PermissionDynamicDefinitionManager(
        IStaticPermissionDefinitionStore staticStore,
        IDynamicPermissionDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options) : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public async virtual Task<PermissionDefinition> GetAsync(string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined permission: " + name);
    }

    public async virtual Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        var staticGroupDefinitions = await StaticStore.GetGroupsAsync();
        var dynamicGroupDefinitions = await DynamicStore.GetGroupsAsync();

        return await GetGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions);
    }

    public async virtual Task<PermissionDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    public async virtual Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        var staticDefinitions = await StaticStore.GetPermissionsAsync();
        var dynamicDefinitions = await DynamicStore.GetPermissionsAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<PermissionDefinition> GetResourcePermissionAsync(string resourceName, string name)
    {
        return await GetResourcePermissionOrNullAsync(resourceName, name)
            ?? throw new AbpException($"Undefined resource permission: {name} for resource: {resourceName}");
    }

    public async virtual Task<PermissionDefinition?> GetResourcePermissionOrNullAsync(string resourceName, string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetResourcePermissionOrNullAsync(resourceName, name);
        var dynamicDefinition = await DynamicStore.GetResourcePermissionOrNullAsync(resourceName, name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    public async virtual Task<IReadOnlyList<PermissionDefinition>> GetResourcePermissionsAsync()
    {
        var staticDefinitions = await StaticStore.GetResourcePermissionsAsync();
        var dynamicDefinitions = await DynamicStore.GetResourcePermissionsAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    protected override string GetDefinitionKey(PermissionDefinition definition)
    {
        return definition.Name;
    }

    protected override string GetGroupDefinitionKey(PermissionGroupDefinition groupDefinition)
    {
        return groupDefinition.Name;
    }

    protected override Task<PermissionDefinition> MergeDefinitionAsync(PermissionDefinition targetDefinition, PermissionDefinition sourceDefinition)
    {
        targetDefinition.DisplayName = sourceDefinition.DisplayName;
        if (sourceDefinition.ResourceName != null)
        {
            targetDefinition.ResourceName = sourceDefinition.ResourceName;
        }
        if (sourceDefinition.ManagementPermissionName != null)
        {
            targetDefinition.ManagementPermissionName = sourceDefinition.ManagementPermissionName;
        }

        if (sourceDefinition.MultiTenancySide != MultiTenancySides.Both)
        {
            targetDefinition.MultiTenancySide |= sourceDefinition.MultiTenancySide;
        }

        targetDefinition.IsEnabled = targetDefinition.IsEnabled || sourceDefinition.IsEnabled;

        CopyPermissionDetails(targetDefinition, sourceDefinition);

        return Task.FromResult(targetDefinition);
    }

    protected async override Task MergeGroupDefinitionAsync(PermissionGroupDefinition targetGroupDefinition, PermissionGroupDefinition sourceGroupDefinition)
    {
        if (sourceGroupDefinition.DisplayName != null)
        {
            targetGroupDefinition.DisplayName = sourceGroupDefinition.DisplayName;
        }
        foreach (var property in sourceGroupDefinition.Properties)
        {
            targetGroupDefinition.Properties[property.Key] = property.Value;
        }

        foreach (var sourcePermission in sourceGroupDefinition.Permissions)
        {
            var existingPermission = targetGroupDefinition.GetPermissionOrNull(sourcePermission.Name);

            if (existingPermission == null)
            {
                var newPermission = targetGroupDefinition.AddPermission(
                    sourcePermission.Name,
                    sourcePermission.DisplayName,
                    sourcePermission.MultiTenancySide,
                    sourcePermission.IsEnabled
                );
                newPermission.ResourceName = sourcePermission.ResourceName;
                newPermission.ManagementPermissionName = sourcePermission.ManagementPermissionName;

                CopyPermissionDetails(sourcePermission, newPermission);

                foreach (var child in sourcePermission.Children)
                {
                    AddChildPermissionRecursively(newPermission, child);
                }
            }
            else
            {
                await MergeDefinitionAsync(existingPermission, sourcePermission);

                foreach (var sourceChild in sourcePermission.Children)
                {
                    await MergeChildPermissions(existingPermission, sourceChild);
                }
            }
        }
    }

    protected async override Task AfterMergeDefinitionAsync(PermissionDefinition targetDefinition, PermissionDefinition sourceDefinition)
    {
        foreach (var child in sourceDefinition.Children)
        {
            await MergeChildPermissions(targetDefinition, child);
        }
    }

    private async Task MergeChildPermissions(PermissionDefinition parent, PermissionDefinition sourceChild)
    {
        var existingChild = parent.Children.FirstOrDefault(c => c.Name == sourceChild.Name);

        if (existingChild == null)
        {
            var newChild = parent.AddChild(
                sourceChild.Name,
                sourceChild.DisplayName,
                sourceChild.MultiTenancySide,
                sourceChild.IsEnabled
            );
            newChild.ResourceName = sourceChild.ResourceName;
            newChild.ManagementPermissionName = sourceChild.ManagementPermissionName;

            CopyPermissionDetails(sourceChild, newChild);

            foreach (var grandchild in sourceChild.Children)
            {
                AddChildPermissionRecursively(newChild, grandchild);
            }
        }
        else
        {
            await MergeDefinitionAsync(existingChild, sourceChild);

            foreach (var grandchild in sourceChild.Children)
            {
                await MergeChildPermissions(existingChild, grandchild);
            }
        }
    }

    private static void AddChildPermissionRecursively(PermissionDefinition parent, PermissionDefinition sourceChild)
    {
        var newChild = parent.AddChild(
            sourceChild.Name,
            sourceChild.DisplayName,
            sourceChild.MultiTenancySide,
            sourceChild.IsEnabled
        );
        newChild.ResourceName = sourceChild.ResourceName;
        newChild.ManagementPermissionName = sourceChild.ManagementPermissionName;

        CopyPermissionDetails(sourceChild, newChild);

        foreach (var grandchild in sourceChild.Children)
        {
            AddChildPermissionRecursively(newChild, grandchild);
        }
    }

    private static void CopyPermissionDetails(PermissionDefinition source, PermissionDefinition target)
    {
        foreach (var property in source.Properties)
        {
            target.Properties[property.Key] = property.Value;
        }

        foreach (var provider in source.Providers)
        {
            if (!target.Providers.Contains(provider))
            {
                target.Providers.Add(provider);
            }
        }

        foreach (var checker in source.StateCheckers)
        {
            if (!target.StateCheckers.Contains(checker))
            {
                target.StateCheckers.Add(checker);
            }
        }
    }
}
