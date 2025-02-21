import { useCallback, useRef, useState } from "react";
import { Button, Card, Modal, Space, Table, Tag } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import type { MultiTenancySides, PermissionDefinitionDto } from "#/management/permissions/definitions";
import { type ActionType, ProTable, type ProColumns } from "@ant-design/pro-table";

import { localizationSerializer } from "@/utils/abp/localization-serializer";

import { deleteApi, getListApi as getPermissionsApi } from "@/api/management/permissions/definitions";
import { getListApi as getGroupsApi } from "@/api/management/permissions/groups";
import { GroupDefinitionsPermissions } from "@/constants/management/permissions";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import PermissionDefinitionModal from "./permission-definition-modal";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { useTypesMap } from "./types";
import { listToTree } from "@/utils/tree";
import type { ExtraPropertyDictionary } from "#/abp-core";
import type { ColumnsType } from "antd/es/table";
import { toast } from "sonner";

interface PermissionVo {
	children: PermissionVo[];
	displayName: string;
	groupName: string;
	isEnabled: boolean;
	isStatic: boolean;
	multiTenancySide: MultiTenancySides;
	name: string;
	parentName?: string;
	providers: string[];
	stateCheckers: string;
	extraProperties: ExtraPropertyDictionary;
}

interface PermissionGroupVo {
	displayName: string;
	name: string;
	permissions: PermissionVo[];
}

const PermissionDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const [modal, contextHolder] = Modal.useModal();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedPermission, setSelectedPermission] = useState<PermissionDefinitionDto>();

	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	const { Lr } = useLocalizer(
		undefined,
		useCallback(() => {
			actionRef.current?.reload();
			setModalVisible(false);
		}, []),
	);

	const { deserialize } = localizationSerializer();
	const { multiTenancySidesMap, providersMap } = useTypesMap($t);

	// 删除权限操作
	const { mutateAsync: deletePermission } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["permissions", "permissionGroups"] });
		},
	});

	// 获取权限数据
	const { data, isLoading } = useQuery({
		queryKey: ["permissions", "permissionGroups", searchParams],
		queryFn: async () => {
			const [groupRes, permissionRes] = await Promise.all([
				getGroupsApi(searchParams),
				getPermissionsApi(searchParams),
			]);

			const groups: PermissionGroupVo[] = groupRes.items.map((group) => {
				const localizableGroup = deserialize(group.displayName);
				const permissions = permissionRes.items
					.filter((permission) => permission.groupName === group.name)
					.map((permission) => {
						const localizablePermission = deserialize(permission.displayName);
						return {
							...permission,
							displayName: Lr(localizablePermission.resourceName, localizablePermission.name),
						};
					});
				return {
					...group,
					displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
					permissions: listToTree<PermissionVo>(permissions, {
						id: "name",
						pid: "parentName",
					}),
				};
			});

			return groups;
		},
	});

	const handleDelete = async (permission: PermissionDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: permission.name }),
			onOk: () => deletePermission(permission.name),
		});
	};

	const mainColumns: ProColumns<PermissionGroupVo>[] = [
		{
			title: $t("abp.sequence"),
			dataIndex: "index",
			valueType: "index",
			width: 50,
			render: (_, __, index) => index + 1,
		},
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true, // hide in table
		},
		{
			title: $t("AbpPermissionManagement.DisplayName:Name"),
			dataIndex: "name",
			width: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpPermissionManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
			hideInSearch: true,
		},
	];

	const subColumns: ColumnsType<PermissionVo> = [
		{
			title: $t("AbpPermissionManagement.DisplayName:Name"),
			dataIndex: "name",
			width: 200,
			ellipsis: true,
		},
		{
			title: $t("AbpPermissionManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 200,
		},
		{
			align: "center",
			minWidth: 100,
			// slots: { default: 'tenant' },
			title: $t("AbpPermissionManagement.DisplayName:MultiTenancySide"),
			render: (_, record) => (
				<Space>
					<Tag color="blue">{multiTenancySidesMap[record.multiTenancySide]}</Tag>
				</Space>
			),
		},
		{
			title: $t("AbpPermissionManagement.DisplayName:Providers"),
			width: 200,
			render: (_, record) => (
				<Space>
					{record.providers.map((provider) => (
						<Tag key={provider} color="blue">
							{providersMap[provider]}
						</Tag>
					))}
				</Space>
			),
		},
		{
			title: $t("AbpUi.Actions"),
			width: 180,
			fixed: "right",
			render: (_, record) => (
				<Space>
					{hasAccessByCodes([GroupDefinitionsPermissions.Update]) && (
						<Button
							type="link"
							icon={<EditOutlined />}
							onClick={() => {
								setSelectedPermission(record);
								setModalVisible(true);
							}}
						>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{!record.isStatic && hasAccessByCodes([GroupDefinitionsPermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
				</Space>
			),
		},
	];

	const expandedRowRender = (group: PermissionGroupVo) => {
		return (
			<Table<PermissionVo>
				columns={subColumns}
				dataSource={group.permissions}
				pagination={false}
				rowKey={(record) => record.name}
				indentSize={30}
				expandable={{
					defaultExpandAllRows: false,
					childrenColumnName: "children", // 指定子项字段
				}}
			/>
		);
	};

	// 工具栏
	const toolBarRender = () => [
		hasAccessByCodes([GroupDefinitionsPermissions.Create]) && (
			<Button
				type="primary"
				icon={<PlusOutlined />}
				onClick={() => {
					setSelectedPermission(undefined);
					setModalVisible(true);
				}}
			>
				{$t("AbpPermissionManagement.PermissionDefinitions:AddNew")}
			</Button>
		),
	];

	return (
		<>
			{contextHolder}

			<Card>
				<ProTable<PermissionGroupVo>
					headerTitle={$t("AbpPermissionManagement.PermissionDefinitions")}
					actionRef={actionRef}
					columns={mainColumns}
					dataSource={data}
					loading={isLoading}
					rowKey={(record) => record.name}
					expandable={{ expandedRowRender }}
					toolBarRender={toolBarRender}
					pagination={{
						showSizeChanger: true,
						total: data?.length,
					}}
					search={{
						labelWidth: "auto",
						span: 12, //search part width
						defaultCollapsed: true,
					}}
					request={async (params) => {
						const { filter } = params;
						setSearchParams({ filter: filter });
						//强制重新请求数据（用于table的刷新按钮）
						await queryClient.invalidateQueries({ queryKey: ["permissions", "permissionGroups"] });
						return { data, success: true, total: data?.length };
					}}
				/>
			</Card>
			<PermissionDefinitionModal
				visible={modalVisible}
				permission={selectedPermission}
				onClose={() => {
					setModalVisible(false);
					setSelectedPermission(undefined);
				}}
				onChange={() => {
					setModalVisible(false);
					actionRef.current?.reload();
				}}
			/>
		</>
	);
};

export default PermissionDefinitionTable;
