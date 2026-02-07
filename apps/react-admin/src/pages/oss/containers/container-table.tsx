import { useRef, useState } from "react";
import { Button, Modal, Space } from "antd";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useQueryClient, useMutation } from "@tanstack/react-query";
import { formatToDateTime } from "@/utils/abp";
import { deleteApi, getListApi } from "@/api/oss/containes";
import type { OssContainerDto } from "#/oss/containes";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";
import ContainerModal from "./container-modal";
import Card from "@/components/card";

const ContainerTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	const [createModalVisible, setCreateModalVisible] = useState(false);

	const { mutateAsync: deleteContainer } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["oss-containers"], exact: false });
		},
	});

	const handleCreate = () => {
		setCreateModalVisible(true);
	};

	const handleDelete = (row: OssContainerDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: async () => {
				await deleteContainer(row.name);
			},
		});
	};

	const columns: ProColumns<OssContainerDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpOssManagement.DisplayName:Name"),
			dataIndex: "name",
			sorter: true,
			minWidth: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpOssManagement.DisplayName:CreationDate"),
			dataIndex: "creationDate",
			sorter: true,
			minWidth: 150,
			hideInSearch: true,
			render: (_, row) => formatToDateTime(row.creationDate),
		},
		{
			title: $t("AbpOssManagement.DisplayName:LastModifiedDate"),
			dataIndex: "lastModifiedDate",
			sorter: true,
			minWidth: 150,
			hideInSearch: true,
			render: (_, row) => formatToDateTime(row.lastModifiedDate),
		},
		{
			title: $t("AbpOssManagement.DisplayName:Size"),
			dataIndex: "size",
			sorter: true,
			minWidth: 100,
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 150,
			render: (_, record) => (
				<Space>
					<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
						{$t("AbpUi.Delete")}
					</Button>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<OssContainerDto>
					headerTitle={$t("AbpOssManagement.Containers")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={{
						labelWidth: "auto",
					}}
					toolBarRender={() => [
						<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
							{$t("AbpOssManagement.Containers:Create")}
						</Button>,
					]}
					request={async (params, sorter) => {
						const { current, pageSize, filter } = params;
						const sorting =
							sorter && Object.keys(sorter).length > 0
								? Object.keys(sorter)
										.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
										.join(", ")
								: undefined;
						// that just how this api is.
						const query = await queryClient.fetchQuery({
							queryKey: ["oss-containers", params, sorter],
							queryFn: () =>
								getListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									sorting: sorting,
									prefix: filter, //?
								}),
						});

						return {
							data: query.containers,
							total: query.maxKeys,
							success: true,
						};
					}}
					pagination={{
						defaultPageSize: 10,
						showSizeChanger: true,
					}}
				/>
			</Card>
			<ContainerModal
				visible={createModalVisible}
				onClose={() => setCreateModalVisible(false)}
				onChange={() => queryClient.invalidateQueries({ queryKey: ["oss-containers"], exact: false })}
			/>
		</>
	);
};

export default ContainerTable;
