import type React from "react";
import { useRef, useState } from "react";
import { Button, Tag, Dropdown, Modal, Space, Card } from "antd";
import { EditOutlined, DeleteOutlined, EllipsisOutlined, SendOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi, reSendApi, bulkDeleteApi, bulkReSendApi } from "@/api/webhooks/send-attempts";
import { getPagedListApi as getTenantsApi } from "@/api/saas/tenants";
import type { WebhookSendRecordDto } from "#/webhooks/send-attempts";
import { WebhooksSendAttemptsPermissions } from "@/constants/webhooks/permissions"; // Adjust
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { formatToDateTime } from "@/utils/abp";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";
import WebhookSendAttemptDrawer from "./webhook-send-attempt-drawer";
import { useHttpStatusCodeMap } from "@/hooks/abp/fake-hooks/use-http-status-code-map";
import { HttpStatusCode } from "@/constants/request/http-status";

const WebhookSendAttemptTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const { getHttpStatusColor, httpStatusCodeMap } = useHttpStatusCodeMap();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	const [drawerVisible, setDrawerVisible] = useState(false);
	const [selectedRecordId, setSelectedRecordId] = useState<string | undefined>();
	const [selectedKeys, setSelectedKeys] = useState<React.Key[]>([]);

	// Mutation for single resend
	const { mutateAsync: resend } = useMutation({
		mutationFn: reSendApi,
		onSuccess: () => {
			toast.success($t("WebhooksManagement.SuccessfullySent"));
			actionRef.current?.reload();
		},
	});

	// Bulk mutations
	const { mutateAsync: bulkResend } = useMutation({
		mutationFn: bulkReSendApi,
		onSuccess: () => {
			toast.success($t("WebhooksManagement.SuccessfullySent"));
			actionRef.current?.reload();
		},
	});

	const { mutateAsync: bulkDelete } = useMutation({
		mutationFn: bulkDeleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			setSelectedKeys([]);
			actionRef.current?.reload();
		},
	});

	const handleShow = (row: WebhookSendRecordDto) => {
		setSelectedRecordId(row.id);
		setDrawerVisible(true);
	};

	const handleDelete = (row: WebhookSendRecordDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: $t("WebhooksManagement.SelectedItems") }),
			onOk: async () => {
				await deleteApi(row.id);
				toast.success($t("AbpUi.DeletedSuccessfully"));
				actionRef.current?.reload();
			},
		});
	};

	const handleSend = (row: WebhookSendRecordDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("WebhooksManagement.ItemWillBeResendMessageWithFormat", {
				0: $t("WebhooksManagement.SelectedItems"),
			}),
			onOk: () => resend(row.id),
		});
	};

	const handleBulkSend = () => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("WebhooksManagement.ItemWillBeResendMessageWithFormat", {
				0: $t("WebhooksManagement.SelectedItems"),
			}),
			onOk: () => bulkResend({ recordIds: selectedKeys as string[] }),
		});
	};

	const handleBulkDelete = () => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: $t("WebhooksManagement.SelectedItems") }),
			onOk: () => bulkDelete({ recordIds: selectedKeys as string[] }),
		});
	};

	const httpStatusOptions = Object.keys(httpStatusCodeMap).map((key) => ({
		label: httpStatusCodeMap[Number(key)],
		value: key,
	}));

	const columns: ProColumns<WebhookSendRecordDto>[] = [
		{
			title: $t("WebhooksManagement.DisplayName:TenantId"),
			dataIndex: "tenantId",
			width: 200,
			sorter: true,
			valueType: "select",
			request: async ({ keyWords }) => {
				// Simple search logic for ProTable Select
				const res = await getTenantsApi({ filter: keyWords, maxResultCount: 20 });
				return res.items.map((t) => ({ label: t.name, value: t.id }));
			},
			fieldProps: {
				showSearch: true,
				allowClear: true,
			},
		},
		{
			title: $t("WebhooksManagement.DisplayName:ResponseStatusCode"),
			dataIndex: "responseStatusCode",
			width: 150,
			sorter: true,
			valueType: "select",
			fieldProps: { options: httpStatusOptions },
			render: (_, row) => (
				<Tag color={getHttpStatusColor(row.responseStatusCode || HttpStatusCode.InternalServerError)}>
					{httpStatusCodeMap[row.responseStatusCode || HttpStatusCode.InternalServerError] || row.responseStatusCode}
				</Tag>
			),
		},
		{
			title: $t("WebhooksManagement.DisplayName:CreationTime"),
			dataIndex: "creationTime",
			width: 180,
			sorter: true,
			valueType: "dateRange",
			render: (_, row) => formatToDateTime(row.creationTime),
		},
		{
			title: $t("WebhooksManagement.DisplayName:Response"),
			dataIndex: "response",
			width: 300,
			sorter: true,
			ellipsis: true,
			hideInSearch: true,
		},
		{
			title: $t("WebhooksManagement.DisplayName:State"),
			dataIndex: "state", // Mapped to filter
			hideInTable: true,
			valueType: "select",
			valueEnum: {
				true: { text: $t("WebhooksManagement.ResponseState:Successed"), status: "Success" },
				false: { text: $t("WebhooksManagement.ResponseState:Failed"), status: "Error" },
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
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleShow(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[WebhooksSendAttemptsPermissions.Default],
					)}
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[WebhooksSendAttemptsPermissions.Delete],
					)}
					<Dropdown
						menu={{
							items: [
								hasAccessByCodes([WebhooksSendAttemptsPermissions.Resend])
									? {
											key: "send",
											label: $t("WebhooksManagement.Resend"),
											icon: <SendOutlined className="text-green-500" />,
											onClick: () => handleSend(record),
										}
									: null,
							].filter(Boolean) as any,
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
				<ProTable<WebhookSendRecordDto>
					headerTitle={$t("WebhooksManagement.SendAttempts")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					rowSelection={{
						selectedRowKeys: selectedKeys,
						onChange: (keys) => setSelectedKeys(keys),
					}}
					search={{ labelWidth: "auto", defaultCollapsed: true }}
					toolBarRender={() => [
						selectedKeys.length > 0 && hasAccessByCodes([WebhooksSendAttemptsPermissions.Resend]) && (
							<Button key="resend" type="primary" icon={<SendOutlined />} onClick={handleBulkSend}>
								{$t("WebhooksManagement.Resend")}
							</Button>
						),
						selectedKeys.length > 0 && hasAccessByCodes([WebhooksSendAttemptsPermissions.Delete]) && (
							<Button key="delete" danger type="primary" icon={<DeleteOutlined />} onClick={handleBulkDelete}>
								{$t("AbpUi.Delete")}
							</Button>
						),
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
							queryKey: ["sendAttempts", params, sorter],
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
			<WebhookSendAttemptDrawer
				visible={drawerVisible}
				recordId={selectedRecordId}
				onClose={() => setDrawerVisible(false)}
			/>
		</>
	);
};

export default WebhookSendAttemptTable;
