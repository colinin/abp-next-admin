import type React from "react";
import { Modal } from "antd";
import { useTranslation } from "react-i18next";
import Editor from "@/components/editor";

interface Props {
	visible: boolean;
	onClose: () => void;
	content?: string;
}

const EmailMessageModal: React.FC<Props> = ({ visible, onClose, content }) => {
	const { t: $t } = useTranslation();

	return (
		<Modal
			title={$t("AppPlatform.EmailMessages")}
			open={visible}
			onCancel={onClose}
			footer={null}
			width={800}
			destroyOnClose
		>
			<div className="p-4 border rounded bg-gray-50 min-h-[300px] max-h-[600px] overflow-auto">
				{content ? (
					<Editor value={content ?? ""} readOnly sample hiddleToolbar />
				) : (
					<span className="text-gray-400">{$t("AbpUi.NoData")}</span>
				)}
			</div>
		</Modal>
	);
};

export default EmailMessageModal;
