import type { IHasExtraProperties } from '@abp/core';

export enum MultiTenancySides {
  Both = 0x3,
  Host = 0x2,
  Tenant = 0x1,
}
interface PermissionDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
  isEnabled: boolean;
  parentName?: string;
  providers: string[];
  stateCheckers: string;
}

interface PermissionDefinitionCreateDto
  extends PermissionDefinitionCreateOrUpdateDto {
  groupName: string;
  name: string;
}

interface PermissionDefinitionDto extends IHasExtraProperties {
  displayName: string;
  groupName: string;
  isEnabled: boolean;
  isStatic: boolean;
  multiTenancySide: MultiTenancySides;
  name: string;
  parentName?: string;
  providers: string[];
  stateCheckers: string;
}

interface PermissionDefinitionGetListInput {
  filter?: string;
  groupName?: string;
}

type PermissionDefinitionUpdateDto = PermissionDefinitionCreateOrUpdateDto;

export type {
  PermissionDefinitionCreateDto,
  PermissionDefinitionDto,
  PermissionDefinitionGetListInput,
  PermissionDefinitionUpdateDto,
};
