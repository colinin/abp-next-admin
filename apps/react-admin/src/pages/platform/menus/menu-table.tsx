import type React from "react";
import { useRef, useState } from "react";
import { Button, Space, Modal, Card } from "antd";
import { PlusOutlined, EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation } from "@tanstack/react-query";
import { deleteApi, getAllApi } from "@/api/platform/menus";
import { getPagedListApi as getLayoutsApi } from "@/api/platform/layouts";
import type { MenuDto } from "#/platform/menus";
import { MenuPermissions } from "@/constants/platform/permissions";
import { withAccessChecker } from "@/utils/abp/access-checker";
import { Iconify } from "@/components/icon";

import MenuDrawer from "./menu-drawer";
import { listToTree } from "@/utils/tree";

const MenuTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const [modal, contextHolder] = Modal.useModal();

	const [drawerVisible, setDrawerVisible] = useState(false);
	const [editMenuState, setEditMenuState] = useState<
		{ id?: string; parentId?: string; layoutId?: string } | undefined
	>();
	const [menuTree, setMenuTree] = useState<MenuDto[]>([]);

	const { mutateAsync: deleteMenu } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const handleCreate = (row?: MenuDto) => {
		setEditMenuState({
			layoutId: row?.layoutId,
			parentId: row?.id,
		});
		setDrawerVisible(true);
	};

	const handleUpdate = (row: MenuDto) => {
		setEditMenuState({ id: row.id });
		setDrawerVisible(true);
	};

	const handleDelete = (row: MenuDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: () => deleteMenu(row.id),
		});
	};

	const columns: ProColumns<MenuDto>[] = [
		{
			title: $t("AppPlatform.DisplayName:Layout"),
			dataIndex: "layoutId",
			valueType: "select",
			hideInTable: true,
			request: async ({ keyWords }) => {
				const res = await getLayoutsApi({ filter: keyWords, maxResultCount: 20 });
				return res.items.map((l) => ({ label: l.displayName, value: l.id }));
			},
			fieldProps: { showSearch: true, allowClear: true },
		},
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Name"),
			dataIndex: "name",
			width: 250,
			render: (_, row) => (
				<Space>
					{row.meta?.icon && <Iconify icon={row.meta.icon} />}
					{row.name}
				</Space>
			),
		},
		{
			title: $t("AppPlatform.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
		},
		{
			title: $t("AppPlatform.DisplayName:Description"),
			dataIndex: "description",
			ellipsis: true,
		},
		{
			title: $t("AbpUi.Actions"),
			key: "actions",
			fixed: "right",
			width: 220,
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<PlusOutlined />} onClick={() => handleCreate(record)}>
							{$t("AppPlatform.Menu:AddChildren")}
						</Button>,
						[MenuPermissions.Create],
					)}
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[MenuPermissions.Update],
					)}
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[MenuPermissions.Delete],
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<MenuDto>
					headerTitle={$t("AppPlatform.DisplayName:Menus")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={() => handleCreate()}>
								{$t("AppPlatform.Menu:AddNew")}
							</Button>,
							[MenuPermissions.Create],
						),
					]}
					request={async (params) => {
						const { layoutId, filter } = params;
						const { items } = await getAllApi({ layoutId, filter });
						const tree = listToTree(items, { id: "id", pid: "parentId" });

						setMenuTree(tree); // Save for drawer

						return {
							data: tree,
							success: true,
							total: items.length,
						};
					}}
					expandable={{ defaultExpandAllRows: true }}
					pagination={false}
				/>
			</Card>
			<MenuDrawer
				visible={drawerVisible}
				onClose={() => setDrawerVisible(false)}
				onChange={() => actionRef.current?.reload()}
				editMenu={editMenuState}
				rootMenus={menuTree}
			/>
		</>
	);
};

export default MenuTable;
