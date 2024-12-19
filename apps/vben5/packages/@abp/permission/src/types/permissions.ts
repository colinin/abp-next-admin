interface PermissionProvider {
  providerKey?: string;
  providerName: string;
}

interface PermissionDto {
  allowedProviders: string[];
  displayName: string;
  grantedProviders: PermissionProvider[];
  isGranted: boolean;
  name: string;
  parentName?: string;
}

interface PermissionGroupDto {
  displayName: string;
  name: string;
  permissions: PermissionDto[];
}

interface PermissionUpdateDto {
  /** 是否授权 */
  isGranted: boolean;
  /** 权限名称 */
  name: string;
}

interface PermissionsUpdateDto {
  permissions: PermissionUpdateDto[];
}

interface PermissionResultDto {
  entityDisplayName: string;
  groups: PermissionGroupDto[];
}

interface PermissionTree {
  /** 子节点 */
  children: PermissionTree[];
  /** 是否禁用 */
  disabled: boolean;
  /** 显示名称 */
  displayName: string;
  /** 是否授权 */
  isGranted?: boolean;
  isRoot: boolean;
  /** 权限标识 */
  name: string;
  /** 父节点 */
  parentName?: string;
}

export type {
  PermissionDto,
  PermissionGroupDto,
  PermissionProvider,
  PermissionResultDto,
  PermissionsUpdateDto,
  PermissionTree,
};
