import type React from "react";
import { useState, useEffect } from "react";
import { Modal, Form, Input, Button, Col, Row } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { changePhoneNumberApi, sendChangePhoneNumberCodeApi } from "@/api/account/profile";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (phoneNumber: string) => void;
}

const ChangePhoneNumberModal: React.FC<Props> = ({ visible, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);
	const [countdown, setCountdown] = useState(0);

	// Custom countdown logic using useEffect
	useEffect(() => {
		let timer: NodeJS.Timeout;
		if (countdown > 0) {
			timer = setInterval(() => {
				setCountdown((prev) => (prev > 0 ? prev - 1 : 0));
			}, 1000);
		}
		return () => clearInterval(timer);
	}, [countdown]);

	const { mutateAsync: sendCode } = useMutation({
		mutationFn: sendChangePhoneNumberCodeApi,
		onSuccess: () => {
			setCountdown(60); // Set countdown to 60 seconds
			toast.success($t("AbpUi.SavedSuccessfully"));
		},
	});

	const { mutateAsync: changePhone } = useMutation({
		mutationFn: changePhoneNumberApi,
		onSuccess: (_, variables) => {
			toast.success($t("AbpAccount.PhoneNumberChangedMessage"));
			onChange(variables.newPhoneNumber);
			onClose();
			form.resetFields();
		},
	});

	const handleSendCode = async () => {
		try {
			await form.validateFields(["newPhoneNumber"]);
			const newPhone = form.getFieldValue("newPhoneNumber");
			await sendCode({ newPhoneNumber: newPhone });
		} catch (error) {
			console.error(error);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);
			await changePhone({
				code: values.code,
				newPhoneNumber: values.newPhoneNumber,
			});
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Modal
			title={$t("AbpIdentity.PhoneNumber")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item
					name="newPhoneNumber"
					label={$t("AbpIdentity.DisplayName:NewPhoneNumber")}
					rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}
				>
					<Input />
				</Form.Item>

				<Form.Item label={$t("AbpIdentity.DisplayName:SmsVerifyCode")}>
					<Row gutter={8}>
						<Col span={16}>
							<Form.Item name="code" noStyle rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}>
								<Input />
							</Form.Item>
						</Col>
						<Col span={8}>
							<Button block disabled={countdown > 0} onClick={handleSendCode}>
								{countdown === 0 ? $t("authentication.sendCode") : `${countdown}s`}
							</Button>
						</Col>
					</Row>
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default ChangePhoneNumberModal;
