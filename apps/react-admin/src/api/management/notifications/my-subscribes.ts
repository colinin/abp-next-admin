import type { ListResultDto } from "#/abp-core";

import type { UserSubscreNotification } from "#/notifications/subscribes";

import requestClient from "../../request";

/**
 * 获取我的所有订阅通知
 * @returns 订阅通知列表
 */
export function getMySubscribesApi(): Promise<ListResultDto<UserSubscreNotification>> {
	return requestClient.get<ListResultDto<UserSubscreNotification>>("/api/notifications/my-subscribes/all");
}

/**
 * 订阅通知
 * @param name 通知名称
 */
export function subscribeApi(name: string): Promise<void> {
	return requestClient.post("/api/notifications/my-subscribes", {
		name,
	});
}

/**
 * 取消订阅通知
 * @param name 通知名称
 */
export function unSubscribeApi(name: string): Promise<void> {
	return requestClient.delete(`/api/notifications/my-subscribes?name=${name}`);
}
