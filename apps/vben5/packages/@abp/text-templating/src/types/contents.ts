interface TextTemplateContentDto {
  content?: string;
  culture?: string;
  name: string;
}

interface TextTemplateContentGetInput {
  culture?: string;
  name: string;
}

interface TextTemplateRestoreInput {
  culture?: string;
}

interface TextTemplateContentUpdateDto {
  content: string;
  culture?: string;
}

export type {
  TextTemplateContentDto,
  TextTemplateContentGetInput,
  TextTemplateContentUpdateDto,
  TextTemplateRestoreInput,
};
