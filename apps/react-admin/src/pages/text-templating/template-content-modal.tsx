import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Button, Card, Alert, Space } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import ReactMarkdown from "react-markdown"; // Used for MarkdownViewer replacement
import useAbpStore from "@/store/abpCoreStore";
import { getApi, restoreToDefaultApi, updateApi } from "@/api/text-templating/template-contents";
import type { TextTemplateContentDto } from "#/text-templating/contents";
import type { TextTemplateDefinitionDto } from "#/text-templating/definitions";
import TemplateContentCurtuleModal from "./template-content-curtule-modal";

interface Props {
	visible: boolean;
	onClose: () => void;
	templateDefinition?: TextTemplateDefinitionDto;
	onChange?: (data: TextTemplateContentDto) => void;
}

const TemplateContentModal: React.FC<Props> = ({ visible, onClose, templateDefinition, onChange }) => {
	const { t: $t } = useTranslation();
	const application = useAbpStore((state) => state.application);
	const [form] = Form.useForm();

	const [modal, contextHolder] = Modal.useModal();
	const [submitting, setSubmitting] = useState(false);
	const [loading, setLoading] = useState(false);
	const [curtuleModalVisible, setCurtuleModalVisible] = useState(false);

	useEffect(() => {
		if (visible && templateDefinition) {
			fetchContent();
		}
	}, [visible, templateDefinition]);

	const fetchContent = async () => {
		if (!templateDefinition) return;
		try {
			setLoading(true);
			const culture = templateDefinition.isInlineLocalized
				? undefined
				: application?.localization.currentCulture.cultureName;

			const dto = await getApi({
				name: templateDefinition.name,
				culture,
			});
			form.setFieldsValue({ content: dto.content });
		} finally {
			setLoading(false);
		}
	};

	const handleSave = async () => {
		if (!templateDefinition) return;
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const culture = templateDefinition.isInlineLocalized
				? undefined
				: application?.localization.currentCulture.cultureName;

			const dto = await updateApi(templateDefinition.name, {
				content: values.content,
				culture,
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
				await restoreToDefaultApi(templateDefinition.name, {});
				toast.success($t("AbpTextTemplating.TemplateContentRestoredToDefault"));
				fetchContent();
			},
		});
	};

	const cardTitle = templateDefinition
		? `${$t("AbpTextTemplating.DisplayName:Name")} - ${templateDefinition.name}`
		: "";

	return (
		<>
			{contextHolder}
			<Modal
				title={$t("AbpTextTemplating.EditContents")}
				open={visible}
				onCancel={onClose}
				onOk={handleSave}
				confirmLoading={submitting}
				width="80%"
				style={{ top: 20 }}
				okText={$t("AbpTextTemplating.SaveContent")}
				destroyOnClose
			>
				{templateDefinition?.isInlineLocalized && (
					<Alert
						type="warning"
						className="mb-4"
						message={<ReactMarkdown>{$t("AbpTextTemplating.InlineContentDescription")}</ReactMarkdown>}
					/>
				)}

				<Card
					title={cardTitle}
					loading={loading}
					extra={
						<Space>
							<Button danger type="primary" onClick={handleRestore}>
								{$t("AbpTextTemplating.RestoreToDefault")}
							</Button>
							<Button onClick={() => setCurtuleModalVisible(true)}>
								{$t("AbpTextTemplating.CustomizePerCulture")}
							</Button>
						</Space>
					}
				>
					<Form form={form} layout="vertical">
						<Form.Item name="content" label={$t("AbpTextTemplating.DisplayName:Content")} rules={[{ required: true }]}>
							<Input.TextArea autoSize={{ minRows: 20 }} showCount />
						</Form.Item>
					</Form>
				</Card>
			</Modal>

			<TemplateContentCurtuleModal
				visible={curtuleModalVisible}
				onClose={() => setCurtuleModalVisible(false)}
				templateDefinition={templateDefinition}
				onChange={(data) => {
					// If data changed in sub-modal, refresh current modal content
					fetchContent();
					onChange?.(data);
				}}
			/>
		</>
	);
};

export default TemplateContentModal;
