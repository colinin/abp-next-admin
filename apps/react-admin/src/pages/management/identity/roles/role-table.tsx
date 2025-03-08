import { useRef, useState } from "react";
import { Button, Dropdown, Modal, Tag, Space, Card } from "antd";
import { EditOutlined, DeleteOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { IdentityRoleDto } from "#/management/identity/role";
import { type ActionType, ProTable, type ProColumns } from "@ant-design/pro-table";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { deleteApi, getPagedListApi } from "@/api/management/identity/role";
import RoleModal from "./role-modal";
import RoleClaimModal from "./role-claim-modal";
import PermissionModal from "@/components/abp/permissions/permission-modal";
import { toast } from "sonner";
import { Iconify } from "@/components/icon";
import useAbpStore from "@/store/abpCoreStore";
import { IdentityRolePermissions } from "@/constants/management/identity/permissions";
import { AuditLogPermissions } from "@/constants/management/auditing/permissions";
import { EntityChangeDrawer } from "@/components/abp/auditing/entity-change-drawer";

const RoleTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const abpStore = useAbpStore();
	const [modal, contextHolder] = Modal.useModal();
	// Modal states
	const [roleModalVisible, setRoleModalVisible] = useState(false);
	const [claimModalVisible, setClaimModalVisible] = useState(false);
	const [permissionModalVisible, setPermissionModalVisible] = useState(false);
	const [entityChangeDrawerVisible, setEntityChangeDrawerVisible] = useState(false);
	const [selectedRole, setSelectedRole] = useState<IdentityRoleDto>();
	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	// 获取角色列表
	const { data, isLoading } = useQuery({
		queryKey: ["roles", searchParams],
		queryFn: () => getPagedListApi(searchParams),
	});

	// 删除角色
	const { mutateAsync: deleteRole } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["roles"] });
		},
	});

	const handleDelete = (role: IdentityRoleDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpIdentity.RoleDeletionConfirmationMessage", { 0: role.name }),
			onOk: () => deleteRole(role.id),
		});
	};

	const handleMenuClick = (key: string, role: IdentityRoleDto) => {
		setSelectedRole(role);
		switch (key) {
			case "permissions":
				setPermissionModalVisible(true);
				break;
			case "claims":
				setClaimModalVisible(true);
				break;
			case "entity-changes":
				setEntityChangeDrawerVisible(true);
				break;
		}
	};

	const columns: ProColumns<IdentityRoleDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:RoleName"),
			dataIndex: "name",
			render: (_, record) => (
				<Space>
					{record.isStatic && <Tag color="#8baac4">{$t("AbpIdentity.Static")}</Tag>}
					{record.isDefault && <Tag color="#108ee9">{$t("AbpIdentity.DisplayName:IsDefault")}</Tag>}
					{record.isPublic && <Tag color="#87d068">{$t("AbpIdentity.Public")}</Tag>}
					<span>{record.name}</span>
				</Space>
			),
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 220,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<div className="flex flex-row">
					<div className="basis-1/3">
						{hasAccessByCodes([IdentityRolePermissions.Update]) && (
							<Button
								type="link"
								icon={<EditOutlined />}
								block
								onClick={() => {
									setSelectedRole(record);
									setRoleModalVisible(true);
								}}
							>
								{$t("AbpUi.Edit")}
							</Button>
						)}
					</div>
					<div className="basis-1/3">
						{hasAccessByCodes([IdentityRolePermissions.Delete]) && !record.isStatic && (
							<Button type="link" danger icon={<DeleteOutlined />} block onClick={() => handleDelete(record)}>
								{$t("AbpUi.Delete")}
							</Button>
						)}
					</div>
					<div className="basis-1/3">
						<Dropdown
							menu={{
								items: [
									hasAccessByCodes([IdentityRolePermissions.ManagePermissions])
										? {
												key: "permissions",
												icon: <Iconify icon="icon-park-outline:permissions" />,
												label: $t("AbpPermissionManagement.Permissions"),
											}
										: null,
									hasAccessByCodes([IdentityRolePermissions.ManageClaims])
										? {
												key: "claims",
												icon: <Iconify icon="la:id-card-solid" />,
												label: $t("AbpIdentity.ManageClaim"),
											}
										: null,
									hasAccessByCodes([AuditLogPermissions.Default])
										? {
												key: "entity-changes",
												icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
												label: $t("AbpAuditLogging.EntitiesChanged"),
											}
										: null,
								].filter(Boolean),
								onClick: ({ key }) => handleMenuClick(key as string, record),
							}}
						>
							<Button type="link" icon={<EllipsisOutlined />} />
						</Dropdown>
					</div>
				</div>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<IdentityRoleDto>
					headerTitle={$t("AbpIdentity.Roles")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					dataSource={data?.items}
					loading={isLoading}
					search={{
						labelWidth: "auto",
						span: 12,
						defaultCollapsed: true,
					}}
					pagination={{
						showSizeChanger: true,
						total: data?.totalCount,
					}}
					request={async (params) => {
						const { filter } = params;
						setSearchParams({ filter });
						// 强制重新请求数据
						await queryClient.invalidateQueries({ queryKey: ["roles"] });
						return { data: data?.items, success: true, total: data?.totalCount };
					}}
					toolBarRender={() => [
						hasAccessByCodes([IdentityRolePermissions.Create]) && (
							<Button
								type="primary"
								onClick={() => {
									setSelectedRole(undefined);
									setRoleModalVisible(true);
								}}
							>
								{$t("AbpIdentity.NewRole")}
							</Button>
						),
					]}
				/>
			</Card>
			<RoleModal
				visible={roleModalVisible}
				role={selectedRole}
				onClose={() => {
					setRoleModalVisible(false);
					setSelectedRole(undefined);
				}}
				onChange={() => {
					// actionRef.current?.reload();
					queryClient.invalidateQueries({ queryKey: ["roles"] });
				}}
			/>
			{selectedRole && (
				<RoleClaimModal
					visible={claimModalVisible}
					role={selectedRole}
					onClose={() => {
						setClaimModalVisible(false);
						setSelectedRole(undefined);
					}}
					// onChange={() => {
					//   actionRef.current?.reload();
					// }}
				/>
			)}
			<PermissionModal
				visible={permissionModalVisible}
				providerKey={selectedRole?.name}
				providerName="R"
				displayName={selectedRole?.name}
				readonly={abpStore.application?.currentUser.roles.includes(selectedRole?.name ?? "")} //TODO 好像有改动
				onClose={() => {
					setPermissionModalVisible(false);
					setSelectedRole(undefined);
				}}
				onChange={() => {
					// actionRef.current?.reload();
					queryClient.invalidateQueries({ queryKey: ["roles"] });
				}}
			/>
			<EntityChangeDrawer
				open={entityChangeDrawerVisible}
				input={{ entityId: selectedRole?.id, entityTypeFullName: "Volo.Abp.Identity.IdentityRole" }}
				onClose={() => {
					setEntityChangeDrawerVisible(false);
					setSelectedRole(undefined);
				}}
			/>
		</>
	);
};

export default RoleTable;
