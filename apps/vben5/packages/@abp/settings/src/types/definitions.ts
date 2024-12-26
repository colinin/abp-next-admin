import type {
  ExtensibleObject,
  IHasConcurrencyStamp,
  IHasExtraProperties,
} from '@abp/core';

interface SettingDefinitionDto extends ExtensibleObject {
  defaultValue?: string;
  description?: string;
  displayName: string;
  isEncrypted: boolean;
  isInherited: boolean;
  isStatic: boolean;
  isVisibleToClients: boolean;
  name: string;
  providers: string[];
}

interface SettingDefinitionGetListInput {
  filter?: string;
  providerName?: string;
}

interface SettingDefinitionCreateOrUpdateDto
  extends IHasConcurrencyStamp,
    IHasExtraProperties {
  defaultValue?: string;
  description?: string;
  displayName: string;
  isEncrypted: boolean;
  isInherited: boolean;
  isVisibleToClients: boolean;
  providers: string[];
}

interface SettingDefinitionCreateDto
  extends SettingDefinitionCreateOrUpdateDto {
  name: string;
}

type SettingDefinitionUpdateDto = SettingDefinitionCreateOrUpdateDto;

export type {
  SettingDefinitionCreateDto,
  SettingDefinitionDto,
  SettingDefinitionGetListInput,
  SettingDefinitionUpdateDto,
};
