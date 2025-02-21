import type { Notification, NotificationInfo } from "#/notifications";
import { getResources } from "../localzations/get-resources";
import { LrWithResources } from "../localzations/lr-with-resources";

export function useNotificationSerializer() {
	function deserialize(notificationInfo: NotificationInfo): Notification {
		const { data } = notificationInfo;
		let title = data.extraProperties.title;
		let message = data.extraProperties.message;
		let description = data.extraProperties.description;
		if (data.extraProperties.L === true || data.extraProperties.L === "true") {
			{
				//用于释放resources
				const resources = getResources([
					data.extraProperties.title.resourceName ?? data.extraProperties.title.ResourceName,
					data.extraProperties.message.resourceName ?? data.extraProperties.message.ResourceName,
					data.extraProperties.description?.resourceName ?? data.extraProperties.description?.ResourceName ?? "AbpUi",
				]);
				title = LrWithResources(
					resources,
					data.extraProperties.title.name ?? data.extraProperties.title.Name,
					data.extraProperties.title.values ?? data.extraProperties.title.Values,
				);
				message = LrWithResources(
					resources,
					data.extraProperties.message.name ?? data.extraProperties.message.Name,
					data.extraProperties.message.values ?? data.extraProperties.message.Values,
				);
				if (description) {
					description = LrWithResources(
						resources,
						data.extraProperties.description.name ?? data.extraProperties.description.Name,
						data.extraProperties.description.values ?? data.extraProperties.description.Values,
					);
				}
			}
		}
		return {
			contentType: notificationInfo.contentType,
			creationTime: notificationInfo.creationTime,
			data: data.extraProperties,
			description,
			lifetime: notificationInfo.lifetime,
			message,
			name: notificationInfo.name,
			severity: notificationInfo.severity,
			title,
			type: notificationInfo.type,
		};
	}

	return {
		deserialize,
	};
}
