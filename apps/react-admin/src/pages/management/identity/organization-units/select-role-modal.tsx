import { useState } from "react";
import { Modal, Table } from "antd";
import { useTranslation } from "react-i18next";
import type { IdentityRoleDto } from "#/management/identity/role";
import { useQuery } from "@tanstack/react-query";
import { getUnaddedRoleListApi } from "@/api/management/identity/organization-units";

interface Props {
	visible: boolean;
	onClose: () => void;
	onConfirm: (roles: IdentityRoleDto[]) => void;
	organizationUnitId: string;
}

const SelectRoleModal: React.FC<Props> = ({ visible, onClose, onConfirm, organizationUnitId }) => {
	const { t: $t } = useTranslation();
	const [selectedRoles, setSelectedRoles] = useState<IdentityRoleDto[]>([]);
	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	// 获取可添加的角色列表
	const { data, isLoading } = useQuery({
		queryKey: ["organizationUnitRoles", "unAdded", organizationUnitId, searchParams],
		queryFn: () =>
			getUnaddedRoleListApi({
				id: organizationUnitId,
				...searchParams,
			}),
		enabled: visible,
	});

	const columns = [
		{
			title: $t("AbpIdentity.DisplayName:RoleName"),
			dataIndex: "name",
			width: 200,
		},
	];

	return (
		<Modal
			open={visible}
			title={$t("AbpIdentity.Roles")}
			onCancel={onClose}
			onOk={() => onConfirm(selectedRoles)}
			width="800px"
		>
			<Table
				rowKey="id"
				columns={columns}
				dataSource={data?.items}
				loading={isLoading}
				rowSelection={{
					type: "checkbox",
					onChange: (_, rows) => setSelectedRoles(rows),
				}}
				pagination={{
					total: data?.totalCount,
					showSizeChanger: true,
				}}
			/>
		</Modal>
	);
};

export default SelectRoleModal;
