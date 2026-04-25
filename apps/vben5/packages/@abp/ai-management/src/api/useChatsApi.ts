import type { PagedResultDto } from '@abp/core';

import type {
  ReceivedMessageEvent,
  SendTextChatMessageDto,
  TextChatMessageDto,
  TextChatMessageGetListInput,
} from '../types/chats';

import { useRequest, useSseRequest } from '@abp/request';

export function useChatsApi() {
  const { cancel, request } = useRequest();
  const { requestSSE } = useSseRequest();

  /**
   * 发送消息
   * @param input 消息体
   * @param options 选项
   * @param options.onMessage 接收SSE消息回调
   * @param options.onEnd SSE消息完成回调
   */
  function sendMessageApi(
    input: SendTextChatMessageDto,
    options?: {
      onEnd?: () => void;
      onMessage?: (event: ReceivedMessageEvent) => void;
    },
  ) {
    return requestSSE('/api/ai-management/chats', input, {
      onMessage(message) {
        const sseMessages = message.split('\n\n');
        for (const sseMessage of sseMessages) {
          if (!sseMessage.trim()) continue;
          const lines = sseMessage.split('\n');
          for (const line of lines) {
            if (line.startsWith('data:')) {
              const data = line.slice(5).trim();
              const sseData = JSON.parse(data) as { m: string };
              if (sseData.m === 'FINISHED') return;
              options?.onMessage && options?.onMessage({ content: sseData.m });
            }
          }
        }
      },
      onEnd() {
        options?.onEnd && options?.onEnd();
      },
      headers: {
        'content-type': 'application/json',
      },
    });
  }

  /**
   * 获取会话内容分页列表
   * @param input 查询过滤参数
   * @returns 会话内容分页列表
   */
  function getPagedListApi(
    input: TextChatMessageGetListInput,
  ): Promise<PagedResultDto<TextChatMessageDto>> {
    return request<PagedResultDto<TextChatMessageDto>>(
      '/api/ai-management/chats',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  return {
    cancel,
    getPagedListApi,
    sendMessageApi,
  };
}
