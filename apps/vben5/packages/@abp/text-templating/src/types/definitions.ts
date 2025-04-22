import type { ExtensibleObject, IHasConcurrencyStamp } from '@abp/core';

interface TextTemplateDefinitionDto
  extends ExtensibleObject,
    IHasConcurrencyStamp {
  defaultCultureName?: string;
  displayName: string;
  isInlineLocalized: boolean;
  isLayout: boolean;
  isStatic: boolean;
  layout?: string;
  localizationResourceName?: string;
  name: string;
  renderEngine?: string;
}

interface TextTemplateDefinitionCreateOrUpdateDto {
  defaultCultureName?: string;
  displayName: string;
  isInlineLocalized: boolean;
  isLayout: boolean;
  layout?: string;
  localizationResourceName?: string;
  renderEngine?: string;
}

interface TextTemplateDefinitionCreateDto
  extends TextTemplateDefinitionCreateOrUpdateDto {
  name: string;
}

interface TextTemplateDefinitionUpdateDto
  extends IHasConcurrencyStamp,
    TextTemplateDefinitionCreateOrUpdateDto {}

interface TextTemplateDefinitionGetListInput {
  filter?: string;
  isLayout?: boolean;
  isStatic?: boolean;
}

export type {
  TextTemplateDefinitionCreateDto,
  TextTemplateDefinitionDto,
  TextTemplateDefinitionGetListInput,
  TextTemplateDefinitionUpdateDto,
};
