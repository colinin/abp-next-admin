export enum MultiTenancySides {
  Tenant = 0x1,
  Host = 0x2,
  Both = 0x3,
}

interface PermissionDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
  parentName?: string;
  isEnabled: boolean;
  providers: string[];
  stateCheckers: string;
}

export interface PermissionDefinitionCreateDto extends PermissionDefinitionCreateOrUpdateDto {
  groupName: string;
  name: string;
}

export interface PermissionDefinitionDto extends IHasExtraProperties {
  groupName: string;
  name: string;
  displayName: string;
  parentName?: string;
  isEnabled: boolean;
  isStatic: boolean;
  providers: string[];
  stateCheckers: string;
}

export interface PermissionDefinitionGetListInput {
  filter?: string;
  groupName?: string;
}

export type PermissionDefinitionUpdateDto = PermissionDefinitionCreateOrUpdateDto;
