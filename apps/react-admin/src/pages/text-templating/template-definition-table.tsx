import type React from "react";
import { useRef, useState } from "react";
import { Button, Dropdown, Space, Checkbox, type FormInstance, Card } from "antd";
import {
	EditOutlined,
	DeleteOutlined,
	PlusOutlined,
	EllipsisOutlined,
	CheckOutlined,
	CloseOutlined,
} from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import { getListApi, deleteApi } from "@/api/text-templating/template-definitions";
import type { TextTemplateDefinitionDto } from "#/text-templating/definitions";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { TextTemplatePermissions } from "@/constants/text-templating/permissions";
import TemplateDefinitionModal from "./template-definition-modal";
import TemplateContentModal from "./template-content-modal";
import DeleteModal from "@/components/abp/common/delete-modal";
import orderBy from "lodash.orderby";

const TemplateDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const actionRef = useRef<ActionType>();
	// const queryClient = useQueryClient();
	const formRef = useRef<FormInstance>();
	// Modal States
	const [definitionModalVisible, setDefinitionModalVisible] = useState(false);
	const [contentModalVisible, setContentModalVisible] = useState(false);
	const [selectedTemplateName, setSelectedTemplateName] = useState<string | undefined>();
	const [selectedTemplateForContent, setSelectedTemplateForContent] = useState<TextTemplateDefinitionDto | undefined>();

	// Delete State
	const [deleteModalVisible, setDeleteModalVisible] = useState(false);
	const [itemToDelete, setItemToDelete] = useState<TextTemplateDefinitionDto | null>(null);

	const { mutateAsync: deleteTemplate } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
			setDeleteModalVisible(false);
		},
	});

	const handleCreate = () => {
		setSelectedTemplateName(undefined);
		setDefinitionModalVisible(true);
	};

	const handleUpdate = (record: TextTemplateDefinitionDto) => {
		setSelectedTemplateName(record.name);
		setDefinitionModalVisible(true);
	};

	const handleDelete = (record: TextTemplateDefinitionDto) => {
		setItemToDelete(record);
		setDeleteModalVisible(true);
	};

	const handleMenuClick = (key: string, record: TextTemplateDefinitionDto) => {
		if (key === "contents") {
			setSelectedTemplateForContent(record);
			setContentModalVisible(true);
		}
	};

	const columns: ProColumns<TextTemplateDefinitionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpTextTemplating.DisplayName:IsLayout"),
			dataIndex: "isLayout",
			valueType: "checkbox",
			hideInTable: true, // Used for filter
			renderFormItem: () => (
				<Checkbox
					onChange={(e) => {
						const value = e.target.checked; // 获取 Checkbox 的选中状态
						onFilter("isLayout", value, false);
					}}
				>
					{$t("AbpTextTemplating.DisplayName:IsLayout")}
				</Checkbox>
			),
		},
		{
			title: $t("AbpTextTemplating.DisplayName:Name"),
			dataIndex: "name",
			sorter: true,
			minWidth: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpTextTemplating.DisplayName:DisplayName"),
			dataIndex: "displayName",
			sorter: true,
			minWidth: 150,
			hideInSearch: true,
			render: (_, record) => {
				const d = deserialize(record.displayName);
				return Lr(d.resourceName, d.name);
			},
		},
		{
			title: $t("AbpTextTemplating.DisplayName:IsStatic"),
			dataIndex: "isStatic",
			align: "center",
			width: 100,
			hideInSearch: true,
			render: (val) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("AbpTextTemplating.DisplayName:IsInlineLocalized"),
			dataIndex: "isInlineLocalized",
			align: "center",
			width: 150,
			hideInSearch: true,
			render: (val) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("AbpTextTemplating.DisplayName:IsLayout"),
			dataIndex: "isLayout",
			align: "center",
			width: 100,
			hideInSearch: true, // We have a separate filter column for this
			render: (val) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
		},
		{
			title: $t("AbpTextTemplating.DisplayName:Layout"),
			dataIndex: "layout",
			hideInSearch: true,
		},
		{
			title: $t("AbpTextTemplating.DisplayName:DefaultCultureName"),
			dataIndex: "defaultCultureName",
			hideInSearch: true,
		},
		{
			title: $t("AbpTextTemplating.LocalizationResource"),
			dataIndex: "localizationResourceName",
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 220,
			render: (_, record) => (
				<Space>
					{hasAccessByCodes([TextTemplatePermissions.Update]) && (
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{!record.isStatic && hasAccessByCodes([TextTemplatePermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
					<Dropdown
						menu={{
							items: [
								{
									key: "contents",
									label: $t("AbpTextTemplating.EditContents"),
									icon: <EditOutlined />,
								},
							],
							onClick: ({ key }) => handleMenuClick(key, record),
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	const onFilter = (field: string, value: any, shouldSubmit = true) => {
		// 使用 formRef 更新查询条件
		if (formRef.current) {
			formRef.current.setFieldsValue({
				[field]: value,
			});
		}
		if (shouldSubmit) {
			// 触发表格刷新
			formRef.current?.submit();
		}
	};

	return (
		<>
			<Card>
				<ProTable<TextTemplateDefinitionDto>
					headerTitle={$t("AbpTextTemplating.TextTemplates")}
					actionRef={actionRef}
					formRef={formRef} //search form
					rowKey="name"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						hasAccessByCodes([TextTemplatePermissions.Create]) && (
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AbpTextTemplating.TextTemplates:AddNew")}
							</Button>
						),
					]}
					request={async (params, sorter) => {
						// Client side paging/sorting logic mirroring Vue
						const { items } = await getListApi({
							filter: params.filter,
							isLayout: params.isLayout === true ? true : undefined, // Only filter if checked, otherwise undefined
						});

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

						return {
							data: dataSource.slice(startIndex, endIndex),
							total: dataSource.length,
							success: true,
						};
					}}
					pagination={{
						defaultPageSize: 10,
						showSizeChanger: true,
					}}
				/>
			</Card>
			<TemplateDefinitionModal
				visible={definitionModalVisible}
				templateName={selectedTemplateName}
				onClose={() => setDefinitionModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
			/>

			<TemplateContentModal
				visible={contentModalVisible}
				templateDefinition={selectedTemplateForContent}
				onClose={() => setContentModalVisible(false)}
			/>

			<DeleteModal
				visible={deleteModalVisible}
				// description={$t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: itemToDelete?.name })}
				onCancel={() => setDeleteModalVisible(false)}
				onConfirm={() => itemToDelete && deleteTemplate(itemToDelete.name)}
			/>
		</>
	);
};

export default TemplateDefinitionTable;
