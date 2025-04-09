import type React from "react";
import { Modal } from "antd";
import { useTranslation } from "react-i18next";

interface DeleteModalProps {
	visible: boolean;
	onConfirm: () => void;
	onCancel: () => void;
}

const DeleteModal: React.FC<DeleteModalProps> = ({ visible, onConfirm, onCancel }) => {
	const { t: $t } = useTranslation();

	return (
		<Modal title={$t("AbpUi.AreYouSure")} open={visible} onOk={onConfirm} onCancel={onCancel}>
			{$t("AbpUi.ItemWillBeDeletedMessage")}
		</Modal>
	);
};

export default DeleteModal;
