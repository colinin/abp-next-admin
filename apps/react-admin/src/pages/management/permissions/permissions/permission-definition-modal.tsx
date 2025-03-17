import { useEffect, useState } from "react";
import { Modal, Form, Input, Checkbox, Select, Tabs, TreeSelect } from "antd";
import { createApi, getListApi as getPermissionsApi, updateApi } from "@/api/management/permissions/definitions";
import { getListApi as getGroupsApi } from "@/api/management/permissions/groups";
import { toast } from "sonner";
import { useTranslation } from "react-i18next";
import { useTypesMap } from "./types";
import type { PermissionDefinitionDto } from "#/management/permissions/definitions";
// import type { PermissionGroupDefinitionDto } from "#/permissions/groups";
import { listToTree } from "@/utils/tree";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import type { PropertyInfo } from "@/components/abp/properties/types";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

const { TabPane } = Tabs;
const { Option } = Select;

interface PermissionDefinitionModalProps {
	visible: boolean;
	onClose: () => void;
	onChange: () => void;
	permission?: PermissionDefinitionDto;
	groupName?: string;
}

interface PermissionTreeVo {
	children: PermissionTreeVo[];
	displayName: string;
	groupName: string;
	name: string;
	disabled?: boolean;
}

type TabKeys = "basic" | "props";

const defaultModel: PermissionDefinitionDto = {} as PermissionDefinitionDto;

const PermissionDefinitionModal: React.FC<PermissionDefinitionModalProps> = ({
	visible,
	onClose,
	onChange,
	permission,
	groupName,
}) => {
	const { t: $t } = useTranslation();
	const queryClient = useQueryClient();
	const [form] = Form.useForm();
	const { multiTenancySideOptions, providerOptions } = useTypesMap($t);
	const [activeTab, setActiveTab] = useState<TabKeys>("basic");
	const [formModel, setFormModel] = useState<PermissionDefinitionDto>({ ...defaultModel });
	const { Lr } = useLocalizer();
	const { deserialize } = localizationSerializer();

	// 查询权限组列表
	const { data: availableGroups = [] } = useQuery({
		queryKey: ["permissionGroups", groupName],
		queryFn: async () => {
			const { items } = await getGroupsApi({ filter: groupName });
			return items
				.filter((group) => !group.isStatic)
				.map((group) => {
					const localizableGroup = deserialize(group.displayName);
					return {
						...group,
						displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
					};
				});
		},

		enabled: visible,
	});

	// 查询权限列表
	const { data: availablePermissions = [] } = useQuery({
		queryKey: ["permissions", formModel.groupName],
		queryFn: async () => {
			const { items } = await getPermissionsApi({ groupName: formModel.groupName });
			const permissions = items.map((permission) => {
				const localizablePermission = deserialize(permission.displayName);
				return {
					...permission,
					disabled: permission.name === formModel.name,
					displayName: Lr(localizablePermission.resourceName, localizablePermission.name),
				};
			});
			return listToTree<PermissionTreeVo>(permissions, { id: "name", pid: "parentName" });
		},
		enabled: visible && !!formModel.groupName,
	});

	// 创建权限
	const { mutateAsync: createPermission, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["permissions"] });
			queryClient.invalidateQueries({ queryKey: ["permissionGroups"] });
			onChange();
			onClose();
		},
	});

	// 更新权限
	const { mutateAsync: updatePermission, isPending: isUpdating } = useMutation({
		mutationFn: (data: PermissionDefinitionDto) => updateApi(data.name, data),
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["permissions"] });
			queryClient.invalidateQueries({ queryKey: ["permissionGroups"] });
			onChange();
			onClose();
		},
	});

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			form.resetFields();

			if (permission) {
				const initialModel = {
					...permission,
					extraProperties: permission.extraProperties || {},
				};
				setFormModel(initialModel);
				form.setFieldsValue(initialModel);
			} else {
				setFormModel({ ...defaultModel });
				if (groupName) {
					form.setFieldsValue({ groupName });
				}
			}
		}
	}, [visible, permission, groupName]);

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			const submitData = {
				...values,
				extraProperties: formModel.extraProperties,
			};

			if (permission) {
				await updatePermission(submitData);
			} else {
				await createPermission(submitData);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handleGroupChange = (groupName?: string) => {
		setFormModel((prev) => ({ ...prev, groupName: groupName || "" }));
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
			open={visible}
			title={
				permission
					? `${$t("AbpPermissionManagement.PermissionDefinitions")} - ${formModel.name}`
					: $t("AbpPermissionManagement.PermissionDefinitions:AddNew")
			}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isCreating || isUpdating}
			okButtonProps={{ disabled: formModel.isStatic }}
			width="50%"
		>
			<Form form={form} labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={(key) => setActiveTab(key as TabKeys)}>
					<TabPane tab={$t("AbpPermissionManagement.BasicInfo")} key="basic">
						<Form.Item
							label={$t("AbpPermissionManagement.DisplayName:IsEnabled")}
							name="isEnabled"
							valuePropName="checked"
						>
							<Checkbox disabled={formModel.isStatic}>{$t("AbpPermissionManagement.DisplayName:IsEnabled")}</Checkbox>
						</Form.Item>
						<Form.Item
							label={$t("AbpPermissionManagement.DisplayName:GroupName")}
							name="groupName"
							rules={[{ required: true }]}
						>
							<Select allowClear disabled={formModel.isStatic} onChange={handleGroupChange}>
								{availableGroups.map((group) => (
									<Option key={group.name} value={group.name}>
										{group.displayName}
									</Option>
								))}
							</Select>
						</Form.Item>
						{availablePermissions.length > 0 && (
							<Form.Item label={$t("AbpPermissionManagement.DisplayName:ParentName")} name="parentName">
								<TreeSelect
									allowClear
									disabled={formModel.isStatic}
									treeData={availablePermissions}
									fieldNames={{ label: "displayName", value: "name", children: "children" }}
								/>
							</Form.Item>
						)}
						<Form.Item label={$t("AbpPermissionManagement.DisplayName:Name")} name="name" rules={[{ required: true }]}>
							<Input disabled={formModel.isStatic} autoComplete="off" />
						</Form.Item>
						<Form.Item
							label={$t("AbpPermissionManagement.DisplayName:DisplayName")}
							name="displayName"
							rules={[{ required: true }]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>
						<Form.Item label={$t("AbpPermissionManagement.DisplayName:MultiTenancySide")} name="multiTenancySide">
							<Select disabled={formModel.isStatic}>
								{multiTenancySideOptions.map((option) => (
									<Option key={option.value} value={option.value}>
										{option.label}
									</Option>
								))}
							</Select>
						</Form.Item>
						<Form.Item label={$t("AbpPermissionManagement.DisplayName:Providers")} name="providers">
							<Select mode="multiple" allowClear disabled={formModel.isStatic}>
								{providerOptions.map((option) => (
									<Option key={option.value} value={option.value}>
										{option.label}
									</Option>
								))}
							</Select>
						</Form.Item>
					</TabPane>
					<TabPane tab={$t("AbpPermissionManagement.Properties")} key="props">
						<PropertyTable
							data={formModel.extraProperties}
							disabled={formModel.isStatic}
							onChange={handlePropChange}
							onDelete={handlePropDelete}
						/>
					</TabPane>
				</Tabs>
			</Form>
		</Modal>
	);
};

export default PermissionDefinitionModal;
