import type { ListResultDto } from "#/abp-core";

import type { NotificationGroupDto, NotificationTemplateDto } from "#/notifications/definitions";
import type { NotificationSendInput, NotificationTemplateSendInput } from "#/notifications/notifications";

import requestClient from "@/api/request";

/**
 * 获取可用通知列表
 * @returns {Promise<ListResultDto<NotificationGroupDto>>} 可用通知列表
 */
export function getAssignableNotifiersApi(): Promise<ListResultDto<NotificationGroupDto>> {
	return requestClient.get<ListResultDto<NotificationGroupDto>>("/api/notifications/assignables");
}
/**
 * 获取可用通知模板列表
 * @returns {Promise<ListResultDto<NotificationTemplateDto>>} 可用通知模板列表
 */
export function getAssignableTemplatesApi(): Promise<ListResultDto<NotificationTemplateDto>> {
	return requestClient.get<ListResultDto<NotificationTemplateDto>>("/api/notifications/assignable-templates", {
		method: "GET",
	});
}
/**
 * 发送通知
 * @param input 参数
 * @returns {Promise<void>}
 */
export function sendNotiferApi(input: NotificationSendInput): Promise<void> {
	return requestClient.post("/api/notifications/send", input);
}
/**
 * 发送模板通知
 * @param input 参数
 * @returns {Promise<void>}
 */
export function sendTemplateNotiferApi(input: NotificationTemplateSendInput): Promise<void> {
	return requestClient.post("/api/notifications/send/template", input);
}
