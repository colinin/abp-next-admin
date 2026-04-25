import type React from "react";
import { useState } from "react";
import { Button, Table, Space, Popconfirm } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { TenantConnectionStringDto } from "#/saas/tenants";
import TenantConnectionStringModal from "./tenant-connection-string-modal";

interface Props {
	data: TenantConnectionStringDto[];
	dataBaseOptions: { label: string; value: string }[];
	onAdd: (item: TenantConnectionStringDto) => Promise<void>;
	onDelete: (item: TenantConnectionStringDto) => Promise<void>;
	loading?: boolean;
}

const TenantConnectionStringsList: React.FC<Props> = ({ data, dataBaseOptions, onAdd, onDelete, loading = false }) => {
	const { t: $t } = useTranslation();
	const [modalVisible, setModalVisible] = useState(false);
	const [editingItem, setEditingItem] = useState<TenantConnectionStringDto | undefined>();

	const handleCreate = () => {
		setEditingItem(undefined);
		setModalVisible(true);
	};

	const handleEdit = (record: TenantConnectionStringDto) => {
		setEditingItem(record);
		setModalVisible(true);
	};

	const columns = [
		{
			title: $t("AbpSaas.DisplayName:Name"),
			dataIndex: "name",
			width: 150,
		},
		{
			title: $t("AbpSaas.DisplayName:Value"),
			dataIndex: "value",
			ellipsis: true,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 150,
			align: "center" as const,
			render: (_: any, record: TenantConnectionStringDto) => (
				<Space>
					<Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(record)}>
						{$t("AbpUi.Edit")}
					</Button>
					<Popconfirm
						title={$t("AbpUi.AreYouSure")}
						description={$t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: record.name })}
						onConfirm={() => onDelete(record)}
					>
						<Button type="link" danger icon={<DeleteOutlined />}>
							{$t("AbpUi.Delete")}
						</Button>
					</Popconfirm>
				</Space>
			),
		},
	];

	return (
		<div>
			<div className="mb-4 flex justify-end">
				<Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
					{$t("AbpSaas.ConnectionStrings:AddNew")}
				</Button>
			</div>
			<Table
				rowKey="name"
				columns={columns}
				dataSource={data}
				pagination={false}
				size="small"
				loading={loading}
				bordered
			/>
			<TenantConnectionStringModal
				visible={modalVisible}
				data={editingItem}
				dataBaseOptions={dataBaseOptions}
				onClose={() => setModalVisible(false)}
				onSubmit={onAdd}
			/>
		</div>
	);
};

export default TenantConnectionStringsList;
