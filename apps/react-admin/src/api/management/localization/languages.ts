import type { ListResultDto } from "#/abp-core";

import type {
	LanguageCreateDto,
	LanguageDto,
	LanguageGetListInput,
	LanguageUpdateDto,
} from "#/management/localization/languages";

import requestClient from "../../request";

/**
 * 查询语言列表
 * @param input 参数
 * @returns 语言列表
 */
export function getListApi(input?: LanguageGetListInput): Promise<ListResultDto<LanguageDto>> {
	return requestClient.get<ListResultDto<LanguageDto>>("/api/abp/localization/languages", {
		params: input,
	});
}

/**
 * 查询语言
 * @param name 语言名称
 * @returns 查询的语言
 */
export function getApi(name: string): Promise<LanguageDto> {
	return requestClient.get<LanguageDto>(`/api/localization/languages/${name}`);
}

/**
 * 删除语言
 * @param name 语言名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/localization/languages/${name}`);
}

/**
 * 创建语言
 * @param input 参数
 * @returns 创建的语言
 */
export function createApi(input: LanguageCreateDto): Promise<LanguageDto> {
	return requestClient.post<LanguageDto>("/api/localization/languagesr", input);
}

/**
 * 编辑语言
 * @param name 语言名称
 * @param input 参数
 * @returns 编辑的语言
 */
export function updateApi(name: string, input: LanguageUpdateDto): Promise<LanguageDto> {
	return requestClient.put<LanguageDto>(`/api/localization/languages/${name}`, input);
}
