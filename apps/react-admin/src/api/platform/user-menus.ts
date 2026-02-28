import type { ListResultDto } from "#/abp-core";

import type { MenuDto, MenuGetByUserInput, SetUserMenuInput, SetUserMenuStartupInput } from "#/platform/menus";

import requestClient from "../request";

export function getAllApi(input: MenuGetByUserInput): Promise<ListResultDto<MenuDto>> {
	return requestClient.get<ListResultDto<MenuDto>>(`/api/platform/menus/by-user/${input.userId}/${input.framework}`, {
		params: input,
	});
}

export function setMenusApi(input: SetUserMenuInput): Promise<void> {
	return requestClient.put("/api/platform/menus/by-user", input);
}

export function setStartupMenuApi(meudId: string, input: SetUserMenuStartupInput): Promise<void> {
	return requestClient.put(`/api/platform/menus/startup/${meudId}/by-user`, input);
}
