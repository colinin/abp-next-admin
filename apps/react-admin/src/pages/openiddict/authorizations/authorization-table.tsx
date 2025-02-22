import { useRef, useState, useTransition } from "react";
import { Button, Card, Modal, Select, Space } from "antd";
import { EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import type { OpenIddictAuthorizationDto } from "#/openiddict/authorizations";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { formatToDateTime } from "@/utils/abp";
import { getPagedListApi as getApplications } from "@/api/openiddict/applications";
import { deleteApi, getPagedListApi } from "@/api/openiddict/authorizations";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { AuthorizationsPermissions } from "@/constants/openiddict/permissions";
import AuthorizationModal from "./authorization-modal";
import { toast } from "sonner";

const AuthorizationTable = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	// Modal states
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedAuth, setSelectedAuth] = useState<OpenIddictAuthorizationDto>();

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
	const { mutateAsync: deleteAuthorization } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["authorizations"] });
			actionRef.current?.reload();
		},
	});

	const handleDelete = (auth: OpenIddictAuthorizationDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: () => deleteAuthorization(auth.id),
		});
	};

	const columns: ProColumns<OpenIddictAuthorizationDto>[] = [
		{
			title: $t("AbpOpenIddict.DisplayName:ApplicationId"),
			dataIndex: "applicationId",
			width: 300,
			ellipsis: true,
			renderFormItem: () => (
				<Select
					showSearch
					allowClear
					filterOption={false} // 禁用本地过滤
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
			title: $t("AbpOpenIddict.DisplayName:Subject"),
			dataIndex: "subject",
			ellipsis: true,
			width: 300,
		},
		{
			title: $t("AbpOpenIddict.DisplayName:Type"),
			dataIndex: "type",
			hideInSearch: true,
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
		hasAccessByCodes([AuthorizationsPermissions.Delete])
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
									setSelectedAuth(record);
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
					<ProTable<OpenIddictAuthorizationDto>
						headerTitle={$t("AbpOpenIddict.Authorizations")}
						actionRef={actionRef}
						rowKey="id"
						columns={columns}
						request={async (params, sorter) => {
							const { current, pageSize, creationDate, ...filters } = params;
							const [startTime, endTime] = creationDate || [];
							const query = await queryClient.fetchQuery({
								queryKey: ["authorizations", params, sorter],
								queryFn: () =>
									getPagedListApi({
										maxResultCount: pageSize,
										skipCount: ((current || 1) - 1) * (pageSize || 0),
										beginCreationTime: startTime,
										endCreationTime: endTime,
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
							span: 6,
							labelWidth: "auto",
						}}
						scroll={{ x: "max-content" }}
					/>
				</Card>
			</Space>
			<AuthorizationModal
				visible={modalVisible}
				authorizationId={selectedAuth?.id}
				onClose={() => {
					setModalVisible(false);
					setSelectedAuth(undefined);
				}}
			/>
		</>
	);
};

export default AuthorizationTable;
