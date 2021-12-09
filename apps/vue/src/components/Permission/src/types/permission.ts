export interface PermissionProps {
  providerName: string;
  providerKey?: string;
  readonly: boolean;
}

export interface PermissionTree {
  /** 权限标识 */
  name: string;
  /** 显示名称 */
  displayName: string;
  /** 是否授权 */
  isGranted?: boolean;
  /** 是否禁用 */
  disabled: boolean;
  /** 子节点 */
  children: PermissionTree[];
  /** 父节点 */
  parentName?: string;
}
