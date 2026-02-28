import type React from "react";
import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getListApi } from "@/api/management/notifications/notification-group-definitions";
import type { NotificationGroupDefinitionDto } from "#/notifications/groups";
import { GroupDefinitionsPermissions, NotificationDefinitionsPermissions } from "@/constants/notifications/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { Iconify } from "@/components/icon";

import NotificationGroupDefinitionModal from "./notification-group-definition-modal";
import NotificationDefinitionModal from "../notifications/notification-definition-modal";

const NotificationGroupDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const [modal, contextHolder] = Modal.useModal();

	// State
	const [groupModalVisible, setGroupModalVisible] = useState(false);
	const [definitionModalVisible, setDefinitionModalVisible] = useState(false);
	const [selectedGroupName, setSelectedGroupName] = useState<string | undefined>();

	const { mutateAsync: deleteGroup } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["notificationGroups"] });
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedGroupName(undefined);
		setGroupModalVisible(true);
	};

	const handleUpdate = (row: NotificationGroupDefinitionDto) => {
		setSelectedGroupName(row.name);
		setGroupModalVisible(true);
	};

	const handleDelete = (row: NotificationGroupDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: () => deleteGroup(row.name),
		});
	};

	const handleMenuClick = (key: string, row: NotificationGroupDefinitionDto) => {
		if (key === "definitions") {
			setSelectedGroupName(row.name);
			setDefinitionModalVisible(true);
		}
	};

	const columns: ProColumns<NotificationGroupDefinitionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("Notifications.DisplayName:Name"),
			dataIndex: "name",
			width: "auto",
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("Notifications.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: "auto",
			sorter: true,
			hideInSearch: true,
			render: (_, record) => {
				const localizableString = deserialize(record.displayName);
				return Lr(localizableString.resourceName, localizableString.name);
			},
		},
		{
			title: $t("AbpUi.Actions"),
			width: 220,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[GroupDefinitionsPermissions.Update],
					)}

					{!record.isStatic && (
						<>
							{withAccessChecker(
								<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
									{$t("AbpUi.Delete")}
								</Button>,
								[GroupDefinitionsPermissions.Delete],
							)}
							<Dropdown
								menu={{
									items: [
										hasAccessByCodes([NotificationDefinitionsPermissions.Create])
											? {
													key: "definitions",
													label: $t("Notifications.NotificationDefinitions:AddNew"),
													icon: <Iconify icon="nimbus:notification" />,
													onClick: () => handleMenuClick("definitions", record),
												}
											: null,
									].filter(Boolean) as any,
								}}
							>
								<Button type="link" icon={<EllipsisOutlined />} />
							</Dropdown>
						</>
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<NotificationGroupDefinitionDto>
					headerTitle={$t("Notifications.GroupDefinitions")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("Notifications.GroupDefinitions:AddNew")}
							</Button>,
							[GroupDefinitionsPermissions.Create],
						),
					]}
					request={async (params) => {
						const { current, pageSize, ...filters } = params;
						const { items } = await getListApi({
							filter: filters.filter,
						});

						return {
							data: items,
							total: items.length,
							success: true,
						};
					}}
					pagination={{ defaultPageSize: 10, showSizeChanger: true }}
				/>
			</Card>
			<NotificationGroupDefinitionModal
				visible={groupModalVisible}
				groupName={selectedGroupName}
				onClose={() => setGroupModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
			/>

			<NotificationDefinitionModal
				visible={definitionModalVisible}
				groupName={selectedGroupName}
				onClose={() => setDefinitionModalVisible(false)}
				onChange={() => actionRef.current?.reload()} // Optional refresh
			/>
		</>
	);
};

export default NotificationGroupDefinitionTable;
