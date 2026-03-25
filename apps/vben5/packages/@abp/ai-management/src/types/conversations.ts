import type {
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface ConversationDto extends AuditedEntityDto<string> {
  createdAt: Date;
  expiredAt: Date;
  name: string;
  updateAt?: Date;
  workspace: string;
}

interface ConversationGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface ConversationCreateDto {
  name?: string;
  workspace: string;
}

interface ConversationUpdateDto {
  name: string;
}

export type {
  ConversationCreateDto,
  ConversationDto,
  ConversationGetPagedListInput,
  ConversationUpdateDto,
};
