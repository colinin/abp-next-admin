import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Checkbox, Select, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { createApi, updateApi, getApi } from "@/api/management/notifications/notification-definitions";
import { getListApi as getGroupDefinitionsApi } from "@/api/management/notifications/notification-group-definitions";
import { getAssignableProvidersApi, getAssignableTemplatesApi } from "@/api/management/notifications/notifications";
import type {
	NotificationDefinitionDto,
	NotificationProviderDto,
	NotificationTemplateDto,
} from "#/notifications/definitions";
import { NotificationContentType, NotificationLifetime, NotificationType } from "#/notifications"; // Adjust path
import type { PropertyInfo } from "@/components/abp/properties/types";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { useEnumMaps } from "./use-enum-maps";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: NotificationDefinitionDto) => void;
	definitionName?: string;
	groupName?: string;
}

const defaultModel: NotificationDefinitionDto = {
	allowSubscriptionToClients: false,
	contentType: NotificationContentType.Text,
	displayName: "",
	extraProperties: {},
	groupName: "",
	isStatic: false,
	name: "",
	notificationLifetime: NotificationLifetime.Persistent,
	notificationType: NotificationType.Application,
	providers: [],
	template: undefined,
} as NotificationDefinitionDto;

const NotificationDefinitionModal: React.FC<Props> = ({ visible, onClose, onChange, definitionName, groupName }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const { notificationContentTypeOptions, notificationLifetimeOptions, notificationTypeOptions } = useEnumMaps();

	const [activeTab, setActiveTab] = useState("basic");
	const [formModel, setFormModel] = useState<NotificationDefinitionDto>({ ...defaultModel });
	const [isEditModel, setIsEditModel] = useState(false);
	const [loading, setLoading] = useState(false);

	// Dropdown Data
	const [providers, setProviders] = useState<NotificationProviderDto[]>([]);
	const [templates, setTemplates] = useState<NotificationTemplateDto[]>([]);
	const [groups, setGroups] = useState<{ label: string; value: string }[]>([]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			initData();
		}
	}, [visible]);

	const initData = async () => {
		try {
			setLoading(true);
			const [groupsRes, templatesRes, providersRes] = await Promise.all([
				getGroupDefinitionsApi({ filter: groupName }),
				getAssignableTemplatesApi(),
				getAssignableProvidersApi(),
			]);

			setProviders(providersRes.items);
			setTemplates(templatesRes.items);

			const groupOptions = groupsRes.items.map((g) => {
				const d = deserialize(g.displayName);
				return { label: Lr(d.resourceName, d.name), value: g.name };
			});
			setGroups(groupOptions);

			if (definitionName) {
				setIsEditModel(true);
				const dto = await getApi(definitionName);
				setFormModel(dto);
				form.setFieldsValue(dto);
			} else {
				setIsEditModel(false);
				const initial = { ...defaultModel };
				// If pre-filtered by group or only one group exists, auto-select
				if (groupsRes.items.length === 1) initial.groupName = groupsRes.items[0].name;
				else if (groupName) initial.groupName = groupName;

				setFormModel(initial);
				form.setFieldsValue(initial);
			}
		} finally {
			setLoading(false);
		}
	};

	const { mutateAsync: createDef, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		},
	});

	const { mutateAsync: updateDef, isPending: isUpdating } = useMutation({
		mutationFn: (data: NotificationDefinitionDto) => updateApi(data.name, data),
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
				extraProperties: formModel.extraProperties,
			};

			if (isEditModel) {
				await updateDef(submitData);
			} else {
				await createDef(submitData);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handlePropChange = (prop: PropertyInfo) => {
		setFormModel((prev) => ({
			...prev,
			extraProperties: { ...prev.extraProperties, [prop.key]: prop.value },
		}));
	};

	const handlePropDelete = (prop: PropertyInfo) => {
		setFormModel((prev) => {
			const next = { ...prev.extraProperties };
			delete next[prop.key];
			return { ...prev, extraProperties: next };
		});
	};

	const modalTitle = isEditModel
		? `${$t("Notifications.NotificationDefinitions")} - ${formModel.name}`
		: $t("Notifications.NotificationDefinitions:AddNew");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={isCreating || isUpdating || loading}
			okButtonProps={{ disabled: formModel.isStatic }}
			width={800}
			destroyOnClose
		>
			<Form form={form} layout="vertical" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
				<Tabs activeKey={activeTab} onChange={setActiveTab}>
					<Tabs.TabPane key="basic" tab={$t("Notifications.BasicInfo")}>
						<Form.Item name="groupName" label={$t("Notifications.DisplayName:GroupName")} rules={[{ required: true }]}>
							<Select disabled={formModel.isStatic} allowClear options={groups} />
						</Form.Item>

						<Form.Item name="name" label={$t("Notifications.DisplayName:Name")} rules={[{ required: true }]}>
							<Input disabled={formModel.isStatic} autoComplete="off" />
						</Form.Item>

						<Form.Item
							name="displayName"
							label={$t("Notifications.DisplayName:DisplayName")}
							rules={[{ required: true }]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						<Form.Item name="description" label={$t("Notifications.DisplayName:Description")}>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						<Form.Item
							name="allowSubscriptionToClients"
							valuePropName="checked"
							label={$t("Notifications.DisplayName:AllowSubscriptionToClients")}
							extra={$t("Notifications.Description:AllowSubscriptionToClients")}
						>
							<Checkbox disabled={formModel.isStatic}>
								{$t("Notifications.DisplayName:AllowSubscriptionToClients")}
							</Checkbox>
						</Form.Item>

						<Form.Item
							name="notificationType"
							label={$t("Notifications.DisplayName:NotificationType")}
							extra={$t("Notifications.Description:NotificationType")}
						>
							<Select disabled={formModel.isStatic} allowClear options={notificationTypeOptions} />
						</Form.Item>

						<Form.Item
							name="notificationLifetime"
							label={$t("Notifications.DisplayName:NotificationLifetime")}
							extra={$t("Notifications.Description:NotificationLifetime")}
						>
							<Select disabled={formModel.isStatic} allowClear options={notificationLifetimeOptions} />
						</Form.Item>

						<Form.Item
							name="contentType"
							label={$t("Notifications.DisplayName:ContentType")}
							extra={$t("Notifications.Description:ContentType")}
						>
							<Select disabled={formModel.isStatic} allowClear options={notificationContentTypeOptions} />
						</Form.Item>

						<Form.Item
							name="providers"
							label={$t("Notifications.DisplayName:Providers")}
							extra={$t("Notifications.Description:Providers")}
						>
							<Select
								disabled={formModel.isStatic}
								allowClear
								mode="multiple"
								options={providers.map((p) => ({ label: p.name, value: p.name }))}
							/>
						</Form.Item>

						<Form.Item
							name="template"
							label={$t("Notifications.DisplayName:Template")}
							extra={$t("Notifications.Description:Template")}
						>
							<Select
								disabled={formModel.isStatic}
								allowClear
								options={templates.map((t) => ({ label: t.name, value: t.name }))}
							/>
						</Form.Item>
					</Tabs.TabPane>

					<Tabs.TabPane key="props" tab={$t("Notifications.Properties")}>
						<PropertyTable
							data={formModel.extraProperties}
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

export default NotificationDefinitionModal;
