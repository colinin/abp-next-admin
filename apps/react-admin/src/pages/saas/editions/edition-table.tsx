import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi } from "@/api/saas/editions";
import type { EditionDto } from "#/saas/editions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { EditionsPermissions } from "@/constants/saas/permissions"; // Assuming constants location
import { AuditLogPermissions } from "@/constants/management/auditing/permissions";

import { Iconify } from "@/components/icon";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";

import EditionModal from "./edition-modal";
import FeatureModal from "@/components/abp/features/feature-modal";
import { useFeatures } from "@/hooks/abp/fake-hooks/use-abp-feature";
import { EntityChangeDrawer } from "@/components/abp/auditing/entity-change-drawer";

const EditionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const featureChecker = useFeatures();

	// Modal States
	const [editionModalVisible, setEditionModalVisible] = useState(false);
	const [featureModalVisible, setFeatureModalVisible] = useState(false);
	const [entityChangeDrawerVisible, setEntityChangeDrawerVisible] = useState(false);

	const [selectedEditionId, setSelectedEditionId] = useState<string | undefined>();
	const [selectedEditionName, setSelectedEditionName] = useState<string | undefined>();

	const { mutateAsync: deleteEdition } = useMutation({
		mutationFn: deleteApi,
		onSuccess: async () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			await queryClient.invalidateQueries({ queryKey: ["editions"], exact: false });
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedEditionId(undefined);
		setEditionModalVisible(true);
	};

	const handleUpdate = (row: EditionDto) => {
		setSelectedEditionId(row.id);
		setEditionModalVisible(true);
	};

	const handleDelete = (row: EditionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpSaas.EditionDeletionConfirmationMessage", { 0: row.displayName }),
			onOk: async () => {
				await deleteEdition(row.id);
			},
		});
	};

	const handleMenuClick = (key: string, row: EditionDto) => {
		if (key === "features") {
			setSelectedEditionId(row.id);
			setSelectedEditionName(row.displayName);
			setFeatureModalVisible(true);
		} else if (key === "entity-changes") {
			setSelectedEditionId(row.id);
			setSelectedEditionName(row.displayName);
			setEntityChangeDrawerVisible(true);
		}
	};

	const columns: ProColumns<EditionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpSaas.DisplayName:EditionName"),
			dataIndex: "displayName",
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 220,
			hideInTable: !hasAccessByCodes([EditionsPermissions.Update, EditionsPermissions.Delete]),
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[EditionsPermissions.Update],
					)}
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[EditionsPermissions.Delete],
					)}

					<Dropdown
						menu={{
							items: [
								featureChecker.isEnabled("AbpAuditing.Logging.AuditLog") &&
								hasAccessByCodes([AuditLogPermissions.Default])
									? {
											key: "entity-changes",
											label: $t("AbpAuditLogging.EntitiesChanged"),
											icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
										}
									: null,
								hasAccessByCodes([EditionsPermissions.ManageFeatures])
									? {
											key: "features",
											label: $t("AbpSaas.ManageFeatures"),
											icon: <Iconify icon="pajamas:feature-flag" />,
										}
									: null,
							].filter(Boolean) as any,
							onClick: ({ key }) => handleMenuClick(key, record),
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<EditionDto>
					headerTitle={$t("AbpSaas.Editions")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={{
						labelWidth: "auto",
					}}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AbpSaas.NewEdition")}
							</Button>,
							[EditionsPermissions.Create],
						),
					]}
					request={async (params, sorter) => {
						const { current, pageSize, ...filters } = params;
						const sorting =
							sorter && Object.keys(sorter).length > 0
								? Object.keys(sorter)
										.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
										.join(", ")
								: undefined;

						const query = await queryClient.fetchQuery({
							queryKey: ["editions", params, sorter],
							queryFn: () =>
								getPagedListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									sorting: sorting,
									filter: filters.filter,
								}),
						});

						return {
							data: query.items,
							total: query.totalCount,
							success: true,
						};
					}}
					pagination={{
						defaultPageSize: 10,
						showSizeChanger: true,
					}}
				/>
			</Card>
			<EditionModal
				visible={editionModalVisible}
				editionId={selectedEditionId}
				onClose={() => setEditionModalVisible(false)}
				onChange={async () => {
					await queryClient.invalidateQueries({ queryKey: ["editions"], exact: false });
					actionRef.current?.reload();
				}}
			/>

			<FeatureModal
				visible={featureModalVisible}
				providerName="E"
				providerKey={selectedEditionId}
				displayName={selectedEditionName}
				onClose={() => setFeatureModalVisible(false)}
			/>

			<EntityChangeDrawer
				open={entityChangeDrawerVisible}
				input={{ entityId: selectedEditionId, entityTypeFullName: "LINGYUN.Abp.Saas.Edition" }}
				subject={selectedEditionName}
				onClose={() => setEntityChangeDrawerVisible(false)}
			/>
		</>
	);
};

export default EditionTable;
