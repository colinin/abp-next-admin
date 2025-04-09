import { useRef, useState, useTransition } from "react";
import { Button, Card, Modal, Select, Space, Tag } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import type { IdentitySessionDto } from "#/management/identity/sessions";
import type { IdentityUserDto } from "#/management/identity/user";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { IdentitySessionPermissions } from "@/constants/management/identity/permissions";
import { getPagedListApi as getUserListApi } from "@/api/management/identity/users";
import { getSessionsApi, revokeSessionApi } from "@/api/management/identity/user-sessions";
import { useApplication } from "@/store/abpCoreStore";
import { toast } from "sonner";

const SessionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const application = useApplication();

	const [searchText, setSearchText] = useState("");
	const [isPending, startTransition] = useTransition();

	// Fetch users for select
	const { data: users = [] } = useQuery({
		queryKey: ["users", searchText],
		queryFn: () =>
			getUserListApi({
				maxResultCount: 25,
				filter: searchText,
			}).then((res) => res.items),
		enabled: !isPending,
	});

	// Revoke session mutation
	const { mutateAsync: revokeSession } = useMutation({
		mutationFn: revokeSessionApi,
		onSuccess: () => {
			toast.success($t("AbpIdentity.SuccessfullyRevoked"));
			actionRef.current?.reload();
		},
	});

	const handleDelete = (session: IdentitySessionDto) => {
		if (session.sessionId === application?.currentUser.sessionId) return;

		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpIdentity.SessionWillBeRevokedMessage"),
			onOk: () => revokeSession(session.sessionId),
		});
	};

	const columns: ProColumns<IdentitySessionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			hideInTable: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:UserName"),
			dataIndex: "userId",
			hideInTable: true,
			renderFormItem: () => (
				<Select
					showSearch
					allowClear
					filterOption={false}
					options={users.map((user: IdentityUserDto) => ({
						label: user.userName,
						value: user.id,
					}))}
					onSearch={(value) => {
						startTransition(() => {
							setSearchText(value);
						});
					}}
					placeholder={$t("ui.placeholder.select")}
				/>
			),
		},
		{
			align: "left",
			dataIndex: "sessionId",
			minWidth: 150,
			title: $t("AbpIdentity.DisplayName:SessionId"),
		},
		{
			title: $t("AbpIdentity.DisplayName:ClientId"),
			dataIndex: "clientId",
		},

		{
			title: $t("AbpIdentity.DisplayName:Device"),
			dataIndex: "device",
			render: (_, record) => (
				<Space>
					{record.device}
					{record.sessionId === application?.currentUser.sessionId && (
						<Tag color="#87d068">{$t("AbpIdentity.CurrentSession")}</Tag>
					)}
				</Space>
			),
		},
		{
			title: $t("AbpIdentity.DisplayName:DeviceInfo"),
			dataIndex: "deviceInfo",
			ellipsis: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:IpAddresses"),
			dataIndex: "ipAddresses",
			hideInSearch: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:SignedIn"),
			dataIndex: "signedIn",
			ellipsis: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpIdentity.DisplayName:LastAccessed"),
			ellipsis: true,
			dataIndex: "lastAccessed",
			hideInSearch: true,
		},
		hasAccessByCodes([IdentitySessionPermissions.Default, IdentitySessionPermissions.Revoke])
			? {
					title: $t("AbpUi.Actions"),
					width: 150,
					fixed: "right",
					hideInSearch: true,
					render: (_, record) => (
						<Space>
							{record.sessionId !== application?.currentUser.sessionId &&
								hasAccessByCodes([IdentitySessionPermissions.Revoke]) && (
									<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
										{$t("AbpIdentity.RevokeSession")}
									</Button>
								)}
						</Space>
					),
				}
			: {},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<IdentitySessionDto>
					headerTitle={$t("AbpIdentity.IdentitySessions")}
					actionRef={actionRef}
					rowKey="sessionId"
					columns={columns}
					scroll={{ x: "max-content" }}
					request={async (params) => {
						const { current, pageSize, userId, clientId, device } = params;
						const query = await queryClient.fetchQuery({
							queryKey: ["sessions", params],
							queryFn: () =>
								getSessionsApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									userId,
									clientId,
									device,
								}),
						});
						return {
							data: query.items,
							success: true,
							total: query.totalCount,
						};
					}}
					search={{
						labelWidth: "auto",
						defaultCollapsed: false,
					}}
				/>
			</Card>
		</>
	);
};

export default SessionTable;
