import { useEffect, useState } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { createApi, updateApi, getApi } from "@/api/saas/editions";
import type { EditionDto } from "#/saas/editions";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: EditionDto) => void;
	editionId?: string;
}

const EditionModal: React.FC<Props> = ({ visible, onClose, onChange, editionId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);
	const [concurrencyStamp, setConcurrencyStamp] = useState<string | undefined>(undefined);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (editionId) {
				fetchEdition(editionId);
			} else {
				setConcurrencyStamp(undefined);
			}
		}
	}, [visible, editionId, form]);

	const fetchEdition = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			form.setFieldsValue(dto);
			setConcurrencyStamp(dto.concurrencyStamp);
		} finally {
			setLoading(false);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const api = editionId ? updateApi(editionId, { ...values, concurrencyStamp }) : createApi(values);

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

	const modalTitle = editionId
		? `${$t("AbpSaas.Editions")} - ${form.getFieldValue("displayName") || ""}`
		: $t("AbpSaas.NewEdition");

	return (
		<Modal
			open={visible}
			title={modalTitle}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			loading={loading}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item label={$t("AbpSaas.DisplayName:EditionName")} name="displayName" rules={[{ required: true }]}>
					<Input autoComplete="off" allowClear />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default EditionModal;
