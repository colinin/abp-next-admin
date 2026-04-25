import type { ListResultDto } from "#/abp-core";

import type { MenuCreateDto, MenuDto, MenuGetAllInput, MenuUpdateDto } from "#/platform/menus";

import requestClient from "../request";

export function createApi(input: MenuCreateDto): Promise<MenuDto> {
	return requestClient.post<MenuDto>("/api/platform/menus", input);
}

export function getAllApi(input?: MenuGetAllInput): Promise<ListResultDto<MenuDto>> {
	return requestClient.get<ListResultDto<MenuDto>>("/api/platform/menus/all", {
		params: input,
	});
}

export function getApi(id: string): Promise<MenuDto> {
	return requestClient.get<MenuDto>(`/api/platform/menus/${id}`);
}

export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/platform/menus/${id}`);
}

export function updateApi(id: string, input: MenuUpdateDto): Promise<MenuDto> {
	return requestClient.put<MenuDto>(`/api/platform/menus/${id}`, input);
}
