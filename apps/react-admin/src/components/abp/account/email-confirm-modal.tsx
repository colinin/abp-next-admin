import { useState } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation } from "@tanstack/react-query";
import { confirmEmailApi } from "@/api/account/profile";
import { toast } from "sonner";

interface Props {
	visible: boolean;
	onSuccess: () => void;
	onClose: () => void;
	initialState: {
		confirmToken: string;
		email: string;
		returnUrl?: string;
		userId: string;
	};
}

const EmailConfirmModal: React.FC<Props> = ({ visible, onClose, onSuccess, initialState }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [confirmLoading, setConfirmLoading] = useState(false);

	const { mutateAsync: confirmEmail } = useMutation({
		mutationFn: confirmEmailApi,
		onSuccess: () => {
			toast.success($t("AbpAccount.YourEmailIsSuccessfullyConfirm"));
			onSuccess();
			onClose();
			if (initialState.returnUrl) {
				window.location.href = initialState.returnUrl;
			}
		},
		onSettled: () => {
			setConfirmLoading(false);
		},
	});

	const handleSubmit = async () => {
		try {
			setConfirmLoading(true);
			await confirmEmail({
				confirmToken: decodeURIComponent(initialState.confirmToken),
			});
		} catch (error) {
			console.error("Email confirmation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={$t("AbpAccount.EmailConfirm")}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={confirmLoading}
		>
			<Form form={form} initialValues={initialState}>
				<Form.Item label={$t("AbpAccount.DisplayName:Email")} name="email">
					<Input readOnly />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default EmailConfirmModal;
