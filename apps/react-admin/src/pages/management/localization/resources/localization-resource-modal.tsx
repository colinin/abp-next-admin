import { useEffect, useState } from "react";
import { Modal, Form, Input, Checkbox } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { createApi, updateApi, getApi } from "@/api/management/localization/resources";
import type { ResourceDto } from "#/management/localization/resources";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: ResourceDto) => void;
	resourceName?: string;
}

const defaultModel: ResourceDto = {
	displayName: "",
	enable: true,
	name: "",
} as ResourceDto;

const LocalizationResourceModal: React.FC<Props> = ({ visible, onClose, onChange, resourceName }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (resourceName) {
				fetchResource(resourceName);
			} else {
				form.setFieldsValue(defaultModel);
			}
		}
	}, [visible, resourceName, form]);

	const fetchResource = async (name: string) => {
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

			const api = resourceName ? updateApi(resourceName, values) : createApi(values);

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

	const modalTitle = resourceName
		? `${$t("AbpLocalization.Resources")} - ${resourceName}`
		: $t("LocalizationManagement.Resource:AddNew");

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
			<Form form={form} layout="vertical" initialValues={defaultModel}>
				<Form.Item name="enable" valuePropName="checked">
					<Checkbox>{$t("LocalizationManagement.DisplayName:Enable")}</Checkbox>
				</Form.Item>
				<Form.Item label={$t("AbpLocalization.DisplayName:ResourceName")} name="name" rules={[{ required: true }]}>
					<Input disabled={!!resourceName} autoComplete="off" />
				</Form.Item>
				<Form.Item
					label={$t("AbpLocalization.DisplayName:DisplayName")}
					name="displayName"
					rules={[{ required: true }]}
				>
					<Input autoComplete="off" />
				</Form.Item>
				<Form.Item label={$t("AbpLocalization.DisplayName:Description")} name="description">
					<Input autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default LocalizationResourceModal;
