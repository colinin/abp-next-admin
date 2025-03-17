import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

import { toast } from "sonner";
import type { UserInfo, UserToken } from "#/entity";
import { StorageEnum } from "#/enum";
import { getUserInfoApi, loginApi } from "@/api/account";
import type { PasswordTokenRequestModel } from "#/account";
import { getConfigApi } from "@/api/abp-core";
import useAbpStore from "./abpCoreStore";
import { useEventBus } from "@/utils/abp/useEventBus";
import { Events } from "@/constants/abp-core";

const { VITE_APP_HOMEPAGE: HOMEPAGE } = import.meta.env;

type UserStore = {
	userInfo: Partial<UserInfo>;
	userToken: UserToken;
	accessCodes: string[]; //权限码 TODO 之后和动态生成菜单放一块去
	// 使用 actions 命名空间来存放所有的 action
	actions: {
		setUserInfo: (userInfo: UserInfo) => void;
		setUserToken: (token: UserToken) => void;
		setAccessCodes: (accessCodes: string[]) => void;
		clearUserInfoAndToken: () => void;
		fetchAndSetUser: () => Promise<UserInfo | null>;
	};
};

const useUserStore = create<UserStore>()(
	persist(
		(set) => ({
			userInfo: {},
			userToken: {},
			accessCodes: [],
			actions: {
				setUserInfo: (userInfo) => {
					set({ userInfo });
				},
				setUserToken: (userToken) => {
					set({ userToken });
				},
				setAccessCodes: (accessCodes) => {
					set({ accessCodes });
				},
				clearUserInfoAndToken() {
					const { publish } = useEventBus();
					publish(Events.UserLogout);
					set({ userInfo: {}, userToken: {} });
				},
				fetchAndSetUser: async () => {
					let userInfo: ({ [key: string]: any } & UserInfo) | null = null;

					try {
						const userInfoRes = await getUserInfoApi();
						const abpConfig = await getConfigApi();

						userInfo = {
							id: userInfoRes.sub, //额外加的
							userId: userInfoRes.sub,
							username: userInfoRes.uniqueName ?? abpConfig.currentUser.userName,
							realName: userInfoRes.name ?? abpConfig.currentUser.name,
							avatar: userInfoRes.avatarUrl ?? userInfoRes.picture,
							desc: userInfoRes.uniqueName ?? userInfoRes.name,
							email: userInfoRes.email ?? userInfoRes.email,
							emailVerified: userInfoRes.emailVerified ?? abpConfig.currentUser.emailVerified,
							phoneNumber: userInfoRes.phoneNumber ?? abpConfig.currentUser.phoneNumber,
							phoneNumberVerified: userInfoRes.phoneNumberVerified ?? abpConfig.currentUser.phoneNumberVerified,
							token: "",
							roles: abpConfig.currentUser.roles,
							homePath: "/",
						};

						// 更新一系列到 zustand store 中
						set({ userInfo });

						useAbpStore.getState().actions.setApplication(abpConfig);

						set({ accessCodes: Object.keys(abpConfig.auth.grantedPolicies) });
					} catch (err) {
						console.error("Failed to fetch user info:", err);
					}

					return userInfo;
				},
			},
		}),
		{
			name: "userStore", // name of the item in the storage (must be unique)
			storage: createJSONStorage(() => localStorage), // (optional) by default, 'localStorage' is used
			partialize: (state) => ({
				[StorageEnum.UserInfo]: state.userInfo,
				[StorageEnum.UserToken]: state.userToken,
				[StorageEnum.AccessCodes]: state.accessCodes,
			}),
		},
	),
);

export const useUserInfo = () => useUserStore((state) => state.userInfo);
export const useUserToken = () => useUserStore((state) => state.userToken);
export const useUserPermission = () => useUserStore((state) => state.userInfo.permissions);
export const useUserAccessCodes = () => useUserStore((state) => state.accessCodes);
export const useUserActions = () => useUserStore((state) => state.actions);

export const useSignIn = () => {
	const navigatge = useNavigate();
	const { setUserToken, fetchAndSetUser } = useUserActions();

	const signInMutation = useMutation({
		mutationFn: loginApi,
		retry: 0,
	});

	const signIn = async (data: PasswordTokenRequestModel) => {
		try {
			const res = await signInMutation.mutateAsync(data);

			const { tokenType, accessToken, refreshToken } = res;
			// 如果成功获取到 accessToken
			if (accessToken) {
				setUserToken({ accessToken: `${tokenType} ${accessToken}`, refreshToken });

				await fetchAndSetUser();

				navigatge(HOMEPAGE, { replace: true });
				toast.success("Sign in success!");
			}
		} catch (err) {
			console.error(err.message);
		}
	};

	return signIn;
};

export default useUserStore;
