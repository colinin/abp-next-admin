import { defHttp } from '/@/utils/http/axios';
import { UserFavoriteMenuDto, UserFavoriteMenuCreateDto, UserFavoriteMenuUpdateDto } from './model';

export const create = (userId: string, input: UserFavoriteMenuCreateDto) => {
  input.framework = 'Vue Vben Admin';
  return defHttp.post<UserFavoriteMenuDto>({
    url: `/api/platform/menus/favorites/${userId}`,
    data: input,
  });
};

export const createMyFavoriteMenu = (input: UserFavoriteMenuCreateDto) => {
  input.framework = 'Vue Vben Admin';
  return defHttp.post<UserFavoriteMenuDto>({
    url: `/api/platform/menus/favorites/my-favorite-menus`,
    data: input,
  });
};

export const del = (userId: string, menuId: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/menus/favorites/${userId}/${menuId}`,
  });
};

export const delMyFavoriteMenu = (menuId: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/menus/favorites/my-favorite-menus/${menuId}`,
  });
};

export const update = (userId: string, input: UserFavoriteMenuUpdateDto) => {
  return defHttp.put<UserFavoriteMenuDto>({
    url: `/api/platform/menus/favorites/${userId}`,
    data: input,
  });
};

export const updateMyFavoriteMenu = (input: UserFavoriteMenuUpdateDto) => {
  return defHttp.put<UserFavoriteMenuDto>({
    url: `/api/platform/menus/favorites/my-favorite-menus`,
    data: input,
  });
};

export const getList = (userId: string) => {
  return defHttp.get<ListResultDto<UserFavoriteMenuDto>>({
    url: `/api/platform/menus/favorites/${userId}?framework=Vue Vben Admin`,
  });
};

export const getMyFavoriteMenuList = () => {
  return defHttp.get<ListResultDto<UserFavoriteMenuDto>>({
    url: `/api/platform/menus/favorites/my-favorite-menus?framework=Vue Vben Admin`,
  });
};
