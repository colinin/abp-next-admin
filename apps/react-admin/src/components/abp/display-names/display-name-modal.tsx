import { Form, Input, Modal, Select } from "antd";
import { useTranslation } from "react-i18next";
import type { DisplayNameInfo } from "./types";
import useAbpStore from "@/store/abpCoreStore";

interface DisplayNameModalProps {
	visible: boolean;
	onClose: () => void;
	onChange: (data: DisplayNameInfo) => void;
}

const DisplayNameModal: React.FC<DisplayNameModalProps> = ({ visible, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const { application } = useAbpStore();

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			onChange({
				culture: values.culture,
				displayName: values.displayName,
			});
			form.resetFields();
			onClose();
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={$t("AbpOpenIddict.DisplayName:AddNew")}
			onCancel={onClose}
			onOk={handleOk}
			width="50%"
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item label={$t("AbpOpenIddict.DisplayName:CultureName")} name="culture" rules={[{ required: true }]}>
					<Select
						className="w-full"
						options={application?.localization.languages.map((lang) => ({
							label: lang.displayName,
							value: lang.cultureName,
						}))}
					/>
				</Form.Item>
				<Form.Item label={$t("AbpOpenIddict.DisplayName:DisplayName")} name="displayName" rules={[{ required: true }]}>
					<Input className="w-full" autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default DisplayNameModal;
