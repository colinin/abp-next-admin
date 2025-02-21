import { Button, Popconfirm, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import { PlusOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { useMemo, useState } from "react";
import PropertyModal from "./property-modal";
import type { PropertyInfo, PropertyProps } from "./types";

const PropertyTable: React.FC<PropertyProps> = ({
	data = {},
	allowDelete = true,
	allowEdit = true,
	disabled = false,
	onChange,
	onDelete,
}) => {
	const { t: $t } = useTranslation();
	const [modalVisible, setModalVisible] = useState(false);

	const dataSource = useMemo((): PropertyInfo[] => {
		return Object.keys(data).map((key) => ({
			key,
			value: data[key],
		}));
	}, [data]);

	const columns: ColumnsType<PropertyInfo> = useMemo(() => {
		const baseColumns: ColumnsType<PropertyInfo> = [
			{
				align: "left",
				dataIndex: "key",
				fixed: "left",
				width: 100,
				title: $t("component.extra_property_dictionary.key"),
			},
			{
				align: "left",
				dataIndex: "value",
				fixed: "left",
				title: $t("component.extra_property_dictionary.value"),
			},
		];

		if (disabled) {
			return baseColumns;
		}

		return [
			...baseColumns,
			{
				align: "center",
				dataIndex: "action",
				fixed: "right",
				key: "action",
				title: $t("component.extra_property_dictionary.actions.title"),
				width: 150,
				render: (_, record: PropertyInfo) =>
					allowDelete && (
						<Popconfirm
							title={$t("component.extra_property_dictionary.itemWillBeDeleted", { key: record.key })}
							onConfirm={() => handleDelete(record)}
						>
							<Button block danger type="link" icon={<DeleteOutlined />}>
								{$t("component.extra_property_dictionary.actions.delete")}
							</Button>
						</Popconfirm>
					),
			},
		];
	}, [disabled, allowDelete, $t]);

	const handleCreate = () => {
		setModalVisible(true);
	};

	const handleDelete = (prop: PropertyInfo) => {
		onDelete?.(prop);
	};

	const handleChange = (prop: PropertyInfo) => {
		onChange?.(prop);
		setModalVisible(false);
	};

	return (
		<div className="flex flex-col gap-4">
			<div className="flex flex-row justify-end">
				{!disabled && allowEdit && (
					<Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
						{$t("component.extra_property_dictionary.actions.create")}
					</Button>
				)}
			</div>
			<div className="justify-center">
				<Table columns={columns} dataSource={dataSource} rowKey="key" />
			</div>
			<PropertyModal visible={modalVisible} onClose={() => setModalVisible(false)} onChange={handleChange} />
		</div>
	);
};

export default PropertyTable;
