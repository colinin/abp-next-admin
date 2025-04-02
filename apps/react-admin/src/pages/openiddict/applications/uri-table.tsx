import { useState } from "react";
import { Button, Popconfirm } from "antd";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ProColumns } from "@ant-design/pro-table";
import UriModal from "./uri-modal";

interface UriTableProps {
	title: string;
	uris?: string[];
	onChange?: (uri: string) => void;
	onDelete?: (uri: string) => void;
}

interface UriItem {
	uri: string;
	key: string;
}

const UriTable: React.FC<UriTableProps> = ({ title, uris = [], onChange, onDelete }) => {
	const { t: $t } = useTranslation();
	const [modalVisible, setModalVisible] = useState(false);

	const dataSource = uris.map((uri) => ({
		uri,
		key: uri,
	}));

	const columns: ProColumns<UriItem>[] = [
		{
			title: "Uri",
			dataIndex: "uri",
			align: "left",
		},
		{
			title: $t("AbpUi.Actions"),
			width: 180,
			fixed: "right",
			render: (_, record) => (
				<Popconfirm
					title={$t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: record.uri })}
					onConfirm={() => onDelete?.(record.uri)}
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
			<ProTable<UriItem>
				headerTitle={title}
				columns={columns}
				dataSource={dataSource}
				search={false}
				pagination={false}
				toolBarRender={() => [
					<Button key="add" type="primary" icon={<PlusOutlined />} onClick={() => setModalVisible(true)}>
						{$t("AbpOpenIddict.Uri:AddNew")}
					</Button>,
				]}
			/>

			<UriModal
				visible={modalVisible}
				onClose={() => setModalVisible(false)}
				onChange={(uri) => {
					onChange?.(uri);
					setModalVisible(false);
				}}
			/>
		</>
	);
};

export default UriTable;
