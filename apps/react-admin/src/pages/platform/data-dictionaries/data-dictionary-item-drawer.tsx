import type React from "react";
import { useEffect, useRef, useState } from "react";
import { Drawer, Button, Space, Modal, Tag } from "antd";
import { PlusOutlined, EditOutlined, DeleteOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { getApi, deleteItemApi } from "@/api/platform/data-dictionaries";
import type { DataDto, DataItemDto } from "#/platform/data-dictionaries";
import { ValueType } from "#/platform/data-dictionaries";
import DataDictionaryItemModal from "./data-dictionary-item-modal";

interface Props {
	visible: boolean;
	onClose: () => void;
	dataDto?: DataDto;
}

const DataDictionaryItemDrawer: React.FC<Props> = ({ visible, onClose, dataDto }) => {
	const { t: $t } = useTranslation();
	const [modal, contextHolder] = Modal.useModal();
	const actionRef = useRef<ActionType>();
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedItem, setSelectedItem] = useState<DataItemDto | undefined>();
	const [dataItems, setDataItems] = useState<DataItemDto[]>([]);
	const [loading, setLoading] = useState(false);

	useEffect(() => {
		if (visible && dataDto) {
			fetchItems();
		}
	}, [visible, dataDto]);

	const fetchItems = async () => {
		if (!dataDto) return;
		try {
			setLoading(true);
			// The API structure implies getting the parent DataDto returns the items list
			const res = await getApi(dataDto.id);
			setDataItems(res.items || []);
		} finally {
			setLoading(false);
		}
	};

	const handleCreate = () => {
		setSelectedItem(undefined);
		setModalVisible(true);
	};

	const handleUpdate = (record: DataItemDto) => {
		setSelectedItem(record);
		setModalVisible(true);
	};

	const handleDelete = (record: DataItemDto) => {
		if (!dataDto) return;
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: async () => {
				await deleteItemApi(dataDto.id, record.name);
				toast.success($t("AbpUi.DeletedSuccessfully"));
				fetchItems();
			},
		});
	};

	const valueTypeMaps: Record<number, string> = {
		[ValueType.Array]: "Array",
		[ValueType.Boolean]: "Boolean",
		[ValueType.Date]: "Date",
		[ValueType.DateTime]: "DateTime",
		[ValueType.Numeic]: "Number",
		[ValueType.Object]: "Object",
		[ValueType.String]: "String",
	};

	const columns: ProColumns<DataItemDto>[] = [
		{
			title: $t("AppPlatform.DisplayName:Name"),
			dataIndex: "name",
			width: 150,
			sorter: (a, b) => a.name.localeCompare(b.name),
		},
		{
			title: $t("AppPlatform.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
		},
		{
			title: $t("AppPlatform.DisplayName:ValueType"),
			dataIndex: "valueType",
			width: 120,
			render: (val) => <Tag>{valueTypeMaps[val as number] || val}</Tag>,
		},
		{
			title: $t("AppPlatform.DisplayName:DefaultValue"),
			dataIndex: "defaultValue",
			width: 150,
			ellipsis: true,
		},
		{
			title: $t("AppPlatform.DisplayName:AllowBeNull"),
			dataIndex: "allowBeNull",
			width: 100,
			align: "center",
			render: (val) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("AbpUi.Actions"),
			key: "actions",
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
			<Drawer
				title={dataDto ? `${$t("AppPlatform.DisplayName:DataDictionary")} - ${dataDto.displayName}` : ""}
				width="60%"
				open={visible}
				onClose={onClose}
				destroyOnClose
			>
				<ProTable<DataItemDto>
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					dataSource={dataItems}
					loading={loading}
					search={false}
					toolBarRender={() => [
						<Button key="add" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
							{$t("AppPlatform.Data:AppendItem")}
						</Button>,
					]}
					pagination={{ pageSize: 15 }}
				/>
			</Drawer>

			<DataDictionaryItemModal
				visible={modalVisible}
				onClose={() => setModalVisible(false)}
				onChange={fetchItems}
				data={dataDto}
				item={selectedItem}
			/>
		</>
	);
};

export default DataDictionaryItemDrawer;
