import { IPermission } from '../../model/baseModel';

export class PermissionProvider {
  providerName!: string;
  providerKey?: string;
}

export interface IPermissionGrant {
  allowedProviders: string[];
  grantedProviders: PermissionProvider[];
  displayName: string;
  isGranted: boolean;
  name: string;
  parentName?: string;
}

export class Permission implements IPermissionGrant {
  allowedProviders: string[] = [];
  grantedProviders: PermissionProvider[] = [];
  displayName!: string;
  isGranted!: boolean;
  name!: string;
  parentName?: string;
}

export interface PermissionGroup {
  displayName: string;
  name: string;
  permissions: Permission[];
}

export class UpdatePermission implements IPermission {
  name!: string;
  isGranted!: boolean;
}

export class UpdatePermissions {
  permissions!: UpdatePermission[];
}

export class PermissionResult {
  entityDisplayName!: string;
  groups: PermissionGroup[] = [];
}
