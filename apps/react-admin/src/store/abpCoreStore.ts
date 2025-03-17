import type { ApplicationConfigurationDto, ApplicationLocalizationDto } from "#/abp-core";
import { StorageEnum } from "#/enum";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

type AbpStore = {
	application: ApplicationConfigurationDto | undefined;
	localization: ApplicationLocalizationDto | undefined;
	actions: {
		getI18nLocales: () => Record<string, any>;
		setApplication: (val: ApplicationConfigurationDto) => void;
		setLocalization: (val: ApplicationLocalizationDto) => void;
		reset: () => void;
	};
};

const useAbpStore = create<AbpStore>()(
	persist(
		(set, get) => ({
			application: undefined,
			localization: undefined,
			actions: {
				// 获取 i18n 格式的本地化文本
				getI18nLocales: () => {
					const localization = get().localization;
					const abpLocales: Record<string, any> = {};
					if (!localization) {
						return abpLocales;
					}
					const resources = localization.resources;
					Object.keys(resources).forEach((resource) => {
						const resourceLocales: Record<string, any> = {};
						const resourcesByName = resources[resource];
						if (resourcesByName) {
							Object.keys(resourcesByName.texts).forEach((key) => {
								// let localeKey = key.replaceAll(".", "_");
								// // 清理多余的 _ 后缀或前缀
								// if (localeKey.endsWith("_")) {
								// 	localeKey = localeKey.slice(0, Math.max(0, localeKey.length - 1));
								// }
								// if (localeKey.startsWith("_")) {
								// 	localeKey = localeKey.slice(1);
								// }
								//(25-02-16) 不用额外转key "AbpIdentity.Users.UserName" -> "AbpIdentity_Users_UserName"

								resourceLocales[key] = resourcesByName.texts[key];
							});
							abpLocales[resource] = resourceLocales;
						}
					});
					return abpLocales;
				},
				// 设置 application 数据
				setApplication: (val) => set({ application: val }),
				// 设置 localization 数据
				setLocalization: (val) => set({ localization: val }),
				// 重置 store
				reset: () => set({ application: undefined, localization: undefined }),
			},
		}),
		{
			name: "abpStore", // localStorage key
			storage: createJSONStorage(() => localStorage),
			partialize: (state) => ({
				[StorageEnum.Application]: state.application,
			}),
		},
	),
);

// Store 的钩子函数
export const useApplication = () => useAbpStore((state) => state.application);
// export const useLocalization = () => useAbpStore((state) => state.localization);
export const useAbpActions = () => useAbpStore((state) => state.actions);

export default useAbpStore;
