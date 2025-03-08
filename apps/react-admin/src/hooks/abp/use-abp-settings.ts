import type { ISettingProvider, SettingValue } from "#/settings";
import useAbpStore from "@/store/abpCoreStore";
import { useMemo } from "react";

export function useAbpSettings(): ISettingProvider {
	const application = useAbpStore.getState().application;

	const getSettings = useMemo(() => {
		if (!application) {
			return [];
		}
		const { values: settings } = application.setting;
		return Object.keys(settings).map((key) => ({
			name: key,
			value: settings[key] ?? "",
		}));
	}, [application]);

	function get(name: string): SettingValue | undefined {
		return getSettings.find((setting) => name === setting.name);
	}

	function getAll(...names: string[]): SettingValue[] {
		if (names.length > 0) {
			return getSettings.filter((setting) => names.includes(setting.name));
		}
		return getSettings;
	}

	function getOrDefault<T>(name: string, defaultValue: T): string | T {
		const setting = get(name);
		if (!setting) {
			return defaultValue;
		}
		return setting.value;
	}

	const settingProvider: ISettingProvider = {
		getAll(...names: string[]) {
			return getAll(...names);
		},
		getNumber(name: string, defaultValue = 0) {
			const value = getOrDefault(name, defaultValue);
			const num = Number(value);
			return Number.isNaN(num) ? defaultValue : num;
		},
		getOrEmpty(name: string) {
			return getOrDefault(name, "");
		},
		isTrue(name: string) {
			const value = getOrDefault(name, "false");
			return typeof value === "string" && value.toLowerCase() === "true";
		},
	};

	return settingProvider;
}
