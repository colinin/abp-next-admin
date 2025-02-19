import { useEffect, useState } from "react";
import { Form, Input, Modal, Tabs, Select, Checkbox } from "antd";
import TextArea from "antd/es/input/TextArea";
import { useTranslation } from "react-i18next";
import type { SettingDefinitionDto } from "#/management/settings/definitions";
import type { PropertyInfo } from "@/components/abp/properties/types";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createApi, getApi, updateApi } from "@/api/management/settings/definitions";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";
import { toast } from "sonner";
import { CircleLoading } from "@/components/loading";
import { mergeDeepRight } from "ramda";

interface Props {
	visible: boolean;
	settingName?: string;
	onClose: () => void;
	onChange: (data: SettingDefinitionDto) => void;
}

type TabKeys = "basic" | "props";

const defaultModel = {} as SettingDefinitionDto;

const SettingDefinitionModal: React.FC<Props> = ({ visible, settingName, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const queryClient = useQueryClient();
	const [activeTab, setActiveTab] = useState<TabKeys>("basic");
	const [formModel, setFormModel] = useState<SettingDefinitionDto>(defaultModel);

	const providerOptions = [
		{ label: $t("AbpSettingManagement.Providers:Default"), value: "D" },
		{ label: $t("AbpSettingManagement.Providers:Configuration"), value: "C" },
		{ label: $t("AbpSettingManagement.Providers:Global"), value: "G" },
		{ label: $t("AbpSettingManagement.Providers:Tenant"), value: "T" },
		{ label: $t("AbpSettingManagement.Providers:User"), value: "U" },
	];
	// Fetch setting data if editing
	const { data: settingData, isLoading } = useQuery({
		queryKey: ["settingDefinition", settingName],
		queryFn: () => {
			if (!settingName) {
				return Promise.reject(new Error("settingName is undefined"));
			}
			return getApi(settingName);
		},
		enabled: !!settingName && visible,
	});

	useEffect(() => {
		if (settingData && settingData.name === settingName) {
			setFormModel(settingData);
			form.setFieldsValue(settingData);
		}
	}, [settingData]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
		}
	}, [visible]);

	// Create/Update mutations
	const { mutateAsync: createSetting } = useMutation({
		mutationFn: createApi,
		onSuccess: (data) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(data);
			onClose();
		},
	});

	const { mutateAsync: updateSetting } = useMutation({
		mutationFn: ({ name, input }: { name: string; input: SettingDefinitionDto }) => updateApi(name, input),
		onSuccess: (data) => {
			queryClient.invalidateQueries({ queryKey: ["settingDefinition", settingName] });
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(data);
			onClose();
		},
	});

	const handlePropertyChange = (prop: PropertyInfo) => {
		setFormModel((prev) => ({
			...prev,
			extraProperties: {
				...prev.extraProperties,
				[prop.key]: prop.value,
			},
		}));
	};

	const handlePropertyDelete = (prop: PropertyInfo) => {
		setFormModel((prev) => {
			const newProperties = { ...prev.extraProperties };
			delete newProperties[prop.key];
			return {
				...prev,
				extraProperties: newProperties,
			};
		});
	};

	const handleSubmit = async () => {
		if (formModel.isStatic) return;

		try {
			await form.validateFields();
			if (settingName) {
				await updateSetting({ name: settingName, input: formModel });
			} else {
				await createSetting(formModel);
			}
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={
				settingName
					? `${$t("AbpSettingManagement.Settings")} - ${formModel.name}`
					: $t("AbpSettingManagement.Definition:AddNew")
			}
			onCancel={() => {
				onClose();
				form.resetFields();
				setFormModel(defaultModel);
				form.resetFields();
			}}
			onClose={() => {
				onClose();
				form.resetFields();
				setFormModel(defaultModel);
				form.resetFields();
			}}
			onOk={handleSubmit}
			okButtonProps={{ disabled: formModel.isStatic }}
			width="50%"
			destroyOnClose
		>
			{!!settingName && visible && isLoading ? (
				<CircleLoading />
			) : (
				<Form
					form={form}
					layout="horizontal"
					labelCol={{ span: 6 }}
					wrapperCol={{ span: 18 }}
					onValuesChange={(changedValues) => {
						setFormModel((prevModel) => {
							return mergeDeepRight(prevModel, changedValues);
						});
					}}
				>
					<Tabs activeKey={activeTab} onChange={(key) => setActiveTab(key as TabKeys)}>
						<Tabs.TabPane key="basic" tab={$t("AbpSettingManagement.BasicInfo")}>
							<Form.Item label={$t("AbpSettingManagement.DisplayName:Name")} name="name" rules={[{ required: true }]}>
								<Input disabled={formModel.isStatic} />
							</Form.Item>
							<Form.Item label={$t("AbpSettingManagement.DisplayName:DefaultValue")} name="defaultValue">
								<TextArea disabled={formModel.isStatic} allowClear autoSize={{ minRows: 3 }} />
							</Form.Item>
							<Form.Item
								label={$t("AbpSettingManagement.DisplayName:DisplayName")}
								name="displayName"
								rules={[{ required: true }]}
							>
								<LocalizableInput disabled={formModel.isStatic} />
							</Form.Item>
							<Form.Item label={$t("AbpSettingManagement.DisplayName:Description")} name="description">
								<LocalizableInput disabled={formModel.isStatic} />
							</Form.Item>
							<Form.Item
								label={$t("AbpSettingManagement.DisplayName:Providers")}
								name="providers"
								extra={$t("AbpSettingManagement.Description:Providers")}
							>
								<Select disabled={formModel.isStatic} mode="multiple" allowClear options={providerOptions} />
							</Form.Item>
							<Form.Item
								label={$t("AbpSettingManagement.DisplayName:IsInherited")}
								name="isInherited"
								valuePropName="checked"
								extra={$t("AbpSettingManagement.Description:IsInherited")}
							>
								<Checkbox disabled={formModel.isStatic}>{$t("AbpSettingManagement.DisplayName:IsInherited")}</Checkbox>
							</Form.Item>
							<Form.Item
								label={$t("AbpSettingManagement.DisplayName:IsEncrypted")}
								name="IsEncrypted"
								valuePropName="checked"
								extra={$t("AbpSettingManagement.Description:IsEncrypted")}
							>
								<Checkbox disabled={formModel.isStatic}>{$t("AbpSettingManagement.DisplayName:IsEncrypted")}</Checkbox>
							</Form.Item>
							<Form.Item
								label={$t("AbpSettingManagement.DisplayName:IsVisibleToClients")}
								name="IsVisibleToClients"
								valuePropName="checked"
								extra={$t("AbpSettingManagement.Description:IsVisibleToClients")}
							>
								<Checkbox disabled={formModel.isStatic}>
									{$t("AbpSettingManagement.DisplayName:IsVisibleToClients")}
								</Checkbox>
							</Form.Item>
						</Tabs.TabPane>

						<Tabs.TabPane key="props" tab={$t("AbpPermissionManagement.Properties")}>
							<PropertyTable
								data={formModel.extraProperties}
								onChange={handlePropertyChange}
								onDelete={handlePropertyDelete}
								disabled={formModel.isStatic}
							/>
						</Tabs.TabPane>
					</Tabs>
				</Form>
			)}
		</Modal>
	);
};

export default SettingDefinitionModal;
