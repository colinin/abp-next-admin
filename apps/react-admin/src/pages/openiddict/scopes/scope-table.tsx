import { useRef, useState } from "react";
import { Button, Card, Dropdown, Modal, Space } from "antd";
import { DeleteOutlined, EditOutlined, EllipsisOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import type { OpenIddictScopeDto } from "#/openiddict/scopes";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { ScopesPermissions } from "@/constants/openiddict/permissions";
import { AuditLogPermissions } from "@/constants/management/auditing/permissions";
import { Iconify } from "@/components/icon";
import { deleteApi, getPagedListApi } from "@/api/openiddict/scopes";
import { useFeatures } from "@/hooks/abp/use-abp-feature";
import { toast } from "sonner";
import ScopeModal from "./scope-modal";
import { EntityChangeDrawer } from "@/components/abp/auditing/entity-change-drawer";

const ScopeTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const { isEnabled } = useFeatures();
	const [modal, contextHolder] = Modal.useModal();

	// Modal visibility states
	const [modalVisible, setModalVisible] = useState(false);
	const [entityChangeDrawerVisible, setEntityChangeDrawerVisible] = useState(false);

	// Selected scope state
	const [selectedScope, setSelectedScope] = useState<OpenIddictScopeDto>();

	// Delete mutation
	const { mutateAsync: deleteScope } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["openIddict.scopes"] });
			actionRef.current?.reload();
		},
	});

	const handleMenuClick = (key: string, scope: OpenIddictScopeDto) => {
		setSelectedScope(scope);
		switch (key) {
			case "entity-changes":
				setEntityChangeDrawerVisible(true);
				break;
		}
	};

	const handleDelete = (scope: OpenIddictScopeDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: scope.name }),
			onOk: () => deleteScope(scope.id),
		});
	};

	const columns: ProColumns<OpenIddictScopeDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			hideInTable: true,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:Name"),
			dataIndex: "name",
			width: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:Description"),
			dataIndex: "description",
			width: 200,
			hideInSearch: true,
			ellipsis: true,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 220,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<Space split>
					{hasAccessByCodes([ScopesPermissions.Update]) && (
						<Button
							type="link"
							icon={<EditOutlined />}
							onClick={() => {
								setSelectedScope(record);
								setModalVisible(true);
							}}
						>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{hasAccessByCodes([ScopesPermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
					{isEnabled("AbpAuditing.Logging.AuditLog") && hasAccessByCodes([AuditLogPermissions.Default]) && (
						<Dropdown
							menu={{
								items: [
									{
										key: "entity-changes",
										icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
										label: $t("AbpAuditLogging.EntitiesChanged"),
									},
								],
								onClick: ({ key }) => handleMenuClick(key, record),
							}}
						>
							<Button type="link" icon={<EllipsisOutlined />} />
						</Dropdown>
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<OpenIddictScopeDto>
					headerTitle={$t("AbpOpenIddict.Scopes")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					request={async (params) => {
						const { current, pageSize, filter } = params;
						const query = await queryClient.fetchQuery({
							queryKey: ["openIddict.scopes", params],
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
					scroll={{ x: "max-content" }}
					toolBarRender={() => [
						hasAccessByCodes([ScopesPermissions.Create]) && (
							<Button
								type="primary"
								icon={<PlusOutlined />}
								onClick={() => {
									setSelectedScope(undefined);
									setModalVisible(true);
								}}
							>
								{$t("AbpOpenIddict.Scopes:AddNew")}
							</Button>
						),
					]}
				/>
			</Card>

			{/* Modals */}
			<ScopeModal
				visible={modalVisible}
				scopeId={selectedScope?.id}
				onClose={() => {
					setModalVisible(false);
					setSelectedScope(undefined);
				}}
				onChange={() => actionRef.current?.reload()}
			/>

			<EntityChangeDrawer
				open={entityChangeDrawerVisible}
				subject={selectedScope?.name}
				input={{
					entityId: selectedScope?.id,
					entityTypeFullName: "Volo.Abp.OpenIddict.Scopes.OpenIddictScope",
				}}
				onClose={() => {
					setEntityChangeDrawerVisible(false);
					setSelectedScope(undefined);
				}}
			/>
		</>
	);
};

export default ScopeTable;
