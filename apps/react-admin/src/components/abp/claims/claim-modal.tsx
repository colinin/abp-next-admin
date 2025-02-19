import type React from "react";
import { useEffect } from "react";
import { Modal, Form, Input, Select } from "antd";
import { useTranslation } from "react-i18next";
import type { IdentityClaimCreateDto, IdentityClaimDto, IdentityClaimUpdateDto } from "#/management/identity/claims";
import { useQuery, useMutation } from "@tanstack/react-query";
import { getAssignableClaimsApi } from "@/api/management/identity/claim-types";
import { toast } from "sonner";

interface ClaimEditModalProps {
	visible: boolean;
	claim?: IdentityClaimDto;
	onClose: () => void;
	onChange: (data: IdentityClaimDto) => void;
	createApi: (input: IdentityClaimCreateDto) => Promise<void>;
	updateApi: (input: IdentityClaimUpdateDto) => Promise<void>;
}

const ClaimModal: React.FC<ClaimEditModalProps> = ({ visible, claim, onClose, onChange, createApi, updateApi }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	// Query for assignable claims
	const { data: assignableClaimsData } = useQuery({
		queryKey: ["assignableClaims"],
		queryFn: getAssignableClaimsApi,
		enabled: visible && !claim?.id,
	});

	// Mutations for create/update
	const { mutateAsync: mutateCreate, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (_, variables) => {
			onChange(variables as IdentityClaimDto);
			onClose();
			toast.success($t("AbpUi.CreatedSuccessfully"));
		},
	});

	const { mutateAsync: mutateUpdate, isPending: isUpdating } = useMutation({
		mutationFn: updateApi,
		onSuccess: (_, variables) => {
			onChange({
				claimType: claim?.claimType,
				claimValue: variables.newClaimValue,
				id: claim?.id,
			} as IdentityClaimDto);
			onClose();
			toast.success($t("AbpUi.SavedSuccessfully"));
		},
	});

	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (claim) {
				form.setFieldsValue({
					...claim,
					newClaimValue: claim.claimValue,
				});
			}
		}
	}, [visible, claim, form.setFieldsValue, form.resetFields]);

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			if (claim?.id) {
				await mutateUpdate({
					claimType: claim.claimType,
					claimValue: claim.claimValue,
					newClaimValue: values.claimValue,
				});
			} else {
				await mutateCreate({
					claimType: values.claimType,
					claimValue: values.claimValue,
				});
			}
		} catch (error) {
			// Form validation error, no need to handle
		}
	};

	return (
		<Modal
			open={visible}
			title={$t("AbpIdentity.ManageClaim")}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isCreating || isUpdating}
		>
			<Form form={form} layout="vertical">
				<Form.Item name="claimType" label={$t("AbpIdentity.DisplayName:ClaimType")} rules={[{ required: true }]}>
					<Select
						disabled={!!claim?.id}
						options={assignableClaimsData?.items.map((item) => ({
							label: item.name,
							value: item.name,
						}))}
					/>
				</Form.Item>
				<Form.Item name="claimValue" label={$t("AbpIdentity.DisplayName:ClaimValue")} rules={[{ required: true }]}>
					<Input.TextArea />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default ClaimModal;
