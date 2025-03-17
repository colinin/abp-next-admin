import { useRef, useState, useTransition } from "react";
import { Button, Card, Modal, Select, Space } from "antd";
import { EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import type { OpenIddictTokenDto } from "#/openiddict/tokens";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { formatToDateTime } from "@/utils/abp";
import { getPagedListApi as getApplications } from "@/api/openiddict/applications";
import { deleteApi, getPagedListApi } from "@/api/openiddict/tokens";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { TokensPermissions } from "@/constants/openiddict/permissions";
import TokenModal from "./token-modal";
import { toast } from "sonner";

const TokenTable = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	// Modal states
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedToken, setSelectedToken] = useState<OpenIddictTokenDto>();

	const [searchText, setSearchText] = useState("");
	const [isPending, startTransition] = useTransition();

	const { data: applications = [] } = useQuery({
		queryKey: ["applications", searchText],
		queryFn: () =>
			getApplications({
				maxResultCount: 25,
				filter: searchText,
			}).then((res) => res.items),
		enabled: !isPending,
	});

	// Delete mutation
	const { mutateAsync: deleteToken } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["tokens"] });
			actionRef.current?.reload();
		},
	});

	const handleDelete = (token: OpenIddictTokenDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: () => deleteToken(token.id),
		});
	};

	const columns: ProColumns<OpenIddictTokenDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ClientId"),
			dataIndex: "clientId",
			valueType: "text",
			hideInTable: true,
			renderFormItem: () => (
				<Select
					showSearch
					allowClear
					filterOption={false}
					options={applications.map((app) => ({
						label: app.clientId,
						value: app.id,
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
			title: $t("AbpOpenIddict.DisplayName:ApplicationId"),
			dataIndex: "applicationId",
			width: 300,
			ellipsis: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:Subject"),
			dataIndex: "subject",
			ellipsis: true,
			width: 300,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:Type"),
			dataIndex: "type",
			width: 150,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:Status"),
			dataIndex: "status",
			width: 150,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:CreationDate"),
			dataIndex: "creationDate",
			render: (_, record) => (record.creationDate ? formatToDateTime(record.creationDate) : "-"),
			width: 200,
			valueType: "dateRange",
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ExpirationDate"),
			dataIndex: "expirationDate",
			render: (_, record) => (record.expirationDate ? formatToDateTime(record.expirationDate) : "-"),
			width: 200,
			valueType: "dateRange",
		},
		{
			title: $t("AbpOpenIddict.DisplayName:ReferenceId"),
			dataIndex: "referenceId",
			valueType: "text",
			hideInTable: true,
		},

		hasAccessByCodes([TokensPermissions.Delete])
			? {
					title: $t("AbpUi.Actions"),
					width: 220,
					fixed: "right",
					hideInSearch: true,
					render: (_, record) => (
						<Space>
							<Button
								type="link"
								icon={<EditOutlined />}
								onClick={() => {
									setSelectedToken(record);
									setModalVisible(true);
								}}
							>
								{$t("AbpUi.Edit")}
							</Button>
							<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
								{$t("AbpUi.Delete")}
							</Button>
						</Space>
					),
				}
			: {},
	];

	return (
		<>
			{contextHolder}
			<Space direction="vertical" size="large" className="w-full">
				<Card>
					<ProTable<OpenIddictTokenDto>
						headerTitle={$t("AbpOpenIddict.Tokens")}
						actionRef={actionRef}
						rowKey="id"
						columns={columns}
						request={async (params, sorter) => {
							const { current, pageSize, creationDate, expirationDate, filter, ...filters } = params;
							const [startCreationTime, endCreationTime] = creationDate || [];
							const [startExpirationTime, endExpirationTime] = expirationDate || [];
							const query = await queryClient.fetchQuery({
								queryKey: ["tokens", params, sorter],
								queryFn: () =>
									getPagedListApi({
										maxResultCount: pageSize,
										skipCount: ((current || 1) - 1) * (pageSize || 0),
										beginCreationTime: startCreationTime,
										endCreationTime: endCreationTime,
										beginExpirationDate: startExpirationTime,
										endExpirationDate: endExpirationTime,
										filter: filter,
										...filters,
									}),
							});

							return {
								data: query.items,
								success: true,
								total: query.totalCount,
							};
						}}
						pagination={{
							showSizeChanger: true,
						}}
						search={{
							defaultCollapsed: true,
							labelWidth: "auto",
						}}
						scroll={{ x: "max-content" }}
					/>
				</Card>
			</Space>
			<TokenModal
				visible={modalVisible}
				tokenId={selectedToken?.id}
				onClose={() => {
					setModalVisible(false);
					setSelectedToken(undefined);
				}}
			/>
		</>
	);
};

export default TokenTable;
