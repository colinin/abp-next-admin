import type { Validator } from "#/abp-core";
import type React from "react";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { getApi, updateApi } from "@/api/management/features/features";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import type { FeatureGroupDto, UpdateFeaturesDto } from "#/management/features/features";

import { Modal, Form, Tabs, Card, Input, InputNumber, Checkbox, Select, Spin } from "antd";
import { useValidation } from "@/hooks/abp/use-validation";
import { useLocalizer } from "@/hooks/abp/use-localization";

interface FeatureManagementModalProps {
	visible: boolean;
	onClose: () => void;
	providerName: string;
	providerKey?: string;
	displayName?: string; // For the modal title
}

// Helper Interface for Form Data Structure
export interface FeatureFormData {
	groups: FeatureGroupDto[];
}

const FeatureManagementModal: React.FC<FeatureManagementModalProps> = ({
	visible,
	onClose,
	providerName,
	providerKey,
	displayName,
}) => {
	const { t: $t } = useTranslation();
	const { Lr } = useLocalizer();
	const [form] = Form.useForm();

	// Validation Hook
	const { fieldMustBeetWeen, fieldMustBeStringWithMinimumLengthAndMaximumLength, fieldRequired } = useValidation();

	// State
	const [activeTabKey, setActiveTabKey] = useState<string>("");
	const [groups, setGroups] = useState<FeatureGroupDto[]>([]);

	// --- Helpers ---

	/**
	 * Generates AntD Form Rules based on ABP Validator metadata (custom hook)
	 */
	const createRules = (fieldLabel: string, validator: Validator) => {
		const rules: any[] = [];
		if (validator?.properties) {
			switch (validator.name) {
				case "NUMERIC": {
					rules.push(
						...fieldMustBeetWeen({
							name: fieldLabel,
							start: Number(validator.properties.MinValue),
							end: Number(validator.properties.MaxValue),
							trigger: "change",
						}),
					);
					break;
				}
				case "STRING": {
					if (validator.properties.AllowNull && String(validator.properties.AllowNull).toLowerCase() === "true") {
						rules.push(
							...fieldRequired({
								name: fieldLabel,
								trigger: "blur",
							}),
						);
					}
					rules.push(
						...fieldMustBeStringWithMinimumLengthAndMaximumLength({
							name: fieldLabel,
							minimum: Number(validator.properties.MinLength),
							maximum: Number(validator.properties.MaxLength),
							trigger: "blur",
						}),
					);
					break;
				}
				default:
					break;
			}
		}
		return rules;
	};

	/**
	 * Process raw API data for the UI
	 * 1. Localize Selection items
	 * 2. Convert string values to boolean/number for inputs
	 */
	const mapFeaturesForUi = (rawGroups: FeatureGroupDto[]) => {
		// Deep clone to avoid mutating read-only props if using strict mode
		const processedGroups = JSON.parse(JSON.stringify(rawGroups)) as FeatureGroupDto[];

		processedGroups.forEach((group) => {
			group.features.forEach((feature) => {
				// Handle Selection Localization
				if (feature.valueType?.name === "SelectionStringValueType") {
					const valueType: any = feature.valueType;
					if (valueType.itemSource?.items) {
						valueType.itemSource.items.forEach((item: any) => {
							if (item.displayText?.resourceName === "Fixed") {
								item.displayName = item.displayText.name;
							} else {
								item.displayName = Lr(item.displayText.resourceName, item.displayText.name);
							}
						});
					}
				}

				// Handle Value Conversion
				else if (feature.valueType?.validator) {
					switch (feature.valueType.validator.name) {
						case "BOOLEAN":
							feature.value = String(feature.value).toLowerCase() === "true";
							break;
						case "NUMERIC":
							feature.value = Number(feature.value);
							break;
						default:
							// Keep as string
							break;
					}
				}
			});
		});

		return processedGroups;
	};

	/**
	 * Convert Form Data back to DTO for API submission
	 */
	const getFeatureInput = (formValues: any): UpdateFeaturesDto => {
		const input: UpdateFeaturesDto = { features: [] };

		const formGroups = formValues.groups as FeatureGroupDto[];
		if (!formGroups) return input;

		formGroups.forEach((g) => {
			if (!g?.features) return;
			g.features.forEach((f) => {
				// Only include non-null values
				if (f.value !== null && f.value !== undefined) {
					input.features.push({
						name: f.name,
						value: String(f.value), // Convert back to string
					});
				}
			});
		});

		return input;
	};

	// --- API Hooks ---

	const { mutateAsync: fetchData, isPending: isLoading } = useMutation({
		mutationFn: async () => {
			if (!providerName) return;
			const res = await getApi({ providerName, providerKey });
			return res.groups;
		},
		onSuccess: (rawGroups) => {
			if (rawGroups) {
				const processed = mapFeaturesForUi(rawGroups);
				setGroups(processed);
				form.setFieldsValue({ groups: processed });

				// Set active tab to first group
				if (processed.length > 0 && !activeTabKey) {
					setActiveTabKey(processed[0].name);
				}
			}
		},
	});

	const { mutateAsync: updateData, isPending: isSaving } = useMutation({
		mutationFn: async (input: UpdateFeaturesDto) => {
			return updateApi({ providerName, providerKey }, input);
		},
		onSuccess: () => {
			toast.success($t("AbpUi.SavedSuccessfully"));
			onClose();
		},
	});

	// --- Effects ---

	useEffect(() => {
		if (visible) {
			form.resetFields();
			setGroups([]);
			setActiveTabKey("");
			fetchData();
		}
	}, [visible, providerName, providerKey]);

	// --- Handlers ---

	const handleOk = async () => {
		try {
			const values = await form.validateFields();
			const input = getFeatureInput(values);
			await updateData(input);
		} catch (e) {
			console.error("Validation Failed", e);
		}
	};

	// --- Renderers ---

	const renderFeatureInput = (feature: any, groupIndex: number, featureIndex: number) => {
		if (!feature.valueType) return null;

		const fieldName = ["groups", groupIndex, "features", featureIndex, "value"];
		const valueTypeName = feature.valueType.name;
		const validatorName = feature.valueType.validator?.name;

		// 1. Checkbox (Boolean)
		if (valueTypeName === "ToggleStringValueType" && validatorName === "BOOLEAN") {
			return (
				<Form.Item
					name={fieldName}
					valuePropName="checked"
					label={feature.displayName}
					extra={feature.description}
					rules={createRules(feature.displayName, feature.valueType.validator)}
				>
					<Checkbox>{feature.displayName}</Checkbox>
				</Form.Item>
			);
		}

		// 2. Select (Selection)
		if (valueTypeName === "SelectionStringValueType") {
			return (
				<Form.Item
					name={fieldName}
					label={feature.displayName}
					extra={feature.description}
					rules={createRules(feature.displayName, feature.valueType.validator)}
				>
					<Select
						options={feature.valueType.itemSource?.items || []}
						fieldNames={{ label: "displayName", value: "value" }}
					/>
				</Form.Item>
			);
		}

		// 3. Free Text (String or Numeric)
		if (valueTypeName === "FreeTextStringValueType") {
			return (
				<Form.Item
					name={fieldName}
					label={feature.displayName}
					extra={feature.description}
					rules={createRules(feature.displayName, feature.valueType.validator)}
				>
					{validatorName === "NUMERIC" ? <InputNumber style={{ width: "100%" }} /> : <Input autoComplete="off" />}
				</Form.Item>
			);
		}

		// Fallback
		return (
			<Form.Item name={fieldName} label={feature.displayName} extra={feature.description}>
				<Input />
			</Form.Item>
		);
	};

	const modalTitle = displayName
		? `${$t("AbpFeatureManagement.Features")} - ${displayName}`
		: $t("AbpFeatureManagement.Features");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleOk}
			confirmLoading={isSaving}
			centered
			width="50%"
			maskClosable={false}
			destroyOnClose
		>
			<Spin spinning={isLoading}>
				<Form form={form} layout="vertical">
					{/* We render hidden inputs for names to ensure they exist in the form values structure for getFeatureInput logic
					 */}
					{groups.map((g, gIdx) => (
						<div key={g.name} style={{ display: "none" }}>
							{g.features.map((f, fIdx) => (
								<Form.Item key={f.name} name={["groups", gIdx, "features", fIdx, "name"]} initialValue={f.name}>
									<Input />
								</Form.Item>
							))}
						</div>
					))}

					<Tabs
						tabPosition="left"
						activeKey={activeTabKey}
						onChange={setActiveTabKey}
						items={groups.map((group, groupIndex) => ({
							key: group.name,
							label: group.displayName,
							children: (
								<div className="h-[34rem] overflow-y-auto pr-2">
									<Card title={group.displayName} bordered={false}>
										{group.features.map((feature, featureIndex) => (
											<div key={feature.name}>{renderFeatureInput(feature, groupIndex, featureIndex)}</div>
										))}
									</Card>
								</div>
							),
						}))}
					/>
				</Form>
			</Spin>
		</Modal>
	);
};

export default FeatureManagementModal;
