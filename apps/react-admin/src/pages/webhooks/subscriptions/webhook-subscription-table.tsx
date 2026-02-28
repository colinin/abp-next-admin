import type React from "react";
import { useRef, useState } from "react";
import { Button, Tag, Space, Modal, Card } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi, getAllAvailableWebhooksApi } from "@/api/webhooks/subscriptions";
import { getPagedListApi as getTenantsApi } from "@/api/saas/tenants";
import type { WebhookSubscriptionDto } from "#/webhooks/subscriptions";
import { WebhookSubscriptionPermissions } from "@/constants/webhooks/permissions"; // Adjust path
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { formatToDateTime } from "@/utils/abp";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";

import WebhookSubscriptionModal from "./webhook-subscription-modal";

const WebhookSubscriptionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	const [modalVisible, setModalVisible] = useState(false);
	const [selectedSubscriptionId, setSelectedSubscriptionId] = useState<string | undefined>();

	const { mutateAsync: deleteSub } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedSubscriptionId(undefined);
		setModalVisible(true);
	};

	const handleUpdate = (row: WebhookSubscriptionDto) => {
		setSelectedSubscriptionId(row.id);
		setModalVisible(true);
	};

	const handleDelete = (row: WebhookSubscriptionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.webhookUri }), // Use URI or suitable display field
			onOk: () => deleteSub(row.id),
		});
	};

	// Helper for init webhooks for search filter
	const fetchWebhookOptions = async () => {
		if (hasAccessByCodes([WebhookSubscriptionPermissions.Default])) {
			const { items } = await getAllAvailableWebhooksApi();
			return items.flatMap((g) => g.webhooks.map((w) => ({ label: w.displayName, value: w.name })));
		}
		return [];
	};

	const columns: ProColumns<WebhookSubscriptionDto>[] = [
		{
			title: $t("WebhooksManagement.DisplayName:IsActive"),
			dataIndex: "isActive",
			width: 100,
			hideInSearch: true,
			align: "center",
			valueType: "checkbox",
			render: (_, record) =>
				record.isActive ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("WebhooksManagement.DisplayName:TenantId"),
			dataIndex: "tenantId",
			width: 200,
			valueType: "select",
			request: async ({ keyWords }) => {
				const res = await getTenantsApi({ filter: keyWords, maxResultCount: 20 });
				return res.items.map((t) => ({ label: t.name, value: t.id }));
			},
			fieldProps: {
				showSearch: true,
				allowClear: true,
			},
		},
		{
			title: $t("WebhooksManagement.DisplayName:WebhookUri"),
			dataIndex: "webhookUri",
			width: 300,
			sorter: true,
		},
		{
			title: $t("WebhooksManagement.DisplayName:Description"),
			dataIndex: "description",
			width: 200,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("WebhooksManagement.DisplayName:CreationTime"),
			dataIndex: "creationTime",
			width: 150,
			sorter: true,
			valueType: "dateRange",
			render: (_, row) => formatToDateTime(row.creationTime),
		},
		{
			title: $t("WebhooksManagement.DisplayName:Webhooks"),
			dataIndex: "webhooks", // This might need custom filter mapping if backend expects list
			width: 300,
			render: (_, row) => (
				<Space wrap>
					{row.webhooks?.map((w) => (
						<Tag key={w} color="blue">
							{w}
						</Tag>
					))}
				</Space>
			),
			valueType: "select",
			request: fetchWebhookOptions,
			fieldProps: {
				showSearch: true,
				mode: "multiple",
			},
		},
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 150,
			render: (_, record) => (
				<Space>
					<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
						{$t("AbpUi.Edit")}
					</Button>
					<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
						{$t("AbpUi.Delete")}
					</Button>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<WebhookSubscriptionDto>
					headerTitle={$t("WebhooksManagement.Subscriptions")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={{ labelWidth: "auto", defaultCollapsed: true }}
					toolBarRender={() => [
						<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
							{$t("WebhooksManagement.Subscriptions:AddNew")}
						</Button>,
					]}
					request={async (params, sorter) => {
						const { current, pageSize, creationTime, ...filters } = params;
						const [beginCreationTime, endCreationTime] = creationTime || [];

						const sorting =
							sorter && Object.keys(sorter).length > 0
								? Object.keys(sorter)
										.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
										.join(", ")
								: undefined;

						const query = await queryClient.fetchQuery({
							queryKey: ["subscriptions", params, sorter],
							queryFn: () =>
								getPagedListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									sorting: sorting,
									beginCreationTime,
									endCreationTime,
									...filters,
								}),
						});

						return {
							data: query.items,
							total: query.totalCount,
							success: true,
						};
					}}
					pagination={{ defaultPageSize: 10, showSizeChanger: true }}
				/>
			</Card>
			<WebhookSubscriptionModal
				visible={modalVisible}
				subscriptionId={selectedSubscriptionId}
				onClose={() => setModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
			/>
		</>
	);
};

export default WebhookSubscriptionTable;
