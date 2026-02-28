import type { PagedResultDto } from "#/abp-core";

import type {
	WebhookSendRecordDeleteManyInput,
	WebhookSendRecordDto,
	WebhookSendRecordGetListInput,
	WebhookSendRecordResendManyInput,
} from "#/webhooks/send-attempts";

import requestClient from "../request";

/**
 * 查询发送记录
 * @param id 记录Id
 * @returns 发送记录Dto
 */
export function getApi(id: string): Promise<WebhookSendRecordDto> {
	return requestClient.get<WebhookSendRecordDto>(`/api/webhooks/send-attempts/${id}`);
}

/**
 * 删除发送记录
 * @param id 记录Id
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/webhooks/send-attempts/${id}`);
}

/**
 * 批量删除发送记录
 * @param input 参数
 */
export function bulkDeleteApi(input: WebhookSendRecordDeleteManyInput): Promise<void> {
	return requestClient.delete("/api/webhooks/send-attempts/delete-many", {
		data: input,
	});
}

/**
 * 查询发送记录分页列表
 * @param input 过滤参数
 * @returns 发送记录Dto分页列表
 */
export function getPagedListApi(input: WebhookSendRecordGetListInput): Promise<PagedResultDto<WebhookSendRecordDto>> {
	return requestClient.get<PagedResultDto<WebhookSendRecordDto>>("/api/webhooks/send-attempts", {
		params: input,
	});
}

/**
 * 重新发送
 * @param id 记录Id
 */
export function reSendApi(id: string): Promise<void> {
	return requestClient.post(`/api/webhooks/send-attempts/${id}/resend`);
}

/**
 * 批量重新发送
 * @param input 参数
 */
export function bulkReSendApi(input: WebhookSendRecordResendManyInput): Promise<void> {
	return requestClient.post("/api/webhooks/send-attempts/resend-many", input);
}
