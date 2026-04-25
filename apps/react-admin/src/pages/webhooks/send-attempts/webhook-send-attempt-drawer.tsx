import type React from "react";
import { useEffect, useState } from "react";
import { Drawer, Form, Input, Checkbox, Tabs, Tag } from "antd";
import { useTranslation } from "react-i18next";
import { getApi } from "@/api/webhooks/send-attempts";
import { getApi as getSubscriptionApi } from "@/api/webhooks/subscriptions";
import { getApi as getTenantApi } from "@/api/saas/tenants";
import type { WebhookSendRecordDto } from "#/webhooks/send-attempts";
import type { WebhookSubscriptionDto } from "#/webhooks/subscriptions";
import type { TenantDto } from "#/saas/tenants";
import { WebhookSubscriptionPermissions } from "@/constants/webhooks/permissions"; // Adjust path
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { formatToDateTime } from "@/utils/abp";

import Editor from "@/components/editor";
import { useHttpStatusCodeMap } from "@/hooks/abp/fake-hooks/use-http-status-code-map";
import JsonEdit from "@/components/abp/common/json-edit";

interface Props {
	visible: boolean;
	onClose: () => void;
	recordId?: string;
}

const WebhookSendAttemptDrawer: React.FC<Props> = ({ visible, onClose, recordId }) => {
	const { t: $t } = useTranslation();

	const { getHttpStatusColor, httpStatusCodeMap } = useHttpStatusCodeMap();
	const [activeTab, setActiveTab] = useState("basic");
	const [loading, setLoading] = useState(false);
	const [formModel, setFormModel] = useState<WebhookSendRecordDto>();
	const [webhookSubscription, setWebhookSubscription] = useState<WebhookSubscriptionDto>();
	const [webhookTenant, setWebhookTenant] = useState<TenantDto>();

	useEffect(() => {
		if (visible && recordId) {
			initData(recordId);
		} else {
			setFormModel(undefined);
			setWebhookSubscription(undefined);
			setWebhookTenant(undefined);
		}
	}, [visible, recordId]);

	const initData = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			setFormModel(dto);

			const [sub, tenant] = await Promise.all([
				fetchSubscription(dto.webhookSubscriptionId),
				fetchTenant(dto.tenantId),
			]);
			setWebhookSubscription(sub);
			setWebhookTenant(tenant);
		} finally {
			setLoading(false);
		}
	};

	const fetchSubscription = async (id: string) => {
		if (hasAccessByCodes([WebhookSubscriptionPermissions.Default])) {
			return await getSubscriptionApi(id);
		}
		return undefined;
	};

	const fetchTenant = async (id?: string) => {
		if (id && hasAccessByCodes(["AbpSaas.Tenants"])) {
			return await getTenantApi(id);
		}
		return undefined;
	};

	return (
		<Drawer
			title={$t("WebhooksManagement.SendAttempts")}
			open={visible}
			onClose={onClose}
			width="50%"
			destroyOnClose
			loading={loading}
		>
			{formModel && (
				<Form layout="vertical">
					<Tabs activeKey={activeTab} onChange={setActiveTab}>
						{/* Basic Info Tab */}
						<Tabs.TabPane key="basic" tab={$t("WebhooksManagement.BasicInfo")}>
							{webhookTenant && (
								<Form.Item label={$t("WebhooksManagement.DisplayName:TenantId")}>
									<Input value={webhookTenant.name} disabled />
								</Form.Item>
							)}

							<Form.Item>
								<Checkbox checked={formModel.sendExactSameData} disabled>
									{$t("WebhooksManagement.DisplayName:SendExactSameData")}
								</Checkbox>
							</Form.Item>

							<Form.Item label={$t("WebhooksManagement.DisplayName:CreationTime")}>
								<Input value={formatToDateTime(formModel.creationTime)} disabled />
							</Form.Item>

							<Form.Item label={$t("WebhooksManagement.DisplayName:RequestHeaders")}>
								<JsonEdit data={formModel.requestHeaders || ""} />
							</Form.Item>

							{formModel.responseStatusCode && (
								<Form.Item label={$t("WebhooksManagement.DisplayName:ResponseStatusCode")}>
									<Tag color={getHttpStatusColor(formModel.responseStatusCode)}>
										{httpStatusCodeMap[formModel.responseStatusCode] || formModel.responseStatusCode}
									</Tag>
								</Form.Item>
							)}

							<Form.Item label={$t("WebhooksManagement.DisplayName:ResponseHeaders")}>
								<JsonEdit data={formModel.responseHeaders || ""} />
							</Form.Item>

							<Form.Item label={$t("WebhooksManagement.DisplayName:Response")}>
								<Editor value={formModel.response ?? ""} readOnly sample hiddleToolbar />
								{/* <Input.TextArea value={formModel.response} autoSize={{ minRows: 5 }} readOnly /> */}
							</Form.Item>
						</Tabs.TabPane>

						{/* Event Tab */}
						<Tabs.TabPane key="event" tab={$t("WebhooksManagement.WebhookEvent")}>
							<Form.Item label={$t("WebhooksManagement.DisplayName:WebhookEventId")}>
								<Input value={formModel.webhookEventId} disabled />
							</Form.Item>

							<Form.Item label={$t("WebhooksManagement.DisplayName:WebhookName")}>
								<Input value={formModel.webhookEvent?.webhookName} disabled />
							</Form.Item>

							<Form.Item label={$t("WebhooksManagement.DisplayName:CreationTime")}>
								<Input value={formatToDateTime(formModel.webhookEvent?.creationTime)} disabled />
							</Form.Item>

							<Form.Item label={$t("WebhooksManagement.DisplayName:Data")}>
								<JsonEdit data={formModel.webhookEvent?.data || ""} />
							</Form.Item>
						</Tabs.TabPane>

						{/* Subscriber Tab */}
						{webhookSubscription && (
							<Tabs.TabPane key="subscriber" tab={$t("WebhooksManagement.Subscriptions")}>
								<Form.Item>
									<Checkbox checked={webhookSubscription.isActive} disabled>
										{$t("WebhooksManagement.DisplayName:IsActive")}
									</Checkbox>
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:WebhookSubscriptionId")}>
									<Input value={webhookSubscription.id} disabled />
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:WebhookUri")}>
									<Input value={webhookSubscription.webhookUri} disabled />
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:Description")}>
									<Input.TextArea value={webhookSubscription.description} autoSize={{ minRows: 2 }} disabled />
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:Secret")}>
									<Input.Password value={webhookSubscription.secret} disabled />
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:CreationTime")}>
									<Input value={formatToDateTime(webhookSubscription.creationTime)} disabled />
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:Webhooks")}>
									{webhookSubscription.webhooks?.map((w) => (
										<Tag key={w} color="blue">
											{w}
										</Tag>
									))}
								</Form.Item>

								<Form.Item label={$t("WebhooksManagement.DisplayName:Headers")}>
									<JsonEdit data={webhookSubscription.headers || ""} />
								</Form.Item>
							</Tabs.TabPane>
						)}
					</Tabs>
				</Form>
			)}
		</Drawer>
	);
};

export default WebhookSendAttemptDrawer;
