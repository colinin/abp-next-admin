import { useEffect, useState } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { createApi, updateApi, getApi } from "@/api/management/localization/languages";
import type { LanguageDto } from "#/management/localization/languages";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: LanguageDto) => void;
	cultureName?: string;
}

const LocalizationLanguageModal: React.FC<Props> = ({ visible, onClose, onChange, cultureName }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (cultureName) {
				fetchLanguage(cultureName);
			}
		}
	}, [visible, cultureName, form]);

	const fetchLanguage = async (name: string) => {
		try {
			setLoading(true);
			const dto = await getApi(name);
			form.setFieldsValue(dto);
		} finally {
			setLoading(false);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const api = cultureName ? updateApi(cultureName, values) : createApi(values);

			const res = await api;
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	const modalTitle = cultureName
		? `${$t("AbpLocalization.Languages")} - ${cultureName}`
		: $t("LocalizationManagement.Language:AddNew");

	return (
		<Modal
			open={visible}
			title={modalTitle}
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
					<Input disabled={!!cultureName} autoComplete="off" />
				</Form.Item>
				<Form.Item label={$t("AbpLocalization.DisplayName:UiCultureName")} name="uiCultureName">
					<Input autoComplete="off" />
				</Form.Item>
				<Form.Item
					label={$t("AbpLocalization.DisplayName:DisplayName")}
					name="displayName"
					rules={[{ required: true }]}
				>
					<Input autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default LocalizationLanguageModal;
