import type React from "react";
import { useEffect, useState } from "react";
import { Drawer, Form, Input, Checkbox, Button, Steps, Card, TreeSelect, Select, InputNumber, DatePicker } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import dayjs from "dayjs";
import { createApi, updateApi, getApi } from "@/api/platform/menus";
import { getPagedListApi as getLayoutsApi, getApi as getLayoutApi } from "@/api/platform/layouts";
import { getApi as getDataDictionaryApi } from "@/api/platform/data-dictionaries";
import type { MenuDto, MenuCreateDto, MenuUpdateDto } from "#/platform/menus";
import { ValueType } from "#/platform/data-dictionaries";
import ApiSelect from "@/components/abp/adapter/api-select";
import IconPicker from "@/components/abp/adapter/icon-picker";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: MenuDto) => void;
	// Passing initial state via props instead of drawerApi.getData
	editMenu?: { id?: string; parentId?: string; layoutId?: string };
	rootMenus: MenuDto[]; // Tree data for Parent selection
}

const MenuDrawer: React.FC<Props> = ({ visible, onClose, onChange, editMenu, rootMenus }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm(); // Basic Info
	const [metaForm] = Form.useForm(); // Meta Info

	const [currentStep, setCurrentStep] = useState(0);
	const [submitting, setSubmitting] = useState(false);
	const [loading, setLoading] = useState(false);
	const [menuMetas, setMenuMetas] = useState<any[]>([]); // Dynamic fields definition

	// Watch Layout ID to load meta definitions
	const layoutId = Form.useWatch("layoutId", form);

	useEffect(() => {
		if (visible) {
			init();
		}
	}, [visible, editMenu]);

	// Load Meta definitions when Layout changes
	useEffect(() => {
		if (layoutId && visible) {
			loadMetaDefinitions(layoutId);
		} else {
			setMenuMetas([]);
		}
	}, [layoutId, visible]);

	const init = async () => {
		setCurrentStep(0);
		form.resetFields();
		metaForm.resetFields();
		setMenuMetas([]);

		// If ID exists, load data (Edit Mode)
		if (editMenu?.id) {
			setLoading(true);
			try {
				const dto = await getApi(editMenu.id);
				form.setFieldsValue(dto);
				// Meta values will be set after meta definitions are loaded via layoutId effect
				// We might need a ref or state to hold meta values temporarily
				setTimeout(() => setMetaValues(dto.meta, dto.layoutId), 500);
			} finally {
				setLoading(false);
			}
		} else {
			// Create Mode
			form.setFieldsValue({
				parentId: editMenu?.parentId,
				layoutId: editMenu?.layoutId,
			});
			// Handle path prefix based on parent
			if (editMenu?.parentId) {
				const parent = await getApi(editMenu.parentId);
				// Antd Input addonBefore logic handled in render
			}
		}
	};

	const loadMetaDefinitions = async (lId: string) => {
		try {
			setLoading(true);
			const layoutDto = await getLayoutApi(lId);
			// Auto-fill component path if creating new
			if (!editMenu?.id) {
				form.setFieldValue("component", layoutDto.path);
			}

			const dataDto = await getDataDictionaryApi(layoutDto.dataId);
			setMenuMetas(dataDto.items?.sort() || []);
		} finally {
			setLoading(false);
		}
	};

	const setMetaValues = (meta: Record<string, any>, lId: string) => {
		// This runs after definitions are loaded
		// We need to map raw values to component values (e.g. string 'true' to boolean true)
		// Since setMenuMetas is async/state based, robust implementation would map inside the render or a useEffect depending on menuMetas
		// Simplified logic here assuming definitions loaded:

		// We need a way to parse values based on type which is in menuMetas state.
		// This is tricky with async state updates.
		// For now, let's rely on the form submission to format outgoing data,
		// and try to set fields directly if possible.
		if (!meta) return;

		// Quick parse attempt based on known logic
		const parsedValues: any = { ...meta };
		// Refinement would require iterating over menuMetas to cast types (e.g. "true" -> true)
		metaForm.setFieldsValue(parsedValues);
	};

	const handleParentChange = async (val: string) => {
		if (val) {
			const parent = await getApi(val);
			// Store parent path for display/logic
		}
	};

	const onSubmit = async () => {
		try {
			setSubmitting(true);
			const basicValues = await form.validateFields();
			const metaValues = await metaForm.validateFields();

			// Format meta values to strings/etc as expected by backend DTO
			const formattedMeta: Record<string, string> = {};
			Object.keys(metaValues).forEach((key) => {
				const val = metaValues[key];
				if (val === undefined || val === null) return;

				// Simple casting logic based on value type check or metadata if available
				if (dayjs.isDayjs(val)) formattedMeta[key] = val.format("YYYY-MM-DD HH:mm:ss");
				else if (Array.isArray(val)) formattedMeta[key] = val.join(",");
				else formattedMeta[key] = String(val);
			});

			let path = basicValues.path;
			if (!path.startsWith("/")) path = `/${path}`;
			// Add parent path logic if needed

			const payload = {
				...basicValues,
				path,
				meta: formattedMeta,
			};

			let res: MenuDto;
			if (basicValues.id) {
				res = await updateApi(basicValues.id, payload as MenuUpdateDto);
			} else {
				res = await createApi(payload as MenuCreateDto);
			}

			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		} catch (e) {
			console.error(e);
		} finally {
			setSubmitting(false);
		}
	};

	const renderMetaInput = (item: any) => {
		switch (item.valueType) {
			case ValueType.Boolean:
				return <Checkbox>{item.displayName}</Checkbox>;
			case ValueType.Numeic:
				return <InputNumber className="w-full" />;
			case ValueType.Date:
				return <DatePicker className="w-full" />;
			case ValueType.DateTime:
				return <DatePicker showTime className="w-full" />;
			case ValueType.Array:
				return <Select mode="tags" />;
			case ValueType.String:
				if (item.name.toLowerCase().includes("icon")) {
					return <IconPicker />;
				}
				return <Input autoComplete="off" />;
			default:
				return <Input autoComplete="off" />;
		}
	};

	const drawerTitle = editMenu?.id
		? `${$t("AppPlatform.Menu:Edit")} - ${form.getFieldValue("name") || ""}`
		: $t("AppPlatform.Menu:AddNew");

	return (
		<Drawer
			title={drawerTitle}
			width={720}
			open={visible}
			onClose={onClose}
			destroyOnClose
			maskClosable={false}
			footer={
				<div className="flex justify-end gap-2">
					{currentStep === 1 && <Button onClick={() => setCurrentStep(0)}>{$t("AppPlatform.PreStep")}</Button>}
					{currentStep === 0 && (
						<Button type="primary" onClick={() => form.validateFields().then(() => setCurrentStep(1))}>
							{$t("AppPlatform.NextStep")}
						</Button>
					)}
					{currentStep === 1 && (
						<Button type="primary" loading={submitting} onClick={onSubmit}>
							{$t("AbpUi.Submit")}
						</Button>
					)}
				</div>
			}
		>
			<Steps current={currentStep} className="mb-8 max-w-lg mx-auto">
				<Steps.Step title={$t("AppPlatform.DisplayName:Basic")} />
				<Steps.Step title={$t("AppPlatform.DisplayName:Meta")} />
			</Steps>

			{/* Basic Form */}
			<div style={{ display: currentStep === 0 ? "block" : "none" }}>
				<Form form={form} layout="vertical">
					<Form.Item name="id" hidden>
						<Input />
					</Form.Item>

					<Form.Item name="layoutId" label={$t("AppPlatform.DisplayName:Layout")} rules={[{ required: true }]}>
						<ApiSelect
							api={() => getLayoutsApi({ maxResultCount: 100 })}
							labelField="displayName"
							valueField="id"
							resultField="items"
							allowClear
						/>
					</Form.Item>

					<Form.Item name="isPublic" valuePropName="checked">
						<Checkbox>{$t("AppPlatform.DisplayName:IsPublic")}</Checkbox>
					</Form.Item>

					<Form.Item name="parentId" label={$t("AppPlatform.DisplayName:ParentMenu")}>
						<TreeSelect
							treeData={rootMenus}
							fieldNames={{ label: "displayName", value: "id", children: "children" }}
							allowClear
							treeDefaultExpandAll
							onChange={handleParentChange}
						/>
					</Form.Item>

					<Form.Item name="name" label={$t("AppPlatform.DisplayName:Name")} rules={[{ required: true }]}>
						<Input autoComplete="off" />
					</Form.Item>

					<Form.Item name="displayName" label={$t("AppPlatform.DisplayName:DisplayName")} rules={[{ required: true }]}>
						<Input autoComplete="off" />
					</Form.Item>

					<Form.Item name="path" label={$t("AppPlatform.DisplayName:Path")} rules={[{ required: true }]}>
						<Input autoComplete="off" />
					</Form.Item>

					<Form.Item name="component" label={$t("AppPlatform.DisplayName:Component")} rules={[{ required: true }]}>
						<Input autoComplete="off" />
					</Form.Item>

					<Form.Item name="redirect" label={$t("AppPlatform.DisplayName:Redirect")}>
						<Input autoComplete="off" />
					</Form.Item>

					<Form.Item name="description" label={$t("AppPlatform.DisplayName:Description")}>
						<Input.TextArea autoSize={{ minRows: 3 }} />
					</Form.Item>
				</Form>
			</div>

			{/* Meta Form */}
			<div style={{ display: currentStep === 1 ? "block" : "none" }}>
				<Form form={metaForm} layout="vertical">
					{menuMetas.map((item) => (
						<Form.Item
							key={item.name}
							name={item.name}
							label={item.displayName}
							tooltip={item.description}
							rules={item.allowBeNull ? [] : [{ required: true }]}
							valuePropName={item.valueType === ValueType.Boolean ? "checked" : "value"}
						>
							{renderMetaInput(item)}
						</Form.Item>
					))}
					{menuMetas.length === 0 && <div className="text-center text-gray-500 py-10">No Meta Fields Defined</div>}
				</Form>
			</div>
		</Drawer>
	);
};

export default MenuDrawer;
