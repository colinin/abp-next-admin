import type React from "react";
import { Modal } from "antd";
import { useTranslation } from "react-i18next";
import type { IdentityRoleDto } from "#/management/identity";
import type {
	IdentityClaimCreateDto,
	IdentityClaimDeleteDto,
	IdentityClaimUpdateDto,
} from "#/management/identity/claims";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createClaimApi, deleteClaimApi, getClaimsApi, updateClaimApi } from "@/api/management/identity/role";
import { IdentityRolePermissions } from "@/constants/management/identity/permissions";
import ClaimTable from "@/components/abp/claims/claim-table";

interface Props {
	visible: boolean;
	onClose: () => void;
	role: IdentityRoleDto;
}

//TODO on change 结合测试配置的role的claims和其它calims获取的地方
const RoleClaimModal: React.FC<Props> = ({ visible, onClose, role }) => {
	const { t: $t } = useTranslation();
	const queryClient = useQueryClient();
	const queryKey = ["roleClaims", role.id];

	// Mutations for CRUD operations
	const { mutateAsync: createClaim } = useMutation({
		mutationFn: (input: IdentityClaimCreateDto) => createClaimApi(role.id, input),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey });
		},
	});

	const { mutateAsync: updateClaim } = useMutation({
		mutationFn: (input: IdentityClaimUpdateDto) => updateClaimApi(role.id, input),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey });
		},
	});

	const { mutateAsync: deleteClaim } = useMutation({
		mutationFn: (input: IdentityClaimDeleteDto) => deleteClaimApi(role.id, input),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey });
		},
	});

	return (
		<Modal
			title={$t("AbpIdentity.ManageClaim")}
			open={visible}
			onCancel={onClose}
			footer={null}
			width={800}
			destroyOnClose
		>
			<ClaimTable
				createApi={createClaim}
				createPolicy={IdentityRolePermissions.ManageClaims}
				deleteApi={deleteClaim}
				deletePolicy={IdentityRolePermissions.ManageClaims}
				updateApi={updateClaim}
				updatePolicy={IdentityRolePermissions.ManageClaims}
				getApi={async () => getClaimsApi(role.id)}
				queryKey={queryKey}
			/>
		</Modal>
	);
};

export default RoleClaimModal;
