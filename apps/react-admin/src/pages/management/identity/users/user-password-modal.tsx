import { Form, Input, Modal, Button } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation } from "@tanstack/react-query";
import { changePasswordApi } from "@/api/management/identity/users";
import { toast } from "sonner";

interface UserPasswordModalProps {
	visible: boolean;
	userId?: string;
	onClose: () => void;
	onChange: () => void;
}

const UserPasswordModal: React.FC<UserPasswordModalProps> = ({ visible, userId, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const { mutateAsync: changePassword, isPending } = useMutation({
		mutationFn: (password: string) => {
			if (!userId) {
				return Promise.reject(new Error("userId is undefined"));
			}
			return changePasswordApi(userId, { password });
		},
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange();
			onClose();
			form.resetFields();
		},
	});

	// Generate a random password
	const generatePassword = () => {
		const length = 12;
		const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+";
		let password = "";
		for (let i = 0; i < length; i++) {
			password += charset.charAt(Math.floor(Math.random() * charset.length));
		}
		form.setFieldsValue({ password });
	};

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			await changePassword(values.password);
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={$t("AbpIdentity.SetPassword")}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isPending}
			width={500}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item label={$t("AbpIdentity.Password")} name="password" rules={[{ required: true }]}>
					<Input.Search
						className="w-full"
						enterButton={<Button type="primary">{$t("AbpIdentity.RandomPassword")}</Button>}
						onSearch={generatePassword}
						allowClear={false}
					/>
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default UserPasswordModal;
