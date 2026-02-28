import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Checkbox, Select, Tabs, Tooltip } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { createApi, updateApi, getApi, getAllAvailableWebhooksApi } from "@/api/webhooks/subscriptions";
import { getPagedListApi as getTenantsApi } from "@/api/saas/tenants";
import type { WebhookSubscriptionDto, WebhookAvailableGroupDto } from "#/webhooks/subscriptions";
import type { TenantDto } from "#/saas/tenants";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import type { PropertyInfo } from "@/components/abp/properties/types";
import PropertyTable from "@/components/abp/properties/property-table";
import debounce from "lodash.debounce";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: WebhookSubscriptionDto) => void;
	subscriptionId?: string;
}

// 1. Define a local type that includes the missing property
interface LocalWebhookSubscriptionDto extends WebhookSubscriptionDto {
	isStatic?: boolean;
}

// 2. Use the local type for the default model
const defaultModel: LocalWebhookSubscriptionDto = {
	creationTime: new Date(),
	displayName: "",
	extraProperties: {},
	headers: {},
	id: "",
	isActive: true,
	isStatic: false,
	webhooks: [],
	webhookUri: "",
} as LocalWebhookSubscriptionDto;

const WebhookSubscriptionModal: React.FC<Props> = ({ visible, onClose, onChange, subscriptionId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const [activeTab, setActiveTab] = useState("basic");
	// 3. State uses the local type
	const [formModel, setFormModel] = useState<LocalWebhookSubscriptionDto>({ ...defaultModel });
	const [loading, setLoading] = useState(false);
	const [isEditModel, setIsEditModel] = useState(false);

	const [webhookGroups, setWebhookGroups] = useState<WebhookAvailableGroupDto[]>([]);
	const [tenants, setTenants] = useState<TenantDto[]>([]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			form.resetFields();
			initData();

			if (subscriptionId) {
				fetchSubscription(subscriptionId);
				setIsEditModel(true);
			} else {
				setFormModel({ ...defaultModel });
				setIsEditModel(false);
			}
		}
	}, [visible, subscriptionId]);

	const initData = async () => {
		try {
			const [groupRes, tenantRes] = await Promise.all([getAllAvailableWebhooksApi(), fetchTenants()]);
			setWebhookGroups(groupRes.items);
			setTenants(tenantRes);
		} catch (error) {
			console.error(error);
		}
	};

	const fetchTenants = async (filter?: string) => {
		if (!hasAccessByCodes(["AbpSaas.Tenants"])) {
			return [];
		}
		const { items } = await getTenantsApi({ filter, maxResultCount: 50 });
		return items;
	};

	const fetchSubscription = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			// 4. Cast the API response to the local type
			setFormModel(dto as LocalWebhookSubscriptionDto);
			form.setFieldsValue(dto);
		} finally {
			setLoading(false);
		}
	};

	const { mutateAsync: createSub, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		},
	});

	const { mutateAsync: updateSub, isPending: isUpdating } = useMutation({
		mutationFn: (data: WebhookSubscriptionDto) => updateApi(data.id, data),
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		},
	});

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			const submitData = {
				...formModel,
				...values,
			};

			if (isEditModel) {
				await updateSub(submitData);
			} else {
				await createSub(submitData);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handleTenantsSearch = debounce(async (input: string) => {
		const res = await fetchTenants(input);
		setTenants(res);
	}, 500);

	const handlePropChange = (prop: PropertyInfo) => {
		setFormModel((prev) => ({
			...prev,
			headers: {
				...prev.headers,
				[prop.key]: prop.value,
			},
		}));
	};

	const handlePropDelete = (prop: PropertyInfo) => {
		setFormModel((prev) => {
			const next = { ...prev.headers };
			delete next[prop.key];
			return { ...prev, headers: next };
		});
	};

	const modalTitle = isEditModel
		? $t("WebhooksManagement.Subscriptions:Edit")
		: $t("WebhooksManagement.Subscriptions:AddNew");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={isCreating || isUpdating || loading}
			// 5. Use the property safely
			okButtonProps={{ disabled: formModel.isStatic }}
			width={800}
			destroyOnClose
		>
			<Form form={form} layout="vertical" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={setActiveTab}>
					<Tabs.TabPane key="basic" tab={$t("WebhooksManagement.BasicInfo")}>
						<Form.Item
							name="isActive"
							valuePropName="checked"
							label={$t("WebhooksManagement.DisplayName:IsActive")}
							extra={$t("WebhooksManagement.Description:IsActive")}
						>
							<Checkbox disabled={formModel.isStatic}>{$t("WebhooksManagement.DisplayName:IsActive")}</Checkbox>
						</Form.Item>

						<Form.Item
							name="webhookUri"
							label={$t("WebhooksManagement.DisplayName:WebhookUri")}
							extra={$t("WebhooksManagement.Description:WebhookUri")}
							rules={[{ required: true }]}
						>
							<Input disabled={formModel.isStatic} autoComplete="off" allowClear />
						</Form.Item>

						<Form.Item
							name="webhooks"
							label={$t("WebhooksManagement.DisplayName:Webhooks")}
							extra={$t("WebhooksManagement.Description:Webhooks")}
						>
							<Select
								disabled={formModel.isStatic}
								allowClear
								mode="multiple"
								options={webhookGroups.map((group) => ({
									label: group.displayName,
									options: group.webhooks.map((wh) => ({
										label: (
											<Tooltip title={wh.description} placement="right">
												{wh.displayName}
											</Tooltip>
										),
										value: wh.name,
									})),
								}))}
							/>
						</Form.Item>

						{hasAccessByCodes(["AbpSaas.Tenants"]) && (
							<Form.Item
								name="tenantId"
								label={$t("WebhooksManagement.DisplayName:TenantId")}
								extra={$t("WebhooksManagement.Description:TenantId")}
							>
								<Select
									disabled={formModel.isStatic}
									allowClear
									showSearch
									onSearch={handleTenantsSearch}
									filterOption={false}
									options={tenants.map((t) => ({ label: t.name, value: t.id }))}
								/>
							</Form.Item>
						)}

						<Form.Item
							name="secret"
							label={$t("WebhooksManagement.DisplayName:Secret")}
							extra={$t("WebhooksManagement.Description:Secret")}
						>
							<Input.Password disabled={formModel.isStatic} autoComplete="off" allowClear />
						</Form.Item>

						<Form.Item name="description" label={$t("WebhooksManagement.DisplayName:Description")}>
							<Input.TextArea disabled={formModel.isStatic} autoSize={{ minRows: 3 }} allowClear />
						</Form.Item>
					</Tabs.TabPane>

					<Tabs.TabPane key="headers" tab={$t("WebhooksManagement.DisplayName:Headers")}>
						<PropertyTable
							data={formModel.headers}
							disabled={formModel.isStatic}
							onChange={handlePropChange}
							onDelete={handlePropDelete}
						/>
					</Tabs.TabPane>
				</Tabs>
			</Form>
		</Modal>
	);
};

export default WebhookSubscriptionModal;
