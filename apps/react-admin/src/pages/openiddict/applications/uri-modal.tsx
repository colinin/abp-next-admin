import { Form, Input, Modal } from "antd";
import { useTranslation } from "react-i18next";

interface UriModalProps {
	visible: boolean;
	onClose: () => void;
	onChange: (uri: string) => void;
}

const UriModal: React.FC<UriModalProps> = ({ visible, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			onChange(values.uri);
			form.resetFields();
			onClose();
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={$t("AbpOpenIddict.Uri:AddNew")}
			onCancel={onClose}
			onOk={handleOk}
			width="50%"
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item label="Uri" name="uri" rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default UriModal;
