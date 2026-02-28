import { useEffect, useRef, useState } from "react";
import { Form, Input, Modal, Tabs, Select, TreeSelect, InputNumber, Checkbox } from "antd";
import { useTranslation } from "react-i18next";
import type { FeatureDefinitionDto } from "#/management/features/definitions";
import type { FeatureGroupDefinitionDto } from "#/management/features/groups";
import type { PropertyInfo } from "@/components/abp/properties/types";
import {
	createApi,
	getApi,
	getListApi as getDefinitionsApi,
	updateApi,
} from "@/api/management/features/feature-definitions";
import { getListApi as getGroupsApi } from "@/api/management/features/feature-group-definitions";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import PropertyTable from "@/components/abp/properties/property-table";
import ValueTypeInput, { type ValueTypeInputHandle } from "@/components/abp/string-value-type/string-value-type-input";
import { listToTree } from "@/utils/tree";
import { toast } from "sonner";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";
import {
	valueTypeSerializer,
	FreeTextStringValueType,
	SelectionStringValueType,
	ToggleStringValueType,
} from "@/components/abp/string-value-type";
import { useValidation } from "@/hooks/abp/use-validation";
import { ValidationEnum } from "@/constants/abp-core";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: FeatureDefinitionDto) => void;
	featureName?: string;
	groupName?: string;
}

type TabKeys = "basic" | "valueType" | "props";

const defaultModel: FeatureDefinitionDto = {
	allowedProviders: [],
	displayName: "",
	extraProperties: {},
	groupName: "",
	isAvailableToHost: true,
	isEnabled: true,
	isStatic: false,
	isVisibleToClients: false,
	name: "",
	valueType: new FreeTextStringValueType() as any, // Initial serialized or object
} as FeatureDefinitionDto;

const FeatureDefinitionModal: React.FC<Props> = ({ visible, onClose, onChange, featureName, groupName }) => {
	const { t: $t } = useTranslation();
	const { Lr } = useLocalizer();
	const { deserialize, validate: validateLocalizer } = localizationSerializer();
	const queryClient = useQueryClient();
	const [form] = Form.useForm();

	// Refs & State
	const valueTypeInputRef = useRef<ValueTypeInputHandle>(null);
	const [formModel, setFormModel] = useState<FeatureDefinitionDto>({ ...defaultModel });
	const [isEditModel, setIsEditModel] = useState(false);
	const [activeTab, setActiveTab] = useState<TabKeys>("basic");

	// Options
	const [availableGroups, setAvailableGroups] = useState<FeatureGroupDefinitionDto[]>([]);
	const [availableDefinitions, setAvailableDefinitions] = useState<any[]>([]);

	// Dynamic Default Value Control logic
	const [currentValueTypeName, setCurrentValueTypeName] = useState<string>("FreeTextStringValueType");
	const [currentValidatorName, setCurrentValidatorName] = useState<string>("NULL");
	const [selectionOptions, setSelectionOptions] = useState<{ label: string; value: string }[]>([]);

	//
	const { mapEnumValidMessage } = useValidation();
	// for checkbox refreshing issue
	const defaultValue = Form.useWatch("defaultValue", form);

	// --- API Mutations ---

	const { mutateAsync: fetchGroups } = useMutation({
		mutationFn: getGroupsApi,
		onSuccess: (res) => {
			const groups = res.items.map((group) => {
				const localizableGroup = deserialize(group.displayName);
				return {
					...group,
					displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
				};
			});
			setAvailableGroups(groups);
		},
	});

	const { mutateAsync: fetchDefinitions } = useMutation({
		mutationFn: getDefinitionsApi,
		onSuccess: (res) => {
			const features = res.items.map((item) => {
				const displayName = deserialize(item.displayName);
				return {
					...item,
					disabled: item.name === formModel.name, // Prevent selecting self as parent
					title: Lr(displayName.resourceName, displayName.name),
					value: item.name,
					key: item.name,
				};
			});
			setAvailableDefinitions(listToTree(features, { id: "name", pid: "parentName" }));
		},
	});

	const { mutateAsync: fetchFeature, isPending: isFetching } = useMutation({
		mutationFn: getApi,
		onSuccess: (dto) => {
			setFormModel(dto);
			form.setFieldsValue(dto);
			// Initialize dynamic states based on fetched data
			handleValueTypeStateUpdate(dto.valueType);

			// Load definitions for the group
			if (dto.groupName) {
				fetchDefinitions({ groupName: dto.groupName });
			}
		},
	});

	const { mutateAsync: createFeature, isPending: isCreating } = useMutation({
		mutationFn: createApi,
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["featureDefinitions"] });
			onChange(res);
			onClose();
		},
	});

	const { mutateAsync: updateFeature, isPending: isUpdating } = useMutation({
		mutationFn: (data: FeatureDefinitionDto) => updateApi(data.name, data),
		onSuccess: (res) => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			queryClient.invalidateQueries({ queryKey: ["featureDefinitions"] });
			onChange(res);
			onClose();
		},
	});

	// --- Effects ---

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			setFormModel({ ...defaultModel });
			form.resetFields();

			// Reset dynamic states
			setCurrentValueTypeName("FreeTextStringValueType");
			setCurrentValidatorName("NULL");
			setSelectionOptions([]);
			setAvailableDefinitions([]);

			const init = async () => {
				await fetchGroups({});
				if (featureName) {
					setIsEditModel(true);
					await fetchFeature(featureName);
				} else {
					setIsEditModel(false);
					// Pre-select group if provided
					if (groupName) {
						form.setFieldValue("groupName", groupName);
						setFormModel((prev) => ({ ...prev, groupName: groupName }));
						await fetchDefinitions({ groupName });
					}
				}
			};
			init();
		}
	}, [visible, featureName, groupName]);

	// --- Handlers ---

	const handleGroupChange = async (val: string) => {
		setFormModel((prev) => ({ ...prev, groupName: val }));
		if (val) {
			await fetchDefinitions({ groupName: val });
		} else {
			setAvailableDefinitions([]);
		}
	};

	/**
	 * Helper to parse valueType string/obj and update local state
	 * to determine which input widget to show for DefaultValue
	 */
	const handleValueTypeStateUpdate = (val: string | any) => {
		let vTypeObj: any;
		if (typeof val === "string") {
			try {
				vTypeObj = valueTypeSerializer.deserialize(val);
			} catch {
				return;
			}
		} else {
			vTypeObj = val;
		}

		if (!vTypeObj) return;

		setCurrentValueTypeName(vTypeObj.name);
		setCurrentValidatorName(vTypeObj.validator?.name || "NULL");

		if (vTypeObj instanceof SelectionStringValueType) {
			const options = vTypeObj.itemSource.items.map((item) => ({
				label: Lr(item.displayText.resourceName, item.displayText.name),
				value: item.value,
			}));
			setSelectionOptions(options);

			// Clear default value if current isn't in options
			const currentDef = form.getFieldValue("defaultValue");
			if (currentDef && !options.find((o) => o.value === currentDef)) {
				form.setFieldValue("defaultValue", undefined);
			}
		} else if (vTypeObj instanceof ToggleStringValueType) {
			// Ensure default value is boolean-string
			const currentDef = form.getFieldValue("defaultValue");
			if (currentDef !== "true" && currentDef !== "false") {
				form.setFieldValue("defaultValue", "false");
			}
		}
	};

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			const submitData = {
				...formModel,
				...values,
			};

			// Ensure defaultValue is string
			if (submitData.defaultValue !== undefined && typeof submitData.defaultValue !== "string") {
				submitData.defaultValue = String(submitData.defaultValue);
			}

			if (isEditModel) {
				await updateFeature(submitData);
			} else {
				await createFeature(submitData);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handlePropChange = (prop: PropertyInfo) => {
		setFormModel((prev) => ({
			...prev,
			extraProperties: {
				...prev.extraProperties,
				[prop.key]: prop.value,
			},
		}));
	};

	const handlePropDelete = (prop: PropertyInfo) => {
		setFormModel((prev) => {
			const newProps = { ...prev.extraProperties };
			delete newProps[prop.key];
			return {
				...prev,
				extraProperties: newProps,
			};
		});
	};

	// Render the specific input for Default Value based on ValueType settings
	const renderDefaultValueInput = () => {
		if (currentValueTypeName === "SelectionStringValueType") {
			return <Select allowClear options={selectionOptions} disabled={formModel.isStatic} />;
		}

		if (currentValueTypeName === "ToggleStringValueType" && currentValidatorName === "BOOLEAN") {
			return (
				<Checkbox
					disabled={formModel.isStatic}
					checked={defaultValue === "true"}
					onChange={(e) => form.setFieldValue("defaultValue", String(e.target.checked).toLowerCase())}
				>
					{$t("AbpFeatureManagement.DisplayName:DefaultValue")}
				</Checkbox>
			);
		}

		if (currentValueTypeName === "FreeTextStringValueType" && currentValidatorName === "NUMERIC") {
			return <InputNumber className="w-full" disabled={formModel.isStatic} />;
		}

		// Default: Textarea
		return <Input.TextArea autoSize={{ minRows: 3 }} allowClear disabled={formModel.isStatic} />;
	};

	return (
		<Modal
			title={
				isEditModel
					? `${$t("AbpFeatureManagement.FeatureDefinitions")} - ${formModel.name}`
					: $t("AbpFeatureManagement.FeatureDefinitions:AddNew")
			}
			open={visible}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isCreating || isUpdating || isFetching}
			okButtonProps={{ disabled: formModel.isStatic }}
			width="50%"
		>
			<Form form={form} layout="vertical">
				<Tabs activeKey={activeTab} onChange={(key) => setActiveTab(key as TabKeys)}>
					{/* Basic Info */}
					<Tabs.TabPane key="basic" tab={$t("AbpFeatureManagement.BasicInfo")}>
						<Form.Item
							label={$t("AbpFeatureManagement.DisplayName:GroupName")}
							name="groupName"
							rules={[{ required: true }]}
						>
							<Select
								showSearch
								optionFilterProp="displayName"
								fieldNames={{ label: "displayName", value: "name" }}
								options={availableGroups}
								onChange={handleGroupChange}
								disabled={formModel.isStatic}
								allowClear
							/>
						</Form.Item>

						{availableDefinitions.length > 0 && (
							<Form.Item label={$t("AbpFeatureManagement.DisplayName:ParentName")} name="parentName">
								<TreeSelect
									treeData={availableDefinitions}
									disabled={formModel.isStatic}
									allowClear
									treeDefaultExpandAll
								/>
							</Form.Item>
						)}

						<Form.Item label={$t("AbpFeatureManagement.DisplayName:Name")} name="name" rules={[{ required: true }]}>
							<Input disabled={formModel.isStatic} autoComplete="off" />
						</Form.Item>

						<Form.Item
							label={$t("AbpFeatureManagement.DisplayName:DisplayName")}
							name="displayName"
							rules={[
								{ required: true },
								{
									validator: async (_, value) => {
										if (!validateLocalizer(value)) {
											return Promise.reject(
												mapEnumValidMessage(ValidationEnum.FieldRequired, [
													$t("AbpFeatureManagement.DisplayName:DisplayName"),
												]),
											);
										}
										return Promise.resolve();
									},
								},
							]}
						>
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						<Form.Item label={$t("AbpFeatureManagement.DisplayName:Description")} name="description">
							<LocalizableInput disabled={formModel.isStatic} />
						</Form.Item>

						{/* Dynamic Default Value Input */}
						{/* Note: Logic for Toggle checkbox handles label inside, others outside */}
						<Form.Item
							label={$t("AbpFeatureManagement.DisplayName:DefaultValue")}
							name="defaultValue"
							rules={[
								{
									validator: async (_, value) => {
										if (valueTypeInputRef.current) {
											return valueTypeInputRef.current.validate(value);
										}
										return Promise.resolve();
									},
								},
							]}
						>
							{renderDefaultValueInput()}
						</Form.Item>

						<Form.Item name="isVisibleToClients" valuePropName="checked">
							<Checkbox>{$t("AbpFeatureManagement.DisplayName:IsVisibleToClients")}</Checkbox>
						</Form.Item>
						<div className="mb-4 text-gray-400 text-xs">
							{$t("AbpFeatureManagement.Description:IsVisibleToClients")}
						</div>

						<Form.Item name="isAvailableToHost" valuePropName="checked">
							<Checkbox>{$t("AbpFeatureManagement.DisplayName:IsAvailableToHost")}</Checkbox>
						</Form.Item>
						<div className="mb-4 text-gray-400 text-xs">{$t("AbpFeatureManagement.Description:IsAvailableToHost")}</div>
					</Tabs.TabPane>

					{/* Value Validator Type Configuration */}
					<Tabs.TabPane key="valueType" tab={$t("AbpFeatureManagement.ValueValidator")}>
						<Form.Item name="valueType">
							<ValueTypeInput
								ref={valueTypeInputRef}
								disabled={formModel.isStatic}
								allowDelete={true}
								allowEdit={true}
								onChange={(val) => {
									// When internal configuration changes, update the local state for the UI
									handleValueTypeStateUpdate(val);
								}}
								// Sync up local state changes immediately
								onValueTypeChange={(type) => setCurrentValueTypeName(type)}
								onValidatorChange={(val) => setCurrentValidatorName(val)}
								onSelectionChange={(items) => {
									setSelectionOptions(
										items.map((i) => ({
											label: Lr(i.displayText.resourceName, i.displayText.name),
											value: i.value,
										})),
									);
								}}
							/>
						</Form.Item>
					</Tabs.TabPane>

					{/* Properties */}
					<Tabs.TabPane key="props" tab={$t("AbpFeatureManagement.Properties")}>
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

export default FeatureDefinitionModal;
