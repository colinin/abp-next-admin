import type React from "react";
import { useRef, useState } from "react";
import { Button, Tag, Space, Modal, Dropdown, Table, Card } from "antd";
import {
	EditOutlined,
	DeleteOutlined,
	PlusOutlined,
	CheckOutlined,
	CloseOutlined,
	EllipsisOutlined,
	SendOutlined,
} from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation } from "@tanstack/react-query";
import { deleteApi, getListApi as getDefinitionsApi } from "@/api/management/notifications/notification-definitions";
import { getListApi as getGroupsApi } from "@/api/management/notifications/notification-group-definitions";
import type { NotificationDefinitionDto } from "#/notifications/definitions";
import type { NotificationGroupDefinitionDto } from "#/notifications/groups";
import { NotificationDefinitionsPermissions, NotificationPermissions } from "@/constants/notifications/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";

import NotificationDefinitionModal from "./notification-definition-modal";
import { useEnumMaps } from "./use-enum-maps";
import NotificationSendModal from "./notification-send-modal";
import { listToTree } from "@/utils/tree";
// Extended interface for Table
interface ExtendedGroupDto extends NotificationGroupDefinitionDto {
	items: NotificationDefinitionDto[];
}

const NotificationDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	// const queryClient = useQueryClient();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const [modal, contextHolder] = Modal.useModal();
	const { notificationContentTypeMap, notificationLifetimeMap, notificationTypeMap } = useEnumMaps();

	// State
	const [definitionModalVisible, setDefinitionModalVisible] = useState(false);
	const [sendModalVisible, setSendModalVisible] = useState(false);
	const [selectedDefinitionName, setSelectedDefinitionName] = useState<string | undefined>();
	const [selectedDefinition, setSelectedDefinition] = useState<NotificationDefinitionDto | undefined>();

	const { mutateAsync: deleteDef } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedDefinitionName(undefined);
		setDefinitionModalVisible(true);
	};

	const handleUpdate = (row: NotificationDefinitionDto) => {
		setSelectedDefinitionName(row.name);
		setDefinitionModalVisible(true);
	};

	const handleDelete = (row: NotificationDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: () => deleteDef(row.name),
		});
	};

	const handleSend = (row: NotificationDefinitionDto) => {
		setSelectedDefinition(row);
		setSendModalVisible(true);
	};

	// --- Sub-Table Columns (Definitions) ---
	const definitionColumns = [
		{
			title: $t("Notifications.DisplayName:Name"),
			dataIndex: "name",
			width: 250,
		},
		{
			title: $t("Notifications.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 200,
		},
		{
			title: $t("Notifications.DisplayName:Description"),
			dataIndex: "description",
			ellipsis: true,
		},
		{
			title: $t("Notifications.DisplayName:AllowSubscriptionToClients"),
			dataIndex: "allowSubscriptionToClients",
			width: 100,
			align: "center" as const,
			render: (val: boolean) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("Notifications.DisplayName:Template"),
			dataIndex: "template",
			width: 150,
		},
		{
			title: $t("Notifications.DisplayName:NotificationLifetime"),
			dataIndex: "notificationLifetime",
			width: 150,
			// Fix: Cast map to Record<number, string>
			render: (val: number) => (notificationLifetimeMap as Record<number, string>)[val] || val,
		},
		{
			title: $t("Notifications.DisplayName:NotificationType"),
			dataIndex: "notificationType",
			width: 150,
			// Fix: Cast map
			render: (val: number) => (notificationTypeMap as Record<number, string>)[val] || val,
		},
		{
			title: $t("Notifications.DisplayName:ContentType"),
			dataIndex: "contentType",
			width: 150,
			// Fix: Cast map
			render: (val: number) => (notificationContentTypeMap as Record<number, string>)[val] || val,
		},
		{
			title: $t("Notifications.DisplayName:Providers"),
			dataIndex: "providers",
			width: 200,
			render: (items: string[]) => (
				<Space wrap>
					{items?.map((p) => (
						<Tag key={p} color="blue">
							{p}
						</Tag>
					))}
				</Space>
			),
		},
		{
			title: $t("AbpUi.Actions"),
			key: "actions",
			fixed: "right" as const,
			width: 150,
			render: (_: any, record: NotificationDefinitionDto) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[NotificationDefinitionsPermissions.Update],
					)}

					{!record.isStatic &&
						withAccessChecker(
							<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
								{$t("AbpUi.Delete")}
							</Button>,
							[NotificationDefinitionsPermissions.Delete],
						)}

					{hasAccessByCodes([NotificationPermissions.Create]) && (
						<Dropdown
							menu={{
								items: [
									{
										key: "send",
										label: $t("Notifications.Notifications:Send"),
										icon: <SendOutlined />,
										onClick: () => handleSend(record),
									},
								],
							}}
						>
							<Button type="link" icon={<EllipsisOutlined />} />
						</Dropdown>
					)}
				</Space>
			),
		},
	];

	// --- Main Table Columns (Groups) ---
	const columns: ProColumns<ExtendedGroupDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("Notifications.DisplayName:Name"),
			dataIndex: "name",
			width: 200,
			fixed: "left",
		},
		{
			title: $t("Notifications.DisplayName:DisplayName"),
			dataIndex: "displayName",
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<ExtendedGroupDto>
					headerTitle={$t("Notifications.NotificationDefinitions")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("Notifications.NotificationDefinitions:AddNew")}
							</Button>,
							[NotificationDefinitionsPermissions.Create],
						),
					]}
					scroll={{ x: "max-content" }}
					// Nested Table for Definitions
					expandable={{
						expandedRowRender: (record) => (
							<Table
								columns={definitionColumns}
								dataSource={record.items}
								pagination={false}
								rowKey="name"
								size="small"
								scroll={{ x: "max-content" }}
								// Support tree structure if definitions have children (Vue code implies listToTree)
								expandable={{ defaultExpandAllRows: true, childrenColumnName: "children" }}
							/>
						),
						defaultExpandAllRows: true,
					}}
					request={async (params) => {
						const { filter } = params;
						const [groupRes, defRes] = await Promise.all([getGroupsApi({ filter }), getDefinitionsApi({ filter })]);

						const data: ExtendedGroupDto[] = groupRes.items.map((group) => {
							const groupLocal = deserialize(group.displayName);

							const groupDefinitions = defRes.items
								.filter((d) => d.groupName === group.name)
								.map((d) => {
									const dName = deserialize(d.displayName);
									const dDesc = deserialize(d.description);
									return {
										...d,
										displayName: Lr(dName.resourceName, dName.name),
										description: dDesc ? Lr(dDesc.resourceName, dDesc.name) : "",
									};
								});

							return {
								...group,
								displayName: Lr(groupLocal.resourceName, groupLocal.name),
								items: listToTree(groupDefinitions, { id: "name", pid: "parentName" }),
							};
						});

						return {
							data: data,
							success: true,
							total: groupRes.items.length,
						};
					}}
					pagination={false}
				/>
			</Card>
			<NotificationDefinitionModal
				visible={definitionModalVisible}
				definitionName={selectedDefinitionName}
				onClose={() => setDefinitionModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
			/>

			<NotificationSendModal
				visible={sendModalVisible}
				notification={selectedDefinition}
				onClose={() => setSendModalVisible(false)}
			/>
		</>
	);
};

export default NotificationDefinitionTable;
