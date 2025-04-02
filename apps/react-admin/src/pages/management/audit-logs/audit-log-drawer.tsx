import type React from "react";
import { useEffect, useState } from "react";
import { Descriptions, Drawer, Tabs, Tag } from "antd";
import { useTranslation } from "react-i18next";
import type { AuditLogDto, Action } from "#/management/auditing/audit-logs";
import { Table } from "antd";
import { useAuditLogs } from "@/hooks/abp/auditing/use-audit-logs";
import { getApi } from "@/api/management/auditing/audit-logs";
import { EntityChangeTable } from "@/components/abp/auditing/entity-change-table";
import { formatToDateTime } from "@/utils/abp";
import JsonEdit from "@/components/abp/common/json-edit";

interface Props {
	visible: boolean;
	onClose: () => void;
	auditLog?: AuditLogDto;
}

const AuditLogDrawer: React.FC<Props> = ({ visible, onClose, auditLog }) => {
	const { t: $t } = useTranslation();
	const [activeTab, setActiveTab] = useState("basic");
	const [auditLogModel, setAuditLogModel] = useState<AuditLogDto>({} as AuditLogDto);
	const { getHttpMethodColor, getHttpStatusCodeColor } = useAuditLogs();

	const actionColumns = [
		{
			title: $t("AbpAuditLogging.ServiceName"),
			dataIndex: "serviceName",
			key: "serviceName",
			sorter: true,
		},
		{
			title: $t("AbpAuditLogging.MethodName"),
			dataIndex: "methodName",
			key: "methodName",
			ellipsis: true,
			sorter: true,
		},
		{
			title: $t("AbpAuditLogging.ExecutionTime"),
			dataIndex: "executionTime",
			key: "executionTime",
			ellipsis: true,
			sorter: true,
			render: (value: string) => (value ? formatToDateTime(value) : value),
		},
		{
			title: $t("AbpAuditLogging.ExecutionDuration"),
			dataIndex: "executionDuration",
			key: "executionDuration",
			ellipsis: true,
			width: 150,
			sorter: true,
		},
	];

	useEffect(() => {
		if (visible && auditLog?.id) {
			fetchAuditLog(auditLog.id);
		}
	}, [visible, auditLog]);

	const fetchAuditLog = async (id: string) => {
		const dto = await getApi(id);
		setAuditLogModel(dto);
	};

	const expandedRowRender = (record: Action) => (
		<Descriptions column={1} bordered size="small">
			<Descriptions.Item label={$t("AbpAuditLogging.Parameters")}>
				{<JsonEdit data={record.parameters || ""} />}
			</Descriptions.Item>
			<Descriptions.Item label={$t("AbpAuditLogging.Additional")}>
				{<JsonEdit data={record.extraProperties || ""} />}
			</Descriptions.Item>
		</Descriptions>
	);

	return (
		<Drawer title={$t("AbpAuditLogging.AuditLog")} open={visible} onClose={onClose} width={800}>
			<Tabs activeKey={activeTab} onChange={setActiveTab}>
				<Tabs.TabPane key="basic" tab={$t("AbpAuditLogging.Operation")}>
					<Descriptions column={2} bordered size="small">
						<Descriptions.Item label={$t("AbpAuditLogging.ApplicationName")}>
							{auditLogModel.applicationName}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ExecutionTime")}>
							{formatToDateTime(auditLogModel.executionTime)}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.UserName")}>{auditLogModel.userName}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.HttpMethod")}>
							<Tag color={getHttpMethodColor(auditLogModel.httpMethod)}>{auditLogModel.httpMethod}</Tag>
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.RequestUrl")} span={2}>
							{auditLogModel.url}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.HttpStatusCode")}>
							<Tag color={getHttpStatusCodeColor(auditLogModel.httpStatusCode)}>{auditLogModel.httpStatusCode}</Tag>
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ExecutionDuration")}>
							{auditLogModel.executionDuration}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ClientId")}>{auditLogModel.clientId}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ClientIpAddress")}>
							{auditLogModel.clientIpAddress}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.ClientName")}>{auditLogModel.clientName}</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.CorrelationId")}>
							{auditLogModel.correlationId}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.BrowserInfo")} labelStyle={{ width: "110px" }} span={2}>
							{auditLogModel.browserInfo}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Comments")} span={2}>
							{auditLogModel.comments}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Exception")} span={2}>
							{<JsonEdit data={auditLogModel.exceptions || ""} />}
						</Descriptions.Item>
						<Descriptions.Item label={$t("AbpAuditLogging.Additional")} span={2}>
							{auditLogModel?.extraProperties ? JSON.stringify(auditLogModel.extraProperties, null, 2) : ""}
						</Descriptions.Item>
					</Descriptions>
				</Tabs.TabPane>

				{auditLogModel?.actions && auditLogModel.actions.length > 0 && (
					<Tabs.TabPane key="opera" tab={`${$t("AbpAuditLogging.Actions")} (${auditLogModel.actions?.length || 0})`}>
						<Table
							columns={actionColumns}
							dataSource={auditLogModel.actions}
							rowKey="id"
							expandable={{ expandedRowRender }}
							pagination={false}
						/>
					</Tabs.TabPane>
				)}

				{auditLogModel.entityChanges && auditLogModel.entityChanges?.length > 0 && (
					<Tabs.TabPane
						key="entity"
						tab={`${$t("AbpAuditLogging.EntityChanges")} (${auditLogModel.entityChanges.length})`}
					>
						<EntityChangeTable data={auditLogModel.entityChanges} />
					</Tabs.TabPane>
				)}
			</Tabs>
		</Drawer>
	);
};

export default AuditLogDrawer;
