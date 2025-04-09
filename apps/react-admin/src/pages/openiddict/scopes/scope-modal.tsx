import { useEffect, useState } from "react";
import { Form, Input, Modal, Tabs, Transfer } from "antd";
import { useTranslation } from "react-i18next";
import type { OpenIddictScopeDto, OpenIddictScopeUpdateDto } from "#/openiddict/scopes";
import type { DisplayNameInfo } from "@/components/abp/display-names/types";
import type { PropertyInfo } from "@/components/abp/properties/types";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createApi, getApi, updateApi } from "@/api/openiddict/scopes";
import { discoveryApi } from "@/api/openiddict/open-id";
import DisplayNameTable from "@/components/abp/display-names/display-name-table";
import PropertyTable from "@/components/abp/properties/property-table";
import { toast } from "sonner";
import { CircleLoading } from "@/components/loading";

interface Props {
	visible: boolean;
	scopeId?: string;
	onClose: () => void;
	onChange: (data: OpenIddictScopeDto) => void;
}

type TabKeys = "basic" | "displayName" | "description" | "resource" | "props";

const defaultModel = {
	name: "",
	extraProperties: {},
};

const ScopeModal: React.FC<Props> = ({ visible, scopeId, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const queryClient = useQueryClient();
	const [activeTab, setActiveTab] = useState<TabKeys>("basic");
	const [formModel, setFormModel] = useState<OpenIddictScopeUpdateDto>(defaultModel);

	// Fetch OpenID configuration
	const { data: openIdConfig } = useQuery({
		queryKey: ["openid-configuration"],
		queryFn: discoveryApi,
		enabled: visible,
	});

	// Fetch scope data if editing
	const { data: scopeData, isLoading } = useQuery({
		queryKey: ["scopeId", scopeId],
		queryFn: () => {
			if (!scopeId) {
				return Promise.reject(new Error("scopeId is undefined"));
			}
			return getApi(scopeId);
		},
		enabled: !!scopeId && visible,
	});

	useEffect(() => {
		if (scopeData && scopeData.id === scopeId) {
			setFormModel(scopeData);
			form.setFieldsValue(scopeData);
		}
	}, [scopeData]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
		}
	}, [visible]);

	// Create/Update mutations
	const { mutateAsync: createScope } = useMutation({
		mutationFn: createApi,
		onSuccess: (data) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(data);
			onClose();
		},
	});

	const { mutateAsync: updateScope } = useMutation({
		mutationFn: ({ id, input }: { id: string; input: OpenIddictScopeUpdateDto }) => updateApi(id, input),
		onSuccess: (data) => {
			queryClient.invalidateQueries({ queryKey: ["scopeId", scopeId] });
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(data);
			onClose();
		},
	});

	const handleDisplayNameChange = (displayName: DisplayNameInfo) => {
		setFormModel((prev) => ({
			...prev,
			displayNames: {
				...prev.displayNames,
				[displayName.culture]: displayName.displayName,
			},
		}));
	};

	const handleDisplayNameDelete = (displayName: DisplayNameInfo) => {
		setFormModel((prev) => {
			const newDisplayNames = { ...prev.displayNames };
			delete newDisplayNames[displayName.culture];
			return {
				...prev,
				displayNames: newDisplayNames,
			};
		});
	};

	const handleDescriptionChange = (description: DisplayNameInfo) => {
		setFormModel((prev) => ({
			...prev,
			descriptions: {
				...prev.descriptions,
				[description.culture]: description.displayName,
			},
		}));
	};

	const handleDescriptionDelete = (description: DisplayNameInfo) => {
		setFormModel((prev) => {
			const newDescriptions = { ...prev.descriptions };
			delete newDescriptions[description.culture];
			return {
				...prev,
				descriptions: newDescriptions,
			};
		});
	};

	const handlePropertyChange = (prop: PropertyInfo) => {
		setFormModel((prev) => ({
			...prev,
			properties: {
				...prev.properties,
				[prop.key]: prop.value,
			},
		}));
	};

	const handlePropertyDelete = (prop: PropertyInfo) => {
		setFormModel((prev) => {
			const newProperties = { ...prev.properties };
			delete newProperties[prop.key];
			return {
				...prev,
				properties: newProperties,
			};
		});
	};

	const handleSubmit = async () => {
		try {
			await form.validateFields();
			if (scopeId) {
				await updateScope({ id: scopeId, input: formModel });
			} else {
				await createScope(formModel);
			}
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={scopeId ? `${$t("AbpOpenIddict.Scopes")} - ${formModel.name}` : $t("AbpOpenIddict.Scopes:AddNew")}
			onCancel={() => {
				onClose();
				form.resetFields();
				setFormModel(defaultModel);
				form.setFieldsValue(defaultModel);
			}}
			onClose={() => {
				onClose();
				form.resetFields();
				setFormModel(defaultModel);
				form.setFieldsValue(defaultModel);
			}}
			onOk={handleSubmit}
			width="50%"
			destroyOnClose
		>
			{!!scopeId && visible && isLoading ? (
				<CircleLoading />
			) : (
				<Form form={form} layout="horizontal" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
					<Tabs activeKey={activeTab} onChange={(key) => setActiveTab(key as TabKeys)}>
						{/* Basic Info Tab */}
						<Tabs.TabPane key="basic" tab={$t("AbpOpenIddict.BasicInfo")}>
							<Form.Item label={$t("AbpOpenIddict.DisplayName:Name")} name="name" rules={[{ required: true }]}>
								<Input autoComplete="off" />
							</Form.Item>
						</Tabs.TabPane>

						{/* Display Names Tab */}
						<Tabs.TabPane key="displayName" tab={$t("AbpOpenIddict.DisplayNames")}>
							<Form.Item label={$t("AbpOpenIddict.DisplayName:DefaultDisplayName")} name="displayName">
								<Input autoComplete="off" />
							</Form.Item>
							<DisplayNameTable
								data={formModel.displayNames}
								onChange={handleDisplayNameChange}
								onDelete={handleDisplayNameDelete}
							/>
						</Tabs.TabPane>

						{/* Descriptions Tab */}
						<Tabs.TabPane key="description" tab={$t("AbpOpenIddict.Descriptions")}>
							<Form.Item label={$t("AbpOpenIddict.DisplayName:DefaultDescription")} name="description">
								<Input autoComplete="off" />
							</Form.Item>
							<DisplayNameTable
								data={formModel.descriptions}
								onChange={handleDescriptionChange}
								onDelete={handleDescriptionDelete}
							/>
						</Tabs.TabPane>

						{/* Resources Tab */}
						<Tabs.TabPane key="resource" tab={$t("AbpOpenIddict.Resources")}>
							<Transfer
								dataSource={openIdConfig?.claims_supported?.map((claim) => ({
									key: claim,
									title: claim,
								}))}
								targetKeys={formModel.resources || []}
								onChange={(targetKeys) => {
									const stringTargetKeys = targetKeys.map(String);
									setFormModel((prev) => ({
										...prev,
										resources: stringTargetKeys,
									}));
								}}
								render={(item) => item.title}
								listStyle={{
									width: "47%",
									height: "338px",
								}}
								titles={[$t("AbpOpenIddict.Assigned"), $t("AbpOpenIddict.Available")]}
							/>
						</Tabs.TabPane>

						{/* Properties Tab */}
						<Tabs.TabPane key="props" tab={$t("AbpOpenIddict.Propertites")}>
							<PropertyTable
								data={formModel.properties}
								onChange={handlePropertyChange}
								onDelete={handlePropertyDelete}
							/>
						</Tabs.TabPane>
					</Tabs>
				</Form>
			)}
		</Modal>
	);
};

export default ScopeModal;
