import { Form, Input, Modal } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery } from "@tanstack/react-query";
import type { OpenIddictApplicationDto } from "#/openiddict/applications";
import { getApi, updateApi } from "@/api/openiddict/applications";
import { toast } from "sonner";

interface ApplicationSecretModalProps {
	visible: boolean;
	applicationId?: string;
	clientId?: string;
	onClose: () => void;
	onChange: (data: OpenIddictApplicationDto) => void;
}

const ApplicationSecretModal: React.FC<ApplicationSecretModalProps> = ({
	visible,
	applicationId,
	clientId,
	onClose,
	onChange,
}) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	// 获取应用详情
	const { data: applicationData, isLoading: isLoadingApp } = useQuery({
		queryKey: ["application", applicationId],
		queryFn: () => {
			if (!applicationId) {
				return Promise.reject(new Error("applicationId is undefined"));
			}
			return getApi(applicationId);
		},
		enabled: visible && !!applicationId,
	});

	// 更新应用密钥
	const { mutateAsync: updateSecret, isPending: isUpdating } = useMutation({
		mutationFn: (clientSecret: string) => {
			if (!applicationData) {
				return Promise.reject(new Error("applicationData is undefined"));
			}
			return updateApi(applicationData.id, {
				...applicationData,
				clientSecret,
			});
		},
		onSuccess: (data) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(data);
			onClose();
			form.resetFields();
		},
	});

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			await updateSecret(values.clientSecret);
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={`${$t("AbpOpenIddict.ManageSecret")}${clientId ? ` - ${clientId}` : ""}`}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isUpdating || isLoadingApp}
			width="50%"
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item
					label={$t("AbpOpenIddict.DisplayName:ClientSecret")}
					name="clientSecret"
					rules={[{ required: true }]}
				>
					<Input.Password className="w-full" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default ApplicationSecretModal;
