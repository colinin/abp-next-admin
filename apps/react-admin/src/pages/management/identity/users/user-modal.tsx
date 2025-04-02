import { useEffect, useState } from "react";
import { Modal, Form, Input, Checkbox, Tabs, Transfer, Tree } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { IdentityUserDto } from "#/management/identity/user";
import type { DataNode } from "antd/es/tree";
import type { TransferItem } from "antd/es/transfer";
import { toast } from "sonner";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import {
	createApi,
	getApi,
	updateApi,
	getAssignableRolesApi,
	getRolesApi,
	getOrganizationUnitsApi,
} from "@/api/management/identity/users";
import { getRootListApi, getChildrenApi } from "@/api/management/identity/organization-units";
import { useAbpSettings } from "@/hooks/abp/use-abp-settings";

interface UserModalProps {
	visible: boolean;
	userId?: string;
	onClose: () => void;
	onChange: () => void;
}

const defaultModel: Partial<IdentityUserDto> = {
	isActive: true,
};

const UserModal: React.FC<UserModalProps> = ({ visible, userId, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const queryClient = useQueryClient();
	const [form] = Form.useForm();
	const { isTrue } = useAbpSettings();

	const [activeTab, setActiveTab] = useState("info");
	const [assignedRoles, setAssignedRoles] = useState<TransferItem[]>([]);
	const [organizationUnits, setOrganizationUnits] = useState<DataNode[]>([]);
	const [loadedOuKeys, setLoadedOuKeys] = useState<string[]>([]);
	const [checkedOuKeys, setCheckedOuKeys] = useState<string[]>([]);
	const [targetKeys, setTargetKeys] = useState<string[]>([]);

	// Check policies
	const canManageRoles = hasAccessByCodes(["AbpIdentity.Users.Update.ManageRoles"]);
	const canManageOu = hasAccessByCodes(["AbpIdentity.Users.ManageOrganizationUnits"]);

	// API mutations
	const { mutateAsync: createUser, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: () => {
			toast.success($t("AbpUi.CreatedSuccessfully"));
			onChange();
			onClose();
			queryClient.invalidateQueries({ queryKey: ["users"] });
		},
	});

	const { mutateAsync: updateUser, isPending: isUpdating } = useMutation({
		mutationFn: (data: IdentityUserDto) => updateApi(data.id, data),
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange();
			onClose();
			queryClient.invalidateQueries({ queryKey: ["users"] });
			queryClient.invalidateQueries({ queryKey: ["userRoles"] });
			queryClient.invalidateQueries({ queryKey: ["assignableRoles"] });
			queryClient.invalidateQueries({ queryKey: ["permissions"] }); //角色更新会带来permission的变化
		},
	});

	// Fetch user data
	const { data: userData } = useQuery({
		queryKey: ["users", userId],
		queryFn: () => {
			if (!userId) {
				return Promise.reject(new Error("userId is undefined"));
			}
			return getApi(userId);
		},
		enabled: visible && !!userId,
	});

	// Fetch roles
	const { data: userRoles } = useQuery({
		queryKey: ["userRoles", userId],
		queryFn: () => {
			if (!userId) {
				return Promise.reject(new Error("userId is undefined"));
			}
			return getRolesApi(userId);
		},
		enabled: visible && !!userId && canManageRoles,
	});

	// Fetch assignable roles
	const { data: assignableRoles } = useQuery({
		queryKey: ["assignableRoles"],
		queryFn: getAssignableRolesApi,
		enabled: visible && canManageRoles,
	});

	// Fetch organization units
	const { data: userOus } = useQuery({
		queryKey: ["userOus", userId],
		queryFn: () => {
			if (!userId) {
				return Promise.reject(new Error("userId is undefined"));
			}
			return getOrganizationUnitsApi(userId);
		},
		enabled: visible && !!userId && canManageOu,
	});

	const { data: ousRootList } = useQuery({
		queryKey: ["ousRootList"],
		queryFn: () => {
			if (!userId) {
				return Promise.reject(new Error("userId is undefined"));
			}
			return getRootListApi();
		},
		enabled: visible && !!userId && canManageOu,
	});

	useEffect(() => {
		if (visible) {
			// Reset states
			setActiveTab("info");
			setAssignedRoles([]);
			setOrganizationUnits([]);
			setLoadedOuKeys([]);
			form.resetFields();

			if (userData) {
				// userData.roleNames = userRoles?.items.map((item) => item.name) || [];
				form.setFieldsValue(userData);
			} else {
				form.setFieldsValue(defaultModel);
			}

			// Initialize roles
			if (assignableRoles) {
				setAssignedRoles(
					assignableRoles.items.map((item) => ({
						key: item.name,
						title: item.name,
						...item,
					})),
				);
			}
			if (userRoles) {
				// 初始化 targetKeys
				const userRoleNames = userRoles.items.map((item) => item.name);
				setTargetKeys(userRoleNames);
				form.setFieldValue("roleNames", userRoleNames);
			}

			// Initialize organization units
			if (userOus && ousRootList) {
				//TODO 根据用户的组织关系信息,计算checkedKeys，让用户的父组织们都halfChecked
				setOrganizationUnits(
					ousRootList.items.map((item) => ({
						isLeaf: false,
						key: item.id,
						title: item.displayName,
						children: [],
					})),
				);
				setCheckedOuKeys(userOus.items.map((item) => item.id));
			}
		}
	}, [visible, userData, assignableRoles, userOus]);

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			if (userId) {
				await updateUser({ ...values, id: userId }); //admin用户更新不了？
			} else {
				await createUser(values);
			}
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	// 更新组织机构树结构
	const updateTreeData = (list: DataNode[], key: React.Key, children: DataNode[]): DataNode[] => {
		return list.map((node) => {
			if (node.key === key) {
				return { ...node, children };
			}
			if (node.children) {
				return { ...node, children: updateTreeData(node.children, key, children) };
			}
			return node;
		});
	};

	// Load organization unit children with tree update
	const onLoadOuChildren = async (treeNode: any) => {
		const nodeKey = String(treeNode.key);
		const { items } = await getChildrenApi({ id: nodeKey });
		const children = items.map(
			(item): DataNode => ({
				isLeaf: false,
				key: item.id,
				title: item.displayName,
				children: [],
			}),
		);
		setOrganizationUnits((prev) => updateTreeData(prev, nodeKey, children));
		setLoadedOuKeys((prev) => [...prev, nodeKey]);
	};

	return (
		<Modal
			open={visible}
			title={userData ? `${$t("AbpIdentity.Users")} - ${userData.userName}` : $t("AbpIdentity.NewUser")}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isCreating || isUpdating}
			width="50%"
			destroyOnClose
		>
			<Form form={form} layout="horizontal" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={setActiveTab}>
					<Tabs.TabPane key="info" tab={$t("AbpIdentity.UserInformations")}>
						<Form.Item
							label={$t("AbpIdentity.DisplayName:IsActive")}
							name="isActive"
							valuePropName="checked"
							initialValue={true}
						>
							<Checkbox>{$t("AbpIdentity.DisplayName:IsActive")}</Checkbox>
						</Form.Item>
						<Form.Item label={$t("AbpIdentity.UserName")} name="userName" rules={[{ required: true }]}>
							<Input disabled={!isTrue("Abp.Identity.User.IsUserNameUpdateEnabled")} />
						</Form.Item>
						{!userId && (
							<Form.Item label={$t("AbpIdentity.Password")} name="password" rules={[{ required: true }]}>
								<Input.Password />
							</Form.Item>
						)}
						<Form.Item
							label={$t("AbpIdentity.DisplayName:Email")}
							name="email"
							rules={[{ required: true }, { type: "email" }]}
						>
							<Input disabled={!isTrue("Abp.Identity.User.IsEmailUpdateEnabled")} />
						</Form.Item>
						<Form.Item label={$t("AbpIdentity.DisplayName:PhoneNumber")} name="phoneNumber">
							<Input />
						</Form.Item>
						<Form.Item label={$t("AbpIdentity.DisplayName:Surname")} name="surname">
							<Input />
						</Form.Item>
						<Form.Item label={$t("AbpIdentity.DisplayName:Name")} name="name">
							<Input />
						</Form.Item>
						<Form.Item
							label={$t("AbpIdentity.DisplayName:LockoutEnabled")}
							name="lockoutEnabled"
							valuePropName="checked"
						>
							<Checkbox>{$t("AbpIdentity.Description:LockoutEnabled")}</Checkbox>
						</Form.Item>
					</Tabs.TabPane>

					{canManageRoles && (
						<Tabs.TabPane key="role" tab={$t("AbpIdentity.Roles")}>
							<Form.Item name="roleNames" noStyle>
								<Transfer
									dataSource={assignedRoles}
									targetKeys={targetKeys}
									onChange={(targetKeys) => {
										console.log("targetKeys", targetKeys);
										const stringTargetKeys = targetKeys.map(String); // 转换成 string[]
										setTargetKeys(stringTargetKeys); // 触发react更新
										form.setFieldValue("roleNames", targetKeys); //提交时从form获取roleNames
									}}
									render={(item) => item.title || ""}
									listStyle={{
										width: "47%",
										height: "338px",
									}}
									titles={[$t("AbpIdentity.Assigned"), $t("AbpIdentity.Available")]}
								/>
							</Form.Item>
						</Tabs.TabPane>
					)}

					{userId && canManageOu && (
						<Tabs.TabPane key="ou" tab={$t("AbpIdentity.OrganizationUnits")}>
							<Tree
								checkable
								checkedKeys={checkedOuKeys}
								loadData={onLoadOuChildren}
								loadedKeys={loadedOuKeys}
								treeData={organizationUnits}
								disabled
								checkStrictly
								blockNode
							/>
						</Tabs.TabPane>
					)}
				</Tabs>
			</Form>
		</Modal>
	);
};

export default UserModal;
