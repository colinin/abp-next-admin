import { Modal, Form, InputNumber, Select } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation } from "@tanstack/react-query";
import { lockApi } from "@/api/management/identity/users";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: () => void;
	userId: string;
}

enum LockType {
	Seconds = 1,
	Minutes = 60,
	Hours = 3600,
	Days = 86400,
	Months = 2678400, // 按31天计算
	Years = 32140800, // 按31*12天计算
}

const UserLockModal: React.FC<Props> = ({ visible, onClose, onChange, userId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	// 锁定用户
	const { mutateAsync: lockUser, isPending } = useMutation({
		mutationFn: ({ id, seconds }: { id: string; seconds: number }) => lockApi(id, seconds),
		onSuccess: () => {
			onChange();
			onClose();
		},
	});

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			const lockSeconds = values.type * values.seconds;
			await lockUser({ id: userId, seconds: lockSeconds });
		} catch (error) {
			// Form validation error
		}
	};

	return (
		<Modal open={visible} title={$t("AbpIdentity.Lock")} onCancel={onClose} onOk={handleOk} confirmLoading={isPending}>
			<Form form={form} labelCol={{ span: 6 }} wrapperCol={{ span: 18 }} initialValues={{ type: LockType.Seconds }}>
				<Form.Item name="seconds" label={$t("AbpIdentity.LockTime")} rules={[{ required: true }]}>
					<InputNumber className="w-full" />
				</Form.Item>

				<Form.Item name="type" label={$t("AbpIdentity.LockType")} rules={[{ required: true }]}>
					<Select
						options={[
							{ label: $t("LockType:Seconds"), value: LockType.Seconds },
							{ label: $t("LockType:Minutes"), value: LockType.Minutes },
							{ label: $t("LockType:Hours"), value: LockType.Hours },
							{ label: $t("LockType:Days"), value: LockType.Days },
							{ label: $t("LockType:Months"), value: LockType.Months },
							{ label: $t("LockType:Years"), value: LockType.Years },
						]}
					/>
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default UserLockModal;
