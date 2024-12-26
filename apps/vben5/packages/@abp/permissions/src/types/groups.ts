import type { IHasExtraProperties } from '@abp/core';

interface PermissionGroupDefinitionCreateOrUpdateDto
  extends IHasExtraProperties {
  displayName: string;
}
interface PermissionGroupDefinitionCreateDto
  extends PermissionGroupDefinitionCreateOrUpdateDto {
  name: string;
}
interface PermissionGroupDefinitionDto extends IHasExtraProperties {
  displayName: string;
  isStatic: boolean;
  name: string;
}
interface PermissionGroupDefinitionGetListInput {
  filter?: string;
}
type PermissionGroupDefinitionUpdateDto =
  PermissionGroupDefinitionCreateOrUpdateDto;

export type {
  PermissionGroupDefinitionCreateDto,
  PermissionGroupDefinitionDto,
  PermissionGroupDefinitionGetListInput,
  PermissionGroupDefinitionUpdateDto,
};
