import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Checkbox, Select, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import useAbpStore from "@/store/abpCoreStore";
import { createApi, updateApi, getApi, getListApi } from "@/api/text-templating/template-definitions";
import { getListApi as getResourcesApi } from "@/api/management/localization/resources";
import type { TextTemplateDefinitionDto } from "#/text-templating/definitions";
import type { ResourceDto } from "#/management/localization/resources";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: TextTemplateDefinitionDto) => void;
	templateName?: string;
}

const TemplateDefinitionModal: React.FC<Props> = ({ visible, onClose, onChange, templateName }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const application = useAbpStore((state) => state.application);
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();

	const [activeTab, setActiveTab] = useState("basic");
	const [isEdit, setIsEdit] = useState(false);
	const [submitting, setSubmitting] = useState(false);
	const [loading, setLoading] = useState(false);
	const [formModel, setFormModel] = useState<Partial<TextTemplateDefinitionDto>>({});

	// Dropdown Options
	const [layouts, setLayouts] = useState<any[]>([]);
	const [resources, setResources] = useState<ResourceDto[]>([]);

	// Derived Options
	const languageOptions = (application?.localization.languages || []).map((l) => ({
		label: l.displayName,
		value: l.cultureName,
	}));

	// Initial Data Fetch
	useEffect(() => {
		if (visible) {
			loadInitialData();
		}
	}, [visible]);

	const loadInitialData = async () => {
		// Load Layouts
		const layoutRes = await getListApi({ isLayout: true });
		const formattedLayouts = layoutRes.items.map((item) => {
			const localizable = deserialize(item.displayName);
			return {
				...item,
				displayName: Lr(localizable.resourceName, localizable.name),
			};
		});
		setLayouts(formattedLayouts);

		// Load Resources if allowed
		if (hasAccessByCodes(["LocalizationManagement.Resource"])) {
			const res = await getResourcesApi();
			setResources(res.items);
		}
	};

	// Load Definition Data
	useEffect(() => {
		if (visible) {
			if (templateName) {
				setIsEdit(true);
				fetchDefinition(templateName);
			} else {
				setIsEdit(false);
				const initial = {
					displayName: "",
					extraProperties: {},
					isInlineLocalized: false,
					isLayout: false,
					isStatic: false,
					name: "",
				};
				setFormModel(initial);
				form.setFieldsValue(initial);
			}
		}
	}, [visible, templateName]);

	const fetchDefinition = async (name: string) => {
		try {
			setLoading(true);
			const dto = await getApi(name);
			setFormModel(dto);
			form.setFieldsValue(dto);
		} finally {
			setLoading(false);
		}
	};

	const handlePropChange = (prop: any) => {
		setFormModel((prev) => ({
			...prev,
			extraProperties: { ...prev.extraProperties, [prop.key]: prop.value },
		}));
	};

	const handlePropDelete = (prop: any) => {
		setFormModel((prev) => {
			const nextProps = { ...prev.extraProperties };
			delete nextProps[prop.key];
			return { ...prev, extraProperties: nextProps };
		});
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);
			const submitData = {
				...formModel,
				...values,
			} as TextTemplateDefinitionDto;

			const api = isEdit ? updateApi(submitData.name, submitData) : createApi(submitData as any);

			const res = await api;
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		} catch (e) {
			console.error(e);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Modal
			title={
				isEdit
					? `${$t("AbpTextTemplating.TextTemplates")} - ${templateName}`
					: $t("AbpTextTemplating.TextTemplates:AddNew")
			}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			loading={loading}
			width="50%"
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Tabs activeKey={activeTab} onChange={setActiveTab}>
					<Tabs.TabPane key="basic" tab={$t("AbpTextTemplating.BasicInfo")}>
						<Form.Item name="name" label={$t("AbpTextTemplating.DisplayName:Name")} rules={[{ required: true }]}>
							<Input disabled={!isEdit && formModel.isStatic} autoComplete="off" />
						</Form.Item>

						<Form.Item
							name="displayName"
							label={$t("WebhooksManagement.DisplayName:DisplayName")}
							rules={[{ required: true }]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						<Form.Item name="isInlineLocalized" valuePropName="checked">
							<Checkbox
								disabled={formModel.isStatic}
								onChange={(e) => {
									// Logic from Vue: if inline localized, clear resource name
									if (e.target.checked) {
										form.setFieldValue("localizationResourceName", undefined);
									}
								}}
							>
								{$t("AbpTextTemplating.DisplayName:IsInlineLocalized")}
							</Checkbox>
						</Form.Item>

						<Form.Item noStyle shouldUpdate={(prev, curr) => prev.isInlineLocalized !== curr.isInlineLocalized}>
							{({ getFieldValue }) =>
								!getFieldValue("isInlineLocalized") ? (
									<Form.Item name="defaultCultureName" label={$t("AbpTextTemplating.DisplayName:DefaultCultureName")}>
										<Select allowClear options={languageOptions} disabled={formModel.isStatic} />
									</Form.Item>
								) : (
									<Form.Item name="localizationResourceName" label={$t("AbpTextTemplating.LocalizationResource")}>
										<Select
											allowClear
											options={resources}
											fieldNames={{ label: "displayName", value: "name" }}
											disabled={formModel.isStatic}
										/>
									</Form.Item>
								)
							}
						</Form.Item>

						<Form.Item name="isLayout" valuePropName="checked">
							<Checkbox
								disabled={formModel.isStatic}
								onChange={(e) => {
									if (e.target.checked) {
										form.setFieldValue("layout", undefined);
									}
								}}
							>
								{$t("AbpTextTemplating.DisplayName:IsLayout")}
							</Checkbox>
						</Form.Item>

						<Form.Item noStyle shouldUpdate={(prev, curr) => prev.isLayout !== curr.isLayout}>
							{({ getFieldValue }) =>
								!getFieldValue("isLayout") && (
									<Form.Item name="layout" label={$t("AbpTextTemplating.DisplayName:Layout")}>
										<Select
											allowClear
											options={layouts}
											fieldNames={{ label: "displayName", value: "name" }}
											disabled={formModel.isStatic}
										/>
									</Form.Item>
								)
							}
						</Form.Item>
					</Tabs.TabPane>

					<Tabs.TabPane key="properties" tab={$t("AbpTextTemplating.Properties")}>
						<PropertyTable
							data={formModel.extraProperties}
							disabled={formModel.isStatic}
							onChange={handlePropChange}
							onDelete={handlePropDelete}
						/>
					</Tabs.TabPane>
				</Tabs>
			</Form>
		</Modal>
	);
};

export default TemplateDefinitionModal;
