using System.Collections.Immutable;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.AdminService.Overrides;

[Dependency(ReplaceServices = true)]
public class MergePermissionDefinitionManager : PermissionDefinitionManager
{
    private readonly IStaticPermissionDefinitionStore _staticStore;
    private readonly IDynamicPermissionDefinitionStore _dynamicStore;

    public MergePermissionDefinitionManager(
        IStaticPermissionDefinitionStore staticStore,
        IDynamicPermissionDefinitionStore dynamicStore) : base(staticStore, dynamicStore)
    {
        _staticStore = staticStore;
        _dynamicStore = dynamicStore;
    }

    public async override Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        var staticGroups = await _staticStore.GetGroupsAsync();
        var dynamicGroups = await _dynamicStore.GetGroupsAsync();

        var mergedGroups = new Dictionary<string, PermissionGroupDefinition>();

        foreach (var staticGroup in staticGroups)
        {
            mergedGroups[staticGroup.Name] = staticGroup;
        }

        foreach (var dynamicGroup in dynamicGroups)
        {
            if (mergedGroups.TryGetValue(dynamicGroup.Name, out var existingGroup))
            {
                MergeGroupPermissions(existingGroup, dynamicGroup);
            }
            else
            {
                mergedGroups[dynamicGroup.Name] = dynamicGroup;
            }
        }

        return mergedGroups.Values.ToImmutableList();
    }

    private static void MergeGroupPermissions(PermissionGroupDefinition target, PermissionGroupDefinition source)
    {
        foreach (var sourcePermission in source.Permissions)
        {
            var existingPermission = target.GetPermissionOrNull(sourcePermission.Name);

            if (existingPermission == null)
            {
                var newPermission = target.AddPermission(
                    sourcePermission.Name,
                    sourcePermission.DisplayName,
                    sourcePermission.MultiTenancySide,
                    sourcePermission.IsEnabled
                );

                CopyPermissionDetails(sourcePermission, newPermission);

                foreach (var child in sourcePermission.Children)
                {
                    AddChildPermissionRecursively(newPermission, child);
                }
            }
            else
            {
                MergePermissionMetadata(existingPermission, sourcePermission);

                foreach (var sourceChild in sourcePermission.Children)
                {
                    MergeChildPermissions(existingPermission, sourceChild);
                }
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

        CopyPermissionDetails(sourceChild, newChild);

        foreach (var grandchild in sourceChild.Children)
        {
            AddChildPermissionRecursively(newChild, grandchild);
        }
    }

    private static void MergeChildPermissions(PermissionDefinition parent, PermissionDefinition sourceChild)
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
            CopyPermissionDetails(sourceChild, newChild);

            foreach (var grandchild in sourceChild.Children)
            {
                AddChildPermissionRecursively(newChild, grandchild);
            }
        }
        else
        {
            MergePermissionMetadata(existingChild, sourceChild);

            foreach (var grandchild in sourceChild.Children)
            {
                MergeChildPermissions(existingChild, grandchild);
            }
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

    private static void MergePermissionMetadata(PermissionDefinition target, PermissionDefinition source)
    {
        target.DisplayName = source.DisplayName;

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

        if (source.MultiTenancySide != MultiTenancySides.Both)
        {
            target.MultiTenancySide |= source.MultiTenancySide;
        }

        target.IsEnabled = target.IsEnabled || source.IsEnabled;
    }
}