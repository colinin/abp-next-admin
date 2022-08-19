import { PagedAndSortedResultRequestDto } from "/@/api/model/baseModel";

export interface TextTemplateDefinition {
  name: string;
  displayName: string;
  defaultCultureName?: string;
  isInlineLocalized: boolean;
  isLayout: boolean;
  layout?: string;
}

export interface TextTemplateContent {
  name: string;
  content?: string;
  culture?: string;
}

export interface TextTemplateContentGetInput {
  name: string;
  culture?: string;
}

export interface TextTemplateRestoreInput {
  name: string;
  culture?: string;
}

export interface TextTemplateUpdateInput {
  name: string;
  culture?: string;
  content: string;
}

export interface TextTemplateDefinitionGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}
