import type React from "react";
import { useEffect, useState } from "react";
import { Card, Collapse, List, Switch, Skeleton } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { getMySubscribesApi, subscribeApi, unSubscribeApi } from "@/api/management/notifications/my-subscribes";
import { getAssignableNotifiersApi } from "@/api/management/notifications/notifications";

interface NotificationItem {
	description?: string;
	displayName: string;
	isSubscribe: boolean;
	loading: boolean;
	name: string;
}

interface NotificationGroup {
	displayName: string;
	name: string;
	notifications: NotificationItem[];
}

const NotificationSettings: React.FC = () => {
	const { t: $t } = useTranslation();
	const [loading, setLoading] = useState(false);
	const [notificationGroups, setNotificationGroups] = useState<NotificationGroup[]>([]);

	useEffect(() => {
		initData();
	}, []);

	const initData = async () => {
		try {
			setLoading(true);
			const [subRes, notifierRes] = await Promise.all([getMySubscribesApi(), getAssignableNotifiersApi()]);

			const groups: NotificationGroup[] = notifierRes.items.map((group) => {
				const notifications: NotificationItem[] = group.notifications.map((notification) => ({
					description: notification.description,
					displayName: notification.displayName,
					// Check if the user is already subscribed
					isSubscribe: subRes.items.some((x) => x.name === notification.name),
					loading: false,
					name: notification.name,
				}));

				return {
					displayName: group.displayName,
					name: group.name,
					notifications,
				};
			});

			setNotificationGroups(groups);
		} catch (error) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	const handleSubscribeChange = async (checked: boolean, groupIndex: number, itemIndex: number) => {
		const targetGroup = notificationGroups[groupIndex];
		const targetItem = targetGroup.notifications[itemIndex];

		// Optimistic Update / Loading State
		const updateState = (isLoading: boolean, isSubscribed: boolean) => {
			setNotificationGroups((prev) => {
				const next = [...prev];
				const group = { ...next[groupIndex] };
				const items = [...group.notifications];
				items[itemIndex] = { ...items[itemIndex], loading: isLoading, isSubscribe: isSubscribed };
				group.notifications = items;
				next[groupIndex] = group;
				return next;
			});
		};

		updateState(true, checked);

		try {
			if (checked) {
				await subscribeApi(targetItem.name);
			} else {
				await unSubscribeApi(targetItem.name);
			}
			toast.success($t("AbpUi.SavedSuccessfully"));
			updateState(false, checked);
		} catch (error) {
			console.error(error);
			// Revert on error
			updateState(false, !checked);
		}
	};

	if (loading && notificationGroups.length === 0) {
		return (
			<Card title={$t("abp.account.settings.noticeSettings")} bordered={false}>
				<Skeleton active paragraph={{ rows: 6 }} />
			</Card>
		);
	}

	return (
		<Card title={$t("abp.account.settings.noticeSettings")} bordered={false}>
			<Collapse defaultActiveKey={notificationGroups.map((g) => g.name)}>
				{notificationGroups.map((group, groupIndex) => (
					<Collapse.Panel header={group.displayName} key={group.name}>
						<List
							itemLayout="horizontal"
							dataSource={group.notifications}
							renderItem={(item, itemIndex) => (
								<List.Item
									actions={[
										<Switch
											key="switch"
											checked={item.isSubscribe}
											loading={item.loading}
											onChange={(checked) => handleSubscribeChange(checked, groupIndex, itemIndex)}
										/>,
									]}
								>
									<List.Item.Meta title={item.displayName} description={item.description} />
								</List.Item>
							)}
						/>
					</Collapse.Panel>
				))}
			</Collapse>
		</Card>
	);
};

export default NotificationSettings;
