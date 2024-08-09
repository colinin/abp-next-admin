
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { NotificationInfo } from '/@/api/messages/notifications/model';
import { NotificationContentType, NotificationLifetime, NotificationSeverity, NotificationType } from '/@/api/realtime/notifications/types';

interface Notification {
    name: string;
    title: string;
    message: string;
    description?: string;
    creationTime: Date;
    lifetime: NotificationLifetime;
    type: NotificationType;
    severity: NotificationSeverity;
    contentType: NotificationContentType;
    data: Recordable<string>;
}

export function useNotificationSerializer() {
    function deserialize(notificationInfo: NotificationInfo): Notification {
        const { data } = notificationInfo;
        let title = data.extraProperties.title;
        let message = data.extraProperties.message;
        let description = data.extraProperties.description;
        if (data.extraProperties.L === true || data.extraProperties.L === 'true') {
            const { L } = useLocalization(
                [data.extraProperties.title.resourceName ?? data.extraProperties.title.ResourceName,
                data.extraProperties.message.resourceName ?? data.extraProperties.message.ResourceName,
                data.extraProperties.description?.resourceName ?? data.extraProperties.description?.ResourceName ?? "AbpUi"]);
            title = L(
            data.extraProperties.title.name ?? data.extraProperties.title.Name,
            data.extraProperties.title.values ?? data.extraProperties.title.Values,
            );
            message = L(
            data.extraProperties.message.name ?? data.extraProperties.message.Name,
            data.extraProperties.message.values ?? data.extraProperties.message.Values,
            );
            if (description) {
                description = L(
                    data.extraProperties.description.name ?? data.extraProperties.description.Name,
                    data.extraProperties.description.values ?? data.extraProperties.description.Values,
                );
            }
        }
        return {
            title,
            message,
            description,
            creationTime: notificationInfo.creationTime,
            contentType: notificationInfo.contentType,
            lifetime: notificationInfo.lifetime,
            severity: notificationInfo.severity,
            type: notificationInfo.type,
            data: data.extraProperties,
            name: notificationInfo.name,
        };
    };

    return {
        deserialize,
    };
}