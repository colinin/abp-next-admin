import { useRef, useState } from "react";
import { Button, Card, Modal } from "antd";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { IdentityUserDto } from "#/management/identity";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import { useMutation, useQuery } from "@tanstack/react-query";
import { addMembers, getUserListApi } from "@/api/management/identity/organization-units";
import { removeOrganizationUnitApi } from "@/api/management/identity/users";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { OrganizationUnitPermissions } from "@/constants/management/identity/permissions";
import SelectMemberModal from "./select-member-modal";
import { toast } from "sonner";

interface Props {
	selectedKey?: string;
}

const OrganizationUnitUserTable: React.FC<Props> = ({ selectedKey }) => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const [modal, contextHolder] = Modal.useModal();
	const [memberModalVisible, setMemberModalVisible] = useState(false);

	// 获取用户列表
	const { data, isLoading, refetch } = useQuery({
		queryKey: ["organizationUnitUsers", selectedKey],
		queryFn: () => {
			if (!selectedKey) {
				return Promise.reject(new Error("selectedKey is undefined"));
			}
			return getUserListApi(selectedKey, {});
		},
		enabled: !!selectedKey,
	});

	// 添加成员
	const { mutateAsync: addUsers } = useMutation({
		mutationFn: (users: IdentityUserDto[]) => {
			if (!selectedKey) {
				return Promise.reject(new Error("selectedKey is undefined"));
			}
			return addMembers(selectedKey, { userIds: users.map((u) => u.id) });
		},
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			refetch();
			actionRef.current?.reload();
			setMemberModalVisible(false);
		},
	});

	// 移除成员
	const { mutateAsync: removeUser } = useMutation({
		mutationFn: (userId: string) => {
			if (!selectedKey) {
				return Promise.reject(new Error("selectedKey is undefined"));
			}
			return removeOrganizationUnitApi(userId, selectedKey);
		},
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			refetch();
			actionRef.current?.reload();
		},
	});

	const columns: ProColumns<IdentityUserDto>[] = [
		{
			title: $t("AbpIdentity.DisplayName:UserName"),
			dataIndex: "userName",
			width: 100,
		},
		{
			title: $t("AbpIdentity.DisplayName:Email"),
			dataIndex: "email",
			width: 120,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 120,
			fixed: "right",
			render: (_, record) => (
				<Button
					type="link"
					danger
					icon={<DeleteOutlined />}
					disabled={!hasAccessByCodes([OrganizationUnitPermissions.ManageUsers])}
					onClick={() => {
						modal.confirm({
							title: $t("AbpUi.AreYouSure"),
							content: $t("AbpIdentity.OrganizationUnit:AreYouSureRemoveUser", { 0: record.userName }),
							onOk: () => removeUser(record.id),
						});
					}}
				>
					{$t("AbpUi.Delete")}
				</Button>
			),
		},
	];

	const getAddMemberEnabled = selectedKey && hasAccessByCodes([OrganizationUnitPermissions.ManageUsers]);

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<IdentityUserDto>
					headerTitle={$t("AbpIdentity.Users")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					dataSource={data?.items}
					loading={isLoading}
					search={false}
					pagination={{
						showSizeChanger: true,
						total: data?.totalCount,
					}}
					toolBarRender={() => [
						getAddMemberEnabled && (
							<Button type="primary" icon={<PlusOutlined />} onClick={() => setMemberModalVisible(true)}>
								{$t("AbpIdentity.OrganizationUnit:AddMember")}
							</Button>
						),
					]}
				/>
			</Card>
			{selectedKey && (
				<SelectMemberModal
					visible={memberModalVisible}
					onClose={() => setMemberModalVisible(false)}
					onConfirm={addUsers}
					organizationUnitId={selectedKey}
				/>
			)}
		</>
	);
};

export default OrganizationUnitUserTable;
