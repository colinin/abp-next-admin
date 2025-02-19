import { useState } from "react";
import { Button, Popconfirm } from "antd";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ProColumns } from "@ant-design/pro-table";
import type { DisplayNameInfo, DisplayNameProps } from "./types";
import DisplayNameModal from "./display-name-modal";

const DisplayNameTable: React.FC<DisplayNameProps> = ({ data, onChange, onDelete }) => {
	const { t: $t } = useTranslation();
	const [modalVisible, setModalVisible] = useState(false);

	const dataSource = Object.entries(data || {}).map(([culture, displayName]) => ({
		culture,
		displayName,
		key: culture,
	}));

	const columns: ProColumns<DisplayNameInfo>[] = [
		{
			title: $t("AbpOpenIddict.DisplayName:CultureName"),
			dataIndex: "culture",
			align: "left",
			width: 200,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:DisplayName"),
			dataIndex: "displayName",
			align: "left",
			width: 200,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 180,
			fixed: "right",
			render: (_, record) => (
				<Popconfirm
					title={$t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: record.culture })}
					onConfirm={() => onDelete?.(record)}
				>
					<Button type="link" danger icon={<DeleteOutlined />} block>
						{$t("AbpUi.Delete")}
					</Button>
				</Popconfirm>
			),
		},
	];

	return (
		<>
			<ProTable<DisplayNameInfo>
				columns={columns}
				dataSource={dataSource}
				search={false}
				pagination={false}
				toolBarRender={() => [
					<Button key="add" type="primary" icon={<PlusOutlined />} onClick={() => setModalVisible(true)}>
						{$t("AbpOpenIddict.DisplayName:AddNew")}
					</Button>,
				]}
			/>

			<DisplayNameModal
				visible={modalVisible}
				onClose={() => setModalVisible(false)}
				onChange={(info) => {
					onChange?.(info);
					setModalVisible(false);
				}}
			/>
		</>
	);
};

export default DisplayNameTable;
