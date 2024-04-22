export interface SettingDefinitionDto extends ExtensibleObject {
  name: string;
  displayName: string;
  description?: string;
  defaultValue?: string;
  isVisibleToClients: boolean;
  providers: string[];
  isInherited: boolean;
  isEncrypted: boolean;
  isStatic: boolean;
}

export interface SettingDefinitionGetListInput {
  filter?: string;
  providerName?: string;
}

interface SettingDefinitionCreateOrUpdateDto extends IHasConcurrencyStamp, IHasExtraProperties {
  displayName: string;
  description?: string;
  defaultValue?: string;
  isInherited: boolean;
  isEncrypted: boolean;
  isVisibleToClients: boolean;
  providers: string[];
}

export interface SettingDefinitionCreateDto extends SettingDefinitionCreateOrUpdateDto {
  name: string;
}

export type SettingDefinitionUpdateDto = SettingDefinitionCreateOrUpdateDto;
