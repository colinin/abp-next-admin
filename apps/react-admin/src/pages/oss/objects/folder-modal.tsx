import { useEffect, useState } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { createApi } from "@/api/oss/objects";
import type { OssObjectDto } from "#/oss/objects";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: OssObjectDto) => void;
	bucket: string;
	path?: string;
}

const FolderModal: React.FC<Props> = ({ visible, onClose, onChange, bucket, path }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);

	useEffect(() => {
		if (visible) {
			form.resetFields();
		}
	}, [visible, form]);

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const dto = await createApi({
				bucket: bucket,
				fileName: values.name,
				overwrite: false,
				path: path,
			});

			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(dto);
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Modal
			open={visible}
			title={$t("AbpOssManagement.Objects:CreateFolder")}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item label={$t("AbpOssManagement.DisplayName:Name")} name="name" rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default FolderModal;
