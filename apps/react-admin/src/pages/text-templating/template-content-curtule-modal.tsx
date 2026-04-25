import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Select, Input, Button, Card, Row, Col } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import useAbpStore from "@/store/abpCoreStore";
import { getApi, restoreToDefaultApi, updateApi } from "@/api/text-templating/template-contents";
import type { TextTemplateContentDto } from "#/text-templating/contents";
import type { TextTemplateDefinitionDto } from "#/text-templating/definitions";

interface Props {
	visible: boolean;
	onClose: () => void;
	templateDefinition?: TextTemplateDefinitionDto;
	onChange?: (data: TextTemplateContentDto) => void;
}

const TemplateContentCurtuleModal: React.FC<Props> = ({ visible, onClose, templateDefinition, onChange }) => {
	const { t: $t } = useTranslation();
	const application = useAbpStore((state) => state.application);

	const [modal, contextHolder] = Modal.useModal();
	const [sourceForm] = Form.useForm();
	const [targetForm] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);
	const [loading, setLoading] = useState(false);

	// Get available languages
	const languages = application?.localization.languages || [];
	const currentCulture = application?.localization.currentCulture.cultureName;

	const fetchContent = async (culture: string | undefined, isSource: boolean) => {
		if (!templateDefinition?.name) return;
		try {
			setLoading(true);
			const data = await getApi({
				name: templateDefinition.name,
				culture: culture,
			});
			if (isSource) {
				sourceForm.setFieldValue("content", data.content);
			} else {
				targetForm.setFieldValue("content", data.content);
			}
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		if (visible && templateDefinition) {
			// Initial Load
			sourceForm.setFieldValue("culture", currentCulture);
			targetForm.setFieldValue("culture", currentCulture);

			// Fetch initial data for both sides
			fetchContent(currentCulture, true);
			fetchContent(currentCulture, false);
		}
	}, [visible, templateDefinition]);

	const handleCultureChange = (val: string, isSource: boolean) => {
		fetchContent(val, isSource);
	};

	const handleSave = async () => {
		if (!templateDefinition) return;
		try {
			const values = await targetForm.validateFields();
			setSubmitting(true);

			const dto = await updateApi(templateDefinition.name, {
				content: values.content,
				culture: values.culture,
			});

			toast.success($t("AbpTextTemplating.TemplateContentUpdated"));
			onChange?.(dto);
			onClose();
		} catch (e) {
			console.error(e);
		} finally {
			setSubmitting(false);
		}
	};

	const handleRestore = () => {
		if (!templateDefinition) return;
		modal.confirm({
			title: $t("AbpTextTemplating.RestoreToDefault"),
			content: $t("AbpTextTemplating.RestoreToDefaultMessage"),
			onOk: async () => {
				const culture = targetForm.getFieldValue("culture");
				await restoreToDefaultApi(templateDefinition.name, { culture });
				toast.success($t("AbpTextTemplating.TemplateContentRestoredToDefault"));
				// Refresh target content
				fetchContent(culture, false);
			},
		});
	};

	return (
		<>
			{contextHolder}
			<Modal
				title={$t("AbpTextTemplating.EditContents")}
				open={visible}
				onCancel={onClose}
				onOk={handleSave}
				confirmLoading={submitting}
				width="90%"
				style={{ top: 20 }}
				okText={$t("AbpTextTemplating.SaveContent")}
				destroyOnClose
			>
				<Card
					title={templateDefinition ? `${$t("AbpTextTemplating.DisplayName:Name")} - ${templateDefinition.name}` : ""}
					extra={
						<Button danger type="primary" onClick={handleRestore}>
							{$t("AbpTextTemplating.RestoreToDefault")}
						</Button>
					}
					loading={loading}
				>
					<Row gutter={16}>
						{/* Source Side (Read Only) */}
						<Col span={12}>
							<Form form={sourceForm} layout="vertical">
								<Form.Item name="culture" label={$t("AbpTextTemplating.BaseCultureName")} rules={[{ required: true }]}>
									<Select
										options={languages}
										fieldNames={{ label: "displayName", value: "cultureName" }}
										onChange={(val) => handleCultureChange(val, true)}
									/>
								</Form.Item>
								<Form.Item name="content" label={$t("AbpTextTemplating.BaseContent")}>
									<Input.TextArea autoSize={{ minRows: 20 }} readOnly />
								</Form.Item>
							</Form>
						</Col>

						{/* Target Side (Editable) */}
						<Col span={12}>
							<Form form={targetForm} layout="vertical">
								<Form.Item
									name="culture"
									label={$t("AbpTextTemplating.TargetCultureName")}
									rules={[{ required: true }]}
								>
									<Select
										options={languages}
										fieldNames={{ label: "displayName", value: "cultureName" }}
										onChange={(val) => handleCultureChange(val, false)}
									/>
								</Form.Item>
								<Form.Item name="content" label={$t("AbpTextTemplating.TargetContent")} rules={[{ required: true }]}>
									<Input.TextArea autoSize={{ minRows: 20 }} showCount />
								</Form.Item>
							</Form>
						</Col>
					</Row>
				</Card>
			</Modal>
		</>
	);
};

export default TemplateContentCurtuleModal;
