import { useRef, useState } from "react";
import { Button, Card, Modal, Space } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { getListApi, deleteApi } from "@/api/management/localization/languages";
import type { LanguageDto } from "#/management/localization/languages";
import LocalizationLanguageModal from "./localization-language-modal";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { LanguagesPermissions } from "@/constants/management/localization/permissions";
import useAbpStore from "@/store/abpCoreStore";
import orderBy from "lodash.orderby";
import { useSetLocale } from "@/store/localeI18nStore";
import { LocalEnum, StorageEnum } from "#/enum";
import { getStringItem } from "@/utils/storage";

const LocalizationLanguageTable: React.FC = () => {
	const { t: $t, i18n } = useTranslation();
	const setLocale = useSetLocale();
	const currentLng = getStringItem(StorageEnum.I18N) || LocalEnum.en_US;

	const actionRef = useRef<ActionType>();
	const [modalVisible, setModalVisible] = useState(false);
	const [currentCulture, setCurrentCulture] = useState<string | undefined>();
	const [modal, contextHolder] = Modal.useModal();

	// Use store to access and update application configuration
	const application = useAbpStore((state) => state.application);

	const handleOpenModal = (cultureName?: string) => {
		setCurrentCulture(cultureName);
		setModalVisible(true);
	};

	const handleDelete = (row: LanguageDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.cultureName }),
			onOk: async () => {
				await deleteApi(row.cultureName);
				toast.success($t("AbpUi.DeletedSuccessfully"));
				handleDataChange(row);
			},
		});
	};

	const handleDataChange = async (data: LanguageDto) => {
		actionRef.current?.reload();

		// If the changed language is the currently active language, refresh the localization config
		const currentCultureName = application?.localization.currentCulture.cultureName;
		if (data.cultureName === currentCultureName) {
			try {
				await setLocale(currentLng as LocalEnum, i18n);
			} catch (error) {
				console.error("Failed to refresh localization", error);
			}
		}
	};

	const columns: ProColumns<LanguageDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpLocalization.DisplayName:CultureName"),
			dataIndex: "cultureName",
			sorter: true,
			hideInSearch: true,
			minWidth: 150,
		},
		{
			title: $t("AbpLocalization.DisplayName:DisplayName"),
			dataIndex: "displayName",
			sorter: true,
			hideInSearch: true,
			minWidth: 150,
		},
		{
			title: $t("AbpLocalization.DisplayName:UiCultureName"),
			dataIndex: "uiCultureName",
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
					{hasAccessByCodes([LanguagesPermissions.Update]) && (
						<Button type="link" icon={<EditOutlined />} onClick={() => handleOpenModal(record.cultureName)}>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{/* !record.isStatic && no isStatic */}
					{hasAccessByCodes([LanguagesPermissions.Delete]) && (
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
				<ProTable<LanguageDto>
					headerTitle={$t("AbpLocalization.Languages")}
					actionRef={actionRef}
					rowKey="cultureName"
					columns={columns}
					search={{
						labelWidth: "auto",
					}}
					toolBarRender={() => [
						hasAccessByCodes([LanguagesPermissions.Create]) && (
							<Button key="add" type="primary" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
								{$t("LocalizationManagement.Language:AddNew")}
							</Button>
						),
					]}
					request={async (params, sorter) => {
						// Client-side pagination and sorting logic mirroring the Vue component
						// 1. Fetch all data
						const { items } = await getListApi({ filter: params.filter });

						let dataSource = [...items];

						// 2. Sort
						if (sorter && Object.keys(sorter).length > 0) {
							const sortField = Object.keys(sorter)[0];
							const sortOrder = sorter[sortField] === "ascend" ? "asc" : "desc";
							dataSource = orderBy(dataSource, [sortField], [sortOrder]);
						}

						// 3. Paginate
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
			<LocalizationLanguageModal
				visible={modalVisible}
				cultureName={currentCulture}
				onClose={() => setModalVisible(false)}
				onChange={handleDataChange}
			/>
		</>
	);
};

export default LocalizationLanguageTable;
