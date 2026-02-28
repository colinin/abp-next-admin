import { useRef, useState } from "react";
import { Button, Card, Modal, Space } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { getListApi, deleteApi } from "@/api/management/localization/resources";
import type { ResourceDto } from "#/management/localization/resources";
import LocalizationResourceModal from "./localization-resource-modal";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { ResourcesPermissions } from "@/constants/management/localization/permissions";
import useAbpStore from "@/store/abpCoreStore";
import orderBy from "lodash.orderby";
import { useSetLocale } from "@/store/localeI18nStore";
import { getStringItem } from "@/utils/storage";
import { LocalEnum, StorageEnum } from "#/enum";

const LocalizationResourceTable: React.FC = () => {
	const { t: $t, i18n } = useTranslation();
	const setLocale = useSetLocale();
	const currentLng = getStringItem(StorageEnum.I18N) || LocalEnum.en_US;
	const actionRef = useRef<ActionType>();
	const [modalVisible, setModalVisible] = useState(false);
	const [currentResource, setCurrentResource] = useState<string | undefined>();
	const [modal, contextHolder] = Modal.useModal();

	const application = useAbpStore((state) => state.application);

	const handleOpenModal = (name?: string) => {
		setCurrentResource(name);
		setModalVisible(true);
	};

	const handleDelete = (row: ResourceDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: async () => {
				await deleteApi(row.name);
				toast.success($t("AbpUi.DeletedSuccessfully"));
				handleDataChange();
			},
		});
	};

	const handleDataChange = async () => {
		actionRef.current?.reload();

		// Refresh localization cache
		const currentCultureName = application?.localization.currentCulture.cultureName;
		if (currentCultureName) {
			try {
				await setLocale(currentLng as LocalEnum, i18n);
			} catch (error) {
				console.error("Failed to refresh localization", error);
			}
		}
	};

	const columns: ProColumns<ResourceDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpFeatureManagement.DisplayName:Name"),
			dataIndex: "name",
			sorter: true,
			hideInSearch: true,
			minWidth: 150,
		},
		{
			title: $t("AbpFeatureManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			sorter: true,
			hideInSearch: true,
			minWidth: 150,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 220,
			render: (_, record) => (
				<Space>
					{hasAccessByCodes([ResourcesPermissions.Update]) && (
						<Button type="link" icon={<EditOutlined />} onClick={() => handleOpenModal(record.name)}>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{/* !record.isStatic && no isStatic */}
					{hasAccessByCodes([ResourcesPermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<ResourceDto>
					headerTitle={$t("AbpLocalization.Resources")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={{
						labelWidth: "auto",
					}}
					toolBarRender={() => [
						hasAccessByCodes([ResourcesPermissions.Create]) && (
							<Button key="add" type="primary" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
								{$t("LocalizationManagement.Resource:AddNew")}
							</Button>
						),
					]}
					request={async (params, sorter) => {
						const { items } = await getListApi({ filter: params.filter });

						let dataSource = [...items];

						if (sorter && Object.keys(sorter).length > 0) {
							const sortField = Object.keys(sorter)[0];
							const sortOrder = sorter[sortField] === "ascend" ? "asc" : "desc";
							dataSource = orderBy(dataSource, [sortField], [sortOrder]);
						}

						const current = params.current || 1;
						const pageSize = params.pageSize || 10;
						const startIndex = (current - 1) * pageSize;
						const endIndex = startIndex + pageSize;
						const pageData = dataSource.slice(startIndex, endIndex);

						return {
							data: pageData,
							success: true,
							total: dataSource.length,
						};
					}}
					pagination={{
						defaultPageSize: 10,
						showSizeChanger: true,
					}}
				/>
			</Card>
			<LocalizationResourceModal
				visible={modalVisible}
				resourceName={currentResource}
				onClose={() => setModalVisible(false)}
				onChange={handleDataChange}
			/>
		</>
	);
};

export default LocalizationResourceTable;
