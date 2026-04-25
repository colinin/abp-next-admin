import { forwardRef, useEffect, useImperativeHandle, useMemo, useState } from "react";
import { Button, Card, Checkbox, Col, Form, Input, InputNumber, Modal, Row, Select, Table, Space } from "antd";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { useLocalizer } from "@/hooks/abp/use-localization";
import {
	valueTypeSerializer,
	FreeTextStringValueType,
	SelectionStringValueType,
	ToggleStringValueType,
	type StringValueType,
	type SelectionStringValueItem,
} from "./value-type";
import {
	AlwaysValidValueValidator,
	BooleanValueValidator,
	NumericValueValidator,
	StringValueValidator,
} from "./validator";
import LocalizableInput from "@/components/abp/localizable-input/localizable-input";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import type { LocalizableStringInfo } from "#/abp-core";

export interface ValueTypeInputProps {
	allowDelete?: boolean;
	allowEdit?: boolean;
	disabled?: boolean;
	value?: string;
	onChange?: (value: string | undefined) => void;
	onValueTypeChange?: (type: string) => void;
	onValidatorChange?: (validator: string) => void;
	onSelectionChange?: (items: SelectionStringValueItem[]) => void;
}

export interface ValueTypeInputHandle {
	validate: (value: any) => Promise<any>;
}

const ValueTypeInput = forwardRef<ValueTypeInputHandle, ValueTypeInputProps>(
	(
		{
			allowDelete = false,
			allowEdit = false,
			disabled = false,
			value = "{}",
			onChange,
			onValueTypeChange,
			onValidatorChange,
			onSelectionChange,
		},
		ref,
	) => {
		const { t: $t } = useTranslation();
		const { Lr } = useLocalizer();

		const {
			deserialize: deserializeLocalizer,
			serialize: serializeLocalizer,
			validate: validateLocalizer,
		} = localizationSerializer();

		// Internal State
		const [valueType, setValueType] = useState<StringValueType>(new FreeTextStringValueType());

		// Modal State for Selection Type
		const [modalVisible, setModalVisible] = useState(false);
		const [modalForm] = Form.useForm();
		const [editingItem, setEditingItem] = useState<{
			isEdit: boolean;
			displayText?: string;
		}>({ isEdit: false });

		// Initialize/Sync State from Props
		useEffect(() => {
			if (!value || value.trim() === "" || value === "{}") {
				setValueType(new FreeTextStringValueType());
			} else {
				try {
					const deserialized = valueTypeSerializer.deserialize(value);
					setValueType(deserialized);
				} catch (e) {
					console.warn("Failed to deserialize valueType", e);
				}
			}
		}, [value]);

		// Notify Parent of Changes
		const triggerChange = (newValueType: StringValueType) => {
			// We need to create a new object reference or clone to ensure React detects state changes
			// Serialization/Deserialization is a safe way to deep clone and ensure logic consistency
			const serialized = valueTypeSerializer.serialize(newValueType);

			// Update internal state only if not controlled (optimization),
			// but here we rely on parent prop update usually.
			// However, to make UI responsive immediately for nested properties:
			setValueType(valueTypeSerializer.deserialize(serialized));

			if (onChange) onChange(serialized);
			if (onValueTypeChange) onValueTypeChange(newValueType.name);
			if (onValidatorChange) onValidatorChange(newValueType.validator.name);

			if (newValueType instanceof SelectionStringValueType && onSelectionChange) {
				onSelectionChange(newValueType.itemSource.items);
			}
		};

		// Expose Validate Method
		useImperativeHandle(ref, () => ({
			validate: async (val: any) => {
				if (valueType instanceof SelectionStringValueType) {
					const items = valueType.itemSource.items;
					if (items.length === 0) {
						return Promise.reject($t("component.value_type_nput.type.SELECTION.itemsNotBeEmpty"));
					}
					if (val && !items.some((item) => item.value === val)) {
						return Promise.reject($t("component.value_type_nput.type.SELECTION.itemsNotFound"));
					}
				}

				if (!valueType.validator.isValid(val)) {
					const validatorNameKey = `component.value_type_nput.validator.${valueType.validator.name}.name`;
					return Promise.reject(
						$t("component.value_type_nput.validator.isInvalidValue", {
							0: $t(validatorNameKey),
						}),
					);
				}
				return Promise.resolve(val);
			},
		}));

		// Handlers
		const handleValueTypeChange = (type: string) => {
			let newValueType: StringValueType;
			switch (type) {
				case "SELECTION":
				case "SelectionStringValueType":
					newValueType = new SelectionStringValueType();
					break;
				case "TOGGLE":
				case "ToggleStringValueType":
					newValueType = new ToggleStringValueType();
					break;
				default:
					newValueType = new FreeTextStringValueType();
					break;
			}
			triggerChange(newValueType);
		};

		const handleValidatorChange = (validatorName: string) => {
			const newValueType = valueTypeSerializer.deserialize(valueTypeSerializer.serialize(valueType));

			switch (validatorName) {
				case "BOOLEAN":
					newValueType.validator = new BooleanValueValidator();
					break;
				case "NULL":
					newValueType.validator = new AlwaysValidValueValidator();
					break;
				case "NUMERIC":
					newValueType.validator = new NumericValueValidator();
					break;
				default:
					newValueType.validator = new StringValueValidator();
					break;
			}
			triggerChange(newValueType);
		};

		// Generic Property Update Handler (Deep Update)
		const updateValidatorProperty = (updater: (validator: any) => void) => {
			const serialized = valueTypeSerializer.serialize(valueType);
			const cloned = valueTypeSerializer.deserialize(serialized);
			updater(cloned.validator);
			triggerChange(cloned);
		};

		// Selection Logic
		const handleAddSelection = () => {
			setEditingItem({ isEdit: false });
			modalForm.resetFields();
			setModalVisible(true);
		};

		const handleClearSelection = () => {
			if (valueType instanceof SelectionStringValueType) {
				const cloned = valueTypeSerializer.deserialize(
					valueTypeSerializer.serialize(valueType),
				) as SelectionStringValueType;
				cloned.itemSource.items = [];
				triggerChange(cloned);
			}
		};

		const handleEditSelection = (record: SelectionStringValueItem) => {
			setEditingItem({ isEdit: true, displayText: serializeLocalizer(record.displayText) });
			modalForm.setFieldsValue({
				displayText: serializeLocalizer(record.displayText),
				value: record.value,
			});
			setModalVisible(true);
		};

		const handleDeleteSelection = (record: SelectionStringValueItem) => {
			if (valueType instanceof SelectionStringValueType) {
				const displayText = serializeLocalizer(record.displayText);
				const cloned = valueType as SelectionStringValueType;
				cloned.itemSource.items = cloned.itemSource.items.filter(
					(x) => serializeLocalizer(x.displayText) !== displayText,
				);

				triggerChange(cloned);
			}
		};

		const handleModalOk = async () => {
			try {
				const values = await modalForm.validateFields();
				if (valueType instanceof SelectionStringValueType) {
					const cloned = valueTypeSerializer.deserialize(
						valueTypeSerializer.serialize(valueType),
					) as SelectionStringValueType;

					if (editingItem.isEdit) {
						const index = cloned.itemSource.items.findIndex(
							(x) => serializeLocalizer(x.displayText) === editingItem.displayText,
						);
						if (index > -1) {
							cloned.itemSource.items[index] = {
								displayText: values.displayText,
								value: values.value,
							};
						}
					} else {
						cloned.itemSource.items.push({
							displayText: deserializeLocalizer(values.displayText),
							value: values.value,
						});
					}
					triggerChange(cloned);
					setModalVisible(false);
					modalForm.resetFields();
				}
			} catch (e) {
				// Validation failed
				console.warn("Validation failed in handleModalOk", e);
			}
		};

		// Columns for Selection Table
		const selectionColumns = useMemo(() => {
			const cols: any[] = [
				{
					title: $t("component.value_type_nput.type.SELECTION.displayText"),
					dataIndex: "displayText",
					key: "displayText",
					width: 180,
					render: (text: LocalizableStringInfo) => Lr(text.resourceName, text.name),
				},
				{
					title: $t("component.value_type_nput.type.SELECTION.value"),
					dataIndex: "value",
					key: "value",
					width: 200,
				},
			];

			if (!disabled) {
				cols.push({
					title: $t("component.value_type_nput.type.SELECTION.actions.title"),
					key: "action",
					width: 220,
					render: (_: any, record: SelectionStringValueItem) => (
						<Space>
							{allowEdit && (
								<Button type="link" icon={<EditOutlined />} onClick={() => handleEditSelection(record)}>
									{$t("component.value_type_nput.type.SELECTION.actions.update")}
								</Button>
							)}
							{allowDelete && (
								<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDeleteSelection(record)}>
									{$t("component.value_type_nput.type.SELECTION.actions.delete")}
								</Button>
							)}
						</Space>
					),
				});
			}
			return cols;
		}, [disabled, allowEdit, allowDelete, $t, Lr]);

		return (
			<div className="w-full">
				<Card
					title={
						<div className="w-full">
							<Row gutter={16}>
								<Col span={11}>{$t("component.value_type_nput.type.name")}</Col>
								<Col span={11} offset={2}>
									{$t("component.value_type_nput.validator.name")}
								</Col>
							</Row>
							<Row gutter={16} className="mt-2">
								<Col span={11}>
									<Select
										className="w-full"
										disabled={disabled}
										value={valueType.name}
										onChange={handleValueTypeChange}
										options={[
											{
												label: $t("component.value_type_nput.type.FREE_TEXT.name"),
												value: "FreeTextStringValueType",
											},
											{
												label: $t("component.value_type_nput.type.TOGGLE.name"),
												value: "ToggleStringValueType",
											},
											{
												label: $t("component.value_type_nput.type.SELECTION.name"),
												value: "SelectionStringValueType",
											},
										]}
									/>
								</Col>
								<Col span={11} offset={2}>
									<Select
										className="w-full"
										disabled={disabled}
										value={valueType.validator.name}
										onChange={handleValidatorChange}
									>
										<Select.Option value="NULL">{$t("component.value_type_nput.validator.NULL.name")}</Select.Option>
										<Select.Option value="BOOLEAN" disabled={valueType.name !== "ToggleStringValueType"}>
											{$t("component.value_type_nput.validator.BOOLEAN.name")}
										</Select.Option>
										<Select.Option value="NUMERIC" disabled={valueType.name !== "FreeTextStringValueType"}>
											{$t("component.value_type_nput.validator.NUMERIC.name")}
										</Select.Option>
										<Select.Option value="STRING" disabled={valueType.name !== "FreeTextStringValueType"}>
											{$t("component.value_type_nput.validator.STRING.name")}
										</Select.Option>
									</Select>
								</Col>
							</Row>
						</div>
					}
				>
					{/* FreeText - NUMERIC */}
					{valueType.name === "FreeTextStringValueType" && valueType.validator.name === "NUMERIC" && (
						<div>
							<Row gutter={16}>
								<Col span={11}>{$t("component.value_type_nput.validator.NUMERIC.minValue")}</Col>
								<Col span={11} offset={2}>
									{$t("component.value_type_nput.validator.NUMERIC.maxValue")}
								</Col>
							</Row>
							<Row gutter={16} className="mt-1">
								<Col span={11}>
									<InputNumber
										className="w-full"
										disabled={disabled}
										value={(valueType.validator as NumericValueValidator).minValue}
										onChange={(val) => updateValidatorProperty((v) => (v.minValue = val ? Number(val) : undefined))}
									/>
								</Col>
								<Col span={11} offset={2}>
									<InputNumber
										className="w-full"
										disabled={disabled}
										value={(valueType.validator as NumericValueValidator).maxValue}
										onChange={(val) => updateValidatorProperty((v) => (v.maxValue = val ? Number(val) : undefined))}
									/>
								</Col>
							</Row>
						</div>
					)}

					{/* FreeText - STRING */}
					{valueType.name === "FreeTextStringValueType" && valueType.validator.name === "STRING" && (
						<div className="flex flex-col gap-4 mt-2">
							<Checkbox
								disabled={disabled}
								checked={(valueType.validator as StringValueValidator).allowNull}
								onChange={(e) => updateValidatorProperty((v) => (v.allowNull = e.target.checked))}
							>
								{$t("component.value_type_nput.validator.STRING.allowNull")}
							</Checkbox>

							<div>
								<div className="mb-1">{$t("component.value_type_nput.validator.STRING.regularExpression")}</div>
								<Input
									className="w-full"
									disabled={disabled}
									value={(valueType.validator as StringValueValidator).regularExpression}
									onChange={(e) => updateValidatorProperty((v) => (v.regularExpression = e.target.value))}
								/>
							</div>

							<div>
								<Row gutter={16}>
									<Col span={11}>{$t("component.value_type_nput.validator.STRING.minLength")}</Col>
									<Col span={11} offset={2}>
										{$t("component.value_type_nput.validator.STRING.maxLength")}
									</Col>
								</Row>
								<Row gutter={16} className="mt-1">
									<Col span={11}>
										<InputNumber
											className="w-full"
											disabled={disabled}
											value={(valueType.validator as StringValueValidator).minLength}
											onChange={(val) => updateValidatorProperty((v) => (v.minLength = val ? Number(val) : undefined))}
										/>
									</Col>
									<Col span={11} offset={2}>
										<InputNumber
											className="w-full"
											disabled={disabled}
											value={(valueType.validator as StringValueValidator).maxLength}
											onChange={(val) => updateValidatorProperty((v) => (v.maxLength = val ? Number(val) : undefined))}
										/>
									</Col>
								</Row>
							</div>
						</div>
					)}

					{/* SELECTION Type */}
					{valueType instanceof SelectionStringValueType && (
						<Card
							type="inner"
							title={
								<Row justify="space-between" align="middle">
									<Col>
										{valueType.itemSource.items.length <= 0 && (
											<span className="text-red-500">
												{$t("component.value_type_nput.type.SELECTION.itemsNotBeEmpty")}
											</span>
										)}
									</Col>
									<Col>
										{!disabled && (
											<Space>
												<Button type="primary" onClick={handleAddSelection} icon={<PlusOutlined />}>
													{$t("component.value_type_nput.type.SELECTION.actions.create")}
												</Button>
												<Button danger onClick={handleClearSelection}>
													{$t("component.value_type_nput.type.SELECTION.actions.clean")}
												</Button>
											</Space>
										)}
									</Col>
								</Row>
							}
						>
							<Table
								rowKey="value"
								columns={selectionColumns}
								dataSource={valueType.itemSource.items}
								pagination={false}
								size="small"
								scroll={{ x: true }}
							/>
						</Card>
					)}
				</Card>

				{/* Modal for Selection Item */}
				<Modal
					title={$t("component.value_type_nput.type.SELECTION.modal.title")}
					open={modalVisible}
					onOk={handleModalOk}
					onCancel={() => {
						setModalVisible(false);
						modalForm.resetFields();
					}}
					maskClosable={false}
					width={600}
				>
					<Form form={modalForm} layout="horizontal" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
						<Form.Item
							name="displayText"
							label={$t("component.value_type_nput.type.SELECTION.displayText")}
							rules={[
								{
									validator: async (_, value) => {
										// Logic to validate duplicate Display Text
										if (!validateLocalizer(value)) {
											return Promise.reject($t("component.value_type_nput.type.SELECTION.displayTextNotBeEmpty"));
										}
										if (valueType instanceof SelectionStringValueType) {
											// const serializedValue = serializeLocalizer(value);
											const exists = valueType.itemSource.items.some((x) => {
												return serializeLocalizer(x.displayText) === value;
											});
											if (exists) {
												return Promise.reject($t("component.value_type_nput.type.SELECTION.duplicateKeyOrValue"));
											}
										}
										return Promise.resolve();
									},
								},
							]}
						>
							<LocalizableInput disabled={disabled || editingItem.isEdit} />
						</Form.Item>
						<Form.Item
							name="value"
							label={$t("component.value_type_nput.type.SELECTION.value")}
							rules={[
								{ required: true },
								{
									validator: async (_, val) => {
										if (valueType instanceof SelectionStringValueType) {
											const exists = valueType.itemSource.items.some((x) => {
												return x.value === val;
											});
											if (exists) {
												return Promise.reject($t("component.value_type_nput.type.SELECTION.duplicateKeyOrValue"));
											}
										}
										return Promise.resolve();
									},
								},
							]}
						>
							<Input disabled={disabled} />
						</Form.Item>
					</Form>
				</Modal>
			</div>
		);
	},
);

ValueTypeInput.displayName = "ValueTypeInput";

export default ValueTypeInput;
