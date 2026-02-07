import type { ListResultDto } from "#/abp-core";

import type {
	TextTemplateDefinitionCreateDto,
	TextTemplateDefinitionDto,
	TextTemplateDefinitionGetListInput,
	TextTemplateDefinitionUpdateDto,
} from "#/text-templating/definitions";

import requestClient from "../request";

/**
 * 新增模板定义
 * @param input 参数
 * @returns 模板定义数据传输对象
 */
export function createApi(input: TextTemplateDefinitionCreateDto): Promise<TextTemplateDefinitionDto> {
	return requestClient.post<TextTemplateDefinitionDto>("/api/text-templating/template/definitions", input);
}

/**
 * 删除模板定义
 * @param name 模板名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/text-templating/template/definitions/${name}`);
}

/**
 * 获取模板定义
 * @param name 模板名称
 * @returns 模板定义数据传输对象
 */
export function getApi(name: string): Promise<TextTemplateDefinitionDto> {
	return requestClient.get<TextTemplateDefinitionDto>(`/api/text-templating/template/definitions/${name}`);
}

/**
 * 获取模板定义列表
 * @param input 过滤参数
 * @returns 模板定义数据传输对象列表
 */
export function getListApi(
	input?: TextTemplateDefinitionGetListInput,
): Promise<ListResultDto<TextTemplateDefinitionDto>> {
	return requestClient.get<ListResultDto<TextTemplateDefinitionDto>>("/api/text-templating/template/definitions", {
		params: input,
	});
}

/**
 * 更新模板定义
 * @param name 模板名称
 * @returns 模板定义数据传输对象
 */
export function updateApi(name: string, input: TextTemplateDefinitionUpdateDto): Promise<TextTemplateDefinitionDto> {
	return requestClient.put<TextTemplateDefinitionDto>(`/api/text-templating/template/definitions/${name}`, input);
}
