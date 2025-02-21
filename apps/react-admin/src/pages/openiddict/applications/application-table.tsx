import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space } from "antd";
import { DeleteOutlined, EditOutlined, EllipsisOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import type { OpenIddictApplicationDto } from "#/openiddict/applications";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { ApplicationsPermissions } from "@/constants/openiddict/permissions";
import { AuditLogPermissions } from "@/constants/management/auditing/permissions";
import { Iconify } from "@/components/icon";
import { deleteApi, getPagedListApi } from "@/api/openiddict/applications";
import { useFeatures } from "@/hooks/abp/use-abp-feature";
import { toast } from "sonner";
import ApplicationModal from "./application-modal";
import ApplicationSecretModal from "./application-secret-modal";
import PermissionModal from "@/components/abp/permissions/permission-modal";
import { EntityChangeDrawer } from "@/components/abp/auditing/entity-change-drawer";

const ApplicationTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const { isEnabled } = useFeatures();
	const [modal, contextHolder] = Modal.useModal();

	// Modal visibility states
	const [modalVisible, setModalVisible] = useState(false);
	const [secretModalVisible, setSecretModalVisible] = useState(false);
	const [permissionModalVisible, setPermissionModalVisible] = useState(false);
	const [entityChangeDrawerVisible, setEntityChangeDrawerVisible] = useState(false);

	// Selected application state
	const [selectedApp, setSelectedApp] = useState<OpenIddictApplicationDto>();

	// Delete mutation
	const { mutateAsync: deleteApplication } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const handleMenuClick = (key: string, app: OpenIddictApplicationDto) => {
		setSelectedApp(app);
		switch (key) {
			case "secret":
				setSecretModalVisible(true);
				break;
			case "permissions":
				setPermissionModalVisible(true);
				break;
			case "entity-changes":
				setEntityChangeDrawerVisible(true);
				break;
		}
	};

	const handleDelete = (app: OpenIddictApplicationDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: app.clientId }),
			onOk: () => deleteApplication(app.id),
		});
	};

	const columns: ProColumns<OpenIddictApplicationDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			hideInTable: true,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ClientId"),
			dataIndex: "clientId",
			width: 150,
			align: "left",
			hideInSearch: true,
			minWidth: 150,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
			ellipsis: true,
			align: "left",
			hideInSearch: true,
			minWidth: 150,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ConsentType"),
			dataIndex: "consentType",
			width: 200,
			hideInSearch: true,
			align: "center",
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ClientType"),
			dataIndex: "clientType",
			width: 120,
			hideInSearch: true,
			align: "center",
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ApplicationType"),
			dataIndex: "applicationType",
			width: 150,
			hideInSearch: true,
			align: "center",
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ClientUri"),
			dataIndex: "clientUri",
			width: 150,
			hideInSearch: true,
			align: "left",
		},
		{
			title: $t("AbpOpenIddict.DisplayName:LogoUri"),
			dataIndex: "logoUri",
			width: 120,
			hideInSearch: true,
			align: "left",
		},
		{
			title: $t("AbpUi.Actions"),
			width: 220,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<Space split>
					{hasAccessByCodes([ApplicationsPermissions.Update]) && (
						<Button
							type="link"
							icon={<EditOutlined />}
							onClick={() => {
								setSelectedApp(record);
								setModalVisible(true);
							}}
						>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{hasAccessByCodes([ApplicationsPermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
					<Dropdown
						menu={{
							items: getDropdownMenuItems(record),
							onClick: ({ key }) => handleMenuClick(key, record),
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	const getDropdownMenuItems = (record: OpenIddictApplicationDto) => {
		const items = [];

		if (record.clientType === "confidential" && hasAccessByCodes([ApplicationsPermissions.ManageSecret])) {
			items.push({
				key: "secret",
				icon: <Iconify icon="codicon:gist-secret" />,
				label: $t("AbpOpenIddict.GenerateSecret"),
			});
		}

		if (hasAccessByCodes([ApplicationsPermissions.ManagePermissions])) {
			items.push({
				key: "permissions",
				icon: <Iconify icon="icon-park-outline:permissions" />,
				label: $t("AbpOpenIddict.ManagePermissions"),
			});
		}

		if (isEnabled("AbpAuditing.Logging.AuditLog") && hasAccessByCodes([AuditLogPermissions.Default])) {
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
				<ProTable<OpenIddictApplicationDto>
					headerTitle={$t("AbpOpenIddict.Applications")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					scroll={{ x: "max-content" }}
					request={async (params) => {
						const { current, pageSize, filter } = params;
						const query = await queryClient.fetchQuery({
							queryKey: ["openIddict.applications", params],
							queryFn: () =>
								getPagedListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									filter: filter,
								}),
						});
						return {
							data: query.items,
							success: true,
							total: query.totalCount,
						};
					}}
					search={{
						span: 12,
						labelWidth: "auto",
					}}
					toolBarRender={() => [
						hasAccessByCodes([ApplicationsPermissions.Create]) && (
							<Button
								type="primary"
								icon={<PlusOutlined />}
								onClick={() => {
									setSelectedApp(undefined);
									setModalVisible(true);
								}}
							>
								{$t("AbpOpenIddict.Applications:AddNew")}
							</Button>
						),
					]}
				/>
			</Card>
			{/* Modals */}
			<ApplicationModal
				visible={modalVisible}
				applicationId={selectedApp?.id}
				onClose={() => {
					setModalVisible(false);
					setSelectedApp(undefined);
				}}
				onChange={() => actionRef.current?.reload()}
			/>

			{selectedApp && (
				<ApplicationSecretModal
					visible={secretModalVisible}
					applicationId={selectedApp.id}
					clientId={selectedApp.clientId}
					onClose={() => {
						setSecretModalVisible(false);
						setSelectedApp(undefined);
					}}
					onChange={() => actionRef.current?.reload()}
				/>
			)}

			<PermissionModal
				visible={permissionModalVisible}
				providerName="C"
				providerKey={selectedApp?.clientId}
				displayName={selectedApp?.clientId}
				onClose={() => {
					setPermissionModalVisible(false);
					setSelectedApp(undefined);
				}}
			/>

			<EntityChangeDrawer
				open={entityChangeDrawerVisible}
				subject={selectedApp?.clientId}
				input={{
					entityId: selectedApp?.id,
					entityTypeFullName: "Volo.Abp.OpenIddict.Applications.OpenIddictApplication",
				}}
				onClose={() => {
					setEntityChangeDrawerVisible(false);
					setSelectedApp(undefined);
				}}
			/>
		</>
	);
};

export default ApplicationTable;
