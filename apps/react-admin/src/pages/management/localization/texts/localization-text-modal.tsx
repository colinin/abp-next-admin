import { useEffect, useState } from "react";
import { Modal, Form, Input, Select } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { getApi, setApi } from "@/api/management/localization/texts";
import { getListApi as getLanguagesApi } from "@/api/management/localization/languages";
import { getListApi as getResourcesApi } from "@/api/management/localization/resources";
import type { TextDto, TextDifferenceDto } from "#/management/localization/texts";
import type { LanguageDto } from "#/management/localization/languages";
import type { ResourceDto } from "#/management/localization/resources";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: TextDto) => void;
	data?: TextDifferenceDto; // Data passed from the table row
	defaultTargetCulture?: string; // For creation
	defaultResourceName?: string; // For creation
}

const LocalizationTextModal: React.FC<Props> = ({
	visible,
	onClose,
	onChange,
	data,
	defaultTargetCulture,
	defaultResourceName,
}) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);
	const [isEdit, setIsEdit] = useState(false);

	// Dropdown Data
	const [languages, setLanguages] = useState<LanguageDto[]>([]);
	const [resources, setResources] = useState<ResourceDto[]>([]);

	// Fetch Options on Mount
	useEffect(() => {
		const initOptions = async () => {
			const [langRes, resRes] = await Promise.all([getLanguagesApi(), getResourcesApi()]);
			setLanguages(langRes.items);
			setResources(resRes.items);
		};
		initOptions();
	}, []);

	// Handle Modal Open State
	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (data?.targetCultureName) {
				// Edit Mode
				setIsEdit(true);
				fetchTextDetails({
					cultureName: data.targetCultureName,
					key: data.key,
					resourceName: data.resourceName,
				});
			} else {
				// Create Mode
				setIsEdit(false);
				form.setFieldsValue({
					cultureName: defaultTargetCulture,
					resourceName: defaultResourceName,
				});
			}
		}
	}, [visible, data, defaultTargetCulture, defaultResourceName, form]);

	const fetchTextDetails = async (input: { cultureName: string; key: string; resourceName: string }) => {
		try {
			setLoading(true);
			const textDto = await getApi(input);
			form.setFieldsValue(textDto);
		} finally {
			setLoading(false);
		}
	};

	// Handle changing culture dropdown inside the modal to switch context
	const handleLanguageChange = async (newCulture: string) => {
		// Only fetch if we have enough info to look up the key
		const currentValues = form.getFieldsValue();
		if (currentValues.key && currentValues.resourceName) {
			await fetchTextDetails({
				cultureName: newCulture,
				key: currentValues.key,
				resourceName: currentValues.resourceName,
			});
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);
			await setApi(values);
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(values);
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Modal
			open={visible}
			title={isEdit ? `${$t("AbpLocalization.Texts")} - ${data?.key}` : $t("LocalizationManagement.Text:AddNew")}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			loading={loading}
			destroyOnClose
			width="50%"
		>
			<Form form={form} layout="vertical">
				<Form.Item
					label={$t("AbpLocalization.DisplayName:CultureName")}
					name="cultureName"
					rules={[{ required: true }]}
				>
					<Select
						showSearch
						optionFilterProp="displayName"
						fieldNames={{ label: "displayName", value: "cultureName" }}
						options={languages}
						onChange={handleLanguageChange}
					/>
				</Form.Item>

				<Form.Item
					label={$t("AbpLocalization.DisplayName:ResourceName")}
					name="resourceName"
					rules={[{ required: true }]}
				>
					<Select
						showSearch
						optionFilterProp="displayName"
						fieldNames={{ label: "displayName", value: "name" }}
						options={resources}
						disabled={isEdit} // Resource is locked when editing a specific key
					/>
				</Form.Item>

				<Form.Item label={$t("AbpLocalization.DisplayName:Key")} name="key" rules={[{ required: true }]}>
					<Input disabled={isEdit} autoComplete="off" />
				</Form.Item>

				<Form.Item label={$t("AbpLocalization.DisplayName:Value")} name="value" rules={[{ required: true }]}>
					<Input.TextArea autoSize={{ minRows: 3 }} autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default LocalizationTextModal;
