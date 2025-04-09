import type React from "react";
import { useState, useRef } from "react";
import { Button, Popconfirm, Space } from "antd";
import { EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import type { IdentityClaimDto } from "#/management/identity/claims";
import type { ClaimModalProps } from "./types";
import { toast } from "sonner";
import { withAccessChecker } from "@/utils/abp/access-checker";
import ClaimModal from "./claim-modal";

const ClaimTable: React.FC<ClaimModalProps> = ({
	createApi,
	createPolicy,
	deleteApi,
	deletePolicy,
	getApi,
	updateApi,
	updatePolicy,
	queryKey,
}) => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedClaim, setSelectedClaim] = useState<IdentityClaimDto | null>(null);

	const { data: claimsData } = useQuery({
		queryKey: queryKey,
		queryFn: getApi,
	});

	const { mutateAsync: deleteClaim } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: queryKey });
		},
	});

	const openModal = (claim?: IdentityClaimDto) => {
		setSelectedClaim(claim || null);
		setModalVisible(true);
	};

	const closeModal = () => {
		setModalVisible(false);
		setSelectedClaim(null);
	};

	const handleDelete = async (row: IdentityClaimDto) => {
		await deleteClaim({
			claimType: row.claimType,
			claimValue: row.claimValue,
		});
	};

	const columns: ProColumns<IdentityClaimDto>[] = [
		{
			title: $t("AbpIdentity.DisplayName:ClaimType"),
			dataIndex: "claimType",
			width: 120,
		},
		{
			title: $t("AbpIdentity.DisplayName:ClaimValue"),
			dataIndex: "claimValue",
			width: "auto",
		},
		{
			title: $t("AbpUi.Actions"),
			dataIndex: "action",
			fixed: "right",
			width: 180,
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => openModal(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[updatePolicy],
					)}
					{withAccessChecker(
						<Popconfirm
							title={$t("AbpIdentity.WillDeleteClaim", { 0: record.claimType })}
							onConfirm={() => handleDelete(record)}
						>
							<Button type="link" danger icon={<DeleteOutlined />}>
								{$t("AbpUi.Delete")}
							</Button>
						</Popconfirm>,
						[deletePolicy],
					)}
				</Space>
			),
		},
	];

	return (
		<div>
			<ProTable<IdentityClaimDto>
				actionRef={actionRef}
				columns={columns}
				rowKey="id"
				search={false}
				dataSource={claimsData?.items}
				pagination={false}
				toolBarRender={() => [
					withAccessChecker(
						<Button type="primary" onClick={() => openModal()}>
							{$t("AbpIdentity.AddClaim")}
						</Button>,
						[createPolicy],
					),
				]}
			/>
			<ClaimModal
				visible={modalVisible}
				claim={selectedClaim || undefined}
				onClose={closeModal}
				onChange={() => {
					queryClient.invalidateQueries({ queryKey: queryKey });
					closeModal();
				}}
				createApi={createApi}
				updateApi={updateApi}
			/>
		</div>
	);
};

export default ClaimTable;
