import type React from "react";
import { useEffect } from "react";
import { Button, Checkbox, Form, Input, Modal, Select, Space } from "antd";
import { toast } from "sonner";
import { useTranslation } from "react-i18next";
import { ValueType, type IdentityClaimTypeDto } from "#/management/identity";
import { createApi, updateApi } from "@/api/management/identity/claim-types";
import { useMutation } from "@tanstack/react-query";

interface Props {
	visible: boolean;
	onClose: () => void;
	onSuccess: () => void;
	claimType?: IdentityClaimTypeDto;
}

const ClaimTypeModal: React.FC<Props> = ({ visible, onClose, onSuccess, claimType }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm<IdentityClaimTypeDto>();

	const { mutateAsync: createClaimType, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: () => {
			toast.success($t("AbpUi.CreatedSuccessfully"));
			onSuccess();
			onClose();
		},
		onError: () => {
			toast.error($t("AbpUi.Error"));
		},
	});

	const { mutateAsync: updateClaimType, isPending: isUpdating } = useMutation({
		mutationFn: ({ id, data }: { id: string; data: IdentityClaimTypeDto }) => updateApi(id, data),
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onSuccess();
			onClose();
		},
		onError: () => {
			toast.error($t("AbpUi.Error"));
		},
	});

	const handleSave = async () => {
		try {
			const values = await form.validateFields();
			if (claimType?.id) {
				await updateClaimType({ id: claimType.id, data: values });
			} else {
				await createClaimType(values);
			}
		} catch (error) {
			// Form validation error, no need to handle
		}
	};

	useEffect(() => {
		if (visible && claimType) {
			form.setFieldsValue(claimType);
		} else {
			form.resetFields();
		}
	}, [visible, claimType, form]);

	return (
		<Modal
			title={
				claimType?.id
					? `${$t("AbpIdentity.DisplayName:ClaimType")} - ${claimType.name}`
					: $t("AbpIdentity.IdentityClaim:New")
			}
			footer={null}
			open={visible}
			onCancel={onClose}
			destroyOnClose
			centered
			confirmLoading={isCreating || isUpdating}
		>
			<Form form={form} layout="vertical" initialValues={{ required: false }}>
				<Form.Item label={$t("AbpIdentity.IdentityClaim:Name")} name="name" rules={[{ required: true }]}>
					<Input />
				</Form.Item>
				<Form.Item label={$t("AbpIdentity.IdentityClaim:Required")} name="required" valuePropName="checked">
					<Checkbox>{$t("AbpIdentity.IdentityClaim:Required")}</Checkbox>
				</Form.Item>
				<Form.Item label={$t("AbpIdentity.IdentityClaim:Regex")} name="regex">
					<Input />
				</Form.Item>
				<Form.Item label={$t("AbpIdentity.IdentityClaim:RegexDescription")} name="regexDescription">
					<Input />
				</Form.Item>
				<Form.Item label={$t("AbpIdentity.IdentityClaim:ValueType")} name="valueType">
					<Select
						options={[
							{ label: "String", value: ValueType.String },
							{ label: "Int", value: ValueType.Int },
							{ label: "Boolean", value: ValueType.Boolean },
							{ label: "DateTime", value: ValueType.DateTime },
						]}
					/>
				</Form.Item>
				<Form.Item label={$t("AbpIdentity.IdentityClaim:Description")} name="description">
					<Input.TextArea />
				</Form.Item>
				<Form.Item>
					<Space style={{ display: "flex", justifyContent: "flex-end" }}>
						<Button onClick={onClose}>{$t("AbpUi.Cancel")}</Button>
						<Button type="primary" onClick={handleSave}>
							{$t("AbpUi.Save")}
						</Button>
					</Space>
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default ClaimTypeModal;
