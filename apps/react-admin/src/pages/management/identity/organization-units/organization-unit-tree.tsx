import { useEffect, useState } from "react";
import { Card, Button, Tree, Dropdown, Modal } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, RedoOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { DataNode } from "antd/lib/tree";
import { useMutation, useQuery } from "@tanstack/react-query";
import { deleteApi, getChildrenApi, getRootListApi, moveTo } from "@/api/management/identity/organization-units";
import { Iconify } from "@/components/icon";
import { toast } from "sonner";
import OrganizationUnitModal from "./organization-unit-modal";
import PermissionModal from "@/components/abp/permissions/permission-modal";

interface Props {
	onSelected?: (id?: string) => void;
}

const OrganizationUnitTree: React.FC<Props> = ({ onSelected }) => {
	const { t: $t } = useTranslation();
	// const queryClient = useQueryClient(); //
	const [modal, contextHolder] = Modal.useModal();

	const [organizationUnits, setOrganizationUnits] = useState<DataNode[]>([]);
	const [loadedKeys, setLoadedKeys] = useState<string[]>([]);
	const [selectedKey, setSelectedKey] = useState<string>();

	// Modal states
	const [editModalVisible, setEditModalVisible] = useState(false);
	const [permissionModalVisible, setPermissionModalVisible] = useState(false);
	const [selectedUnit, setSelectedUnit] = useState<{ id?: string; parentId?: string }>();

	// 获取根节点列表
	const { refetch: refreshTree } = useQuery({
		queryKey: ["organizationUnits", "root"],
		queryFn: async () => {
			const { items } = await getRootListApi();
			setOrganizationUnits(
				items.map((item) => ({
					isLeaf: false,
					key: item.id,
					title: item.displayName,
					children: [],
				})),
			);
			setLoadedKeys([]);
			return items;
		},
	});

	// 移动节点
	const { mutateAsync: moveNode } = useMutation({
		mutationFn: ({ id, parentId }: { id: string; parentId?: string }) => moveTo(id, parentId),
		onSuccess: () => {
			refreshTree();
		},
	});

	// 删除节点
	const { mutateAsync: deleteNode } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			refreshTree();
		},
	});

	const updateTreeData = (list: DataNode[], key: React.Key, children: DataNode[]): DataNode[] => {
		return list.map((node) => {
			if (node.key === key) {
				return {
					...node,
					children, // 更新子节点
				};
			}
			if (node.children) {
				return {
					...node,
					children: updateTreeData(node.children, key, children), // 递归更新子节点
				};
			}
			return node;
		});
	};
	const findNodeById = (nodes: DataNode[], id?: string): DataNode | undefined => {
		if (!id) return undefined;

		for (const node of nodes) {
			if (node.key === id) return node;
			if (node.children) {
				const found = findNodeById(node.children, id);
				if (found) return found;
			}
		}
		return undefined;
	};
	useEffect(() => {
		onSelected?.(selectedKey);
	}, [selectedKey]);

	const handleMenuClick = async (key: string, id: string) => {
		switch (key) {
			case "create":
				setSelectedUnit({ parentId: id });
				setEditModalVisible(true);
				break;
			case "update":
				setSelectedUnit({ id });
				setEditModalVisible(true);
				break;
			case "permissions":
				// const unit = await getApi(id);
				setSelectedUnit({ id });
				setPermissionModalVisible(true);
				break;
			case "delete":
				modal.confirm({
					title: $t("AbpUi.AreYouSure"),
					content: $t("AbpUi.ItemWillBeDeletedMessage"),
					onOk: () => deleteNode(id),
				});
				break;
			case "refresh":
				refreshTree();
				break;
		}
	};

	const menuItems = [
		{
			key: "update",
			icon: <EditOutlined />,
			label: $t("AbpUi.Edit"),
		},
		{
			key: "create",
			icon: <PlusOutlined />,
			label: $t("AbpIdentity.OrganizationUnit:AddChildren"),
		},
		{
			key: "delete",
			icon: <DeleteOutlined />,
			label: $t("AbpUi.Delete"),
		},
		{
			key: "permissions",
			icon: <Iconify icon="icon-park-outline:permissions" />,
			label: $t("AbpPermissionManagement.Permissions"),
		},
		{
			key: "refresh",
			icon: <RedoOutlined />,
			label: $t("AbpIdentity.OrganizationUnit:RefreshRoot"),
		},
	];

	return (
		<>
			{contextHolder}
			<Card
				title={$t("AbpIdentity.OrganizationUnit:Tree")}
				extra={
					<Button
						type="primary"
						icon={<PlusOutlined />}
						onClick={() => {
							setSelectedUnit({});
							setEditModalVisible(true);
						}}
					>
						{$t("AbpIdentity.OrganizationUnit:AddRoot")}
					</Button>
				}
			>
				<Tree
					loadData={async (node) => {
						// 加载子节点
						const nodeKey = String(node.key);
						const { items } = await getChildrenApi({ id: nodeKey });
						node.isLeaf = items.length === 0;
						node.children = items.map((item): DataNode => {
							return {
								isLeaf: false,
								key: item.id,
								title: item.displayName,
								children: [],
							};
						});
						setOrganizationUnits((prevUnits) => updateTreeData(prevUnits, nodeKey, node.children || []));

						setLoadedKeys((prev) => [...prev, String(node.key)]);
					}}
					loadedKeys={loadedKeys}
					treeData={organizationUnits}
					blockNode
					draggable
					onDrop={(info) => {
						if (!info.dragNode.key) return;
						const dragKey = String(info.dragNode.key);
						const dropKey =
							info.dropPosition === -1
								? undefined // 作为根节点
								: String(info.node.key); // 作为子节点
						moveNode({ id: dragKey, parentId: dropKey });
					}}
					onRightClick={(_) => {}}
					onSelect={(selectedKeys) => {
						if (selectedKeys.length === 0) {
							setSelectedKey(undefined);
							return;
						}
						setSelectedKey(String(selectedKeys[0]));
					}}
					titleRender={(nodeData) => (
						<Dropdown
							trigger={["contextMenu"]}
							menu={{
								items: menuItems,
								onClick: ({ key }) => handleMenuClick(key, String(nodeData.key)),
							}}
						>
							<div className="flex items-center justify-between py-1 px-2 rounded cursor-pointer group">
								<span>{String(nodeData.title)}</span>
								<span className="invisible group-hover:visible text-gray-400">
									<Iconify icon="ph:mouse-right-click-light" />
								</span>
							</div>
						</Dropdown>
					)}
				/>
			</Card>
			<OrganizationUnitModal
				visible={editModalVisible}
				id={selectedUnit?.id}
				parentId={selectedUnit?.parentId}
				onClose={() => {
					setEditModalVisible(false);
					setSelectedUnit(undefined);
				}}
				onChange={() => {
					refreshTree();
				}}
			/>

			<PermissionModal
				visible={permissionModalVisible}
				providerKey={selectedUnit?.id}
				providerName="O"
				displayName={findNodeById(organizationUnits, selectedUnit?.id)?.title?.toString()}
				onClose={() => {
					setPermissionModalVisible(false);
					setSelectedUnit(undefined);
				}}
				onChange={() => {
					refreshTree();
				}}
			/>
		</>
	);
};

export default OrganizationUnitTree;
