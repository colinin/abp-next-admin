import { useCallback, useRef, useState } from "react";
import { Button, Card, Dropdown, Modal } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined, EllipsisOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { FeatureGroupDefinitionDto } from "#/management/features/groups";
import { type ActionType, ProTable, type ProColumns } from "@ant-design/pro-table";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { deleteApi, getListApi } from "@/api/management/features/feature-group-definitions";
import {
	FeatureDefinitionsPermissions,
	GroupDefinitionsPermissions,
} from "@/constants/management/features/permissions";
import FeatureGroupDefinitionModal from "./feature-group-definition-modal";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { toast } from "sonner";
import { Iconify } from "@/components/icon";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import FeatureDefinitionModal from "../definitions-features/feature-definition-modal";

const FeatureGroupDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const { deserialize } = localizationSerializer();

	// Modal states
	const [groupModalVisible, setGroupModalVisible] = useState(false);
	const [featureModalVisible, setFeatureModalVisible] = useState(false);
	const [selectedGroup, setSelectedGroup] = useState<FeatureGroupDefinitionDto>();
	const [selectedGroupForFeature, setSelectedGroupForFeature] = useState<string>();

	const { Lr } = useLocalizer(
		undefined,
		useCallback(() => {
			actionRef.current?.reload();
			setFeatureModalVisible(false);
			setGroupModalVisible(false);
		}, []),
	);

	const [searchParams, setSearchParams] = useState<{ filter?: string }>({});

	// Fetch List
	const { data, isLoading } = useQuery({
		queryKey: ["featureGroups", searchParams],
		queryFn: async () => {
			const { items } = await getListApi(searchParams);
			return items.map((item) => {
				const localizableString = deserialize(item.displayName);
				return {
					...item,
					displayName: Lr(localizableString.resourceName, localizableString.name),
				};
			});
		},
	});

	// Delete Group
	const { mutateAsync: deleteGroup } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["featureGroups"] });
		},
	});

	const handleCreate = () => {
		setSelectedGroup(undefined);
		setGroupModalVisible(true);
	};

	const handleUpdate = (group: FeatureGroupDefinitionDto) => {
		setSelectedGroup(group);
		setGroupModalVisible(true);
	};

	const handleDelete = (group: FeatureGroupDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", {
				0: group.name,
			}),
			onOk: () => deleteGroup(group.name),
		});
	};

	const handleMenuClick = (key: string, group: FeatureGroupDefinitionDto) => {
		if (key === "features") {
			setSelectedGroupForFeature(group.name);
			setFeatureModalVisible(true);
		}
	};

	const columns: ProColumns<FeatureGroupDefinitionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			valueType: "text",
			hideInTable: true,
		},
		{
			title: $t("AbpFeatureManagement.DisplayName:Name"),
			dataIndex: "name",
			width: "auto",
			hideInSearch: true,
			sorter: true,
		},
		{
			title: $t("AbpFeatureManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: "auto",
			hideInSearch: true,
			sorter: true,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 220,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<div className="flex flex-row">
					<div className={`${record.isStatic ? "w-full" : "basis-1/3"}`}>
						<Button
							type="link"
							icon={<EditOutlined />}
							block
							disabled={!hasAccessByCodes([GroupDefinitionsPermissions.Update])}
							onClick={() => handleUpdate(record)}
						>
							{$t("AbpUi.Edit")}
						</Button>
					</div>
					{!record.isStatic && (
						<>
							<div className="basis-1/3">
								<Button
									type="link"
									danger
									icon={<DeleteOutlined />}
									block
									disabled={!hasAccessByCodes([GroupDefinitionsPermissions.Delete])}
									onClick={() => handleDelete(record)}
								>
									{$t("AbpUi.Delete")}
								</Button>
							</div>
							<div className="basis-1/3">
								<Dropdown
									menu={{
										items: [
											hasAccessByCodes([FeatureDefinitionsPermissions.Create])
												? {
														key: "features",
														icon: <Iconify icon="ant-design:gold-outlined" />,
														label: $t("AbpFeatureManagement.FeatureDefinitions:AddNew"),
													}
												: null,
										].filter((item) => item !== null),
										onClick: ({ key }) => handleMenuClick(key as string, record),
									}}
								>
									<Button type="link" icon={<EllipsisOutlined />} />
								</Dropdown>
							</div>
						</>
					)}
				</div>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<FeatureGroupDefinitionDto>
					headerTitle={$t("AbpFeatureManagement.GroupDefinitions")}
					actionRef={actionRef}
					columns={columns}
					dataSource={data}
					loading={isLoading}
					rowKey="name"
					pagination={{
						showSizeChanger: true,
						total: data?.length,
					}}
					search={{
						labelWidth: "auto",
						span: 12,
						defaultCollapsed: true,
					}}
					toolBarRender={() => [
						hasAccessByCodes([GroupDefinitionsPermissions.Create]) && (
							<Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("AbpFeatureManagement.GroupDefinitions:AddNew")}
							</Button>
						),
					]}
					request={async (params) => {
						const { filter } = params;
						setSearchParams({ filter });
						await queryClient.invalidateQueries({
							queryKey: ["featureGroups"],
						});
						return { data, success: true, total: data?.length };
					}}
				/>
			</Card>
			<FeatureGroupDefinitionModal
				visible={groupModalVisible}
				groupName={selectedGroup?.name}
				onClose={() => setGroupModalVisible(false)}
				onChange={() => {
					setGroupModalVisible(false);
					actionRef.current?.reload();
				}}
			/>
			<FeatureDefinitionModal
				visible={featureModalVisible}
				onClose={() => setFeatureModalVisible(false)}
				onChange={() => {
					setFeatureModalVisible(false);
					actionRef.current?.reload();
				}}
				groupName={selectedGroupForFeature}
			/>
		</>
	);
};
//TODO 添加功能-给自添加分组
export default FeatureGroupDefinitionTable;
