import type { ListResultDto } from "#/abp-core";

import type { MenuDto, MenuGetByRoleInput, SetRoleMenuInput, SetRoleMenuStartupInput } from "#/platform/menus";

import requestClient from "../request";

export function getAllApi(input: MenuGetByRoleInput): Promise<ListResultDto<MenuDto>> {
	return requestClient.get<ListResultDto<MenuDto>>(`/api/platform/menus/by-role/${input.role}/${input.framework}`, {
		params: input,
	});
}

export function setMenusApi(input: SetRoleMenuInput): Promise<void> {
	return requestClient.put("/api/platform/menus/by-role", input);
}

export function setStartupMenuApi(meudId: string, input: SetRoleMenuStartupInput): Promise<void> {
	return requestClient.put(`/api/platform/menus/startup/${meudId}/by-role`, input);
}
