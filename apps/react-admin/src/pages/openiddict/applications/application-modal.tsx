import { useEffect, useState } from "react";
import { Form, Input, Modal, Select, Tabs, InputNumber, Checkbox, Transfer, Dropdown } from "antd";
import { DownOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { OpenIddictApplicationDto, OpenIddictApplicationUpdateDto } from "#/openiddict/applications";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createApi, getApi, updateApi } from "@/api/openiddict/applications";
import { discoveryApi } from "@/api/openiddict/open-id";
import type { DisplayNameInfo } from "@/components/abp/display-names/types";
import type { PropertyInfo } from "@/components/abp/properties/types";
import DisplayNameTable from "@/components/abp/display-names/display-name-table";
import PropertyTable from "@/components/abp/properties/property-table";
import UriTable from "./uri-table";
import { toast } from "sonner";
import { mergeDeepRight } from "ramda";
import { CircleLoading } from "@/components/loading";

interface Props {
	visible: boolean;
	applicationId?: string;
	onClose: () => void;
	onChange: (data: OpenIddictApplicationDto) => void;
}

type TabKeys = "basic" | "displayName" | "endpoint" | "tokens" | "scope" | "authorize" | "props";

const defaultModel = {
	clientId: "",
	applicationType: "web",
	clientType: "public",
	consentType: "explicit",
	requirements: {
		features: {
			requirePkce: false,
		},
	},
	extraProperties: {},
	settings: {
		tokenLifetime: {
			accessToken: 3600,
			authorizationCode: 600,
			identityToken: 3600,
			refreshToken: 86400,
			deviceCode: 600,
			userCode: 600,
		},
	},
};

const clientTypes = [
	{ label: "public", value: "public" },
	{ label: "confidential", value: "confidential" },
];

const applicationTypes = [
	{ label: "Web", value: "web" },
	{ label: "Native", value: "native" },
];

const consentTypes = [
	{ label: "explicit", value: "explicit" },
	{ label: "external", value: "external" },
	{ label: "implicit", value: "implicit" },
	{ label: "systematic", value: "systematic" },
];

const endpoints = [
	{ label: "authorization", value: "authorization" },
	{ label: "token", value: "token" },
	{ label: "logout", value: "logout" },
	{ label: "device", value: "device" },
	{ label: "revocation", value: "revocation" },
	{ label: "introspection", value: "introspection" },
];

// Token Tabs 相关表单配置
const tokenFormItems = [
	{
		name: ["settings", "tokenLifetime", "accessToken"],
		label: "AbpOpenIddict.DisplayName:AccessTokenLifetime",
		extra: "AbpOpenIddict.Description:AccessTokenLifetime",
		min: 300,
	},
	{
		name: ["settings", "tokenLifetime", "authorizationCode"],
		label: "AbpOpenIddict.DisplayName:AuthorizationCodeLifetime",
		extra: "AbpOpenIddict.Description:AuthorizationCodeLifetime",
		min: 300,
	},
	{
		name: ["settings", "tokenLifetime", "identityToken"],
		label: "AbpOpenIddict.DisplayName:IdentityTokenLifetime",
		extra: "AbpOpenIddict.Description:IdentityTokenLifetime",
		min: 300,
	},
	{
		name: ["settings", "tokenLifetime", "refreshToken"],
		label: "AbpOpenIddict.DisplayName:RefreshTokenLifetime",
		extra: "AbpOpenIddict.Description:RefreshTokenLifetime",
		min: 300,
	},
	{
		name: ["settings", "tokenLifetime", "deviceCode"],
		label: "AbpOpenIddict.DisplayName:DeviceCodeLifetime",
		extra: "AbpOpenIddict.Description:DeviceCodeLifetime",
		min: 300,
	},
	{
		name: ["settings", "tokenLifetime", "userCode"],
		label: "AbpOpenIddict.DisplayName:UserCodeLifetime",
		extra: "AbpOpenIddict.Description:UserCodeLifetime",
		min: 300,
	},
];

const ApplicationModal: React.FC<Props> = ({ visible, applicationId, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const queryClient = useQueryClient();
	const [activeTab, setActiveTab] = useState<TabKeys>("basic");
	const [formModel, setFormModel] = useState<OpenIddictApplicationUpdateDto>(defaultModel);
	const [uriState, setUriState] = useState({
		component: "RedirectUris",
	});

	// Fetch OpenID configuration
	const { data: openIdConfig } = useQuery({
		queryKey: ["openid-configuration"],
		queryFn: discoveryApi,
		enabled: visible,
	});

	const { data: applicationData, isLoading } = useQuery({
		queryKey: ["applicationId", applicationId],
		queryFn: () => {
			if (!applicationId) {
				return Promise.reject(new Error("applicationId is undefined"));
			}
			return getApi(applicationId);
		},
		enabled: !!applicationId && visible,
	});

	useEffect(() => {
		if (applicationData) {
			if (applicationData.id === applicationId) {
				setFormModel(applicationData); //将OpenIddictApplicationDto 转到 OpenIddictApplicationUpdateDto,注意这里的特殊，formModal的展示值和最后的请求值数据结构非常相似
				form.setFieldsValue(applicationData);
			}
		}
	}, [applicationData]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
		}
	}, [visible]);

	// Create/Update mutations
	const { mutateAsync: createApplication } = useMutation({
		mutationFn: createApi,
		onSuccess: (data) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(data);
			onClose();
		},
	});

	const { mutateAsync: updateApplication } = useMutation({
		mutationFn: ({ id, input }: { id: string; input: OpenIddictApplicationUpdateDto }) => updateApi(id, input),
		onSuccess: (data) => {
			queryClient.invalidateQueries({ queryKey: ["applicationId", applicationId] });
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

	const handleUriChange = (uri: string) => {
		const fieldName = uriState.component === "RedirectUris" ? "redirectUris" : "postLogoutRedirectUris";

		setFormModel((prev) => ({
			...prev,
			[fieldName]: [...(prev[fieldName] || []), uri],
		}));
	};

	const handleUriDelete = (uri: string) => {
		const fieldName = uriState.component === "RedirectUris" ? "redirectUris" : "postLogoutRedirectUris";

		setFormModel((prev) => ({
			...prev,
			[fieldName]: (prev[fieldName] || []).filter((u) => u !== uri),
		}));
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
			if (applicationId) {
				await updateApplication({ id: applicationId, input: formModel });
			} else {
				await createApplication(formModel);
			}
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	return (
		<Modal
			open={visible}
			title={
				applicationId
					? `${$t("AbpOpenIddict.Applications")} - ${formModel.clientId || ""}`
					: $t("AbpOpenIddict.Applications:AddNew")
			}
			onCancel={() => {
				onClose();
				form.resetFields();
				setFormModel(defaultModel);
				form.setFieldsValue(defaultModel); //两个同时设置，保持没有applicationId时，handleSubmit调用createApplication时的表单状态和请求数据一致
			}}
			onOk={handleSubmit}
			onClose={() => {
				onClose();
				form.resetFields();
				setFormModel(defaultModel);
				form.resetFields();
			}}
			width="50%"
			destroyOnClose
		>
			{!!applicationId && visible && isLoading ? (
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
						{/* Basic Info Tab */}
						<Tabs.TabPane key="basic" tab={$t("AbpOpenIddict.BasicInfo")}>
							<Form.Item
								label={$t("AbpOpenIddict.DisplayName:ApplicationType")}
								name="applicationType"
								rules={[{ required: true }]}
							>
								<Select options={applicationTypes} />
							</Form.Item>
							<Form.Item label={$t("AbpOpenIddict.DisplayName:ClientId")} name="clientId" rules={[{ required: true }]}>
								<Input autoComplete="off" />
							</Form.Item>
							<Form.Item label={$t("AbpOpenIddict.DisplayName:ClientType")} name="clientType">
								<Select options={clientTypes} />
							</Form.Item>
							{!applicationId && (
								<Form.Item
									label={$t("AbpOpenIddict.DisplayName:ClientSecret")}
									name="clientSecret"
									rules={[{ required: form.getFieldValue("clientType") === "confidential" }]}
								>
									<Input.Password autoComplete="off" />
								</Form.Item>
							)}
							<Form.Item label={$t("AbpOpenIddict.DisplayName:ClientUri")} name="clientUri">
								<Input />
							</Form.Item>
							<Form.Item label={$t("AbpOpenIddict.DisplayName:LogoUri")} name="logoUri">
								<Input autoComplete="off" />
							</Form.Item>
							<Form.Item
								label={$t("AbpOpenIddict.DisplayName:ConsentType")}
								name="consentType"
								labelCol={{ span: 4 }}
								wrapperCol={{ span: 20 }}
								initialValue={formModel.consentType}
							>
								<Select options={consentTypes} />
							</Form.Item>
						</Tabs.TabPane>

						{/* Display Names Tab */}
						<Tabs.TabPane key="displayName" tab={$t("AbpOpenIddict.DisplayNames")}>
							<Form.Item
								label={$t("AbpOpenIddict.DisplayName:DefaultDisplayName")}
								name="displayName"
								initialValue={formModel.displayName}
							>
								<Input autoComplete="off" />
							</Form.Item>

							<DisplayNameTable
								data={formModel.displayNames}
								onChange={handleDisplayNameChange}
								onDelete={handleDisplayNameDelete}
							/>
						</Tabs.TabPane>

						{/* Endpoints Tab */}
						<Tabs.TabPane
							key="endpoint"
							tab={
								<Dropdown
									menu={{
										onClick: ({ key }) => {
											setActiveTab("endpoint");
											setUriState({
												component: key as string,
											});
										},
										items: [
											{
												key: "RedirectUris",
												label: $t("AbpOpenIddict.DisplayName:RedirectUris"),
											},
											{
												key: "PostLogoutRedirectUris",
												label: $t("AbpOpenIddict.DisplayName:PostLogoutRedirectUris"),
											},
										].filter(Boolean),
									}}
								>
									<span>
										{$t("AbpOpenIddict.Endpoints")} <DownOutlined />
									</span>
								</Dropdown>
							}
						>
							<UriTable
								title={$t(
									uriState.component === "RedirectUris"
										? "AbpOpenIddict.DisplayName:RedirectUris"
										: "AbpOpenIddict.DisplayName:PostLogoutRedirectUris",
								)}
								uris={uriState.component === "RedirectUris" ? formModel.redirectUris : formModel.postLogoutRedirectUris}
								onChange={handleUriChange}
								onDelete={handleUriDelete}
							/>
						</Tabs.TabPane>

						{/* Tokens Tab  //！！！注意这个name的数组语法赋值*/}
						<Tabs.TabPane key="tokens" tab={$t("AbpOpenIddict.Tokens")}>
							{tokenFormItems.map((item) => (
								<Form.Item key={item.name.join(".")} label={$t(item.label)} name={item.name} extra={$t(item.extra)}>
									<InputNumber className="w-full" min={item.min} />
								</Form.Item>
							))}
						</Tabs.TabPane>

						{/* Scopes Tab */}
						<Tabs.TabPane key="scope" tab={$t("AbpOpenIddict.Scopes")}>
							<Transfer
								dataSource={openIdConfig?.scopes_supported?.map((scope) => ({
									key: scope,
									title: scope,
								}))}
								targetKeys={formModel.scopes || []}
								onChange={(targetKeys) => {
									const stringTargetKeys = targetKeys.map(String); // 转换成 string[]
									setFormModel((prev) => ({
										...prev,
										scopes: stringTargetKeys,
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

						{/* Authorization Tab */}
						<Tabs.TabPane key="authorize" tab={$t("AbpOpenIddict.Authorizations")}>
							<Form.Item
								label={$t("AbpOpenIddict.Requirements:PKCE")}
								name={["requirements", "features", "requirePkce"]} //antd数组赋值语法
								valuePropName="checked"
							>
								<Checkbox checked={formModel.requirements?.features.requirePkce}>
									{$t("AbpOpenIddict.Requirements:PKCE")}
								</Checkbox>
							</Form.Item>
							<Form.Item
								label={$t("AbpOpenIddict.DisplayName:Endpoints")}
								name="endpoints"
								labelCol={{ span: 4 }}
								wrapperCol={{ span: 20 }}
							>
								<Select mode="tags" options={endpoints} />
							</Form.Item>
							<Form.Item
								label={$t("AbpOpenIddict.DisplayName:GrantTypes")}
								name="grantTypes"
								labelCol={{ span: 4 }}
								wrapperCol={{ span: 20 }}
							>
								<Select
									mode="tags"
									options={openIdConfig?.grant_types_supported?.map((type) => ({
										label: type,
										value: type,
									}))}
								/>
							</Form.Item>
							<Form.Item
								label={$t("AbpOpenIddict.DisplayName:ResponseTypes")}
								name="responseTypes"
								labelCol={{ span: 4 }}
								wrapperCol={{ span: 20 }}
							>
								<Select
									mode="tags"
									options={openIdConfig?.response_types_supported?.map((type) => ({
										label: type,
										value: type,
									}))}
								/>
							</Form.Item>
						</Tabs.TabPane>

						{/* Properties Tab */}
						<Tabs.TabPane key="props" tab={$t("AbpOpenIddict.DisplayName:Properties")}>
							<PropertyTable //openiddict的自带Properties,不是abp的ExtraProperties,但还是用这个组件
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

export default ApplicationModal;
