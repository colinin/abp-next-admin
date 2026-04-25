import type React from "react";
import { useState, useEffect, lazy, Suspense } from "react";
import { Card, Menu, Modal, Spin } from "antd";
import { useTranslation } from "react-i18next";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { toast } from "sonner";
import { getApi, updateApi } from "@/api/account/profile";
import type { UpdateProfileDto } from "#/account/profile";
import { useUserActions, useUserInfo } from "@/store/userStore";
import { useSearchParams } from "react-router";

// Lazy load components
const AuthenticatorSettings = lazy(() => import("@/components/abp/account/authenticator-settings"));
const BasicSettings = lazy(() => import("@/components/abp/account/basic-settings"));
const BindSettings = lazy(() => import("@/components/abp/account/bind-settings"));
const SecuritySettings = lazy(() => import("@/components/abp/account/security-settings"));
const SessionSettings = lazy(() => import("@/components/abp/account/session-settings"));
const NoticeSettings = lazy(() => import("@/components/abp/account/notice-settings"));
// const PersonalDataSettings = lazy(() => import("@/components/abp/account/personal-data-settings"));
const EmailConfirmModal = lazy(() => import("@/components/abp/account/email-confirm-modal"));
const ChangePasswordModal = lazy(() => import("@/components/abp/account/change-password-modal"));
const ChangePhoneNumberModal = lazy(() => import("@/components/abp/account/change-phone-number-modal"));

const MySetting: React.FC = () => {
	const { t: $t } = useTranslation();
	const [searchParams] = useSearchParams();
	const [modal, contextHolder] = Modal.useModal();
	const userInfo = useUserInfo();
	const { fetchAndSetUser } = useUserActions();
	const queryClient = useQueryClient();

	const [selectedKey, setSelectedKey] = useState<string>("basic");

	// Modal Visibility States
	const [emailConfirmVisible, setEmailConfirmVisible] = useState(false);
	const [passwordModalVisible, setPasswordModalVisible] = useState(false);
	const [phoneModalVisible, setPhoneModalVisible] = useState(false);

	const menuItems = [
		{ key: "basic", label: $t("abp.account.settings.basic.title") },
		{ key: "security", label: $t("abp.account.settings.security.title") },
		{ key: "bind", label: $t("abp.account.settings.bindSettings") },
		{ key: "session", label: $t("abp.account.settings.sessionSettings") },
		{ key: "notice", label: $t("abp.account.settings.noticeSettings") },
		{ key: "authenticator", label: $t("abp.account.settings.authenticatorSettings") },
		// { key: "personal-data", label: $t("abp.account.settings.personalDataSettings") },
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
			queryClient.invalidateQueries({ queryKey: ["profile"] });
			await fetchAndSetUser();
		},
	});

	useEffect(() => {
		const confirmToken = searchParams.get("confirmToken");
		if (confirmToken && profile) {
			setEmailConfirmVisible(true);
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
		setPasswordModalVisible(true);
	};

	const handleChangePhoneNumber = () => {
		setPhoneModalVisible(true);
	};

	const onPhoneNumberChanged = async (phoneNumber: string) => {
		// Optimistically update store or refetch user info
		await fetchAndSetUser();
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
			// case "personal-data": return <PersonalDataSettings />;
			default:
				return null;
		}
	};

	return (
		<div>
			{contextHolder}
			<Card>
				<div className="flex flex-col md:flex-row min-h-[600px]">
					<div className="w-full md:w-1/6 border-r border-gray-100">
						<Menu
							selectedKeys={[selectedKey]}
							items={menuItems}
							mode="inline"
							className="border-none"
							onSelect={({ key }) => setSelectedKey(key)}
						/>
					</div>
					<div className="w-full md:w-5/6 p-6">
						<Suspense
							fallback={
								<div className="flex justify-center p-10">
									<Spin />
								</div>
							}
						>
							{renderContent()}
						</Suspense>
					</div>
				</div>
			</Card>

			<Suspense fallback={null}>
				<EmailConfirmModal
					visible={emailConfirmVisible}
					initialState={{
						email: profile?.email || "",
						confirmToken: searchParams.get("confirmToken") || "",
						userId: searchParams.get("userId") || "",
						returnUrl: searchParams.get("returnUrl") || "",
					}}
					onClose={() => setEmailConfirmVisible(false)}
					onSuccess={async () => {
						await fetchAndSetUser();
					}}
				/>

				<ChangePasswordModal visible={passwordModalVisible} onClose={() => setPasswordModalVisible(false)} />

				<ChangePhoneNumberModal
					visible={phoneModalVisible}
					onClose={() => setPhoneModalVisible(false)}
					onChange={onPhoneNumberChanged}
				/>
			</Suspense>
		</div>
	);
};

export default MySetting;
