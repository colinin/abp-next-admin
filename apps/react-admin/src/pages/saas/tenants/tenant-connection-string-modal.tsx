import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Select } from "antd";
import { useTranslation } from "react-i18next";
import { checkConnectionString } from "@/api/saas/tenants";
import type { TenantConnectionStringDto } from "#/saas/tenants";

interface Props {
	visible: boolean;
	onClose: () => void;
	onSubmit: (data: TenantConnectionStringDto) => Promise<void>;
	data?: TenantConnectionStringDto;
	dataBaseOptions: { label: string; value: string }[];
}

const TenantConnectionStringModal: React.FC<Props> = ({ visible, onClose, onSubmit, data, dataBaseOptions }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (data) {
				form.setFieldsValue({
					provider: "MySql", // Default fallback from Vue code
					...data,
				});
			} else {
				form.setFieldsValue({ provider: "MySql" });
			}
		}
	}, [visible, data, form]);

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			// Validate connection string on server
			await checkConnectionString({
				connectionString: values.value,
				name: values.name,
				provider: values.provider,
			});

			await onSubmit(values);
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	const title = data?.name ? `${$t("AbpSaas.ConnectionStrings")} - ${data.name}` : $t("AbpSaas.ConnectionStrings");

	return (
		<Modal title={title} open={visible} onCancel={onClose} onOk={handleOk} confirmLoading={submitting} destroyOnClose>
			<Form form={form} layout="vertical" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Form.Item name="provider" label={$t("AbpSaas.DisplayName:DataBaseProvider")} rules={[{ required: true }]}>
					<Select options={dataBaseOptions} />
				</Form.Item>
				<Form.Item name="name" label={$t("AbpSaas.DisplayName:Name")} rules={[{ required: true }]}>
					<Input disabled={!!data?.name} autoComplete="off" />
				</Form.Item>
				<Form.Item name="value" label={$t("AbpSaas.DisplayName:Value")} rules={[{ required: true }]}>
					<Input.TextArea autoSize={{ minRows: 3 }} />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default TenantConnectionStringModal;
