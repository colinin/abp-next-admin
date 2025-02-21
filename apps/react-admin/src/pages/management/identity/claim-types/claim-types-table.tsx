import { useRef, useState } from "react";
import { Button, Popconfirm, Space, Card, Input } from "antd";
import { EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import ClaimTypeModal from "./claim-type-modal";
import { type IdentityClaimTypeDto, ValueType } from "#/management/identity";
import { toast } from "sonner";
import { IdentityClaimTypePermissions } from "@/constants/management/identity/permissions";
import { withAccessChecker, hasAccessByCodes } from "@/utils/abp/access-checker";
import ProTable, { type ActionType, type ProColumns } from "@ant-design/pro-table";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";
import { deleteApi, getPagedListApi } from "@/api/management/identity/claim-types";
import { useMutation, useQueryClient } from "@tanstack/react-query";

const ClaimTypeTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	// Filter state
	const [filter, setFilter] = useState<string | undefined>();
	// Modal State
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedClaim, setSelectedClaim] = useState<IdentityClaimTypeDto | null>(null);
	const openModal = (claim?: IdentityClaimTypeDto) => {
		setSelectedClaim(claim || null);
		setModalVisible(true);
	};

	const closeModal = () => {
		setModalVisible(false);
		setSelectedClaim(null);
	};

	const { mutateAsync: deleteClaimType } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["claimTypes"] });
		},
	});

	const handleDelete = async (id: string) => {
		await deleteClaimType(id);
		actionRef.current?.reload();
	};

	const columns: ProColumns<IdentityClaimTypeDto>[] = [
		{
			title: $t("AbpIdentity.IdentityClaim:Name"),
			dataIndex: "name",
			sorter: true,
			width: 200,
		},
		{
			title: $t("AbpIdentity.IdentityClaim:ValueType"),
			dataIndex: "valueType",
			sorter: true,
			valueEnum: {
				[ValueType.String]: { text: "String" },
				[ValueType.Int]: { text: "Int" },
				[ValueType.Boolean]: { text: "Boolean" },
				[ValueType.DateTime]: { text: "DateTime" },
			},
			width: 150,
		},
		{
			title: $t("AbpIdentity.IdentityClaim:Regex"),
			dataIndex: "regex",
		},
		{
			align: "center",
			title: $t("AbpIdentity.IdentityClaim:Required"),
			dataIndex: "required",
			width: 100,
			sorter: true,
			render: (value) => (value ? <span style={{ color: "green" }}>✔</span> : <span style={{ color: "red" }}>✘</span>),
		},
		{
			align: "center",
			dataIndex: "isStatic",
			sorter: true,
			title: $t("AbpIdentity.IdentityClaim:IsStatic"),
			render: (value) => (value ? <span style={{ color: "green" }}>✔</span> : <span style={{ color: "red" }}>✘</span>),
		},
		{
			title: $t("AbpIdentity.IdentityClaim:Description"),
			dataIndex: "description",
			ellipsis: true,
			width: 250,
		},
		hasAccessByCodes([IdentityClaimTypePermissions.Update, IdentityClaimTypePermissions.Delete])
			? {
					title: $t("AbpUi.Actions"),
					valueType: "option",
					key: "actions",
					align: "center",
					fixed: "right",
					width: 150,
					render: (_, record) => (
						<div className="flex gap-1">
							{withAccessChecker(
								<Button key="edit" type="link" icon={<EditOutlined />} onClick={() => openModal(record)}>
									{$t("AbpUi.Edit")}
								</Button>,
								[IdentityClaimTypePermissions.Update],
							)}
							{withAccessChecker(
								<Popconfirm
									key="delete"
									title={$t("AbpUi.AreYouSure")}
									onConfirm={() => handleDelete(record.id)}
									okText={$t("AbpUi.Yes")}
									cancelText={$t("AbpUi.No")}
								>
									<Button type="link" danger icon={<DeleteOutlined />}>
										{$t("AbpUi.Delete")}
									</Button>
								</Popconfirm>,
								[IdentityClaimTypePermissions.Delete],
							)}
						</div>
					),
				}
			: {},
	];

	return (
		<>
			<Space direction="vertical" size="large" className="w-full">
				<Card>
					<ProTable<IdentityClaimTypeDto>
						headerTitle={$t("AbpIdentity.DisplayName:ClaimType")}
						actionRef={actionRef}
						rowKey="id"
						columns={columns}
						request={async (params, sorter) => {
							const { current, pageSize, ...filters } = params;

							const query = await queryClient.fetchQuery({
								queryKey: ["claimTypes", params, sorter, filter],
								queryFn: () =>
									getPagedListApi({
										maxResultCount: pageSize,
										skipCount: ((current || 1) - 1) * (pageSize || 0),
										filter,
										sorting: sorter
											? Object.keys(sorter)
													.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
													.join(", ")
											: undefined,
										...filters,
									}),
							});

							return {
								data: query.items,
								total: query.totalCount,
							};
						}}
						pagination={{
							showSizeChanger: true,
						}}
						toolBarRender={() => [
							<Input.Search
								key="customFilter"
								placeholder={$t("AbpUi.Search")}
								allowClear
								onSearch={(value) => {
									setFilter(value); // Update the custom filter state
									actionRef.current?.reload();
								}}
								style={{ width: 240 }}
							/>,
							withAccessChecker(
								<Button type="primary" key="add" onClick={() => openModal()}>
									{$t("AbpIdentity.IdentityClaim:New")}
								</Button>,
								[IdentityClaimTypePermissions.Create],
							),
						]}
						search={false}
						scroll={{ x: "max-content" }}
					/>
				</Card>
			</Space>
			{modalVisible && (
				<ClaimTypeModal
					visible={modalVisible}
					claimType={selectedClaim || undefined}
					onClose={closeModal}
					onSuccess={() => {
						closeModal();
						actionRef.current?.reload();
						queryClient.invalidateQueries({ queryKey: ["claimTypes"] });
					}}
				/>
			)}
		</>
	);
};

export default ClaimTypeTable;
