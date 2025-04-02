import { useRef, useState, useCallback } from "react";
import { Button, Card, Modal, Space } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { ProTable, type ActionType, type ProColumns } from "@ant-design/pro-table";
import type { SettingDefinitionDto } from "#/management/settings/definitions";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getListApi } from "@/api/management/settings/definitions";
import { SettingDefinitionsPermissions } from "@/constants/management/settings/permissions";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import SettingDefinitionModal from "./setting-definition-modal";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { toast } from "sonner";

const SettingDefinitionTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedSetting, setSelectedSetting] = useState<SettingDefinitionDto>();

	const { Lr } = useLocalizer(
		undefined,
		useCallback(() => {
			actionRef.current?.reload();
			setModalVisible(false);
		}, []),
	);

	const { deserialize } = localizationSerializer();

	// Delete mutation
	const { mutateAsync: deleteSetting } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["settings"] });
			actionRef.current?.reload();
		},
	});

	const handleDelete = (setting: SettingDefinitionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: setting.name }),
			onOk: () => deleteSetting(setting.name),
		});
	};

	const columns: ProColumns<SettingDefinitionDto>[] = [
		{
			title: $t("AbpUi.Search"),
			dataIndex: "filter",
			hideInTable: true,
		},
		{
			title: $t("AbpSettingManagement.DisplayName:Name"),
			dataIndex: "name",
			width: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpSettingManagement.DisplayName:DisplayName"),
			dataIndex: "displayName",
			width: 150,
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			width: 180,
			fixed: "right",
			hideInSearch: true,
			render: (_, record) => (
				<Space split>
					{hasAccessByCodes([SettingDefinitionsPermissions.Update]) && (
						<Button
							type="link"
							icon={<EditOutlined />}
							onClick={() => {
								setSelectedSetting(record);
								setModalVisible(true);
							}}
						>
							{$t("AbpUi.Edit")}
						</Button>
					)}
					{!record.isStatic && hasAccessByCodes([SettingDefinitionsPermissions.DeleteOrRestore]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<SettingDefinitionDto>
					headerTitle={$t("AbpSettingManagement.Settings")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					request={async (params) => {
						const { filter } = params;
						const query = await queryClient.fetchQuery({
							queryKey: ["settings", params],
							queryFn: async () => {
								const res = await getListApi({ filter });
								return res.items.map((item) => {
									const localizableString = deserialize(item.displayName);
									return {
										...item,
										displayName: Lr(localizableString.resourceName, localizableString.name),
									};
								});
							},
						});

						return {
							data: query,
							success: true,
							total: query.length,
						};
					}}
					search={{
						labelWidth: "auto",
						defaultCollapsed: false,
					}}
					toolBarRender={() => [
						hasAccessByCodes([SettingDefinitionsPermissions.Create]) && (
							<Button
								type="primary"
								icon={<PlusOutlined />}
								onClick={() => {
									setSelectedSetting(undefined);
									setModalVisible(true);
								}}
							>
								{$t("AbpSettingManagement.Definition:AddNew")}
							</Button>
						),
					]}
				/>
			</Card>
			<SettingDefinitionModal
				visible={modalVisible}
				settingName={selectedSetting?.name}
				onClose={() => {
					setModalVisible(false);
					setSelectedSetting(undefined);
				}}
				onChange={() => actionRef.current?.reload()}
			/>
		</>
	);
};

export default SettingDefinitionTable;
