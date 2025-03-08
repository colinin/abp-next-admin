import { useCallback, useRef, useState } from "react";
import { Button, Card, Dropdown, Modal } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { PermissionGroupDefinitionDto } from "#/management/permissions/groups";
import { type ActionType, ProTable, type ProColumns } from "@ant-design/pro-table";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { deleteApi, getListApi } from "@/api/management/permissions/groups";
import { GroupDefinitionsPermissions, PermissionDefinitionsPermissions } from "@/constants/management/permissions";
import PermissionGroupDefinitionModal from "./permission-group-definition-modal";
import PermissionDefinitionModal from "../permissions/permission-definition-modal";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { toast } from "sonner";
import { Iconify } from "@/components/icon";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

const PermissionGroupDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const { deserialize } = localizationSerializer();

	// Modal states
	const [groupModalVisible, setGroupModalVisible] = useState(false);
	const [permissionModalVisible, setPermissionModalVisible] = useState(false);
	const [selectedGroup, setSelectedGroup] = useState<PermissionGroupDefinitionDto>();
	const [selectedGroupForPermission, setSelectedGroupForPermission] = useState<string>();
	const { Lr } = useLocalizer(
		undefined,
		useCallback(() => {
			actionRef.current?.reload();
			setPermissionModalVisible(false);
			setGroupModalVisible(false);
		}, []),
	);

	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	// 获取权限组列表
	const { data, isLoading } = useQuery({
		queryKey: ["permissionGroups", searchParams],
		queryFn: async () => {
			const { items } = await getListApi(searchParams);
			return items.map((item) => {
				const localizableString = deserialize(item.displayName);
				return {
					...item,
					displayName: Lr(localizableString.resourceName, localizableString.name),
				};
			});
		},
	});

	// 删除权限组
	const { mutateAsync: deleteGroup } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["permissionGroups"] });
		},
	});

	const handleCreate = () => {
		setSelectedGroup(undefined);
		setGroupModalVisible(true);
	};

	const handleUpdate = (group: PermissionGroupDefinitionDto) => {
		setSelectedGroup(group);
		setGroupModalVisible(true);
	};

	const handleDelete = (group: PermissionGroupDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: group.name }),
			onOk: () => deleteGroup(group.name),
		});
	};

	const handleMenuClick = (key: string, group: PermissionGroupDefinitionDto) => {
		if (key === "permissions") {
			setSelectedGroupForPermission(group.name);
			setPermissionModalVisible(true);
		}
	};

	const columns: ProColumns<PermissionGroupDefinitionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true, // hide in table
		},
		{
			title: $t("AbpPermissionManagement.DisplayName:Name"),
			dataIndex: "name",
			width: "auto",
			hideInSearch: true,
		},
		{
			title: $t("AbpPermissionManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: "auto",
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 220,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<div className="flex flex-row">
					<div className={`${record.isStatic ? "w-full" : "basis-1/3"}`}>
						{hasAccessByCodes([GroupDefinitionsPermissions.Update]) && (
							<Button type="link" icon={<EditOutlined />} block onClick={() => handleUpdate(record)}>
								{$t("AbpUi.Edit")}
							</Button>
						)}
					</div>
					{!record.isStatic && (
						<>
							<div className="basis-1/3">
								{hasAccessByCodes([GroupDefinitionsPermissions.Delete]) && (
									<Button type="link" danger icon={<DeleteOutlined />} block onClick={() => handleDelete(record)}>
										{$t("AbpUi.Delete")}
									</Button>
								)}
							</div>
							<div className="basis-1/3">
								<Dropdown
									menu={{
										items: [
											hasAccessByCodes([PermissionDefinitionsPermissions.Create])
												? {
														key: "permissions",
														icon: <Iconify icon="icon-park-outline:permissions" />,
														label: $t("AbpPermissionManagement.PermissionDefinitions:AddNew"),
													}
												: null,
										].filter((item) => item !== null), // 过滤
										onClick: ({ key }) => handleMenuClick(key as string, record),
									}}
								>
									<Button type="link" icon={<EllipsisOutlined />} />
								</Dropdown>
							</div>
						</>
					)}
				</div>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<PermissionGroupDefinitionDto>
					headerTitle={$t("AbpPermissionManagement.GroupDefinitions")}
					actionRef={actionRef}
					columns={columns}
					dataSource={data}
					loading={isLoading}
					rowKey="name"
					pagination={{
						showSizeChanger: true,
						total: data?.length,
					}}
					search={{
						labelWidth: "auto",
						span: 12,
						defaultCollapsed: true,
					}}
					toolBarRender={() => [
						hasAccessByCodes([GroupDefinitionsPermissions.Create]) && (
							<Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AbpPermissionManagement.GroupDefinitions:AddNew")}
							</Button>
						),
					]}
					request={async (params) => {
						const { filter } = params;
						setSearchParams({ filter });
						// 强制重新请求数据
						await queryClient.invalidateQueries({ queryKey: ["permissionGroups"] });
						return { data, success: true, total: data?.length };
					}}
				/>
			</Card>
			<PermissionGroupDefinitionModal
				visible={groupModalVisible}
				groupName={selectedGroup?.name}
				onClose={() => setGroupModalVisible(false)}
				onChange={() => {
					setGroupModalVisible(false);
					actionRef.current?.reload();
				}}
			/>

			<PermissionDefinitionModal
				visible={permissionModalVisible}
				onClose={() => {
					console.log("close");
					setPermissionModalVisible(false);
				}}
				onChange={() => {
					setPermissionModalVisible(false);
					actionRef.current?.reload();
				}}
				groupName={selectedGroupForPermission}
			/>
		</>
	);
};

export default PermissionGroupDefinitionTable;
