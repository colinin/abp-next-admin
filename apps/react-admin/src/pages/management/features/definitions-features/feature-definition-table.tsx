import { useCallback, useRef, useState } from "react";
import { Button, Card, Table, Modal } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { FeatureDefinitionDto } from "#/management/features/definitions";
import type { FeatureGroupDefinitionDto } from "#/management/features/groups";
import { type ActionType, ProTable, type ProColumns } from "@ant-design/pro-table";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { deleteApi, getListApi as getFeaturesApi } from "@/api/management/features/feature-definitions";
import { getListApi as getGroupsApi } from "@/api/management/features/feature-group-definitions";
import { GroupDefinitionsPermissions } from "@/constants/management/permissions";
import FeatureDefinitionModal from "./feature-definition-modal";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { toast } from "sonner";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { listToTree } from "@/utils/tree";

interface FeatureGroupVo extends FeatureGroupDefinitionDto {
	features: FeatureDefinitionDto[];
}

const FeatureDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const { deserialize } = localizationSerializer();

	// Modal States
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedFeature, setSelectedFeature] = useState<FeatureDefinitionDto>();

	const { Lr } = useLocalizer(
		undefined,
		useCallback(() => {
			actionRef.current?.reload();
			setModalVisible(false);
		}, []),
	);

	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	// Fetch Logic: Combined Groups + Features
	const { data, isLoading } = useQuery({
		queryKey: ["featureDefinitions", searchParams],
		queryFn: async () => {
			// Fetch both
			const [groupRes, featureRes] = await Promise.all([getGroupsApi(searchParams), getFeaturesApi(searchParams)]);

			// Map and Nest
			const mappedGroups: FeatureGroupVo[] = groupRes.items.map((group) => {
				const localizableGroup = deserialize(group.displayName);

				// Find features belonging to this group
				const groupFeatures = featureRes.items
					.filter((f) => f.groupName === group.name)
					.map((f) => {
						const fDisplay = deserialize(f.displayName);
						const fDesc = deserialize(f.description);
						return {
							...f,
							displayName: Lr(fDisplay.resourceName, fDisplay.name),
							description: Lr(fDesc.resourceName, fDesc.name),
						};
					});

				return {
					...group,
					displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
					features: listToTree(groupFeatures, { id: "name", pid: "parentName" }),
				};
			});

			return mappedGroups;
		},
	});

	// Delete Feature
	const { mutateAsync: deleteFeature } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["featureDefinitions"] });
		},
	});

	// Handlers
	const handleCreate = () => {
		setSelectedFeature(undefined);
		setModalVisible(true);
	};

	const handleUpdate = (feature: FeatureDefinitionDto) => {
		setSelectedFeature(feature);
		setModalVisible(true);
	};

	const handleDelete = (feature: FeatureDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: feature.name }),
			onOk: () => deleteFeature(feature.name),
		});
	};

	// Outer Table Columns (Groups)
	const groupColumns: ProColumns<FeatureGroupVo>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpFeatureManagement.DisplayName:Name"),
			dataIndex: "name",
			hideInSearch: true,
		},
		{
			title: $t("AbpFeatureManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			hideInSearch: true,
		},
	];

	// Inner Table Columns (Features)
	const expandedRowRender = (groupRecord: FeatureGroupVo) => {
		const featureColumns: any[] = [
			{
				title: $t("AbpFeatureManagement.DisplayName:Name"),
				dataIndex: "name",
				width: 200,
			},
			{
				title: $t("AbpFeatureManagement.DisplayName:DisplayName"),
				dataIndex: "displayName",
				width: 200,
			},
			{
				title: $t("AbpFeatureManagement.DisplayName:Description"),
				dataIndex: "description",
			},
			{
				title: $t("AbpFeatureManagement.DisplayName:IsVisibleToClients"),
				dataIndex: "isVisibleToClients",
				width: 150,
				align: "center",
				render: (val: boolean) =>
					val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
			},
			{
				title: $t("AbpFeatureManagement.DisplayName:IsAvailableToHost"),
				dataIndex: "isAvailableToHost",
				width: 150,
				align: "center",
				render: (val: boolean) =>
					val ? <CheckOutlined className="text-green-500" /> : <CloseOutlined className="text-red-500" />,
			},
			{
				title: $t("AbpUi.Actions"),
				width: 180,
				fixed: "right",
				render: (_: any, record: FeatureDefinitionDto) => (
					<div className="flex flex-row gap-2">
						<Button
							type="link"
							size="small"
							icon={<EditOutlined />}
							disabled={!hasAccessByCodes([GroupDefinitionsPermissions.Update])}
							onClick={() => handleUpdate(record)}
						>
							{$t("AbpUi.Edit")}
						</Button>
						{!record.isStatic && (
							<Button
								type="link"
								size="small"
								danger
								icon={<DeleteOutlined />}
								disabled={!hasAccessByCodes([GroupDefinitionsPermissions.Delete])}
								onClick={() => handleDelete(record)}
							>
								{$t("AbpUi.Delete")}
							</Button>
						)}
					</div>
				),
			},
		];

		return (
			<Table
				columns={featureColumns}
				dataSource={groupRecord.features}
				pagination={false}
				rowKey="name"
				// The tree logic is handled by 'children' property in data, Table detects it automatically
			/>
		);
	};

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<FeatureGroupVo>
					headerTitle={$t("AbpFeatureManagement.FeatureDefinitions")}
					actionRef={actionRef}
					columns={groupColumns}
					dataSource={data}
					loading={isLoading}
					rowKey="name"
					pagination={{
						showSizeChanger: true,
						total: data?.length,
					}}
					scroll={{ x: "max-content" }}
					search={{
						labelWidth: "auto",
						span: 12,
						defaultCollapsed: true,
					}}
					toolBarRender={() => [
						hasAccessByCodes([GroupDefinitionsPermissions.Create]) && (
							<Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AbpFeatureManagement.FeatureDefinitions:AddNew")}
							</Button>
						),
					]}
					request={async (params) => {
						const { filter } = params;
						setSearchParams({ filter });
						// Invalidate to trigger useQuery refetch
						await queryClient.invalidateQueries({ queryKey: ["featureDefinitions"] });
						return { data, success: true, total: data?.length };
					}}
					expandable={{
						expandedRowRender,
						defaultExpandAllRows: false,
					}}
				/>
			</Card>

			<FeatureDefinitionModal
				visible={modalVisible}
				featureName={selectedFeature?.name}
				onClose={() => setModalVisible(false)}
				onChange={() => {
					setModalVisible(false);
					actionRef.current?.reload();
				}}
			/>
		</>
	);
};

export default FeatureDefinitionTable;
