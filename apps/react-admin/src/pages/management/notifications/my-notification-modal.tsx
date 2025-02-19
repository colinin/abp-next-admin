import { Modal } from "antd";
import type { Notification } from "#/notifications";
import Editor from "@/components/editor";

interface Props {
	visible: boolean;
	notification?: Notification;
	onClose: () => void;
}

const MyNotificationModal: React.FC<Props> = ({ visible, notification, onClose }) => {
	return (
		<Modal open={visible} title={notification?.title} onCancel={onClose} footer={null}>
			<Editor
				id="notification-content"
				value={notification?.message ?? ""}
				readOnly
				sample // Use simple toolbar
				hiddleToolbar
			/>
		</Modal>
	);
};

export default MyNotificationModal;
