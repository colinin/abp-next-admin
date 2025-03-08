import { useEffect } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import type {
	OrganizationUnitDto,
	OrganizationUnitCreateDto,
	OrganizationUnitUpdateDto,
} from "#/management/identity/organization-units";
import { useMutation, useQuery } from "@tanstack/react-query";
import { createApi, getApi, updateApi } from "@/api/management/identity/organization-units";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: OrganizationUnitDto) => void;
	id?: string;
	parentId?: string;
}

const defaultModel: Partial<OrganizationUnitDto> = {
	displayName: "",
};

const OrganizationUnitModal: React.FC<Props> = ({ visible, onClose, onChange, id, parentId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	// 查询组织单位详情
	const { data: unitData, isLoading } = useQuery({
		queryKey: ["organizationUnit", id],
		queryFn: () => {
			if (!id) {
				return Promise.reject(new Error("id is undefined"));
			}
			return getApi(id);
		},
		enabled: visible && !!id,
	});

	// 创建组织单位
	const { mutateAsync: createUnit, isPending: isCreating } = useMutation({
		mutationFn: (input: OrganizationUnitCreateDto) => createApi(input),
		onSuccess: (data) => {
			onChange(data);
			onClose();
		},
	});

	// 更新组织单位
	const { mutateAsync: updateUnit, isPending: isUpdating } = useMutation({
		mutationFn: (data: OrganizationUnitUpdateDto) => {
			if (!id) {
				return Promise.reject(new Error("id is undefined"));
			}
			return updateApi(id, data);
		},
		onSuccess: (data) => {
			onChange(data);
			onClose();
		},
	});

	useEffect(() => {
		if (visible) {
			if (id) {
				form.setFieldsValue(unitData);
			} else {
				form.setFieldsValue({
					...defaultModel,
					parentId,
				});
			}
		} else {
			form.resetFields();
		}
	}, [visible, unitData, id, parentId]);

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			if (id) {
				await updateUnit(values);
			} else {
				await createUnit(values);
			}
		} catch (error) {
			// Form validation error
		}
	};

	return (
		<Modal
			open={visible}
			title={
				id ? `${$t("AbpIdentity.OrganizationUnits")}: ${unitData?.displayName}` : $t("AbpIdentity.OrganizationUnit:New")
			}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isCreating || isUpdating || isLoading}
		>
			<Form form={form} labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Form.Item name="id" hidden>
					<Input />
				</Form.Item>
				<Form.Item name="parentId" hidden>
					<Input />
				</Form.Item>
				<Form.Item
					label={$t("AbpIdentity.OrganizationUnit:DisplayName")}
					name="displayName"
					rules={[{ required: true }]}
				>
					<Input autoComplete="off" />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default OrganizationUnitModal;
