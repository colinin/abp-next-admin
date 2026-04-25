import type { ListResultDto } from "#/abp-core";

import type { UserFavoriteMenuCreateDto, UserFavoriteMenuDto } from "#/platform/favorites";

import requestClient from "../request";

/**
 * 新增常用菜单
 * @param input 参数
 * @returns 常用菜单
 */
export function createApi(input: UserFavoriteMenuCreateDto): Promise<UserFavoriteMenuDto> {
	return requestClient.post<UserFavoriteMenuDto>("/api/platform/menus/favorites/my-favorite-menus", input);
}

/**
 * 删除常用菜单
 * @param id 菜单Id
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/platform/menus/favorites/my-favorite-menus/${id}`);
}

/**
 * 获取常用菜单列表
 * @param framework ui框架
 * @returns 菜单列表
 */
export function getListApi(framework?: string): Promise<ListResultDto<UserFavoriteMenuDto>> {
	return requestClient.get<ListResultDto<UserFavoriteMenuDto>>("/api/platform/menus/favorites/my-favorite-menus", {
		params: { framework },
	});
}
