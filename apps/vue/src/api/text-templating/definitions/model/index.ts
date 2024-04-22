export interface TextTemplateDefinitionDto {
  name: string;
  displayName: string;
  defaultCultureName?: string;
  isInlineLocalized: boolean;
  isLayout: boolean;
  layout?: string;
  layoutName?: string;
  isStatic: boolean;
  renderEngine?: string;
}

interface TextTemplateDefinitionCreateOrUpdateDto {
  displayName: string;
  defaultCultureName?: string;
  isInlineLocalized: boolean;
  isLayout: boolean;
  layout: string;
  isStatic: boolean;
  renderEngine?: string;
}

export interface TextTemplateDefinitionCreateDto extends TextTemplateDefinitionCreateOrUpdateDto {
  name: string;
}

export interface TextTemplateDefinitionUpdateDto extends TextTemplateDefinitionCreateOrUpdateDto, IHasConcurrencyStamp { }

export interface TextTemplateDefinitionGetListInput {
  filter?: string;
  isLayout?: boolean;
  isStatic?: boolean;
}