import type { ListResultDto } from "#/abp-core";

import type { MenuDto, MenuGetInput } from "#/platform/menus";

import requestClient from "../request";

export function getAllApi(input?: MenuGetInput): Promise<ListResultDto<MenuDto>> {
	return requestClient.get<ListResultDto<MenuDto>>("/api/platform/menus/by-current-user", {
		params: input,
	});
}
