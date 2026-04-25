import type React from "react";
import { useEffect, useMemo, useState } from "react";
import { Modal, Form, Input, Select, Checkbox, TreeSelect, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { useMutation } from "@tanstack/react-query";
import { createApi, getApi, updateApi } from "@/api/webhooks/webhook-definitions";
import { getListApi as getGroupDefinitionsApi } from "@/api/webhooks/webhook-group-definitions";
import { getListApi as getFeaturesApi } from "@/api/management/features/feature-definitions";
import { getListApi as getFeatureGroupsApi } from "@/api/management/features/feature-group-definitions";
import type { WebhookDefinitionDto } from "#/webhooks/definitions";
import type { WebhookGroupDefinitionDto } from "#/webhooks/groups";
import type { FeatureDefinitionDto, FeatureGroupDefinitionDto } from "#/management/features";
import type { PropertyInfo } from "@/components/abp/properties/types";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";

import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { listToTree } from "@/utils/tree";
import { valueTypeSerializer } from "@/components/abp/string-value-type";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: WebhookDefinitionDto) => void;
	groupName?: string;
	definitionName?: string;
}

const defaultModel: WebhookDefinitionDto = {
	displayName: "",
	extraProperties: {},
	groupName: "",
	isEnabled: true,
	isStatic: false,
	name: "",
	requiredFeatures: [],
} as WebhookDefinitionDto;

const WebhookDefinitionModal: React.FC<Props> = ({ visible, onClose, onChange, groupName, definitionName }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();
	const [activeTab, setActiveTab] = useState("basic");
	const [formModel, setFormModel] = useState<WebhookDefinitionDto>({ ...defaultModel });
	const [isEditModel, setIsEditModel] = useState(false);
	const [loading, setLoading] = useState(false);

	// Data State
	const [webhookGroups, setWebhookGroups] = useState<WebhookGroupDefinitionDto[]>([]);
	const [features, setFeatures] = useState<FeatureDefinitionDto[]>([]);
	const [featureGroups, setFeatureGroups] = useState<FeatureGroupDefinitionDto[]>([]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			initData();
		}
	}, [visible]);

	const initData = async () => {
		try {
			setLoading(true);
			const [groupRes, featureGroupRes, featureRes] = await Promise.all([
				getGroupDefinitionsApi({ filter: groupName }),
				hasAccessByCodes(["FeatureManagement.GroupDefinitions"])
					? getFeatureGroupsApi()
					: Promise.resolve({ items: [] }),
				hasAccessByCodes(["FeatureManagement.Definitions"]) ? getFeaturesApi() : Promise.resolve({ items: [] }),
			]);

			// 1. Process Webhook Groups
			const formattedWebhookGroups = groupRes.items.map((g) => {
				const d = deserialize(g.displayName);
				return { ...g, displayName: Lr(d.resourceName, d.name) };
			});
			setWebhookGroups(formattedWebhookGroups);

			// 2. Process Feature Groups
			const formattedFeatureGroups = featureGroupRes.items.map((g) => {
				const d = deserialize(g.displayName);
				return { ...g, displayName: Lr(d.resourceName, d.name) };
			});
			setFeatureGroups(formattedFeatureGroups);

			// 3. Process Features (Filter Boolean + Localize)
			const formattedFeatures = featureRes.items
				.filter((f) => {
					if (f.valueType) {
						try {
							const vt = valueTypeSerializer.deserialize(f.valueType);
							return vt.validator.name === "BOOLEAN";
						} catch {
							return false;
						}
					}
					return true;
				})
				.map((f) => {
					const d = deserialize(f.displayName);
					return {
						...f,
						displayName: Lr(d.resourceName, d.name),
						// Add UI properties for AntD Tree
						title: Lr(d.resourceName, d.name),
						value: f.name,
						key: f.name,
					};
				});
			setFeatures(formattedFeatures);

			// 4. Set Form Data
			if (definitionName) {
				setIsEditModel(true);
				const dto = await getApi(definitionName);
				setFormModel(dto);
				form.setFieldsValue(dto);
			} else {
				setIsEditModel(false);
				const initial = { ...defaultModel };
				if (groupName) initial.groupName = groupName;
				else if (formattedWebhookGroups.length === 1) initial.groupName = formattedWebhookGroups[0].name;

				setFormModel(initial);
				form.setFieldsValue(initial);
			}
		} finally {
			setLoading(false);
		}
	};

	// --- Fix 1: Build Tree Data Correctly ---
	const featureTreeData = useMemo(() => {
		return featureGroups.map((group) => {
			// Get features for this group
			const groupFeatures = features.filter((f) => f.groupName === group.name);

			// Convert flat list to tree
			const children = listToTree(groupFeatures, { id: "name", pid: "parentName" });

			return {
				title: group.displayName,
				value: group.name,
				key: group.name,
				selectable: false,
				checkable: false, // Groups are containers, not selectable features in this context
				disableCheckbox: true,
				children: children,
			};
		});
	}, [features, featureGroups]);

	// --- Fix 2: Map String[] to Object[] for Strict TreeSelect ---
	const requiredFeaturesValue = useMemo(() => {
		return (formModel.requiredFeatures || []).map((name) => {
			const feature = features.find((f) => f.name === name);
			return {
				label: feature?.displayName || name,
				value: name,
			};
		});
	}, [formModel.requiredFeatures, features]);

	const { mutateAsync: createDef, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		},
	});

	const { mutateAsync: updateDef, isPending: isUpdating } = useMutation({
		mutationFn: (data: WebhookDefinitionDto) => updateApi(data.name, data),
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
				// Don't take values.requiredFeatures directly if using strict mode manual handling,
				// or ensure it matches formModel.requiredFeatures
				requiredFeatures: formModel.requiredFeatures,
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

	// --- Fix 3: Handle TreeSelect Change ---
	const handleFeaturesChange = (labeledValues: { label: React.ReactNode; value: string }[]) => {
		const names = labeledValues.map((v) => v.value);
		setFormModel((prev) => ({ ...prev, requiredFeatures: names }));
		// Update form instance too for validation if needed, though we manage state manually
		form.setFieldValue("requiredFeatures", names);
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

	return (
		<Modal
			title={
				isEditModel
					? `${$t("WebhooksManagement.WebhookDefinitions")} - ${formModel.name}`
					: $t("WebhooksManagement.Webhooks:AddNew")
			}
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
					<Tabs.TabPane key="basic" tab={$t("WebhooksManagement.BasicInfo")}>
						<Form.Item name="isEnabled" valuePropName="checked" label={$t("WebhooksManagement.DisplayName:IsEnabled")}>
							<Checkbox disabled={formModel.isStatic}>{$t("WebhooksManagement.DisplayName:IsEnabled")}</Checkbox>
						</Form.Item>

						<Form.Item
							name="groupName"
							label={$t("WebhooksManagement.DisplayName:GroupName")}
							rules={[{ required: true }]}
						>
							<Select
								disabled={formModel.isStatic}
								options={webhookGroups}
								fieldNames={{ label: "displayName", value: "name" }}
								allowClear
							/>
						</Form.Item>

						<Form.Item name="name" label={$t("WebhooksManagement.DisplayName:Name")} rules={[{ required: true }]}>
							<Input disabled={formModel.isStatic || isEditModel} autoComplete="off" />
						</Form.Item>

						<Form.Item
							name="displayName"
							label={$t("WebhooksManagement.DisplayName:DisplayName")}
							rules={[{ required: true }]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						<Form.Item name="description" label={$t("WebhooksManagement.DisplayName:Description")}>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						{hasAccessByCodes(["FeatureManagement.GroupDefinitions", "FeatureManagement.Definitions"]) && (
							<Form.Item label={$t("WebhooksManagement.DisplayName:RequiredFeatures")}>
								<TreeSelect
									treeData={featureTreeData}
									value={requiredFeaturesValue} // Pass objects
									onChange={handleFeaturesChange} // Receive objects
									disabled={formModel.isStatic}
									allowClear
									treeCheckable
									treeCheckStrictly // Decouples selection
									showCheckedStrategy={TreeSelect.SHOW_ALL}
									placeholder={$t("WebhooksManagement.DisplayName:RequiredFeatures")}
									treeDefaultExpandAll
									fieldNames={{ label: "title", value: "value", children: "children" }}
								/>
							</Form.Item>
						)}
					</Tabs.TabPane>

					<Tabs.TabPane key="props" tab={$t("WebhooksManagement.Properties")}>
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

export default WebhookDefinitionModal;
