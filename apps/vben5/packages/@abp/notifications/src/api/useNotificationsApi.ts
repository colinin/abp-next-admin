import type { ListResultDto } from '@abp/core';

import type {
  NotificationGroupDto,
  NotificationProviderDto,
  NotificationTemplateDto,
} from '../types/definitions';
import type {
  NotificationSendInput,
  NotificationTemplateSendInput,
} from '../types/notifications';

import { useRequest } from '@abp/request';

export function useNotificationsApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取可用通知提供者列表
   * @returns {Promise<ListResultDto<NotificationProviderDto>>} 可用通知提供者列表
   */
  function getAssignableProvidersApi(): Promise<
    ListResultDto<NotificationProviderDto>
  > {
    return request<ListResultDto<NotificationProviderDto>>(
      '/api/notifications/assignable-providers',
      {
        method: 'GET',
      },
    );
  }
  /**
   * 获取可用通知列表
   * @returns {Promise<ListResultDto<NotificationGroupDto>>} 可用通知列表
   */
  function getAssignableNotifiersApi(): Promise<
    ListResultDto<NotificationGroupDto>
  > {
    return request<ListResultDto<NotificationGroupDto>>(
      '/api/notifications/assignables',
      {
        method: 'GET',
      },
    );
  }
  /**
   * 获取可用通知模板列表
   * @returns {Promise<ListResultDto<NotificationTemplateDto>>} 可用通知模板列表
   */
  function getAssignableTemplatesApi(): Promise<
    ListResultDto<NotificationTemplateDto>
  > {
    return request<ListResultDto<NotificationTemplateDto>>(
      '/api/notifications/assignable-templates',
      {
        method: 'GET',
      },
    );
  }
  /**
   * 发送通知
   * @param input 参数
   * @returns {Promise<void>}
   */
  function sendNotiferApi(input: NotificationSendInput): Promise<void> {
    return request('/api/notifications/send', {
      data: input,
      method: 'POST',
    });
  }
  /**
   * 发送模板通知
   * @param input 参数
   * @returns {Promise<void>}
   */
  function sendTemplateNotiferApi(
    input: NotificationTemplateSendInput,
  ): Promise<void> {
    return request('/api/notifications/send/template', {
      data: input,
      method: 'POST',
    });
  }

  return {
    cancel,
    getAssignableNotifiersApi,
    getAssignableProvidersApi,
    getAssignableTemplatesApi,
    sendNotiferApi,
    sendTemplateNotiferApi,
  };
}
