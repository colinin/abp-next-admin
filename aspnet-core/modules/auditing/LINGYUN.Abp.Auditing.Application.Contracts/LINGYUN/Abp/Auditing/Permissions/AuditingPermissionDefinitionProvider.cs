using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Auditing.Permissions
{
    public class AuditingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var auditingGroup = context.AddGroup(
                name: AuditingPermissionNames.GroupName,
                displayName: L("Permissions:Auditing"));

            var auditLogPermission= auditingGroup.AddPermission(
                name: AuditingPermissionNames.AuditLog.Default,
                displayName: L("Permissions:AuditLog"));
            auditLogPermission.AddChild(
                name: AuditingPermissionNames.AuditLog.Delete,
                displayName: L("Permissions:DeleteLog"));

            var securityLogPermission = auditingGroup.AddPermission(
                name: AuditingPermissionNames.SecurityLog.Default,
                displayName: L("Permissions:SecurityLog"));
            securityLogPermission.AddChild(
                name: AuditingPermissionNames.SecurityLog.Delete,
                displayName: L("Permissions:DeleteLog"));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<AuditLoggingResource>(name);
        }
    }
}
