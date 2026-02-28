import type React from "react";
import { useRef, useState } from "react";
import { Button, Dropdown, Space, Modal, Card } from "antd";
import { PlusOutlined, EditOutlined, DeleteOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { deleteApi, getAllApi } from "@/api/platform/data-dictionaries";
import type { DataDto } from "#/platform/data-dictionaries";
import { DataDictionaryPermissions } from "@/constants/platform/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { Iconify } from "@/components/icon";

import DataDictionaryModal from "./data-dictionary-modal";
import DataDictionaryItemDrawer from "./data-dictionary-item-drawer";
import { listToTree } from "@/utils/tree";

const DataDictionaryTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const [modal, contextHolder] = Modal.useModal();

	// State
	const [modalVisible, setModalVisible] = useState(false);
	const [drawerVisible, setDrawerVisible] = useState(false);
	const [selectedData, setSelectedData] = useState<DataDto | undefined>();
	const [selectedParentId, setSelectedParentId] = useState<string | undefined>();

	// Handlers
	const handleCreate = () => {
		setSelectedData(undefined);
		setSelectedParentId(undefined);
		setModalVisible(true);
	};

	const handleUpdate = (record: DataDto) => {
		setSelectedData(record);
		setSelectedParentId(undefined);
		setModalVisible(true);
	};

	const handleAddChild = (record: DataDto) => {
		setSelectedData(undefined);
		setSelectedParentId(record.id);
		setModalVisible(true);
	};

	const handleManageItems = (record: DataDto) => {
		setSelectedData(record);
		setDrawerVisible(true);
	};

	const handleDelete = (record: DataDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: async () => {
				await deleteApi(record.id);
				toast.success($t("AbpUi.DeletedSuccessfully"));
				actionRef.current?.reload();
			},
		});
	};

	const columns: ProColumns<DataDto>[] = [
		{
			title: $t("AppPlatform.DisplayName:Name"),
			dataIndex: "name",
			width: 200,
			render: (_, record) => (
				<Button type="link" onClick={() => handleManageItems(record)} style={{ padding: 0 }}>
					{record.name}
				</Button>
			),
		},
		{
			title: $t("AppPlatform.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 200,
		},
		{
			title: $t("AppPlatform.DisplayName:Description"),
			dataIndex: "description",
			ellipsis: true,
		},
		{
			title: $t("AbpUi.Actions"),
			key: "actions",
			fixed: "right",
			width: 250,
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[DataDictionaryPermissions.Update],
					)}
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[DataDictionaryPermissions.Delete],
					)}
					<Dropdown
						menu={{
							items: [
								{
									key: "add-child",
									label: $t("AppPlatform.Data:AddChildren"),
									icon: <PlusOutlined />,
									onClick: () => handleAddChild(record),
									disabled: !hasAccessByCodes([DataDictionaryPermissions.Create]),
								},
								{
									key: "items",
									label: $t("AppPlatform.Data:Items"),
									icon: <Iconify icon="material-symbols:align-items-stretch" />,
									onClick: () => handleManageItems(record),
									disabled: !hasAccessByCodes([DataDictionaryPermissions.ManageItems]),
								},
							],
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<DataDto>
					headerTitle={$t("AppPlatform.DisplayName:DataDictionary")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={false}
					pagination={false}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AppPlatform.Data:AddNew")}
							</Button>,
							[DataDictionaryPermissions.Create],
						),
					]}
					request={async () => {
						const { items } = await getAllApi();
						const tree = listToTree(items, { id: "id", pid: "parentId" });
						return {
							data: tree,
							success: true,
						};
					}}
					expandable={{
						defaultExpandAllRows: true,
					}}
				/>
			</Card>
			<DataDictionaryModal
				visible={modalVisible}
				onClose={() => setModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
				dataId={selectedData?.id}
				parentId={selectedParentId}
			/>

			<DataDictionaryItemDrawer
				visible={drawerVisible}
				onClose={() => setDrawerVisible(false)}
				dataDto={selectedData}
			/>
		</>
	);
};

export default DataDictionaryTable;
