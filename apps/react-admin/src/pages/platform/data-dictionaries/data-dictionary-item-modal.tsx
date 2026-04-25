import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Select, Checkbox, InputNumber, DatePicker } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import dayjs from "dayjs";
import { createItemApi, updateItemApi } from "@/api/platform/data-dictionaries";
import type { DataDto, DataItemDto, DataItemCreateDto, DataItemUpdateDto } from "#/platform/data-dictionaries";
import { ValueType } from "#/platform/data-dictionaries";
import { formatToDate, formatToDateTime } from "@/utils/abp";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: () => void;
	data?: DataDto;
	item?: DataItemDto;
}

const DataDictionaryItemModal: React.FC<Props> = ({ visible, onClose, onChange, data, item }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);

	// Watch valueType to dynamically render the default value input
	const valueType = Form.useWatch("valueType", form);
	const allowBeNull = Form.useWatch("allowBeNull", form);

	useEffect(() => {
		if (visible && data) {
			form.resetFields();
			if (item) {
				// Edit Mode: Map string values back to typed values
				form.setFieldsValue({
					...item,
					defaultValue: mapFromDefaultValue(item.valueType, item.defaultValue),
				});
			} else {
				// Create Mode defaults
				form.setFieldsValue({
					allowBeNull: true,
					valueType: ValueType.String,
				});
			}
		}
	}, [visible, data, item, form]);

	const mapFromDefaultValue = (type: ValueType, val?: string) => {
		if (!val) return undefined;
		switch (type) {
			case ValueType.Boolean:
				return val.toLowerCase() === "true";
			case ValueType.Numeic:
				return Number(val);
			case ValueType.Array:
				return val.split(",");
			case ValueType.Date:
			case ValueType.DateTime:
				return dayjs(val);
			default:
				return val;
		}
	};

	const mapToDefaultValue = (type: ValueType, val?: any) => {
		if (val === undefined || val === null) return undefined;
		switch (type) {
			case ValueType.Boolean:
				return String(val);
			case ValueType.Array:
				return (val as string[]).join(",");
			case ValueType.Date:
				return formatToDate(val);
			case ValueType.DateTime:
				return formatToDateTime(val);
			case ValueType.Numeic:
				return String(val);
			default:
				return String(val);
		}
	};

	const handleSubmit = async () => {
		if (!data) return;
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const submitData = {
				...values,
				defaultValue: mapToDefaultValue(values.valueType, values.defaultValue),
			};

			if (item) {
				await updateItemApi(data.id, item.name, submitData as DataItemUpdateDto);
			} else {
				await createItemApi(data.id, submitData as DataItemCreateDto);
			}

			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange();
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	const renderDefaultValueInput = () => {
		switch (valueType) {
			case ValueType.Boolean:
				return <Checkbox>{$t("AppPlatform.DisplayName:DefaultValue")}</Checkbox>;
			case ValueType.Numeic:
				return <InputNumber className="w-full" />;
			case ValueType.Date:
				return <DatePicker className="w-full" />;
			case ValueType.DateTime:
				return <DatePicker showTime className="w-full" />;
			case ValueType.Array:
				return <Select mode="tags" />;
			case ValueType.String:
				return <Input autoComplete="off" />;
			default:
				return <Input autoComplete="off" />;
		}
	};

	const modalTitle = item ? `${$t("AppPlatform.Data:EditItem")} - ${item.name}` : $t("AppPlatform.Data:AppendItem");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			width={600}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				{/* Helper hidden field for ID if needed, though we use 'item' prop logic */}

				<Form.Item name="name" label={$t("AppPlatform.DisplayName:Name")} rules={[{ required: true }]}>
					<Input disabled={!!item} autoComplete="off" />
				</Form.Item>

				<Form.Item name="displayName" label={$t("AppPlatform.DisplayName:DisplayName")} rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="valueType" label={$t("AppPlatform.DisplayName:ValueType")} rules={[{ required: true }]}>
					<Select
						disabled={!!item} // Usually changing type after creation causes data issues
						onChange={() => form.setFieldValue("defaultValue", undefined)}
						options={[
							{ label: "String", value: ValueType.String },
							{ label: "Number", value: ValueType.Numeic },
							{ label: "Date", value: ValueType.Date },
							{ label: "DateTime", value: ValueType.DateTime },
							{ label: "Boolean", value: ValueType.Boolean },
							{ label: "Array", value: ValueType.Array },
						]}
					/>
				</Form.Item>

				<Form.Item name="allowBeNull" valuePropName="checked">
					<Checkbox>{$t("AppPlatform.DisplayName:AllowBeNull")}</Checkbox>
				</Form.Item>

				<Form.Item
					name="defaultValue"
					label={$t("AppPlatform.DisplayName:DefaultValue")}
					valuePropName={valueType === ValueType.Boolean ? "checked" : "value"}
					rules={[{ required: !allowBeNull, message: $t("AbpUi.ThisFieldIsRequired") }]}
				>
					{renderDefaultValueInput()}
				</Form.Item>

				<Form.Item name="description" label={$t("AppPlatform.DisplayName:Description")}>
					<Input.TextArea autoSize={{ minRows: 3 }} />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default DataDictionaryItemModal;
