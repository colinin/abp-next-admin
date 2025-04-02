import { useRef, useState } from "react";
import { Button, Card, Modal } from "antd";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { IdentityRoleDto } from "#/management/identity/role";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { OrganizationUnitPermissions } from "@/constants/management/identity/permissions";
import SelectRoleModal from "./select-role-modal";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { removeOrganizationUnitApi } from "@/api/management/identity/role";
import { addRoles, getRoleListApi } from "@/api/management/identity/organization-units";
import { toast } from "sonner";

interface Props {
	selectedKey?: string;
}

const OrganizationUnitRoleTable: React.FC<Props> = ({ selectedKey }) => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const [roleModalVisible, setRoleModalVisible] = useState(false);

	// 获取角色列表
	const { data, isLoading, refetch } = useQuery({
		queryKey: ["organizationUnitRoles", selectedKey],
		queryFn: () => {
			if (!selectedKey) {
				return Promise.reject(new Error("selectedKey is undefined"));
			}
			return getRoleListApi(selectedKey, { filter: "" });
		},
		enabled: !!selectedKey,
	});

	// 添加角色
	const { mutateAsync: addRolesToUnit } = useMutation({
		mutationFn: (roles: IdentityRoleDto[]) => {
			if (!selectedKey) {
				return Promise.reject(new Error("selectedKey is undefined"));
			}
			return addRoles(selectedKey, {
				roleIds: roles.map((role) => role.id),
			});
		},
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			setRoleModalVisible(false);
			refetch();

			actionRef.current?.reload();
		},
	});

	// 移除角色
	const { mutateAsync: removeRole } = useMutation({
		mutationFn: (roleId: string) => {
			if (!selectedKey) {
				return Promise.reject(new Error("selectedKey is undefined"));
			}
			return removeOrganizationUnitApi(roleId, selectedKey);
		},
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			refetch();
			actionRef.current?.reload();
		},
	});

	const handleDelete = (role: IdentityRoleDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpIdentity.OrganizationUnit:AreYouSureRemoveRole", { 0: role.name }),
			onOk: () => removeRole(role.id),
		});
	};

	const columns: ProColumns<IdentityRoleDto>[] = [
		{
			title: $t("AbpIdentity.DisplayName:RoleName"),
			dataIndex: "name",
			width: 200,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 180,
			fixed: "right",
			render: (_, record) => (
				<Button
					type="link"
					danger
					icon={<DeleteOutlined />}
					disabled={!hasAccessByCodes([OrganizationUnitPermissions.ManageRoles])}
					onClick={() => handleDelete(record)}
				>
					{$t("AbpUi.Delete")}
				</Button>
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
					columns={columns}
					dataSource={data?.items}
					loading={isLoading}
					rowKey="id"
					search={false}
					pagination={{
						showSizeChanger: true,
						total: data?.totalCount,
					}}
					toolBarRender={() => [
						selectedKey && hasAccessByCodes([OrganizationUnitPermissions.ManageRoles]) && (
							<Button type="primary" icon={<PlusOutlined />} onClick={() => setRoleModalVisible(true)}>
								{$t("AbpIdentity.OrganizationUnit:AddRole")}
							</Button>
						),
					]}
				/>
			</Card>
			{selectedKey && (
				<SelectRoleModal
					visible={roleModalVisible}
					onClose={() => setRoleModalVisible(false)}
					onConfirm={addRolesToUnit}
					organizationUnitId={selectedKey}
				/>
			)}
		</>
	);
};

export default OrganizationUnitRoleTable;
