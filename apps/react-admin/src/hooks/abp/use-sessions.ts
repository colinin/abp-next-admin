import { useEffect } from "react";
import type { Notification as NotificationInfo } from "#/notifications";
import { Modal } from "antd";
import { useApplication } from "@/store/abpCoreStore";
import { useNotifications } from "@/utils/abp/notifications/useNotifications";
import { NotificationNames } from "@/constants/notifications/notifications";
import { useEventBus } from "@/utils/abp/useEventBus";
import { useUserActions } from "@/store/userStore";
import { useLoginStateContext } from "@/pages/sys/login/providers/LoginStateProvider";

export const useSessions = () => {
	const { clearUserInfoAndToken } = useUserActions();
	const { backToLogin } = useLoginStateContext();
	const { currentUser } = useApplication() ?? {};
	const { subscribe, unSubscribe } = useEventBus();
	const { register, release } = useNotifications();

	const handleSessionExpired = (event?: NotificationInfo) => {
		if (!event) return;

		const { data, title, message } = event;
		const sessionId = data.SessionId;

		if (sessionId === currentUser?.sessionId) {
			release();
			Modal.confirm({
				title,
				content: message,
				centered: true,
				afterClose: () => {
					//TODO 测试
					clearUserInfoAndToken();
					backToLogin();
				},
			});
		}
	};

	useEffect(() => {
		// Register handlers on mount
		subscribe(NotificationNames.SessionExpiration, handleSessionExpired);
		register();

		// Cleanup on unmount
		return () => {
			unSubscribe(NotificationNames.SessionExpiration, handleSessionExpired);
			release();
		};
	}, [currentUser?.sessionId]); // Re-register if sessionId changes
};
