import type React from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import type { PropertyInfo } from "./types";

interface PropertyModalProps {
	visible: boolean;
	onClose: () => void;
	onChange: (data: PropertyInfo) => void;
}

const PropertyModal: React.FC<PropertyModalProps> = ({ visible, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			onChange({
				key: values.key,
				value: values.value,
			});
			form.resetFields();
			onClose();
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	const handleCancel = () => {
		form.resetFields();
		onClose();
	};

	return (
		<Modal
			title={$t("component.extra_property_dictionary.title")}
			open={visible}
			onOk={handleOk}
			onCancel={handleCancel}
			width="50%"
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item label={$t("component.extra_property_dictionary.key")} name="key" rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>
				<Form.Item label={$t("component.extra_property_dictionary.value")} name="value" rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default PropertyModal;
