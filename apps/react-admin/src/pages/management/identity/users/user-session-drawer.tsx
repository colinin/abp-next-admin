import { Modal } from "antd";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Drawer } from "antd";
import { useQuery } from "@tanstack/react-query";
import { useTranslation } from "react-i18next";
import UserSessionTable from "../sessions/user-session-table";

import { toast } from "sonner";
import type { IdentitySessionDto } from "#/management/identity/sessions";
import type { IdentityUserDto } from "#/management/identity/user";
import { getSessionsApi, revokeSessionApi } from "@/api/management/identity/user-sessions";

interface UserSessionDrawerProps {
	open: boolean;
	onClose: () => void;
	user: IdentityUserDto;
}

export const UserSessionDrawer: React.FC<UserSessionDrawerProps> = ({ open, onClose, user }) => {
	const { t: $t } = useTranslation();
	const [modal, contextHolder] = Modal.useModal();
	const queryClient = useQueryClient();
	const queryKey = [`user-Sessions${user.id}`];
	// Fetch sessions
	const { data: sessions = [] } = useQuery({
		queryKey: queryKey,
		queryFn: async () => {
			const { items } = await getSessionsApi({ userId: user.id });
			return items;
		},
	});

	// Revoke session mutation
	const { mutateAsync: revokeSession } = useMutation({
		mutationFn: revokeSessionApi,
		onSuccess: () => {
			toast.success($t("AbpIdentity.SuccessfullyRevoked"));
			queryClient.refetchQueries({ queryKey: queryKey });
		},
	});

	const handleRevoke = (session: IdentitySessionDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpIdentity.SessionWillBeRevokedMessage"),
			centered: true,
			onOk: () => revokeSession(session.sessionId),
		});
	};

	return (
		<>
			{contextHolder}
			<Drawer open={open} onClose={onClose} width={800}>
				<UserSessionTable sessions={sessions} onRevoke={handleRevoke} />
			</Drawer>
		</>
	);
};
