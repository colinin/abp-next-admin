import { useRef, useState, useEffect } from "react";
import { Button, Card, Space, type FormInstance } from "antd";
import { EditOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import ProTable, { type ProColumns, type ActionType } from "@ant-design/pro-table";
import { getListApi } from "@/api/management/localization/texts";
import { getListApi as getLanguagesApi } from "@/api/management/localization/languages";
import { getListApi as getResourcesApi } from "@/api/management/localization/resources";
import type { TextDifferenceDto, TextDto } from "#/management/localization/texts";
import type { LanguageDto } from "#/management/localization/languages";
import type { ResourceDto } from "#/management/localization/resources";
import LocalizationTextModal from "./localization-text-modal";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { TextsPermissions } from "@/constants/management/localization/permissions";
import useAbpStore from "@/store/abpCoreStore";
import orderBy from "lodash.orderby";
import { useSetLocale } from "@/store/localeI18nStore";
import { getStringItem } from "@/utils/storage";
import { LocalEnum, StorageEnum } from "#/enum";

const LocalizationTextTable: React.FC = () => {
	const { t: $t, i18n } = useTranslation();
	const setLocale = useSetLocale();
	const currentLng = getStringItem(StorageEnum.I18N) || LocalEnum.en_US;

	const actionRef = useRef<ActionType>();
	// 2. Fix the type here: Change <Performance> to <ProFormInstance>
	const formRef = useRef<FormInstance>();

	// Modal State
	const [modalVisible, setModalVisible] = useState(false);
	const [currentRow, setCurrentRow] = useState<TextDifferenceDto | undefined>();

	// Filters Data
	const [languages, setLanguages] = useState<LanguageDto[]>([]);
	const [resources, setResources] = useState<ResourceDto[]>([]);

	const application = useAbpStore((state) => state.application);

	useEffect(() => {
		const initData = async () => {
			const [langRes, resRes] = await Promise.all([getLanguagesApi(), getResourcesApi()]);
			setLanguages(langRes.items);
			setResources(resRes.items);
		};
		initData();
	}, []);

	const handleOpenCreate = () => {
		setCurrentRow(undefined);
		setModalVisible(true);
	};

	const handleOpenEdit = (row: TextDifferenceDto) => {
		setCurrentRow(row);
		setModalVisible(true);
	};

	const handleDataChange = async (data: TextDto) => {
		actionRef.current?.reload();

		// If the modified text belongs to the current app culture, refresh the app configuration
		const currentAppCulture = application?.localization.currentCulture.cultureName;
		if (data.cultureName === currentAppCulture) {
			try {
				await setLocale(currentLng as LocalEnum, i18n);
			} catch (error) {
				console.error("Failed to refresh localization cache", error);
			}
		}
	};

	const columns: ProColumns<TextDifferenceDto>[] = [
		{
			title: $t("AbpLocalization.DisplayName:CultureName"),
			dataIndex: "cultureName",
			valueType: "select",
			fieldProps: {
				options: languages,
				fieldNames: { label: "displayName", value: "cultureName" },
				showSearch: true,
				optionFilterProp: "displayName",
			},
			hideInTable: true,
			initialValue: application?.localization.currentCulture.cultureName,
			formItemProps: {
				rules: [{ required: true }],
			},
		},
		{
			title: $t("AbpLocalization.DisplayName:TargetCultureName"),
			dataIndex: "targetCultureName",
			valueType: "select",
			fieldProps: {
				options: languages,
				fieldNames: { label: "displayName", value: "cultureName" },
				showSearch: true,
				optionFilterProp: "displayName",
			},
			hideInTable: true,
			initialValue: application?.localization.currentCulture.cultureName,
			formItemProps: {
				rules: [{ required: true }],
			},
		},
		{
			title: $t("AbpLocalization.DisplayName:ResourceName"),
			dataIndex: "resourceName",
			valueType: "select",
			fieldProps: {
				options: resources,
				fieldNames: { label: "displayName", value: "name" },
				showSearch: true,
				optionFilterProp: "displayName",
			},
			width: 150,
			sorter: true,
		},
		{
			title: $t("AbpLocalization.DisplayName:TargetValue"),
			dataIndex: "onlyNull",
			valueType: "select",
			hideInTable: true,
			fieldProps: {
				options: [
					{ label: $t("AbpLocalization.DisplayName:Any"), value: "false" },
					{ label: $t("AbpLocalization.DisplayName:OnlyNull"), value: "true" },
				],
			},
			initialValue: "false",
		},
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpLocalization.DisplayName:Key"),
			dataIndex: "key",
			width: 200,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpLocalization.DisplayName:Value"),
			dataIndex: "value",
			width: 300,
			sorter: true,
			hideInSearch: true,
			render: (dom) => <div className="break-all whitespace-pre-wrap">{dom}</div>,
		},
		{
			title: $t("AbpLocalization.DisplayName:TargetValue"),
			dataIndex: "targetValue",
			width: 300,
			sorter: true,
			hideInSearch: true,
			render: (dom) => <div className="break-all whitespace-pre-wrap">{dom}</div>,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 100,
			render: (_, record) => (
				<Space>
					{hasAccessByCodes([TextsPermissions.Update]) && (
						<Button type="link" icon={<EditOutlined />} onClick={() => handleOpenEdit(record)}>
							{$t("AbpUi.Edit")}
						</Button>
					)}
				</Space>
			),
		},
	];

	return (
		<>
			<Space direction="vertical" size="large" className="w-full">
				<Card>
					<ProTable<TextDifferenceDto>
						headerTitle={$t("AbpLocalization.Texts")}
						actionRef={actionRef}
						formRef={formRef}
						rowKey={(record) => record.resourceName + record.key}
						columns={columns}
						search={{
							labelWidth: "auto",
							defaultCollapsed: false,
						}}
						toolBarRender={() => [
							hasAccessByCodes([TextsPermissions.Create]) && (
								<Button key="add" type="primary" icon={<PlusOutlined />} onClick={handleOpenCreate}>
									{$t("LocalizationManagement.Text:AddNew")}
								</Button>
							),
						]}
						request={async (params, sorter) => {
							// Provide defaults for required fields if they are missing (initial load)
							const requestParams: any = {
								...params,
								cultureName: params.cultureName || application?.localization.currentCulture.cultureName,
								targetCultureName: params.targetCultureName || application?.localization.currentCulture.cultureName,
								onlyNull: params.onlyNull || "false",
							};

							// Fetch Data
							const { items } = await getListApi(requestParams);

							let dataSource = [...items];

							// Client-side Sort
							if (sorter && Object.keys(sorter).length > 0) {
								const sortField = Object.keys(sorter)[0];
								const sortOrder = sorter[sortField] === "ascend" ? "asc" : "desc";
								dataSource = orderBy(dataSource, [sortField], [sortOrder]);
							}

							// Client-side Pagination
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
						manualRequest={false}
					/>
				</Card>
			</Space>
			<LocalizationTextModal
				visible={modalVisible}
				data={currentRow}
				// Pass current form values as defaults for creation
				defaultTargetCulture={formRef.current?.getFieldValue("targetCultureName")}
				defaultResourceName={formRef.current?.getFieldValue("resourceName")}
				onClose={() => setModalVisible(false)}
				onChange={handleDataChange}
			/>
		</>
	);
};

export default LocalizationTextTable;
