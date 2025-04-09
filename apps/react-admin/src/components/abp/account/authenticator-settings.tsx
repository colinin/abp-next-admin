import { Button, Card, List, Skeleton, Modal } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getAuthenticatorApi, resetAuthenticatorApi } from "@/api/account/profile";
import AuthenticatorSteps from "./authenticator-steps";
import { toast } from "sonner";

const AuthenticatorSettings: React.FC = () => {
	const { t: $t } = useTranslation();
	const [modal, contextHolder] = Modal.useModal();
	const queryClient = useQueryClient();

	const { data: authenticator, isLoading } = useQuery({
		queryKey: ["authenticator"],
		queryFn: getAuthenticatorApi,
	});

	const { mutateAsync: resetAuthenticator } = useMutation({
		mutationFn: resetAuthenticatorApi,
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["authenticator"] });
			toast.success($t("AbpAccount.YourAuthenticatorIsSuccessfullyReset"));
		},
	});

	const handleReset = () => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpAccount.ResetAuthenticatorWarning"),
			centered: true,
			onOk: () => resetAuthenticator(),
		});
	};

	if (isLoading) {
		return (
			<Card bordered={false} title={$t("abp.account.settings.authenticatorSettings")}>
				<Skeleton />
			</Card>
		);
	}

	return (
		<>
			{contextHolder}
			<Card bordered={false} title={$t("abp.account.settings.authenticatorSettings")}>
				{authenticator?.isAuthenticated === false ? (
					<AuthenticatorSteps
						authenticator={authenticator}
						onDone={() => queryClient.invalidateQueries({ queryKey: ["authenticator"] })}
					/>
				) : authenticator?.isAuthenticated === true ? (
					<List>
						<List.Item
							extra={
								<Button type="primary" onClick={handleReset}>
									{$t("AbpAccount.ResetAuthenticator")}
								</Button>
							}
						>
							<List.Item.Meta
								title={$t("AbpAccount.ResetAuthenticator")}
								description={$t("AbpAccount.ResetAuthenticatorDesc")}
							/>
						</List.Item>
					</List>
				) : null}
			</Card>
		</>
	);
};

export default AuthenticatorSettings;
