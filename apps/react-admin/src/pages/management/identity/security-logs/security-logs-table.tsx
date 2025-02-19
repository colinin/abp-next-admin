import { useRef, useState } from "react";
import { Space, Button, Tag, Card } from "antd";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import ProTable, { type ProColumns, type ActionType } from "@ant-design/pro-table";
import { EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { formatToDateTime } from "@/utils/abp";
import type { SecurityLogDto } from "#/management/identity";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";
import { SecurityLogPermissions } from "@/constants/management/identity/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { toast } from "sonner";
import SecurityLogDrawer from "./security-log-drawer";
import { deleteApi, getPagedListApi } from "@/api/management/identity/security-logs";
import DeleteModal from "@/components/abp/common/delete-modal";

const SecurityLogs = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	//drawer
	const [drawerVisible, setDrawerVisible] = useState(false);
	const [selectedLogId, setSelectedLogId] = useState<string | undefined>();
	//Modal
	const [deleteModalVisible, setDeleteModalVisible] = useState(false);

	const openDrawer = (id: string) => {
		setSelectedLogId(id);
		setDrawerVisible(true);
	};

	const closeDrawer = () => {
		setDrawerVisible(false);
		setSelectedLogId(undefined);
	};

	const { mutateAsync: deleteSecurityLog } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["securityLogs"] });
		},
	});

	const handleDelete = (log: SecurityLogDto) => {
		setSelectedLogId(log.id);
		setDeleteModalVisible(true);
	};

	const confirmDelete = async () => {
		if (selectedLogId) {
			await deleteSecurityLog(selectedLogId);
			actionRef.current?.reload();
			setDeleteModalVisible(false);
		}
	};

	const columns: ProColumns<SecurityLogDto>[] = [
		{
			title: $t("AbpAuditLogging.CreationTime"),
			dataIndex: "creationTime",
			valueType: "dateRange",
			sorter: true,
			width: 180,
			render: (_, record) => {
				// formatter
				return record.creationTime ? formatToDateTime(record.creationTime) : record.creationTime;
			},
		},
		{
			title: $t("AbpAuditLogging.Identity"),
			dataIndex: "identity",
			sorter: true,
			width: 180,
		},
		{
			title: $t("AbpAuditLogging.UserName"),
			dataIndex: "userName",
			sorter: true,
			width: 150,
		},
		{
			title: $t("AbpAuditLogging.ClientId"),
			dataIndex: "clientId",
			sorter: true,
			width: 200,
		},
		{
			title: $t("AbpAuditLogging.ClientIpAddress"),
			dataIndex: "clientIpAddress",
			sorter: true,
			width: 200,
			hideInSearch: true,
			render: (_, record) => (
				<>
					{/* 展示地址 */}
					{record.extraProperties?.Location && <Tag color="blue">{record.extraProperties.Location}</Tag>}
					<span>{record.clientIpAddress}</span>
				</>
			),
		},
		{
			title: $t("AbpAuditLogging.ApplicationName"),
			dataIndex: "applicationName",
			sorter: true,
			width: 200,
			ellipsis: true,
		},
		{
			title: $t("AbpAuditLogging.TenantName"),
			dataIndex: "tenantName",
			hideInSearch: true,
			sorter: true,
			width: 180,
		},
		{
			title: $t("AbpAuditLogging.Actions"),
			dataIndex: "action",
			sorter: true,
			width: 180,
		},
		{
			title: $t("AbpAuditLogging.CorrelationId"),
			dataIndex: "correlationId",
			sorter: true,
			width: 200,
		},
		{
			title: $t("AbpAuditLogging.BrowserInfo"),
			dataIndex: "browserInfo",
			width: 200,
			sorter: true,
			hideInSearch: true,
			ellipsis: true,
		},
		hasAccessByCodes([SecurityLogPermissions.Default, SecurityLogPermissions.Delete])
			? {
					title: $t("AbpUi.Actions"),
					key: "actions",
					align: "center",
					fixed: "right",
					hideInSearch: true,
					width: 150,
					render: (_, record) => (
						<div className="flex gap-1">
							{withAccessChecker(
								<Button type="link" icon={<EditOutlined />} onClick={() => openDrawer(record.id)}>
									{$t("AbpUi.Edit")}
								</Button>,
								[SecurityLogPermissions.Default],
							)}
							{withAccessChecker(
								<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
									{$t("AbpUi.Delete")}
								</Button>,
								[SecurityLogPermissions.Delete],
							)}
						</div>
					),
				}
			: {},
	];

	return (
		<>
			<Space direction="vertical" size="large" className="w-full">
				<Card>
					<ProTable<SecurityLogDto>
						headerTitle={$t("AbpAuditLogging.SecurityLog")}
						actionRef={actionRef}
						rowKey="id"
						search={{
							labelWidth: "auto",
							defaultCollapsed: true,
						}}
						columns={columns}
						request={async (params, sorter) => {
							const { creationTime, current, pageSize, ...rest } = params;
							const [startTime, endTime] = creationTime || [];

							const query = await queryClient.fetchQuery({
								queryKey: ["securityLogs", params, sorter],
								queryFn: () =>
									getPagedListApi({
										maxResultCount: pageSize,
										skipCount: ((current || 1) - 1) * (pageSize || 0),
										sorting: sorter
											? Object.keys(sorter)
													.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
													.join(", ")
											: undefined,
										startTime: startTime || undefined, // 转换为 startTime 参数
										endTime: endTime || undefined, // 转换为 endTime 参数
										...rest,
									}),
							});

							return {
								data: query.items,
								total: query.totalCount,
							};
						}}
						pagination={{
							showSizeChanger: true,
						}}
						scroll={{ x: "max-content" }}
					/>
				</Card>
			</Space>
			<DeleteModal
				visible={deleteModalVisible}
				onConfirm={confirmDelete}
				onCancel={() => setDeleteModalVisible(false)}
			/>
			<SecurityLogDrawer visible={drawerVisible} onClose={closeDrawer} securityLogId={selectedLogId} />
		</>
	);
};

export default SecurityLogs;
