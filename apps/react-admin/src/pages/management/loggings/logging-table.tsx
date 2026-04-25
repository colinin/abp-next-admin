import type React from "react";
import { useRef, useState } from "react";
import { Button, Tag, Card, Space, type FormInstance, Checkbox } from "antd";
import { EditOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import ProTable, { type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useQueryClient } from "@tanstack/react-query";
import { formatToDateTime } from "@/utils/abp";
import type { LogDto } from "#/management/auditing/loggings";
import { LogLevel } from "#/management/auditing/loggings";
import { getPagedListApi } from "@/api/management/auditing/loggings";
import { SystemLogPermissions } from "@/constants/management/auditing/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";
import LoggingDrawer from "./logging-drawer";

const LoggingTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const formRef = useRef<FormInstance>();
	const queryClient = useQueryClient();

	const [drawerVisible, setDrawerVisible] = useState(false);
	const [selectedLogId, setSelectedLogId] = useState<string | undefined>(undefined);

	// Log Level Options (mirrors the reactive array in Vue)
	const logLevelOptions = [
		{ color: "purple", label: "Trace", value: LogLevel.Trace },
		{ color: "blue", label: "Debug", value: LogLevel.Debug },
		{ color: "green", label: "Information", value: LogLevel.Information },
		{ color: "orange", label: "Warning", value: LogLevel.Warning },
		{ color: "red", label: "Error", value: LogLevel.Error },
		{ color: "#f50", label: "Critical", value: LogLevel.Critical },
		{ color: "", label: "None", value: LogLevel.None },
	];

	const openDrawer = (log: LogDto) => {
		// Note: LogDto in the grid usually has 'fields.id' but sometimes just 'id' depending on DTO structure.
		// The Vue code accessed `row.fields.id` for the update action.
		setSelectedLogId(log.fields?.id);
		setDrawerVisible(true);
	};

	const closeDrawer = () => {
		setDrawerVisible(false);
		setSelectedLogId(undefined);
	};

	const onFilter = (field: string, value: any) => {
		if (formRef.current) {
			formRef.current.setFieldsValue({ [field]: value });
			formRef.current.submit();
		}
	};

	const columns: ProColumns<LogDto>[] = [
		{
			title: $t("AbpAuditLogging.Level"),
			dataIndex: "level",
			valueType: "select",
			width: 120,
			fieldProps: {
				options: logLevelOptions,
			},
			render: (_, record) => {
				const option = logLevelOptions.find((opt) => opt.value === record.level);
				return (
					<Tag color={option?.color}>
						<Button
							type="link"
							onClick={() => onFilter("level", record.level)}
							style={{ color: "inherit", padding: 0, height: "auto" }}
						>
							{option?.label}
						</Button>
					</Tag>
				);
			},
		},
		{
			title: $t("AbpAuditLogging.TimeStamp"),
			dataIndex: "timeStamp",
			valueType: "dateRange",
			sorter: true,
			width: 150,
			render: (_, record) => formatToDateTime(record.timeStamp),
		},
		{
			title: $t("AbpAuditLogging.Message"),
			dataIndex: "message",
			sorter: true,
			ellipsis: true,
			width: 500,
			hideInSearch: true,
		},
		{
			title: $t("AbpAuditLogging.ApplicationName"),
			dataIndex: "application",
			sorter: true,
			width: 150,
			render: (_, record) => record.fields?.application,
		},
		{
			title: $t("AbpAuditLogging.MachineName"),
			dataIndex: "machineName",
			sorter: true,
			width: 140,
			render: (_, record) => record.fields?.machineName,
		},
		{
			title: $t("AbpAuditLogging.Environment"),
			dataIndex: "environment",
			sorter: true,
			width: 150,
			render: (_, record) => record.fields?.environment,
		},
		{
			title: $t("AbpAuditLogging.RequestId"),
			dataIndex: "requestId",
			sorter: true,
			width: 200,
			render: (_, record) => record.fields?.requestId,
		},
		{
			title: $t("AbpAuditLogging.RequestPath"),
			dataIndex: "requestPath",
			sorter: true,
			width: 300,
			render: (_, record) => record.fields?.requestPath,
		},
		{
			title: $t("AbpAuditLogging.ConnectionId"),
			dataIndex: "connectionId",
			sorter: true,
			width: 150,
			hideInSearch: true, // Not in Vue schema
			render: (_, record) => record.fields?.connectionId,
		},
		{
			title: $t("AbpAuditLogging.CorrelationId"),
			dataIndex: "correlationId",
			sorter: true,
			width: 240,
			render: (_, record) => record.fields?.correlationId,
		},
		{
			title: $t("AbpAuditLogging.HasException"),
			dataIndex: "hasException",
			valueType: "checkbox",
			hideInTable: true,
			renderFormItem: () => {
				return (
					<Checkbox onChange={(e) => onFilter("hasException", e.target.checked)}>
						{$t("AbpAuditLogging.HasException")}
					</Checkbox>
				);
			},
		},
		hasAccessByCodes([SystemLogPermissions.Default])
			? {
					title: $t("AbpUi.Actions"),
					key: "actions",
					fixed: "right",
					width: 180,
					hideInSearch: true,
					render: (_, record) => (
						<Space>
							{withAccessChecker(
								<Button type="link" icon={<EditOutlined />} onClick={() => openDrawer(record)}>
									{$t("AbpAuditLogging.ShowLogDialog")}
								</Button>,
								[SystemLogPermissions.Default],
							)}
						</Space>
					),
				}
			: {},
	];

	return (
		<>
			<Space direction="vertical" size="large" className="w-full">
				<Card>
					<ProTable<LogDto>
						headerTitle={$t("AbpAuditLogging.Logging")}
						actionRef={actionRef}
						formRef={formRef}
						rowKey={(record) => record.fields?.id}
						columns={columns}
						request={async (params, sorter) => {
							const { current, pageSize, timeStamp, ...filters } = params;
							const [startTime, endTime] = timeStamp || [];

							const query = await queryClient.fetchQuery({
								queryKey: ["loggings", params, sorter],
								queryFn: () =>
									getPagedListApi({
										maxResultCount: pageSize,
										skipCount: ((current || 1) - 1) * (pageSize || 0),
										sorting: sorter
											? Object.keys(sorter)
													.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
													.join(", ")
											: undefined,
										startTime: startTime || undefined,
										endTime: endTime || undefined,
										...filters,
									}),
							});
							return {
								data: query.items,
								total: query.totalCount,
								success: true,
							};
						}}
						pagination={{
							showSizeChanger: true,
						}}
						search={{
							labelWidth: "auto",
							defaultCollapsed: true,
						}}
						scroll={{ x: "max-content" }}
					/>
				</Card>
			</Space>

			<LoggingDrawer
				visible={drawerVisible}
				onClose={closeDrawer}
				logId={selectedLogId}
				logLevelOptions={logLevelOptions}
			/>
		</>
	);
};

export default LoggingTable;
