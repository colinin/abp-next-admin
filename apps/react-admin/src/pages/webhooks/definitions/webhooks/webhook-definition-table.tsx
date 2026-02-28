import type React from "react";
import { useRef, useState } from "react";
import { Button, Tag, Space, Modal, Table, Card } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getListApi as getDefinitionsApi } from "@/api/webhooks/webhook-definitions";
import { getListApi as getGroupsApi } from "@/api/webhooks/webhook-group-definitions";
import type { WebhookDefinitionDto } from "#/webhooks/definitions";
import type { WebhookGroupDefinitionDto } from "#/webhooks/groups";
import { WebhookDefinitionsPermissions } from "@/constants/webhooks/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";

import WebhookDefinitionModal from "./webhook-definition-modal";
import { listToTree } from "@/utils/tree";

// Extended interface to hold the nested items for display
interface ExtendedGroupDto extends WebhookGroupDefinitionDto {
	items: WebhookDefinitionDto[];
}

const WebhookDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const [modal, contextHolder] = Modal.useModal();

	const [modalVisible, setModalVisible] = useState(false);
	const [selectedDefinitionName, setSelectedDefinitionName] = useState<string | undefined>();

	const { mutateAsync: deleteDef } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedDefinitionName(undefined);
		setModalVisible(true);
	};

	const handleUpdate = (row: WebhookDefinitionDto) => {
		setSelectedDefinitionName(row.name);
		setModalVisible(true);
	};

	const handleDelete = (row: WebhookDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: () => deleteDef(row.name),
		});
	};

	// --- Sub-Table Columns (Definitions) ---
	const definitionColumns = [
		{
			title: $t("WebhooksManagement.DisplayName:IsEnabled"),
			dataIndex: "isEnabled",
			width: 100,
			align: "center" as const,
			render: (val: boolean) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("WebhooksManagement.DisplayName:Name"),
			dataIndex: "name",
			width: 250,
		},
		{
			title: $t("WebhooksManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 200,
		},
		{
			title: $t("WebhooksManagement.DisplayName:Description"),
			dataIndex: "description",
			ellipsis: true,
		},
		{
			title: $t("WebhooksManagement.DisplayName:RequiredFeatures"),
			dataIndex: "requiredFeatures",
			width: 200,
			render: (features: string[]) => (
				<Space wrap>
					{features?.map((f) => (
						<Tag key={f} color="blue">
							{f}
						</Tag>
					))}
				</Space>
			),
		},
		{
			title: $t("AbpUi.Actions"),
			key: "actions",
			width: 150,
			fixed: "right" as const,
			render: (_: any, record: WebhookDefinitionDto) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[WebhookDefinitionsPermissions.Update],
					)}
					{!record.isStatic &&
						withAccessChecker(
							<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
								{$t("AbpUi.Delete")}
							</Button>,
							[WebhookDefinitionsPermissions.Delete],
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
			title: $t("WebhooksManagement.DisplayName:Name"),
			dataIndex: "name",
			width: 200,
			fixed: "left",
		},
		{
			title: $t("WebhooksManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<ExtendedGroupDto>
					headerTitle={$t("WebhooksManagement.WebhookDefinitions")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("WebhooksManagement.Webhooks:AddNew")}
							</Button>,
							[WebhookDefinitionsPermissions.Create],
						),
					]}
					// Nested Table Renderer
					expandable={{
						expandedRowRender: (record) => (
							<Table
								columns={definitionColumns}
								dataSource={record.items}
								pagination={false}
								rowKey="name"
								size="small"
								// Support tree structure within definitions (Vue logic used listToTree)
								expandable={{
									defaultExpandAllRows: true,
									childrenColumnName: "children",
								}}
							/>
						),
						defaultExpandAllRows: true, // Expand groups by default to match Vue behavior often used for categorization
					}}
					request={async (params) => {
						const { filter } = params;

						// 1. Fetch Groups and Definitions in parallel
						const [groupRes, defRes] = await Promise.all([getGroupsApi({ filter }), getDefinitionsApi({ filter })]);

						// 2. Process and merge data
						const data: ExtendedGroupDto[] = groupRes.items.map((group) => {
							// Localize Group Name
							const groupLocal = deserialize(group.displayName);

							// Filter definitions belonging to this group
							const groupDefinitions = defRes.items
								.filter((d) => d.groupName === group.name)
								.map((d) => {
									// Localize Definition fields
									const dName = deserialize(d.displayName);
									const dDesc = deserialize(d.description);
									return {
										...d,
										displayName: Lr(dName.resourceName, dName.name),
										description: dDesc ? Lr(dDesc.resourceName, dDesc.name) : "",
									};
								});

							// Build Tree for definitions (handling parent/child relationships if any)
							const definitionsTree = listToTree(groupDefinitions, { id: "name", pid: "parentName" });

							return {
								...group,
								displayName: Lr(groupLocal.resourceName, groupLocal.name),
								items: definitionsTree,
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
			<WebhookDefinitionModal
				visible={modalVisible}
				definitionName={selectedDefinitionName}
				onClose={() => setModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
			/>
		</>
	);
};

export default WebhookDefinitionTable;
