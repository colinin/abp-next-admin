import { useEffect, useMemo, useState } from "react";
import { Checkbox, Divider, Card, Tabs, Tree, Modal } from "antd";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getApi, updateApi } from "@/api/management/permissions/permissions";
import type { PermissionTree } from "#/management/permissions";
import {
	generatePermissionTree,
	getGrantedPermissionKeys,
	getGrantPermissionCount,
	getGrantPermissionsCount,
	getParentList,
	getPermissionCount,
	getPermissionsCount,
	toPermissionList,
	getChildren,
} from "./permissions-utils";
import { toast } from "sonner";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange?: () => void;
	providerName: string;
	providerKey?: string;
	displayName?: string;
	readonly?: boolean;
}

//TODO 测试和修改这个modal
const PermissionModal: React.FC<Props> = ({
	visible,
	onClose,
	onChange,
	providerName,
	providerKey,
	displayName,
	readonly = false,
}) => {
	const { t: $t } = useTranslation();
	const [expandedKeys, setExpandedKeys] = useState<string[]>([]);
	const [checkedKeys, setCheckedKeys] = useState<string[]>([]);
	const [permissionTree, setPermissionTree] = useState<PermissionTree[]>([]);
	const queryClient = useQueryClient();
	const { data: permissionData } = useQuery({
		queryKey: ["permissions", providerName, providerKey],
		queryFn: () =>
			getApi({ providerName, providerKey }).then((res) => {
				return { entityDisplayName: res.entityDisplayName, tree: generatePermissionTree(res.groups) };
			}),
		enabled: visible,
	});

	useEffect(() => {
		if (permissionData) {
			setPermissionTree(permissionData.tree);
			setCheckedKeys(getGrantedPermissionKeys(permissionData.tree));
		}
	}, [permissionData]);

	// 更新权限
	const { mutateAsync: updatePermissions, isPending: isUpdating } = useMutation({
		mutationFn: (permissions: any) => updateApi({ providerKey, providerName }, { permissions }),
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["permissions", providerName, providerKey] });
			onChange?.();
			onClose();
		},
	});

	// 计算权限状态
	const permissionState = useMemo(() => {
		const grantCount = getGrantPermissionsCount(permissionTree);
		const permissionCount = getPermissionsCount(permissionTree);
		return {
			checked: grantCount === permissionCount,
			indeterminate: grantCount > 0 && grantCount < permissionCount,
		};
	}, [permissionTree, checkedKeys]);

	// 全选所有节点权限
	const handleCheckAll = (e: CheckboxChangeEvent) => {
		const { checked } = e.target;
		const newCheckedKeys: string[] = [];

		permissionTree.forEach((current) => {
			const children = getChildren(current.children);
			children.forEach((permission) => {
				permission.isGranted = checked;
				if (checked) {
					newCheckedKeys.push(permission.name);
				}
			});
			current.isGranted = checked;
		});

		setCheckedKeys(newCheckedKeys);
	};

	// 全选当前节点权限
	const handleNodeCheckAll = (e: CheckboxChangeEvent, permission: PermissionTree) => {
		const { checked } = e.target;
		const children = getChildren(permission.children ?? []);
		const childKeys = children.map((node) => node.name);

		children.forEach((permission) => {
			permission.isGranted = checked;
		});

		setCheckedKeys((prev) => (checked ? [...prev, ...childKeys] : prev.filter((key) => !childKeys.includes(key))));

		permission.isGranted = checked;
	};

	// 处理节点选中状态
	const handleNodeCheck = (permission: PermissionTree, _keys: any, info: any) => {
		const nodeKey = String(info.node.key);
		const index = checkedKeys.indexOf(nodeKey);
		setCheckedKeys((prev) => (index === -1 ? [...prev, nodeKey] : prev.filter((key) => key !== nodeKey)));

		const checked = info.checked;
		// ✅ 递归更新 `permissionTree`
		setPermissionTree((prevTree) => {
			const newTree = [...prevTree]; // 创建新数组，确保 React 触发更新

			// 递归函数：更新 `isGranted`
			const updateTree = (tree: PermissionTree[]): PermissionTree[] => {
				return tree.map((node) => {
					if (node.name === nodeKey) {
						return { ...node, isGranted: checked }; // 复制对象，确保 React 重新渲染
					}
					if (node.children) {
						return { ...node, children: updateTree(node.children) }; // 递归处理子节点
					}
					return node;
				});
			};

			return updateTree(newTree);
		});
		const currentPermission = info.node as PermissionTree;

		// 上级权限联动
		checkParentGrant(permission, currentPermission, checked);
		// 下级权限联动
		checkChildrenGrant(currentPermission, checked);
		// 自身授权
		currentPermission.isGranted = checked;
	};

	// 处理下级权限联动
	const checkChildrenGrant = (current: PermissionTree, isGranted: boolean) => {
		const children = getChildren(current.children);
		if (!isGranted) {
			const childKeys: string[] = [];
			children.forEach((node) => {
				node.isGranted = false;
				childKeys.push(node.name);
			});
			setCheckedKeys((prev) => prev.filter((key) => !childKeys.includes(key)));
		}
	};

	// 处理上级权限联动
	const checkParentGrant = (root: PermissionTree, current: PermissionTree, isGranted: boolean) => {
		if (!isGranted || !current.parentName) return;

		const parentNodes = getParentList(root.children, current.parentName);
		if (parentNodes) {
			const parentKeys: string[] = [];
			parentNodes.forEach((node) => {
				node.isGranted = true;
				if (!checkedKeys.includes(node.name)) {
					parentKeys.push(node.name);
				}
			});
			setCheckedKeys((prev) => [...prev, ...parentKeys]);
		}
	};

	const handleOk = async () => {
		const permissions = toPermissionList(permissionTree);
		await updatePermissions(permissions);
	};

	return (
		<Modal
			open={visible}
			title={`${$t("AbpPermissionManagement.Permissions")} - ${displayName || permissionData?.entityDisplayName}`}
			onCancel={onClose}
			onOk={handleOk}
			width="50%"
			centered
			confirmLoading={isUpdating}
			className="permission-modal"
		>
			<div className="flex flex-col">
				<div>
					<Checkbox disabled={readonly} onChange={handleCheckAll} {...permissionState}>
						{$t("AbpPermissionManagement.SelectAllInAllTabs")}
					</Checkbox>
				</div>
				<Divider />
				<Tabs
					tabPosition="left"
					type="card"
					className="h-[34rem]"
					tabBarStyle={{ width: "14rem" }}
					items={permissionTree.map((permission) => ({
						key: permission.name,
						label: `${permission.displayName} (${getGrantPermissionCount(permission)}/${getPermissionCount(permission)})`,
						children: (
							<Card bordered={false} title={permission.displayName}>
								<div className="flex flex-col">
									<Checkbox
										disabled={readonly}
										checked={getGrantPermissionCount(permission) === getPermissionCount(permission)}
										indeterminate={
											getGrantPermissionCount(permission) > 0 &&
											getGrantPermissionCount(permission) < getPermissionCount(permission)
										}
										onChange={(e) => handleNodeCheckAll(e, permission)}
									>
										{$t("AbpPermissionManagement.SelectAllInThisTab")}
									</Checkbox>
									<Divider />
									<div className="max-h-[24rem] overflow-auto pr-2">
										<Tree
											checkStrictly
											checkable
											checkedKeys={checkedKeys}
											disabled={readonly}
											expandedKeys={expandedKeys}
											fieldNames={{
												key: "name",
												title: "displayName",
												children: "children",
											}}
											treeData={permission.children}
											onCheck={(keys, info) => handleNodeCheck(permission, keys, info)}
											onExpand={(_keys, info) => {
												const nodeKey = String(info.node.key);
												const index = expandedKeys.indexOf(nodeKey);
												setExpandedKeys((prev) =>
													index === -1 ? [...prev, nodeKey] : prev.filter((key) => key !== nodeKey),
												);
											}}
											onSelect={(_keys, info) => {
												const nodeKey = String(info.node.key);
												const index = expandedKeys.indexOf(nodeKey);
												setExpandedKeys((prev) =>
													index === -1 ? [...prev, nodeKey] : prev.filter((key) => key !== nodeKey),
												);
											}}
										/>
									</div>
								</div>
							</Card>
						),
					}))}
				/>
			</div>
		</Modal>
	);
};

export default PermissionModal;

/*

.permission-modal {
  :global {
    .ant-tabs {
      height: 34rem;

      .ant-tabs-nav {
        width: 14rem;
      }

      .ant-tabs-content-holder {
        overflow: hidden auto !important;
      }
    }
  }
}

*/
