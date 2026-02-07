import type React from "react";
import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space } from "antd";
import {
	EditOutlined,
	DeleteOutlined,
	PlusOutlined,
	EllipsisOutlined,
	CheckOutlined,
	CloseOutlined,
} from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi } from "@/api/saas/tenants";
import type { TenantDto } from "#/saas/tenants";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { TenantsPermissions } from "@/constants/saas/permissions"; // Adjust paths
import { Iconify } from "@/components/icon";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";

import TenantModal from "./tenant-modal";
import TenantConnectionStringsModal from "./tenant-connection-strings-modal";
import FeatureModal from "@/components/abp/features/feature-modal";
import { useFeatures } from "@/hooks/abp/fake-hooks/use-abp-feature";
import { AuditLogPermissions } from "@/constants/management/auditing";
import { EntityChangeDrawer } from "@/components/abp/auditing/entity-change-drawer";

const TenantTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const featureChecker = useFeatures();

	const [modal, contextHolder] = Modal.useModal();
	// Modal/Drawer States
	const [tenantModalVisible, setTenantModalVisible] = useState(false);
	const [connStrModalVisible, setConnStrModalVisible] = useState(false);
	const [featureModalVisible, setFeatureModalVisible] = useState(false);
	const [entityChangeVisible, setEntityChangeVisible] = useState(false);

	const [selectedTenantId, setSelectedTenantId] = useState<string | undefined>();
	const [selectedTenant, setSelectedTenant] = useState<TenantDto | undefined>();

	// Data Base Options (Reactive constant from Vue example)
	const dataBaseOptions = [
		{ label: "MySql", value: "MySql" },
		{ label: "Oracle", value: "Oracle" },
		{ label: "Postgres", value: "Postgres" },
		{ label: "Sqlite", value: "Sqlite" },
		{ label: "SqlServer", value: "SqlServer" },
	];

	const { mutateAsync: deleteTenant } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedTenantId(undefined);
		setTenantModalVisible(true);
	};

	const handleUpdate = (row: TenantDto) => {
		setSelectedTenantId(row.id);
		setTenantModalVisible(true);
	};

	const handleDelete = (row: TenantDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpSaas.TenantDeletionConfirmationMessage", { 0: row.name }),
			onOk: async () => {
				await deleteTenant(row.id);
			},
		});
	};

	const handleMenuClick = (key: string, row: TenantDto) => {
		setSelectedTenant(row);
		if (key === "connection-strings") {
			setConnStrModalVisible(true);
		} else if (key === "features") {
			setFeatureModalVisible(true);
		} else if (key === "entity-changes") {
			setEntityChangeVisible(true);
		}
	};

	const columns: ProColumns<TenantDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpSaas.DisplayName:IsActive"),
			dataIndex: "isActive",
			align: "center",
			width: 120,
			render: (val) =>
				val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
			hideInSearch: true,
		},
		{
			title: $t("AbpSaas.DisplayName:Name"),
			dataIndex: "name",
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpSaas.DisplayName:EditionName"),
			dataIndex: "editionName",
			width: 160,
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 220,
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[TenantsPermissions.Update],
					)}
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[TenantsPermissions.Delete],
					)}
					<Dropdown
						menu={{
							items: [
								hasAccessByCodes([TenantsPermissions.ManageConnectionStrings])
									? {
											key: "connection-strings",
											label: $t("AbpSaas.ConnectionStrings"),
											icon: <Iconify icon="mdi:connection" />,
										}
									: null,
								hasAccessByCodes([TenantsPermissions.ManageFeatures])
									? {
											key: "features",
											label: $t("AbpSaas.ManageFeatures"),
											icon: <Iconify icon="pajamas:feature-flag" />,
										}
									: null,
								featureChecker.isEnabled("AbpAuditing.Logging.AuditLog") &&
								hasAccessByCodes([AuditLogPermissions.Default])
									? {
											key: "entity-changes",
											label: $t("AbpAuditLogging.EntitiesChanged"),
											icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
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
				<ProTable<TenantDto>
					headerTitle={$t("AbpSaas.Tenants")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AbpSaas.NewTenant")}
							</Button>,
							[TenantsPermissions.Create],
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
							queryKey: ["tenants", params, sorter],
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
			<TenantModal
				visible={tenantModalVisible}
				tenantId={selectedTenantId}
				dataBaseOptions={dataBaseOptions}
				onClose={() => setTenantModalVisible(false)}
				onChange={() => actionRef.current?.reload()}
			/>

			<TenantConnectionStringsModal
				visible={connStrModalVisible}
				tenant={selectedTenant}
				dataBaseOptions={dataBaseOptions}
				onClose={() => setConnStrModalVisible(false)}
			/>

			{selectedTenant && (
				<>
					<FeatureModal
						visible={featureModalVisible}
						providerName="T"
						providerKey={selectedTenant.id}
						displayName={selectedTenant.name}
						onClose={() => setFeatureModalVisible(false)}
					/>
					<EntityChangeDrawer
						open={entityChangeVisible}
						input={{ entityId: selectedTenant.id, entityTypeFullName: "LINGYUN.Abp.Saas.Tenant" }}
						subject={selectedTenant.name}
						onClose={() => setEntityChangeVisible(false)}
					/>
				</>
			)}
		</>
	);
};

export default TenantTable;
