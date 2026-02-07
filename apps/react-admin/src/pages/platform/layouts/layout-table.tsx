import type React from "react";
import { useRef, useState } from "react";
import { Button, Space, Modal, Card } from "antd";
import { PlusOutlined, EditOutlined, DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi } from "@/api/platform/layouts";
import type { LayoutDto } from "#/platform/layouts";
import { LayoutPermissions } from "@/constants/platform/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";

import LayoutModal from "./layout-modal";

const LayoutTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	const [modalVisible, setModalVisible] = useState(false);
	const [selectedLayoutId, setSelectedLayoutId] = useState<string | undefined>();

	const { mutateAsync: deleteLayout } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["layouts"] });
			actionRef.current?.reload();
		},
	});

	const handleCreate = () => {
		setSelectedLayoutId(undefined);
		setModalVisible(true);
	};

	const handleUpdate = (row: LayoutDto) => {
		setSelectedLayoutId(row.id);
		setModalVisible(true);
	};

	const handleDelete = (row: LayoutDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: () => deleteLayout(row.id),
		});
	};

	const columns: ProColumns<LayoutDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Name"),
			dataIndex: "name",
			width: 180,
			sorter: true,
			fixed: "left",
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Path"),
			dataIndex: "path",
			width: 200,
			align: "center",
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:UIFramework"),
			dataIndex: "framework",
			width: 180,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Description"),
			dataIndex: "description",
			width: 220,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Redirect"),
			dataIndex: "redirect",
			width: 160,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 150,
			hideInTable: !hasAccessByCodes([LayoutPermissions.Update, LayoutPermissions.Delete]),
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleUpdate(record)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[LayoutPermissions.Update],
					)}
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[LayoutPermissions.Delete],
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<LayoutDto>
					headerTitle={$t("AppPlatform.DisplayName:Layout")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={{ labelWidth: "auto" }}
					toolBarRender={() => [
						withAccessChecker(
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AppPlatform.Layout:AddNew")}
							</Button>,
							[LayoutPermissions.Create],
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
							queryKey: ["layouts", params, sorter],
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
			<LayoutModal
				visible={modalVisible}
				layoutId={selectedLayoutId}
				onClose={() => setModalVisible(false)}
				onChange={async () => {
					await queryClient.invalidateQueries({ queryKey: ["layouts"] });
					actionRef.current?.reload();
				}}
			/>
		</>
	);
};

export default LayoutTable;
