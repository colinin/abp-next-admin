import type { PagedResultDto } from "#/abp-core";

import type {
	GetMyNotifilerPagedListInput,
	MarkReadStateInput,
	UserNotificationDto,
} from "#/notifications/my-notifilers";

import requestClient from "@/api/request";
/**
 * 获取我的通知列表
 * @param {GetMyNotifilerPagedListInput} input 参数
 * @returns {Promise<PagedResultDto<UserNotificationDto>>} 通知分页列表
 */
export function getMyNotifilersApi(input?: GetMyNotifilerPagedListInput): Promise<PagedResultDto<UserNotificationDto>> {
	return requestClient.get<PagedResultDto<UserNotificationDto>>("/api/notifications/my-notifilers", {
		params: input,
	});
}
/**
 * 删除我的通知
 * @param {string} id 通知id
 * @returns {void}
 */
export function deleteMyNotifilerApi(id: string): Promise<void> {
	return requestClient.delete(`/api/notifications/my-notifilers/${id}`);
}
/**
 * 设置通知已读状态
 * @param {MarkReadStateInput} input 参数
 * @returns {void}
 */
export function markReadStateApi(input: MarkReadStateInput): Promise<void> {
	return requestClient.put("/api/notifications/my-notifilers/mark-read-state", input);
}
