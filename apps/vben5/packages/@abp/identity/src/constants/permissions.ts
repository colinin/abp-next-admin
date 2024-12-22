/** 用户权限 */
export const IdentityUserPermissions = {
  /** 新增 */
  Create: 'AbpIdentity.Users.Create',
  /** 删除 */
  Delete: 'AbpIdentity.Users.Delete',
  /** 管理声明 */
  ManageClaims: 'AbpIdentity.Users.ManageClaims',
  /** 管理权限 */
  ManagePermissions: 'AbpIdentity.Users.ManagePermissions',
  /** 更新 */
  Update: 'AbpIdentity.Users.Update',
};
/** 角色权限 */
export const IdentityRolePermissions = {
  /** 新增 */
  Create: 'AbpIdentity.Roles.Create',
  /** 删除 */
  Delete: 'AbpIdentity.Roles.Delete',
  /** 管理声明 */
  ManageClaims: 'AbpIdentity.Roles.ManageClaims',
  /** 管理权限 */
  ManagePermissions: 'AbpIdentity.Roles.ManagePermissions',
  /** 更新 */
  Update: 'AbpIdentity.Roles.Update',
};
/** 声明类型权限 */
export const IdentityClaimTypePermissions = {
  /** 新增 */
  Create: 'AbpIdentity.IdentityClaimTypes.Create',
  /** 删除 */
  Delete: 'AbpIdentity.IdentityClaimTypes.Delete',
  /** 更新 */
  Update: 'AbpIdentity.IdentityClaimTypes.Update',
};
/** 组织机构权限 */
export const OrganizationUnitPermissions = {
  /** 管理角色 */
  ManageRoles: 'AbpIdentity.OrganizationUnits.ManageRoles',
  /** 管理用户 */
  ManageUsers: 'AbpIdentity.OrganizationUnits.ManageUsers',
};
/** 安全日志权限 */
export const SecurityLogPermissions = {
  /** 删除 */
  Delete: 'AbpAuditing.SecurityLog.Delete',
};
/**
 * 搜索用户权限
 * @deprecated 后台服务删除权限后将无法使用.
 * @todo 需要检查abp框架权限定义
 */
export const UserLookupPermissions = {
  Default: 'AbpIdentity.UserLookup',
};
