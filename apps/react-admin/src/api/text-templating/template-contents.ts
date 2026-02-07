import type {
	TextTemplateContentDto,
	TextTemplateContentGetInput,
	TextTemplateContentUpdateDto,
	TextTemplateRestoreInput,
} from "#/text-templating/contents";

import requestClient from "../request";

/**
 * 获取模板内容
 * @param input 参数
 * @returns 模板内容数据传输对象
 */
export function getApi(input: TextTemplateContentGetInput): Promise<TextTemplateContentDto> {
	let url = "/api/text-templating/templates/content";
	url += input.culture ? `/${input.culture}/${input.name}` : `/${input.name}`;
	return requestClient.get<TextTemplateContentDto>(url);
}

/**
 * 重置模板内容为默认值
 * @param name 模板名称
 * @param input 参数
 * @returns 模板定义数据传输对象列表
 */
export function restoreToDefaultApi(name: string, input: TextTemplateRestoreInput): Promise<void> {
	return requestClient.put(`/api/text-templating/templates/content/${name}/restore-to-default`, input);
}

/**
 * 更新模板内容
 * @param name 模板名称
 * @param input 参数
 * @returns 模板内容数据传输对象
 */
export function updateApi(name: string, input: TextTemplateContentUpdateDto): Promise<TextTemplateContentDto> {
	return requestClient.put<TextTemplateContentDto>(`/api/text-templating/templates/content/${name}`, input);
}
