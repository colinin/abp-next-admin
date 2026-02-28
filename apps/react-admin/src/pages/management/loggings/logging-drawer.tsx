import type React from "react";
import { useEffect, useState } from "react";
import { Descriptions, Drawer, Tabs, Tag, Table, Input } from "antd";
import { useTranslation } from "react-i18next";
import type { LogDto, LogExceptionDto, LogLevel } from "#/management/auditing/loggings";
import { getApi } from "@/api/management/auditing/loggings";
import { formatToDateTime } from "@/utils/abp";

interface LoggingDrawerProps {
	visible: boolean;
	onClose: () => void;
	logId?: string;
	logLevelOptions: { color: string; label: string; value: LogLevel }[];
}

const LoggingDrawer: React.FC<LoggingDrawerProps> = ({ visible, onClose, logId, logLevelOptions }) => {
	const { t: $t } = useTranslation();
	const [activeTab, setActiveTab] = useState("basic");
	const [logModel, setLogModel] = useState<LogDto>({} as LogDto);
	const [loading, setLoading] = useState(false);

	// Helper to find level details
	const getLevelOption = (level: LogLevel) => {
		return logLevelOptions.find((opt) => opt.value === level);
	};

	useEffect(() => {
		if (visible && logId) {
			fetchLog(logId);
		} else {
			setLogModel({} as LogDto);
		}
	}, [visible, logId]);

	const fetchLog = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			setLogModel(dto);
		} finally {
			setLoading(false);
		}
	};

	const exceptionColumns = [
		{
			title: $t("AbpAuditLogging.Class"),
			dataIndex: "class",
			key: "class",
			sorter: true,
		},
	];

	const expandedExceptionRender = (record: LogExceptionDto) => (
		<Descriptions column={1} bordered size="small" labelStyle={{ minWidth: "100px" }}>
			<Descriptions.Item label={$t("AbpAuditLogging.Class")}>{record.class}</Descriptions.Item>
			<Descriptions.Item label={$t("AbpAuditLogging.Message")}>{record.message}</Descriptions.Item>
			<Descriptions.Item label={$t("AbpAuditLogging.Source")}>{record.source}</Descriptions.Item>
			<Descriptions.Item label={$t("AbpAuditLogging.StackTrace")}>
				<Input.TextArea readOnly autoSize={{ minRows: 4, maxRows: 15 }} value={record.stackTrace} />
			</Descriptions.Item>
			<Descriptions.Item label={$t("AbpAuditLogging.HResult")}>{record.hResult}</Descriptions.Item>
			<Descriptions.Item label={$t("AbpAuditLogging.HelpURL")}>{record.helpUrl}</Descriptions.Item>
		</Descriptions>
	);

	return (
		<Drawer title={$t("AbpAuditLogging.AuditLog")} open={visible} onClose={onClose} width={800} loading={loading}>
			<Tabs activeKey={activeTab} onChange={setActiveTab}>
				{/* Basic Operation Tab */}
				<Tabs.TabPane key="basic" tab={$t("AbpAuditLogging.Operation")}>
					<Descriptions column={1} bordered size="small" labelStyle={{ minWidth: "110px" }}>
						<Descriptions.Item label={$t("AbpAuditLogging.TimeStamp")}>
							{formatToDateTime(logModel.timeStamp)}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Level")}>
							{logModel.level !== undefined && (
								<Tag color={getLevelOption(logModel.level)?.color}>{getLevelOption(logModel.level)?.label}</Tag>
							)}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Message")}>{logModel.message}</Descriptions.Item>
					</Descriptions>
				</Tabs.TabPane>

				{/* Fields Tab */}
				<Tabs.TabPane key="fields" tab={$t("AbpAuditLogging.Fields")}>
					<Descriptions column={1} bordered size="small" labelStyle={{ minWidth: "110px" }}>
						<Descriptions.Item label={$t("AbpAuditLogging.ApplicationName")}>
							{logModel.fields?.application}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.MachineName")}>
							{logModel.fields?.machineName}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Environment")}>
							{logModel.fields?.environment}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ProcessId")}>{logModel.fields?.processId}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ThreadId")}>{logModel.fields?.threadId}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Context")}>{logModel.fields?.context}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ActionId")}>{logModel.fields?.actionId}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.MethodName")}>
							{logModel.fields?.actionName}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.RequestId")}>{logModel.fields?.requestId}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.RequestPath")}>
							{logModel.fields?.requestPath}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ConnectionId")}>
							{logModel.fields?.connectionId}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.CorrelationId")}>
							{logModel.fields?.correlationId}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ClientId")}>{logModel.fields?.clientId}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.UserId")}>{logModel.fields?.userId}</Descriptions.Item>
					</Descriptions>
				</Tabs.TabPane>

				{/* Exceptions Tab */}
				{logModel.exceptions && logModel.exceptions.length > 0 && (
					<Tabs.TabPane key="exception" tab={$t("AbpAuditLogging.Exceptions")}>
						<Table
							columns={exceptionColumns}
							dataSource={logModel.exceptions}
							rowKey={(record) => record?.class + (record?.message || "")} // Generate a key if no ID
							expandable={{ expandedRowRender: expandedExceptionRender }}
							pagination={false}
							bordered
						/>
					</Tabs.TabPane>
				)}
			</Tabs>
		</Drawer>
	);
};

export default LoggingDrawer;
