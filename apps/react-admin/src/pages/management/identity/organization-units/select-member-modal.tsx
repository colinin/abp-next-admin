import { useState } from "react";
import { Modal } from "antd";
import { useTranslation } from "react-i18next";
import type { IdentityUserDto } from "#/management/identity";
import { ProTable, type ProColumns } from "@ant-design/pro-table";
import { useQuery } from "@tanstack/react-query";
import { getUnaddedUserListApi } from "@/api/management/identity/organization-units";

interface Props {
	visible: boolean;
	onClose: () => void;
	onConfirm: (users: IdentityUserDto[]) => void;
	organizationUnitId: string;
}

const SelectMemberModal: React.FC<Props> = ({ visible, onClose, onConfirm, organizationUnitId }) => {
	const { t: $t } = useTranslation();
	const [selectedUsers, setSelectedUsers] = useState<IdentityUserDto[]>([]);
	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	// 获取可添加的用户列表
	const { data, isLoading } = useQuery({
		queryKey: ["organizationUnitUsers", "unAdded", organizationUnitId, searchParams],
		queryFn: () =>
			getUnaddedUserListApi({
				id: organizationUnitId,
				...searchParams,
			}),
		enabled: visible,
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
	];

	return (
		<Modal
			open={visible}
			title={$t("AbpIdentity.Users")}
			onCancel={onClose}
			onOk={() => onConfirm(selectedUsers)}
			width="800px"
		>
			<ProTable<IdentityUserDto>
				rowKey="id"
				columns={columns}
				dataSource={data?.items}
				loading={isLoading}
				rowSelection={{
					selections: true,
					onChange: (_, rows) => setSelectedUsers(rows),
				}}
				search={{
					labelWidth: "auto",
					span: 12,
				}}
				request={async (params) => {
					setSearchParams({ filter: params.filter });
					return {
						data: data?.items,
						success: true,
						total: data?.totalCount,
					};
				}}
				pagination={{
					showSizeChanger: true,
					total: data?.totalCount,
				}}
			/>
		</Modal>
	);
};

export default SelectMemberModal;
