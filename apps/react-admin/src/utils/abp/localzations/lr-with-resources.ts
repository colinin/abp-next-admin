import type { Dictionary } from "#/abp-core";
import { format } from "@/utils/string";

/**
 * 允许外部传入 mergedResources 进行翻译
 * @param mergedResources 预计算的资源对象
 * @param key 需要翻译的 key
 * @param args 可选参数
 * @returns 翻译后的字符串
 */
export function LrWithResources(
	mergedResources: Dictionary<string, string>,
	key: string,
	args?: any[] | Record<string, string>,
) {
	if (!key) return "";
	return mergedResources[key] ? format(mergedResources[key], args ?? []) : key;
}
