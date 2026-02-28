import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space, type MenuProps } from "antd";
import { DeleteOutlined, DownOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { type Notification, NotificationReadState, NotificationType } from "#/notifications";
import {
	deleteMyNotifilerApi,
	getMyNotifilersApi,
	markReadStateApi,
} from "@/api/management/notifications/my-notifications";
import MyNotificationModal from "./my-notification-modal";
import { formatToDateTime } from "@/utils/abp";
import { Icon } from "@iconify/react";
import { toast } from "sonner";
import { useNotificationSerializer } from "@/utils/abp/notifications/useNotificationSerializer";

const MyNotificationTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const [modal, contextHolder] = Modal.useModal();
	const queryClient = useQueryClient();

	const { deserialize } = useNotificationSerializer();
	const [selectedRows, setSelectedRows] = useState<Notification[]>([]);
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedNotification, setSelectedNotification] = useState<Notification>();

	// Delete notification mutation
	const { mutateAsync: deleteNotification } = useMutation({
		mutationFn: deleteMyNotifilerApi,
		onSuccess: () => {
			toast.success($t("AbpUi.SuccessfullyDeleted"));
			queryClient.invalidateQueries({ queryKey: ["notifications"] });
			actionRef.current?.reload();
		},
	});

	// Mark read state mutation
	const { mutateAsync: markReadState } = useMutation({
		mutationFn: markReadStateApi,
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["notifications"] });
			actionRef.current?.reload();
		},
	});

	const handleDelete = (notification: Notification) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: notification.title }),
			onOk: () => deleteNotification(notification.id),
		});
	};

	const handleRead = async (ids: string[], state: NotificationReadState) => {
		await markReadState({ idList: ids, state });
		queryClient.invalidateQueries({ queryKey: ["notifications"] });
		setSelectedRows([]);
	};

	const handleClickNotification = (notification: Notification) => {
		setSelectedNotification(notification);
		setModalVisible(true);
		handleRead([notification.id], NotificationReadState.Read);
	};

	const bulkActionMenu: MenuProps["items"] = [
		{
			key: "read",
			icon: <Icon icon="ic:outline-mark-email-read" color="#00DD00" />,
			label: $t("Notifications.Read"),
			onClick: () =>
				handleRead(
					selectedRows.map((row) => row.id),
					NotificationReadState.Read,
				),
		},
		{
			key: "unread",
			icon: <Icon icon="ic:outline-mark-email-unread" color="#FF7744" />,
			label: $t("Notifications.UnRead"),
			onClick: () =>
				handleRead(
					selectedRows.map((row) => row.id),
					NotificationReadState.UnRead,
				),
		},
	];

	const rowActionMenu = (record: Notification): MenuProps["items"] => [
		{
			key: "read",
			icon: <Icon icon="ic:outline-mark-email-read" color="#00DD00" />,
			label: $t("Notifications.Read"),
			onClick: () => handleRead([record.id], NotificationReadState.Read),
		},
		{
			key: "unread",
			icon: <Icon icon="ic:outline-mark-email-unread" color="#FF7744" />,
			label: $t("Notifications.UnRead"),
			onClick: () => handleRead([record.id], NotificationReadState.UnRead),
		},
	];

	const columns: ProColumns<Notification>[] = [
		{
			title: $t("Notifications.Notifications:State"),
			dataIndex: "readState",
			valueType: "select",
			fieldProps: {
				allowClear: true,
				options: [
					{
						label: $t("Notifications.Read"),
						value: NotificationReadState.Read,
					},
					{
						label: $t("Notifications.UnRead"),
						value: NotificationReadState.UnRead,
					},
				],
			},
			initialValue: NotificationReadState.UnRead,
			hideInTable: true,
		},
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true, // hide in table
		},
		{
			title: $t("Notifications.Notifications:Type"),
			dataIndex: "type",

			hideInSearch: true,
			valueEnum: {
				[NotificationType.Application]: $t("Notifications.NotificationType:Application"),
				[NotificationType.ServiceCallback]: $t("Notifications.NotificationType:ServiceCallback"),
				[NotificationType.System]: $t("Notifications.NotificationType:System"),
				[NotificationType.User]: $t("Notifications.NotificationType:User"),
			},
		},
		{
			title: $t("Notifications.Notifications:SendTime"),
			dataIndex: "creationTime",
			hideInSearch: true,
			render: (_, record) => formatToDateTime(record.creationTime),
		},
		{
			title: $t("Notifications.Notifications:Title"),
			dataIndex: "title",
			hideInSearch: true,
			render: (_, record) => (
				<div className="flex items-center gap-1">
					<Icon
						icon={
							record.state === NotificationReadState.Read
								? "ic:outline-mark-email-read"
								: "ic:outline-mark-email-unread"
						}
						className="text-xl"
						color={record.state === NotificationReadState.Read ? "#00DD00" : "#FF7744"}
					/>
					<button type="button" onClick={() => handleClickNotification(record)}>
						{record.title}
					</button>
				</div>
			),
		},
		{
			title: $t("Notifications.Notifications:Content"),
			dataIndex: "message",
			hideInSearch: true,
			render: (_, record) => (
				<button type="button" onClick={() => handleClickNotification(record)}>
					{record.message}
				</button>
			),
		},
		{
			title: $t("AbpUi.Actions"),
			fixed: "right",
			hideInSearch: true,
			width: 200,
			render: (_, record) => (
				<Space>
					<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
						{$t("AbpUi.Delete")}
					</Button>
					<Dropdown menu={{ items: rowActionMenu(record) }}>
						<Button type="link">
							<Space>
								<Icon icon="material-symbols:bookmark-outline" />
								{$t("Notifications.MarkAs")}
								<DownOutlined />
							</Space>
						</Button>
					</Dropdown>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<Notification>
					headerTitle={$t("Notifications.Notifications")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					rowSelection={{
						onChange: (_, rows) => setSelectedRows(rows),
					}}
					toolBarRender={() => [
						selectedRows.length > 0 && (
							<Dropdown menu={{ items: bulkActionMenu }}>
								<Button>
									<Space>
										<Icon icon="material-symbols:bookmark-outline" />
										{$t("Notifications.MarkAs")}
										<DownOutlined />
									</Space>
								</Button>
							</Dropdown>
						),
					]}
					request={async (params) => {
						const { current, pageSize, filter, readState } = params;
						const query = await queryClient.fetchQuery({
							queryKey: ["notifications", params],
							queryFn: async () =>
								getMyNotifilersApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									filter,
									readState,
								}),
						});
						return {
							data: query.items.map((item) => {
								const notification = deserialize(item);
								return {
									...item,
									...notification,
								};
							}),
							success: true,
							total: query.totalCount,
						};
					}}
					pagination={{
						showSizeChanger: true,
					}}
					scroll={{ x: "max-content" }}
					search={{
						labelWidth: "auto",
						defaultCollapsed: false,
					}}
				/>
			</Card>
			<MyNotificationModal
				visible={modalVisible}
				notification={selectedNotification}
				onClose={() => setModalVisible(false)}
			/>
		</>
	);
};

export default MyNotificationTable;
