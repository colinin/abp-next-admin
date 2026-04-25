import type React from "react";
import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getListApi } from "@/api/webhooks/webhook-group-definitions";
import type { WebhookGroupDefinitionDto } from "#/webhooks/groups";
import { GroupDefinitionsPermissions } from "@/constants/webhooks/permissions"; // Adjust path as needed
import { withAccessChecker } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { Iconify } from "@/components/icon";

import WebhookGroupDefinitionModal from "./webhook-group-definition-modal";
import WebhookDefinitionModal from "../webhooks/webhook-definition-modal";

const WebhookGroupDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const [modal, contextHolder] = Modal.useModal();

	// Modal States
	const [groupModalVisible, setGroupModalVisible] = useState(false);
	const [webhookModalVisible, setWebhookModalVisible] = useState(false);
	const [selectedGroupName, setSelectedGroupName] = useState<string | undefined>();

	const { mutateAsync: deleteGroup } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["webhookGroups"] });
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedGroupName(undefined);
		setGroupModalVisible(true);
	};

	const handleUpdate = (row: WebhookGroupDefinitionDto) => {
		setSelectedGroupName(row.name);
		setGroupModalVisible(true);
	};

	const handleDelete = (row: WebhookGroupDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: () => deleteGroup(row.name),
		});
	};

	const handleMenuClick = (key: string, row: WebhookGroupDefinitionDto) => {
		if (key === "webhooks") {
			setSelectedGroupName(row.name);
			setWebhookModalVisible(true);
		}
	};

	const columns: ProColumns<WebhookGroupDefinitionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("WebhooksManagement.DisplayName:Name"),
			dataIndex: "name",
			width: "auto",
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("WebhooksManagement.DisplayName:DisplayName"),
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

					{!record.isStatic &&
						withAccessChecker(
							<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
								{$t("AbpUi.Delete")}
							</Button>,
							[GroupDefinitionsPermissions.Delete],
						)}

					<Dropdown
						menu={{
							items: [
								{
									key: "webhooks",
									label: $t("WebhooksManagement.Webhooks:AddNew"),
									icon: <Iconify icon="material-symbols:webhook" />,
								},
							],
							onClick: ({ key }) => handleMenuClick(key, record),
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<WebhookGroupDefinitionDto>
					headerTitle={$t("WebhooksManagement.GroupDefinitions")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={{
						labelWidth: "auto",
					}}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("WebhooksManagement.GroupDefinitions:AddNew")}
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
					pagination={{
						defaultPageSize: 10,
						showSizeChanger: true,
					}}
				/>
			</Card>
			<WebhookGroupDefinitionModal
				visible={groupModalVisible}
				groupName={selectedGroupName}
				onClose={() => setGroupModalVisible(false)}
				onChange={() => {
					actionRef.current?.reload();
					queryClient.invalidateQueries({ queryKey: ["webhookGroups"] });
				}}
			/>

			<WebhookDefinitionModal
				visible={webhookModalVisible}
				groupName={selectedGroupName}
				onClose={() => setWebhookModalVisible(false)}
				onChange={() => {
					// actionRef.current?.reload();
				}}
			/>
		</>
	);
};

export default WebhookGroupDefinitionTable;
