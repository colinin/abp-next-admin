import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Select, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { sendNotiferApi } from "@/api/management/notifications/notifications";
import type { NotificationDefinitionDto } from "#/notifications/definitions";
import { NotificationContentType } from "#/notifications";
import PropertyTable from "@/components/abp/properties/property-table";
import { useEnumMaps } from "./use-enum-maps";

// import MarkdownEditor from "@/components/markdown-editor"; //TODO

interface Props {
	visible: boolean;
	onClose: () => void;
	notification?: NotificationDefinitionDto;
}

const NotificationSendModal: React.FC<Props> = ({ visible, onClose, notification }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const { notificationSeverityOptions } = useEnumMaps();

	const [activeTab, setActiveTab] = useState("basic");
	const [submitting, setSubmitting] = useState(false);
	const [properties, setProperties] = useState<Record<string, any>>({});

	useEffect(() => {
		if (visible && notification) {
			form.resetFields();
			setProperties({});
			setActiveTab("basic");
		}
	}, [visible, notification]);

	const handleSubmit = async () => {
		if (!notification) return;
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			await sendNotiferApi({
				culture: values.culture, // Add culture input if needed
				data: {
					description: values.description,
					message: values.message,
					title: values.title,
					...properties, // Merge extra properties
				},
				name: notification.name,
				severity: values.severity,
			});

			toast.success($t("Notifications.SendSuccessfully"));
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	const isMarkdown = notification?.contentType === NotificationContentType.Markdown;
	const isWeChat = notification?.providers?.some((p) => p.toLowerCase().includes("wechat"));

	return (
		<Modal
			title={$t("Notifications.Notifications:Send")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			width={800}
			destroyOnClose
		>
			<Form form={form} layout="vertical" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={setActiveTab}>
					<Tabs.TabPane key="basic" tab={$t("Notifications.BasicInfo")}>
						<Form.Item label={$t("Notifications.DisplayName:Name")}>
							<Input disabled value={notification?.displayName || notification?.name} />
						</Form.Item>

						{!notification?.template && (
							<>
								<Form.Item name="title" label={$t("Notifications.Notifications:Title")} rules={[{ required: true }]}>
									<Input.TextArea autoSize={{ minRows: 1 }} />
								</Form.Item>

								<Form.Item
									name="message"
									label={$t("Notifications.Notifications:Message")}
									rules={[{ required: true }]}
								>
									{isMarkdown ? (
										<Input.TextArea rows={10} placeholder="Markdown content..." />
										// Replace with <MarkdownEditor /> if available.
									) : (
										<Input.TextArea autoSize={{ minRows: 4 }} />
									)}
								</Form.Item>

								<Form.Item name="description" label={$t("Notifications.Notifications:Description")}>
									<Input.TextArea autoSize={{ minRows: 2 }} />
								</Form.Item>
							</>
						)}

						<Form.Item name="severity" label={$t("Notifications.Notifications:Severity")}>
							<Select allowClear options={notificationSeverityOptions} />
						</Form.Item>
					</Tabs.TabPane>

					<Tabs.TabPane key="props" tab={$t("Notifications.Properties")}>
						{/* Reusing PropertyTable to edit the 'data' payload for the notification */}
						<PropertyTable
							data={properties}
							onChange={(prop) => setProperties((prev) => ({ ...prev, [prop.key]: prop.value }))}
							onDelete={(prop) => {
								const next = { ...properties };
								delete next[prop.key];
								setProperties(next);
							}}
						/>
					</Tabs.TabPane>
				</Tabs>
			</Form>
		</Modal>
	);
};

export default NotificationSendModal;
