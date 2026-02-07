import type React from "react";
import { useState } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { changePasswordApi } from "@/api/account/profile";
import { usePasswordValidator } from "@/hooks/abp/identity/usePasswordValidator";

interface Props {
	visible: boolean;
	onClose: () => void;
}

const ChangePasswordModal: React.FC<Props> = ({ visible, onClose }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);
	const { validate } = usePasswordValidator();

	const { mutateAsync: changePassword } = useMutation({
		mutationFn: changePasswordApi,
		onSuccess: () => {
			toast.success($t("AbpIdentity.PasswordChangedMessage"));
			onClose();
			form.resetFields();
		},
	});

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);
			await changePassword({
				currentPassword: values.currentPassword,
				newPassword: values.newPassword,
			});
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Modal
			title={$t("AbpAccount.ResetMyPassword")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item
					name="currentPassword"
					label={$t("AbpAccount.DisplayName:CurrentPassword")}
					rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}
				>
					<Input.Password />
				</Form.Item>

				<Form.Item
					name="newPassword"
					label={$t("AbpAccount.DisplayName:NewPassword")}
					rules={[
						{ required: true, message: $t("AbpUi.ThisFieldIsRequired") },
						({ getFieldValue }) => ({
							validator: async (_, value) => {
								if (!value) return Promise.resolve();
								if (value === getFieldValue("currentPassword")) {
									return Promise.reject(new Error($t("AbpAccount.NewPasswordSameAsOld")));
								}
								try {
									await validate(value);
									return Promise.resolve();
								} catch (error: any) {
									return Promise.reject(error); // Validation error from hook
								}
							},
						}),
					]}
				>
					<Input.Password />
				</Form.Item>

				<Form.Item
					name="newPasswordConfirm"
					label={$t("AbpAccount.DisplayName:NewPasswordConfirm")}
					dependencies={["newPassword"]}
					rules={[
						{ required: true, message: $t("AbpUi.ThisFieldIsRequired") },
						({ getFieldValue }) => ({
							validator(_, value) {
								if (!value || getFieldValue("newPassword") === value) {
									return Promise.resolve();
								}
								return Promise.reject(new Error($t("AbpIdentity.Volo_Abp_Identity:PasswordConfirmationFailed")));
							},
						}),
					]}
				>
					<Input.Password />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default ChangePasswordModal;
