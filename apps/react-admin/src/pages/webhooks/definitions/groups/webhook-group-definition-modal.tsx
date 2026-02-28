import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { createApi, getApi, updateApi } from "@/api/webhooks/webhook-group-definitions";
import type { WebhookGroupDefinitionDto } from "#/webhooks/groups";
import type { PropertyInfo } from "@/components/abp/properties/types";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: WebhookGroupDefinitionDto) => void;
	groupName?: string;
}

const defaultModel: WebhookGroupDefinitionDto = {} as WebhookGroupDefinitionDto;

const WebhookGroupDefinitionModal: React.FC<Props> = ({ visible, onClose, onChange, groupName }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const [activeTab, setActiveTab] = useState("basic");
	const [formModel, setFormModel] = useState<WebhookGroupDefinitionDto>({ ...defaultModel });
	const [isEditModel, setIsEditModel] = useState(false);
	const [loading, setLoading] = useState(false);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			form.resetFields();
			if (groupName) {
				fetchGroup(groupName);
				setIsEditModel(true);
			} else {
				setFormModel({ ...defaultModel });
				setIsEditModel(false);
			}
		}
	}, [visible, groupName]);

	const fetchGroup = async (name: string) => {
		try {
			setLoading(true);
			const dto = await getApi(name);
			setFormModel(dto);
			form.setFieldsValue(dto);
		} finally {
			setLoading(false);
		}
	};

	const { mutateAsync: createGroup, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		},
	});

	const { mutateAsync: updateGroup, isPending: isUpdating } = useMutation({
		mutationFn: (data: WebhookGroupDefinitionDto) => updateApi(data.name, data),
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		},
	});

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			const submitData = {
				...values,
				extraProperties: formModel.extraProperties,
			};

			if (isEditModel) {
				await updateGroup(submitData);
			} else {
				await createGroup(submitData);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handlePropChange = (prop: PropertyInfo) => {
		setFormModel((prev) => ({
			...prev,
			extraProperties: {
				...prev.extraProperties,
				[prop.key]: prop.value,
			},
		}));
	};

	const handlePropDelete = (prop: PropertyInfo) => {
		setFormModel((prev) => {
			const newProps = { ...prev.extraProperties };
			delete newProps[prop.key];
			return {
				...prev,
				extraProperties: newProps,
			};
		});
	};

	const modalTitle = isEditModel
		? `${$t("WebhooksManagement.GroupDefinitions")} - ${formModel.name}`
		: $t("WebhooksManagement.GroupDefinitions:AddNew");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={isCreating || isUpdating || loading}
			okButtonProps={{ disabled: formModel.isStatic }}
			width={800}
			destroyOnClose
		>
			<Form form={form} layout="vertical" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={setActiveTab}>
					<Tabs.TabPane key="basic" tab={$t("WebhooksManagement.BasicInfo")}>
						<Form.Item name="name" label={$t("WebhooksManagement.DisplayName:Name")} rules={[{ required: true }]}>
							<Input disabled={formModel.isStatic || isEditModel} autoComplete="off" />
						</Form.Item>
						<Form.Item
							name="displayName"
							label={$t("WebhooksManagement.DisplayName:DisplayName")}
							rules={[{ required: true }]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>
					</Tabs.TabPane>

					<Tabs.TabPane key="props" tab={$t("WebhooksManagement.Properties")}>
						<PropertyTable
							data={formModel.extraProperties}
							disabled={formModel.isStatic}
							onChange={handlePropChange}
							onDelete={handlePropDelete}
						/>
					</Tabs.TabPane>
				</Tabs>
			</Form>
		</Modal>
	);
};

export default WebhookGroupDefinitionModal;
