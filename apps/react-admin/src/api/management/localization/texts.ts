import type { ListResultDto } from "#/abp-core";

import type {
	GetTextByKeyInput,
	GetTextsInput,
	SetTextInput,
	TextDifferenceDto,
	TextDto,
} from "#/management/localization/texts";

import requestClient from "../../request";

/**
 * 查询文本列表
 * @param input 参数
 * @returns 文本列表
 */
export function getListApi(input: GetTextsInput): Promise<ListResultDto<TextDifferenceDto>> {
	return requestClient.get<ListResultDto<TextDifferenceDto>>("/api/abp/localization/texts", {
		params: input,
	});
}

/**
 * 查询文本
 * @param input 参数
 * @returns 查询的文本
 */
export function getApi(input: GetTextByKeyInput): Promise<TextDto> {
	return requestClient.get<TextDto>("/api/abp/localization/texts/by-culture-key", {
		params: input,
	});
}

/**
 * 设置文本
 * @param input 参数
 */
export function setApi(input: SetTextInput): Promise<void> {
	return requestClient.put("/api/localization/texts", input);
}
