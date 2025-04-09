import { useState, useEffect } from "react";
import { Card, List, Button, Tag, Switch } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery } from "@tanstack/react-query";
import { getTwoFactorEnabledApi, changeTwoFactorEnabledApi, sendEmailConfirmLinkApi } from "@/api/account/profile";
import type { UserInfo } from "#/account/user";

interface Props {
	userInfo: UserInfo | null;
	onChangePassword: () => void;
	onChangePhoneNumber: () => void;
}

const SecuritySettings: React.FC<Props> = ({ userInfo, onChangePassword, onChangePhoneNumber }) => {
	const { t: $t } = useTranslation();
	const [loading, setLoading] = useState(false);
	const [sendMailInterval, setSendMailInterval] = useState(0);
	let timer: NodeJS.Timeout;

	// Get two factor status
	const { data: twoFactor, refetch: refetchTwoFactor } = useQuery({
		queryKey: ["twoFactorEnabled"],
		queryFn: getTwoFactorEnabledApi,
	});

	// Change two factor mutation
	const { mutateAsync: changeTwoFactor } = useMutation({
		mutationFn: changeTwoFactorEnabledApi,
		onMutate: () => setLoading(true),
		onSettled: () => setLoading(false),
		onError: () => refetchTwoFactor(),
		onSuccess: () => refetchTwoFactor(),
	});

	// Send email confirmation mutation
	const { mutateAsync: sendEmailConfirm } = useMutation({
		mutationFn: (email: string) =>
			sendEmailConfirmLinkApi({
				appName: "ReactAdmin",
				email,
				returnUrl: window.location.href,
			}),
		onSuccess: () => {
			setSendMailInterval(60);
			timer = setInterval(() => {
				setSendMailInterval((prev) => {
					if (prev <= 0) {
						clearInterval(timer);
						return 0;
					}
					return prev - 1;
				});
			}, 1000);
		},
		onError: () => setSendMailInterval(0),
	});

	useEffect(() => {
		return () => {
			if (timer) clearInterval(timer);
		};
	}, []);

	const getSendMailTitle = () => {
		if (sendMailInterval > 0) {
			return `${sendMailInterval} s`;
		}
		return $t("AbpAccount.ClickToValidation");
	};

	return (
		<Card bordered={false} title={$t("abp.account.settings.security.title")}>
			<List>
				{/* Password */}
				<List.Item
					extra={
						<Button type="link" onClick={onChangePassword}>
							{$t("AbpUi.Edit")}
						</Button>
					}
				>
					<List.Item.Meta
						title={$t("abp.account.settings.security.password")}
						description={$t("abp.account.settings.security.passwordDesc")}
					/>
				</List.Item>

				{/* Phone Number */}
				<List.Item
					extra={
						<Button type="link" onClick={onChangePhoneNumber}>
							{$t("AbpUi.Edit")}
						</Button>
					}
				>
					<List.Item.Meta
						title={$t("abp.account.settings.security.phoneNumber")}
						description={
							<>
								{userInfo?.phoneNumber}
								{!userInfo?.phoneNumber ? (
									<Tag color="warning">{$t("abp.account.settings.security.unSet")}</Tag>
								) : userInfo?.phoneNumberVerified ? (
									<Tag color="success">{$t("abp.account.settings.security.verified")}</Tag>
								) : (
									<Tag color="warning">{$t("abp.account.settings.security.unVerified")}</Tag>
								)}
							</>
						}
					/>
				</List.Item>

				{/* Email */}
				<List.Item
					extra={
						userInfo?.email &&
						!userInfo?.emailVerified && (
							<Button type="link" disabled={sendMailInterval > 0} onClick={() => sendEmailConfirm(userInfo.email)}>
								{getSendMailTitle()}
							</Button>
						)
					}
				>
					<List.Item.Meta
						title={$t("abp.account.settings.security.email")}
						description={
							<>
								{userInfo?.email}
								{userInfo?.emailVerified ? (
									<Tag color="success">{$t("abp.account.settings.security.verified")}</Tag>
								) : (
									<Tag color="warning">{$t("abp.account.settings.security.unVerified")}</Tag>
								)}
							</>
						}
					/>
				</List.Item>

				{/* Two Factor Authentication */}
				{twoFactor && (
					<List.Item
						extra={
							<Switch
								checked={twoFactor.enabled}
								loading={loading}
								onChange={(checked) => changeTwoFactor({ enabled: checked })}
							/>
						}
					>
						<List.Item.Meta title={$t("AbpAccount.TwoFactor")} description={$t("AbpAccount.TwoFactor")} />
					</List.Item>
				)}
			</List>
		</Card>
	);
};

export default SecuritySettings;
