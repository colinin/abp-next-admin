import { defHttp } from '/@/utils/http/axios';
import {
  GetUserLastMessageRequest,
  GetUserMessagePagedRequest,
  GetGroupMessagePagedRequest,
  ChatMessage,
  LastChatMessage,
} from './model';

export const getLastMessages = (input: GetUserLastMessageRequest) => {
  return defHttp.get<ListResultDto<LastChatMessage>>({
    url: '/api/im/chat/my-last-messages',
    params: input,
  });
};

export const getChatMessages = (input: GetUserMessagePagedRequest) => {
  return defHttp.get<PagedResultDto<ChatMessage>>({
    url: '/api/im/chat/my-messages',
    params: input,
  });
};

export const getGroupMessages = (input: GetGroupMessagePagedRequest) => {
  return defHttp.get<PagedResultDto<ChatMessage>>({
    url: '/api/im/chat/group/messages',
    params: input,
  });
};
