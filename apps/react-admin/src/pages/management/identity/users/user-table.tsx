import { useRef, useState } from "react";
import { Button, Dropdown, Modal, Space, Tag } from "antd";
import {
	EditOutlined,
	DeleteOutlined,
	EllipsisOutlined,
	LockOutlined,
	UnlockOutlined,
	PlusOutlined,
} from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { IdentityUserDto } from "#/management/identity/user";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi, unLockApi } from "@/api/management/identity/users";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { IdentityUserPermissions } from "@/constants/management/identity/permissions";
import { AuditLogPermissions } from "@/constants/management/auditing/permissions";
import { Iconify } from "@/components/icon";
import { toast } from "sonner";
import { formatToDateTime } from "@/utils/abp";
import useAbpStore from "@/store/abpCoreStore";

// Modals
import UserModal from "./user-modal";
import UserLockModal from "./user-lock-modal";
import UserClaimModal from "./user-claim-modal";
import UserPasswordModal from "./user-password-modal";
import PermissionModal from "@/components/abp/permissions/permission-modal";
import { EntityChangeDrawer } from "@/components/abp/auditing/entity-change-drawer";
import Card from "@/components/card";

const UserTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const [modal, contextHolder] = Modal.useModal();
	const queryClient = useQueryClient();
	const abpStore = useAbpStore();

	// Modal visibility states
	const [userModalVisible, setUserModalVisible] = useState(false);
	const [lockModalVisible, setLockModalVisible] = useState(false);
	const [claimModalVisible, setClaimModalVisible] = useState(false);
	const [passwordModalVisible, setPasswordModalVisible] = useState(false);
	const [permissionModalVisible, setPermissionModalVisible] = useState(false);
	const [entityChangeDrawerVisible, setEntityChangeDrawerVisible] = useState(false);

	// Selected user state
	const [selectedUser, setSelectedUser] = useState<IdentityUserDto>();

	// Check if user is locked
	const isLocked = (user: IdentityUserDto) => {
		if (!user.lockoutEnd) return false;
		const lockTime = new Date(user.lockoutEnd);
		return lockTime > new Date();
	};

	// API Mutations
	const { mutateAsync: deleteUser } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["users"] });
		},
	});

	const { mutateAsync: unlockUser } = useMutation({
		mutationFn: unLockApi,
		onSuccess: () => {
			toast.success($t("AbpIdentity.SavedSuccessfully"));
			actionRef.current?.reload();
			queryClient.invalidateQueries({ queryKey: ["users"] });
		},
	});

	// Handle actions
	const handleMenuClick = (key: string, user: IdentityUserDto) => {
		setSelectedUser(user);
		switch (key) {
			case "lock":
				setLockModalVisible(true);
				break;
			case "unlock":
				modal.confirm({
					title: $t("AbpUi.AreYouSure"),
					onOk: () => unlockUser(user.id),
				});
				break;
			case "permissions":
				setPermissionModalVisible(true);
				break;
			case "claims":
				setClaimModalVisible(true);
				break;
			case "password":
				setPasswordModalVisible(true);
				break;
			case "entity-changes":
				setEntityChangeDrawerVisible(true);
				break;
		}
	};

	const columns: ProColumns<IdentityUserDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			hideInTable: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:IsActive"),
			dataIndex: "isActive",
			width: 100,
			align: "center",
			hideInSearch: true,
			render: (_, record) => (
				<Tag color={record.isActive ? "success" : "error"}>{record.isActive ? $t("AbpUi.Yes") : $t("AbpUi.No")}</Tag>
			),
		},
		{
			title: $t("AbpIdentity.DisplayName:UserName"),
			dataIndex: "userName",
			hideInSearch: true,
			width: 150,
		},
		{
			title: $t("AbpIdentity.DisplayName:Email"),
			dataIndex: "email",
			hideInSearch: true,
			width: 180,
			sorter: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:PhoneNumber"),
			dataIndex: "phoneNumber",
			hideInSearch: true,
			width: 120,
		},
		{
			title: $t("AbpIdentity.DisplayName:Surname"),
			dataIndex: "surname",
			hideInSearch: true,
			width: 100,
		},
		{
			title: $t("AbpIdentity.DisplayName:Name"),
			dataIndex: "name",
			hideInSearch: true,

			width: 100,
		},
		{
			title: $t("AbpIdentity.LockoutEnd"),
			hideInSearch: true,
			dataIndex: "lockoutEnd",
			width: 160,
			render: (_, record) => (record.lockoutEnd ? formatToDateTime(record.lockoutEnd) : "-"),
		},
		{
			title: $t("AbpUi.Actions"),
			width: 200,
			hideInSearch: true,
			fixed: "right",
			render: (_, record) => (
				<Space>
					{hasAccessByCodes([IdentityUserPermissions.Update]) && (
						<Button
							type="link"
							icon={<EditOutlined />}
							onClick={() => {
								setSelectedUser(record);
								setUserModalVisible(true);
							}}
						>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{hasAccessByCodes([IdentityUserPermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
					<Dropdown
						menu={{
							items: getDropdownItems(record),
							onClick: ({ key }) => handleMenuClick(key, record),
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	const handleDelete = (user: IdentityUserDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpIdentity.UserDeletionConfirmationMessage", { 0: user.userName }),
			onOk: () => deleteUser(user.id),
		});
	};

	const getDropdownItems = (record: IdentityUserDto) => {
		const items = [];

		// Lock/Unlock
		if (hasAccessByCodes([IdentityUserPermissions.Update])) {
			if (record.isActive && !isLocked(record)) {
				items.push({
					key: "lock",
					icon: <LockOutlined />,
					label: $t("AbpIdentity.Lock"),
				});
			}
			if (record.isActive && isLocked(record)) {
				items.push({
					key: "unlock",
					icon: <UnlockOutlined />,
					label: $t("AbpIdentity.UnLock"),
				});
			}
		}

		// Permissions
		if (hasAccessByCodes([IdentityUserPermissions.ManagePermissions])) {
			items.push({
				key: "permissions",
				icon: <Iconify icon="icon-park-outline:permissions" />,
				label: $t("AbpPermissionManagement.Permissions"),
			});
		}

		// Claims
		if (hasAccessByCodes([IdentityUserPermissions.ManageClaims])) {
			items.push({
				key: "claims",
				icon: <Iconify icon="la:id-card-solid" />,
				label: $t("AbpIdentity.ManageClaim"),
			});
		}

		// Password
		if (hasAccessByCodes([IdentityUserPermissions.Update])) {
			items.push({
				key: "password",
				icon: <Iconify icon="carbon:password" />,
				label: $t("AbpIdentity.SetPassword"),
			});
		}

		// Audit Log
		if (hasAccessByCodes([AuditLogPermissions.Default])) {
			items.push({
				key: "entity-changes",
				icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
				label: $t("AbpAuditLogging.EntitiesChanged"),
			});
		}

		return items;
	};

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<IdentityUserDto>
					actionRef={actionRef}
					columns={columns}
					request={async (params) => {
						const { current, pageSize, filter } = params;
						const query = await queryClient.fetchQuery({
							queryKey: ["users", params],
							queryFn: () =>
								getPagedListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									filter: filter,
								}),
						});
						return {
							data: query.items,
							total: query.totalCount,
						};
					}}
					rowKey="id"
					search={{
						span: 12,
						labelWidth: "auto",
					}}
					pagination={{
						showSizeChanger: true,
					}}
					toolBarRender={() => [
						hasAccessByCodes([IdentityUserPermissions.Create]) && (
							<Button
								type="primary"
								icon={<PlusOutlined />}
								onClick={() => {
									setSelectedUser(undefined);
									setUserModalVisible(true);
								}}
							>
								{$t("AbpIdentity.NewUser")}
							</Button>
						),
					]}
				/>
			</Card>

			{/* User Modal */}
			<UserModal
				visible={userModalVisible}
				userId={selectedUser?.id}
				onClose={() => {
					setUserModalVisible(false);
					setSelectedUser(undefined);
				}}
				onChange={() => actionRef.current?.reload()}
			/>

			{/* Lock Modal */}
			{selectedUser && (
				<UserLockModal
					visible={lockModalVisible}
					userId={selectedUser.id}
					onClose={() => {
						setLockModalVisible(false);
						setSelectedUser(undefined);
					}}
					onChange={() => {
						actionRef.current?.reload();
						queryClient.invalidateQueries({ queryKey: ["users"] });
					}}
				/>
			)}

			{/* Claim Modal */}
			{selectedUser && (
				<UserClaimModal
					visible={claimModalVisible}
					user={selectedUser}
					onClose={() => {
						setClaimModalVisible(false);
						setSelectedUser(undefined);
					}}
					onChange={() => actionRef.current?.reload()}
				/>
			)}

			{/* Password Modal */}
			{selectedUser && (
				<UserPasswordModal
					visible={passwordModalVisible}
					userId={selectedUser.id}
					onClose={() => {
						setPasswordModalVisible(false);
						setSelectedUser(undefined);
					}}
					onChange={() => actionRef.current?.reload()}
				/>
			)}

			{/* Permission Modal */}
			<PermissionModal
				visible={permissionModalVisible}
				providerKey={selectedUser?.id}
				providerName="U"
				displayName={selectedUser?.userName}
				readonly={abpStore.application?.currentUser.id === selectedUser?.id}
				onClose={() => {
					setPermissionModalVisible(false);
					setSelectedUser(undefined);
				}}
				onChange={() => actionRef.current?.reload()}
			/>

			{/* Entity Change Drawer */}
			<EntityChangeDrawer
				open={entityChangeDrawerVisible}
				input={{
					entityId: selectedUser?.id,
					entityTypeFullName: "Volo.Abp.Identity.IdentityUser",
				}}
				onClose={() => {
					setEntityChangeDrawerVisible(false);
					setSelectedUser(undefined);
				}}
			/>
		</>
	);
};

export default UserTable;
