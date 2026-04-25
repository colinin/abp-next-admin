import type { PagedResultDto } from "#/abp-core";

import type { EmailMessageDto, EmailMessageGetListInput } from "#/platform/messages";

import requestClient from "../request";

/**
 * 获取邮件消息分页列表
 * @param {EmailMessageGetListInput} input 参数
 * @returns {Promise<PagedResultDto<EmailMessageDto>>} 邮件消息列表
 */
export function getPagedListApi(input?: EmailMessageGetListInput): Promise<PagedResultDto<EmailMessageDto>> {
	return requestClient.get<PagedResultDto<EmailMessageDto>>("/api/platform/messages/email", {
		params: input,
	});
}

/**
 * 获取邮件消息
 * @param id Id
 * @returns {EmailMessageDto} 邮件消息
 */
export function getApi(id: string): Promise<EmailMessageDto> {
	return requestClient.get<EmailMessageDto>(`/api/platform/messages/email/${id}`);
}

/**
 * 删除邮件消息
 * @param id Id
 * @returns {void}
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/platform/messages/email/${id}`);
}

/**
 * 发送邮件消息
 * @param id Id
 * @returns {void}
 */
export function sendApi(id: string): Promise<void> {
	return requestClient.post(`/api/platform/messages/email/${id}/send`);
}
