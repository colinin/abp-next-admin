import { useMemo } from "react";
import { useTranslation } from "react-i18next";
import { NotificationContentType, NotificationLifetime, NotificationSeverity, NotificationType } from "#/notifications";

export function useEnumMaps() {
	const { t: $t } = useTranslation();

	const notificationTypeOptions = useMemo(
		() => [
			{
				label: $t("Notifications.NotificationType:User"),
				value: NotificationType.User,
			},
			{
				label: $t("Notifications.NotificationType:System"),
				value: NotificationType.System,
			},
			{
				label: $t("Notifications.NotificationType:Application"),
				value: NotificationType.Application,
			},
			{
				label: $t("Notifications.NotificationType:ServiceCallback"),
				value: NotificationType.ServiceCallback,
			},
		],
		[$t],
	);

	const notificationTypeMap = useMemo(
		() => ({
			[NotificationType.Application]: $t("Notifications.NotificationType:Application"),
			[NotificationType.ServiceCallback]: $t("Notifications.NotificationType:ServiceCallback"),
			[NotificationType.System]: $t("Notifications.NotificationType:System"),
			[NotificationType.User]: $t("Notifications.NotificationType:User"),
		}),
		[$t],
	);

	const notificationLifetimeOptions = useMemo(
		() => [
			{
				label: $t("Notifications.NotificationLifetime:OnlyOne"),
				value: NotificationLifetime.OnlyOne,
			},
			{
				label: $t("Notifications.NotificationLifetime:Persistent"),
				value: NotificationLifetime.Persistent,
			},
		],
		[$t],
	);

	const notificationLifetimeMap = useMemo(
		() => ({
			[NotificationLifetime.OnlyOne]: $t("Notifications.NotificationLifetime:OnlyOne"),
			[NotificationLifetime.Persistent]: $t("Notifications.NotificationLifetime:Persistent"),
		}),
		[$t],
	);

	const notificationContentTypeOptions = useMemo(
		() => [
			{
				label: $t("Notifications.NotificationContentType:Text"),
				value: NotificationContentType.Text,
			},
			{
				label: $t("Notifications.NotificationContentType:Json"),
				value: NotificationContentType.Json,
			},
			{
				label: $t("Notifications.NotificationContentType:Html"),
				value: NotificationContentType.Html,
			},
			{
				label: $t("Notifications.NotificationContentType:Markdown"),
				value: NotificationContentType.Markdown,
			},
		],
		[$t],
	);

	const notificationContentTypeMap = useMemo(
		() => ({
			[NotificationContentType.Html]: $t("Notifications.NotificationContentType:Html"),
			[NotificationContentType.Json]: $t("Notifications.NotificationContentType:Json"),
			[NotificationContentType.Markdown]: $t("Notifications.NotificationContentType:Markdown"),
			[NotificationContentType.Text]: $t("Notifications.NotificationContentType:Text"),
		}),
		[$t],
	);

	const notificationSeverityOptions = useMemo(
		() => [
			{
				label: $t("Notifications.NotificationSeverity:Success"),
				value: NotificationSeverity.Success,
			},
			{
				label: $t("Notifications.NotificationSeverity:Info"),
				value: NotificationSeverity.Info,
			},
			{
				label: $t("Notifications.NotificationSeverity:Warn"),
				value: NotificationSeverity.Warn,
			},
			{
				label: $t("Notifications.NotificationSeverity:Fatal"),
				value: NotificationSeverity.Fatal,
			},
			{
				label: $t("Notifications.NotificationSeverity:Error"),
				value: NotificationSeverity.Error,
			},
		],
		[$t],
	);

	return {
		notificationContentTypeMap,
		notificationContentTypeOptions,
		notificationLifetimeMap,
		notificationLifetimeOptions,
		notificationSeverityOptions,
		notificationTypeMap,
		notificationTypeOptions,
	};
}
