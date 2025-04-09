import { useState, useEffect, lazy } from "react";
import { Card, Menu, Modal } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { UpdateProfileDto } from "#/account/profile";
import { useUserActions, useUserInfo } from "@/store/userStore";
import { getApi, updateApi } from "@/api/account/profile";
import { toast } from "sonner";
import { useSearchParams } from "react-router";

// Lazy load components
const AuthenticatorSettings = lazy(() => import("@/components/abp/account/authenticator-settings"));
const BasicSettings = lazy(() => import("@/components/abp/account/basic-settings"));
const BindSettings = lazy(() => import("@/components/abp/account/bind-settings"));
const SecuritySettings = lazy(() => import("@/components/abp/account/security-settings"));
const SessionSettings = lazy(() => import("@/components/abp/account/session-settings"));
const NoticeSettings = lazy(() => import("@/components/abp/account/notice-settings"));
const EmailConfirmModal = lazy(() => import("@/components/abp/account/email-confirm-modal"));

const MySetting = () => {
	const { t: $t } = useTranslation();
	const [searchParams] = useSearchParams();
	const [modal, contextHolder] = Modal.useModal();
	const userInfo = useUserInfo();
	const { fetchAndSetUser } = useUserActions();
	const queryClient = useQueryClient();

	const [selectedKey, setSelectedKey] = useState<string>("basic");
	const [emailConfirmModalVisible, setEmailConfirmModalVisible] = useState(false);

	const menuItems = [
		{ key: "basic", label: $t("abp.account.settings.basic.title") },
		{ key: "security", label: $t("abp.account.settings.security.title") },
		{ key: "bind", label: $t("abp.account.settings.bindSettings") },
		{ key: "session", label: $t("abp.account.settings.sessionSettings") },
		{ key: "notice", label: $t("abp.account.settings.noticeSettings") },
		{ key: "authenticator", label: $t("abp.account.settings.authenticatorSettings") },
	];

	// Fetch profile data
	const { data: profile } = useQuery({
		queryKey: ["profile"],
		queryFn: getApi,
	});

	// Update profile mutation
	const { mutateAsync: updateProfile } = useMutation({
		mutationFn: updateApi,
		onSuccess: async () => {
			toast.success($t("AbpAccount.PersonalSettingsChangedConfirmationModalTitle"));
			queryClient.invalidateQueries({ queryKey: ["profile"] }); //refresh user info
			await fetchAndSetUser();
		},
	});

	useEffect(() => {
		const confirmToken = searchParams.get("confirmToken");
		if (confirmToken && profile) {
			setEmailConfirmModalVisible(true);
		}
	}, [searchParams, profile]);

	const handleUpdateProfile = (input: UpdateProfileDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpAccount.PersonalSettingsSaved"),
			centered: true,
			onOk: () => updateProfile(input),
		});
	};

	const handleChangePassword = () => {
		// TODO: Implement password change
		console.warn("onChangePassword not implemented yet!");
	};

	const handleChangePhoneNumber = () => {
		// TODO: Implement phone number change
		console.warn("onChangePhoneNumber not implemented yet!");
	};

	const renderContent = () => {
		switch (selectedKey) {
			case "basic":
				return profile && <BasicSettings profile={profile} onSubmit={handleUpdateProfile} />;
			case "bind":
				return <BindSettings />;
			case "security":
				return (
					userInfo && (
						<SecuritySettings
							userInfo={{
								email: userInfo.email ?? "",
								emailVerified: userInfo.emailVerified,
								name: userInfo.name,
								phoneNumber: userInfo.phoneNumber,
								phoneNumberVerified: userInfo.phoneNumberVerified,
								preferredUsername: userInfo.username ?? "",
								role: userInfo.roles ?? [],
								sub: userInfo.userId ?? "",
								uniqueName: userInfo.username ?? "",
								givenName: userInfo.givenName ?? "",
							}}
							onChangePassword={handleChangePassword}
							onChangePhoneNumber={handleChangePhoneNumber}
						/>
					)
				);
			case "notice":
				return <NoticeSettings />;
			case "authenticator":
				return <AuthenticatorSettings />;
			case "session":
				return <SessionSettings />;
			default:
				return null;
		}
	};

	return (
		<div>
			{contextHolder}
			<Card>
				<div className="flex">
					<div className="basis-1/6">
						<Menu
							selectedKeys={[selectedKey]}
							items={menuItems}
							mode="inline"
							onSelect={({ key }) => setSelectedKey(key)}
						/>
					</div>
					<div className="basis-5/6 overflow-hidden">{renderContent()}</div>
				</div>
			</Card>

			<EmailConfirmModal
				visible={emailConfirmModalVisible}
				initialState={{
					email: profile?.email || "",
					confirmToken: searchParams.get("confirmToken") || "",
					userId: searchParams.get("userId") || "",
					returnUrl: searchParams.get("returnUrl") || "",
				}}
				onClose={() => setEmailConfirmModalVisible(false)}
				onSuccess={async () => {
					await fetchAndSetUser();
				}}
			/>
		</div>
	);
};

export default MySetting;
