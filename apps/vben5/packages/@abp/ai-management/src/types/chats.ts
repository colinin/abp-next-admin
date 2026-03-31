import type {
  ExtensibleAuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

type ChatRole = 'assistant' | 'system' | 'tool' | 'user';

interface ChatMessageDto extends ExtensibleAuditedEntityDto<string> {
  conversationId?: string;
  createdAt: Date;
  replyAt?: Date;
  replyMessage?: string;
  role: ChatRole;
  userId?: string;
  workspace: string;
}

interface TextChatMessageDto extends ChatMessageDto {
  content: string;
}

interface TextChatMessageGetListInput extends PagedAndSortedResultRequestDto {
  conversationId: string;
}

interface SendTextChatMessageDto {
  content: string;
  conversationId: string;
  workspace: string;
}

interface ReceivedMessageEvent {
  content: string;
}

export type {
  ChatRole,
  ReceivedMessageEvent,
  SendTextChatMessageDto,
  TextChatMessageDto,
  TextChatMessageGetListInput,
};
