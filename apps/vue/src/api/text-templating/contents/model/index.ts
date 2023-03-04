export interface TextTemplateContentDto {
  name: string;
  content?: string;
  culture?: string;
}

export interface TextTemplateContentGetInput {
  name: string;
  culture?: string;
}

export interface TextTemplateRestoreInput {
  culture?: string;
}

export interface TextTemplateContentUpdateDto {
  culture?: string;
  content: string;
}
