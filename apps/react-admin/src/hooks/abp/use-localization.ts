import { useEffect, useMemo } from "react";
import type { Dictionary } from "#/abp-core/global";
import { format } from "@/utils/string";
import useLocaleStore, { useLocale } from "@/store/localeI18nStore";
import { getResources } from "@/utils/abp/localzations/get-resources";

// const { L } = useLocalizer(['AbpAuditLogging', 'AbpUi']);

/*
	const { t: $t } = useTranslation();
	const { L } = useLocalizer();
  console.log(L('DisplayName:ClaimValue')) //和getI18nLocales()获取的不同，这个是没有前缀key的
  console.log($t('AbpIdentity.DisplayName:ClaimValue'))
*/
export function useLocalizer(resourceNames?: string | string[], callback?: () => void) {
	const localizations = useLocaleStore.getState().localizations || {};
	const locale = useLocale();

	const mergedResources = useMemo(() => getResources(resourceNames), [resourceNames]);

	useEffect(() => {
		if (callback) {
			callback();
		}
	}, [locale, callback]);

	/**
	 * 注意，使用react hook 来存储 key
	 * @param key
	 * @param args
	 * @returns
	 */
	function L(key: string, args?: any[] | Record<string, string>) {
		if (!key) return "";
		return mergedResources[key] ? format(mergedResources[key], args ?? []) : key;
	}
	/**
	 * 实际没调用国react hook,可以在很多地方调用
	 * @param resource
	 * @param key
	 * @param args
	 * @returns
	 */
	function Lr(resource: string, key: string, args?: any[] | Record<string, string>) {
		if (!resource || !key) return "";
		// const { localizations } = useLocaleStore.getState();
		const subResource = localizations[resource] ?? {};
		return subResource[key] ? format(subResource[key], args ?? []) : key;
	}

	return { L, Lr };
}
