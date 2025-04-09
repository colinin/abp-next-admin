import { formatToDateTime } from "@/utils/abp";
import { useTranslation } from "react-i18next";
import type { EntityChangeDto, PropertyChange, ChangeType } from "#/management/auditing/entity-changes";
import { Tag, Table, Card } from "antd";
import type { ColumnsType } from "antd/es/table";
import { useState, useMemo } from "react";
import { useAuditLogs } from "@/hooks/abp/auditing/use-audit-logs";
import Scrollbar from "@/components/scrollbar";

interface EntityChangeTableProps {
	data: EntityChangeDto[];
	showUserName?: boolean;
}

export const EntityChangeTable: React.FC<EntityChangeTableProps> = ({ data, showUserName = false }) => {
	const [pageSize, setPageSize] = useState(10);
	const [current, setCurrent] = useState(1);
	const { t } = useTranslation();
	const { getChangeTypeColor, getChangeTypeValue } = useAuditLogs();

	const columns: ColumnsType<EntityChangeDto> = [
		showUserName && {
			title: t("AbpAuditLogging.UserName"),
			dataIndex: "userName",
			align: "center",
			width: 100, // 设置列宽
		},
		{
			title: t("AbpAuditLogging.ChangeType"),
			dataIndex: "changeType",
			sorter: true,
			align: "center",
			width: 120, // 设置列宽
			render: (type: ChangeType) => <Tag color={getChangeTypeColor(type)}>{getChangeTypeValue(type)}</Tag>,
		},
		{
			title: t("AbpAuditLogging.StartTime"),
			dataIndex: "changeTime",
			sorter: true,
			width: 200, // 设置列宽
			render: (value: Date) => value && formatToDateTime(value),
		},
		{
			title: t("AbpAuditLogging.EntityTypeFullName"),
			dataIndex: "entityTypeFullName",
			ellipsis: true,
			sorter: true,
			width: 400, // 设置列宽
		},
		{
			title: t("AbpAuditLogging.EntityId"),
			dataIndex: "entityId",
			sorter: true,
			width: 312, // 设置列宽
		},
		{
			title: t("AbpAuditLogging.TenantId"),
			dataIndex: "entityTenantId",
			sorter: true,
			width: 312, // 设置列宽
		},
	].filter(Boolean) as ColumnsType<EntityChangeDto>;

	const propertyColumns: ColumnsType<PropertyChange> = [
		{
			title: t("AbpAuditLogging.PropertyName"),
			dataIndex: "propertyName",
			sorter: true,
			width: 150, // 设置列宽
		},
		{
			title: t("AbpAuditLogging.NewValue"),
			dataIndex: "newValue",
			sorter: true,
			className: "font-medium text-success",
			width: 250, // 设置列宽
		},
		{
			title: t("AbpAuditLogging.OriginalValue"),
			dataIndex: "originalValue",
			sorter: true,
			className: "font-medium text-error",
			width: 250, // 设置列宽
		},
		{
			title: t("AbpAuditLogging.PropertyTypeFullName"),
			dataIndex: "propertyTypeFullName",
			sorter: true,
			width: 250, // 设置列宽
		},
	];

	const paginatedData = useMemo(() => {
		const startIndex = (current - 1) * pageSize;
		return data.slice(startIndex, startIndex + pageSize);
	}, [data, current, pageSize]);

	return (
		<Card>
			<Scrollbar>
				<div className="flex flex-col space-y-4">
					<div className="w-auto">
						<Table
							columns={columns}
							dataSource={paginatedData}
							rowKey="id"
							tableLayout="fixed" //https://github.com/ant-design/ant-design/issues/39437#issuecomment-1344152456
							pagination={{
								current,
								pageSize,
								total: data.length,
								onChange: (page, size) => {
									setCurrent(page);
									setPageSize(size);
								},
								showSizeChanger: true,
								pageSizeOptions: ["10", "25", "50", "100"],
							}}
							expandable={{
								expandedRowRender: (record) => (
									<div>
										<Table
											tableLayout="fixed"
											columns={propertyColumns}
											dataSource={record.propertyChanges}
											pagination={false}
											rowKey="propertyName"
											bordered
											className="w-2/3"
										/>
									</div>
								),
							}}
						/>
					</div>
				</div>
			</Scrollbar>
		</Card>
	);
};
