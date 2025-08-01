﻿using PackageName.CompanyName.ProjectName.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PackageName.CompanyName.ProjectName.Permissions;

public class ProjectNamePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(ProjectNamePermissions.GroupName, L("Permission:ProjectName"));

        group.AddPermission(
            ProjectNamePermissions.ManageSettings,
            L("Permission:ManageSettings"));
        
        var userPermission = group.AddPermission(ProjectNamePermissions.User.Default, L("Permission:User"));
        userPermission.AddChild(ProjectNamePermissions.User.Create, L("Permission:Create"));
        userPermission.AddChild(ProjectNamePermissions.User.Update, L("Permission:Update"));
        userPermission.AddChild(ProjectNamePermissions.User.Delete, L("Permission:Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProjectNameResource>(name);
    }
}
