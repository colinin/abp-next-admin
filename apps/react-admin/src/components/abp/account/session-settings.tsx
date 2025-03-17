import { Card, Modal } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { IdentitySessionDto } from "#/management/identity/sessions";
import { getSessionsApi, revokeSessionApi } from "@/api/account/my-session";
import { toast } from "sonner";
import UserSessionTable from "@/pages/management/identity/sessions/user-session-table";

const SessionSettings: React.FC = () => {
	const { t: $t } = useTranslation();
	const [modal, contextHolder] = Modal.useModal();
	const queryClient = useQueryClient();

	// Fetch sessions
	const { data: sessions = [] } = useQuery({
		queryKey: ["mySessions"],
		queryFn: async () => {
			const { items } = await getSessionsApi();
			return items;
		},
	});

	// Revoke session mutation
	const { mutateAsync: revokeSession } = useMutation({
		mutationFn: revokeSessionApi,
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["mySessions"] });
			toast.success($t("AbpIdentity.SuccessfullyRevoked"));
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
			<Card bordered={false} title={$t("abp.account.settings.sessionSettings")}>
				<UserSessionTable sessions={sessions} onRevoke={handleRevoke} />
			</Card>
		</>
	);
};

export default SessionSettings;
