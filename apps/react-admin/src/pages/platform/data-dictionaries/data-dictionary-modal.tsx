import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, TreeSelect } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { createApi, updateApi, getApi, getAllApi } from "@/api/platform/data-dictionaries";
import type { DataDto, DataCreateDto, DataUpdateDto } from "#/platform/data-dictionaries";
import { listToTree } from "@/utils/tree";
interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: DataDto) => void;
	dataId?: string;
	parentId?: string;
}

const DataDictionaryModal: React.FC<Props> = ({ visible, onClose, onChange, dataId, parentId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);
	const [treeData, setTreeData] = useState<any[]>([]);
	const [formData, setFormData] = useState<DataDto | null>(null);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			initData();
		}
	}, [visible, dataId, parentId]);

	const initData = async () => {
		// Fetch tree data for Parent selection
		const { items } = await getAllApi();
		const tree = listToTree(items, { id: "id", pid: "parentId" });
		setTreeData(tree);

		if (dataId) {
			const dto = await getApi(dataId);
			setFormData(dto);
			form.setFieldsValue(dto);
		} else if (parentId) {
			setFormData(null);
			form.setFieldValue("parentId", parentId);
		} else {
			setFormData(null);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			let res: DataDto;
			if (dataId) {
				res = await updateApi(dataId, values as DataUpdateDto);
			} else {
				res = await createApi(values as DataCreateDto);
			}

			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	const modalTitle = dataId
		? `${$t("AppPlatform.Data:Edit")} - ${formData?.name || ""}`
		: parentId
			? $t("AppPlatform.Data:AddChildren")
			: $t("AppPlatform.Data:AddNew");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item name="parentId" label={$t("AppPlatform.DisplayName:ParentData")}>
					<TreeSelect
						treeData={treeData}
						fieldNames={{ label: "displayName", value: "id", children: "children" }}
						allowClear
						treeDefaultExpandAll
						disabled={!!dataId} // Often prevent moving parent during edit for simplicity
					/>
				</Form.Item>

				<Form.Item name="name" label={$t("AppPlatform.DisplayName:Name")} rules={[{ required: true }]}>
					<Input disabled={!!dataId} autoComplete="off" />
				</Form.Item>

				<Form.Item name="displayName" label={$t("AppPlatform.DisplayName:DisplayName")} rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="description" label={$t("AppPlatform.DisplayName:Description")}>
					<Input.TextArea autoSize={{ minRows: 3 }} />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default DataDictionaryModal;
