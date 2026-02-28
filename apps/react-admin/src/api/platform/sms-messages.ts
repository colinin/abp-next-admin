import type { PagedResultDto } from "#/abp-core";

import type { SmsMessageDto, SmsMessageGetListInput } from "#/platform/messages";

import requestClient from "../request";

/**
 * 获取短信消息分页列表
 * @param {EmailMessageGetListInput} input 参数
 * @returns {Promise<PagedResultDto<EmailMessageDto>>} 短信消息列表
 */
export function getPagedListApi(input?: SmsMessageGetListInput): Promise<PagedResultDto<SmsMessageDto>> {
	return requestClient.get<PagedResultDto<SmsMessageDto>>("/api/platform/messages/sms", {
		params: input,
	});
}

/**
 * 获取短信消息
 * @param id Id
 * @returns {SmsMessageDto} 短信消息
 */
export function getApi(id: string): Promise<SmsMessageDto> {
	return requestClient.get<SmsMessageDto>(`/api/platform/messages/sms/${id}`);
}

/**
 * 删除短信消息
 * @param id Id
 * @returns {void}
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/platform/messages/sms/${id}`);
}

/**
 * 发送短信消息
 * @param id Id
 * @returns {void}
 */
export function sendApi(id: string): Promise<void> {
	return requestClient.post(`/api/platform/messages/sms/${id}/send`);
}
