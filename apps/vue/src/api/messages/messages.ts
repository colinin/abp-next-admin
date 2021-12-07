import { defHttp } from '/@/utils/http/axios';
import {
  ChatMessagePagedResult,
  LastChatMessageListResult,
  GetUserLastMessageRequest,
  GetUserMessagePagedRequest,
  GetGroupMessagePagedRequest,
} from './model/messagesModel';

enum Api {
  GetChatMessages = '/api/im/chat/my-messages',
  GetLastMessages = '/api/im/chat/my-last-messages',
  GetGroupMessages = '/api/im/chat/group/messages',
}

export const getLastMessages = (input: GetUserLastMessageRequest) => {
  return defHttp.get<LastChatMessageListResult>({
    url: Api.GetLastMessages,
    params: input,
  });
};

export const getChatMessages = (input: GetUserMessagePagedRequest) => {
  return defHttp.get<ChatMessagePagedResult>({
    url: Api.GetChatMessages,
    params: input,
  });
};

export const getGroupMessages = (input: GetGroupMessagePagedRequest) => {
  return defHttp.get<ChatMessagePagedResult>({
    url: Api.GetGroupMessages,
    params: input,
  });
};
