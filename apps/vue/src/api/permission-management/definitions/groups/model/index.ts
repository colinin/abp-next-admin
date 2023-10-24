interface PermissionGroupDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
}

export interface PermissionGroupDefinitionCreateDto extends PermissionGroupDefinitionCreateOrUpdateDto {
  name: string;
}

export interface PermissionGroupDefinitionDto extends IHasExtraProperties {
  name: string;
  displayName: string;
  isStatic: boolean;
}

export interface PermissionGroupDefinitionGetListInput {
  filter?: string;
}

export type PermissionGroupDefinitionUpdateDto = PermissionGroupDefinitionCreateOrUpdateDto;