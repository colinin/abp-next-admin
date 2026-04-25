import { useEffect, useState } from "react";
import { Form, Input, Modal, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import type { FeatureGroupDefinitionDto } from "#/management/features/groups";
import type { PropertyInfo } from "@/components/abp/properties/types";
import { createApi, getApi, updateApi } from "@/api/management/features/feature-group-definitions";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";
import { toast } from "sonner";
import { useMutation, useQueryClient } from "@tanstack/react-query";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: FeatureGroupDefinitionDto) => void;
	groupName?: string;
}

type TabKeys = "basic" | "props";

const defaultModel: FeatureGroupDefinitionDto = {} as FeatureGroupDefinitionDto;

const FeatureGroupDefinitionModal: React.FC<Props> = ({ visible, onClose, onChange, groupName }) => {
	const { t: $t } = useTranslation();
	const queryClient = useQueryClient();
	const [form] = Form.useForm();
	const [formModel, setFormModel] = useState<FeatureGroupDefinitionDto>({
		...defaultModel,
	});
	const [isEditModel, setIsEditModel] = useState(false);
	const [activeTab, setActiveTab] = useState<TabKeys>("basic");

	// Fetch Group Details( mutate for GET request to allow manual triggering )
	const { mutateAsync: fetchGroup, isPending: isFetching } = useMutation({
		mutationFn: getApi,
		onMutate: () => {
			setIsEditModel(true);
		},
		onSuccess: (dto) => {
			setFormModel(dto);
			form.setFieldsValue(dto);
		},
	});

	// Create Group
	const { mutateAsync: createGroup, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["featureGroups"] });
			onChange(res);
			onClose();
		},
	});

	// Update Group
	const { mutateAsync: updateGroup, isPending: isUpdating } = useMutation({
		mutationFn: (data: FeatureGroupDefinitionDto) => updateApi(data.name, data),
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["featureGroups"] });
			onChange(res);
			onClose();
		},
	});

	useEffect(() => {
		if (visible) {
			setIsEditModel(false);
			setActiveTab("basic");
			setFormModel({ ...defaultModel });
			form.resetFields();

			if (groupName) {
				fetchGroup(groupName);
			}
		}
	}, [visible, groupName]);

	const handleOk = async () => {
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

	return (
		<Modal
			title={
				isEditModel
					? `${$t("AbpFeatureManagement.GroupDefinitions")} - ${formModel.name}`
					: $t("AbpFeatureManagement.GroupDefinitions:AddNew")
			}
			open={visible}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isCreating || isUpdating || isFetching}
			okButtonProps={{ disabled: formModel.isStatic }}
			width="50%"
		>
			<Form form={form} labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={(key) => setActiveTab(key as TabKeys)}>
					<Tabs.TabPane key="basic" tab={$t("AbpFeatureManagement.BasicInfo")}>
						<Form.Item label={$t("AbpFeatureManagement.DisplayName:Name")} name="name" rules={[{ required: true }]}>
							<Input disabled={formModel.isStatic} autoComplete="off" />
						</Form.Item>
						<Form.Item
							label={$t("AbpFeatureManagement.DisplayName:DisplayName")}
							name="displayName"
							rules={[{ required: true }]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>
					</Tabs.TabPane>
					<Tabs.TabPane key="props" tab={$t("AbpFeatureManagement.Properties")}>
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

export default FeatureGroupDefinitionModal;
