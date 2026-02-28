import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { createApi, updateApi, getApi } from "@/api/platform/layouts";
import {
	getByNameApi as getDataDictionaryByNameApi,
	getAllApi as getDataDictionariesApi,
} from "@/api/platform/data-dictionaries";
import type { LayoutDto, LayoutCreateDto, LayoutUpdateDto } from "#/platform/layouts";
import { listToTree } from "@/utils/tree";
import ApiSelect from "@/components/abp/adapter/api-select";
import ApiTreeSelect from "@/components/abp/adapter/api-tree-select";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: LayoutDto) => void;
	layoutId?: string;
}

const LayoutModal: React.FC<Props> = ({ visible, onClose, onChange, layoutId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);
	const [loading, setLoading] = useState(false);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			if (layoutId) {
				fetchLayout(layoutId);
			}
		}
	}, [visible, layoutId]);

	const fetchLayout = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			form.setFieldsValue(dto);
		} finally {
			setLoading(false);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			let res: LayoutDto;
			if (layoutId) {
				res = await updateApi(layoutId, values as LayoutUpdateDto);
			} else {
				res = await createApi(values as LayoutCreateDto);
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

	const uiFrameworkApi = () => getDataDictionaryByNameApi("UI Framework");

	const constraintsApi = async () => {
		const { items } = await getDataDictionariesApi();
		return listToTree(items, {
			id: "id",
			pid: "parentId",
		});
	};

	const modalTitle = layoutId
		? `${$t("AppPlatform.Layout:Edit")} - ${form.getFieldValue("displayName") || ""}`
		: $t("AppPlatform.Layout:AddNew");

	return (
		<Modal
			title={modalTitle}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			loading={loading}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				{!layoutId && (
					<>
						<Form.Item
							name="framework"
							label={$t("AppPlatform.DisplayName:UIFramework")}
							rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}
						>
							<ApiSelect
								api={uiFrameworkApi}
								resultField="items"
								labelField="displayName"
								valueField="name"
								allowClear
							/>
						</Form.Item>

						<Form.Item
							name="dataId"
							label={$t("AppPlatform.DisplayName:LayoutConstraint")}
							rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}
						>
							<ApiTreeSelect
								api={constraintsApi}
								fieldNames={{ label: "displayName", value: "id", children: "children" }}
								allowClear
								treeDefaultExpandAll
							/>
						</Form.Item>
					</>
				)}

				<Form.Item name="name" label={$t("AppPlatform.DisplayName:Name")} rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="displayName" label={$t("AppPlatform.DisplayName:DisplayName")} rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="path" label={$t("AppPlatform.DisplayName:Path")} rules={[{ required: true }]}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="redirect" label={$t("AppPlatform.DisplayName:Redirect")}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="description" label={$t("AppPlatform.DisplayName:Description")}>
					<Input.TextArea autoSize={{ minRows: 3 }} />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default LayoutModal;
