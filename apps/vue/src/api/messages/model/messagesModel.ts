import {
  ExtensibleObject,
  ILimitedResultRequest,
  ISortedResultRequest,
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

export enum MessageType {
  Text = 0,
  Image = 10,
  Link = 20,
  Video = 30,
  Voice = 40,
  File = 50,
}

export enum MessageState {
  Send = 0,
  Read = 1,
  ReCall = 10,
  Failed = 50,
  BackTo = 100,
}

export enum MessageSourceTye {
  User = 0,
  System = 10,
}

export interface ChatMessage extends ExtensibleObject {
  // tenantId: string;
  groupId?: string;
  groupName?: string;
  messageId: string;
  formUserId: string;
  formUserName: string;
  toUserId: string;
  content: string;
  sendTime: Date;
  isAnonymous: boolean;
  messageType: MessageType;
  source: MessageSourceTye;
}

export interface LastChatMessage {
  avatar: string;
  object: string;
  groupId?: string;
  groupName?: string;
  messageId: string;
  formUserId: string;
  formUserName: string;
  toUserId: string;
  content: string;
  sendTime: Date;
  isAnonymous: boolean;
  messageType: MessageType;
  source: MessageSourceTye;
}

export interface GetUserLastMessageRequest extends ILimitedResultRequest, ISortedResultRequest {
  state?: MessageState;
}

export interface GetUserMessagePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
  receiveUserId: string;
  messageType?: MessageType;
}

export interface GetGroupMessagePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
  groupId: string;
  messageType?: MessageType;
}

export interface ChatMessagePagedResult extends PagedResultDto<ChatMessage> {}

export interface ChatMessageListResult extends ListResultDto<ChatMessage> {}

export interface LastChatMessageListResult extends ListResultDto<LastChatMessage> {}
