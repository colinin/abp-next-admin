import { create } from "zustand";
import useAbpStore from "./abpCoreStore";

// Interface
export interface SettingValue {
	name: string;
	value: string | null;
}

type AbpSettingStore = {
	actions: {
		get: (name: string) => string | undefined;
		getOrEmpty: (name: string) => string;
		getNumber: (name: string, defaultValue?: number) => number;
		isTrue: (name: string) => boolean;
		getAll: (...names: string[]) => SettingValue[];
	};
};

const useAbpSettingStore = create<AbpSettingStore>()(() => ({
	actions: {
		/**
		 * Gets a setting value by name.
		 */
		get: (name: string) => {
			const values = useAbpStore.getState().application?.setting?.values;
			return values ? values[name] : undefined;
		},

		/**
		 * Gets a setting value or returns an empty string if not found.
		 */
		getOrEmpty: (name: string) => {
			const values = useAbpStore.getState().application?.setting?.values;
			return values?.[name] ?? "";
		},

		/**
		 * Gets a setting value as a number.
		 */
		getNumber: (name: string, defaultValue = 0) => {
			const values = useAbpStore.getState().application?.setting?.values;
			const value = values?.[name];

			if (value === undefined || value === null) {
				return defaultValue;
			}

			const num = Number(value);
			return Number.isNaN(num) ? defaultValue : num;
		},

		/**
		 * Checks if a setting value is "true" (case-insensitive).
		 */
		isTrue: (name: string) => {
			const values = useAbpStore.getState().application?.setting?.values;
			const value = values?.[name];
			return value?.toLowerCase() === "true";
		},

		/**
		 * Gets all settings, optionally filtered by specific names.
		 * Returns them as an array of objects { name, value } to match Vue behavior.
		 */
		getAll: (...names: string[]) => {
			const values = useAbpStore.getState().application?.setting?.values;
			if (!values) return [];

			const settingsSet: SettingValue[] = Object.keys(values).map((key) => ({
				name: key,
				value: values[key],
			}));

			if (names.length > 0) {
				return settingsSet.filter((setting) => names.includes(setting.name));
			}

			return settingsSet;
		},
	},
}));

// Export actions directly for ease of use
export const useAbpSettings = () => useAbpSettingStore((state) => state.actions);

// Optional: specific hooks if it need reactive re-renders in components
export const useSettingValue = (name: string) => useAbpStore((state) => state.application?.setting?.values?.[name]);

export default useAbpSettingStore;
