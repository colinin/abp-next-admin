import { defHttp } from '/@/utils/http/axios';
import {
  Menu,
  CreateMenu,
  UpdateMenu,
  GetAllMenuRequest,
  GetMenuPagedRequest,
  UserMenu,
  RoleMenu,
} from './model';

export const create = (input: CreateMenu) => {
  return defHttp.post<Menu>({
    url: '/api/platform/menus',
    data: input,
  });
};

export const update = (id: string, input: UpdateMenu) => {
  return defHttp.put<Menu>({
    url: `/api/platform/menus/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/menus/${id}`,
  });
};

export const getById = (id: string) => {
  return defHttp.get<Menu>({
    url: `/api/platform/menus/${id}`,
  });
};

export const getAll = (input: GetAllMenuRequest) => {
  return defHttp.get<ListResultDto<Menu>>({
    url: '/api/platform/menus/all',
    params: input,
  });
};

export const getList = (input: GetMenuPagedRequest) => {
  return defHttp.get<PagedResultDto<Menu>>({
    url: '/api/platform/menus',
    params: input,
  });
};

export const getListByUser = (userId: string, framework: string) => {
  return defHttp.get<ListResultDto<Menu>>({
    url: `/api/platform/menus/by-user/${userId}/${framework}`,
  });
};

export const getListByRole = (role: string, framework: string) => {
  return defHttp.get<ListResultDto<Menu>>({
    url: `/api/platform/menus/by-role/${role}/${framework}`,
  });
};

export const setUserMenu = (input: UserMenu) => {
  return defHttp.put<void>({
    url: '/api/platform/menus/by-user',
    data: input,
  });
};

export const setUserStartupMenu = (userId: string, menuId: string) => {
  return defHttp.put<void>({
    url: `/api/platform/menus/startup/${menuId}/by-user`,
    data: {
      userId: userId,
    },
  });
};

export const setRoleMenu = (input: RoleMenu) => {
  return defHttp.put<void>({
    url: '/api/platform/menus/by-role',
    data: input,
  });
};

export const setRoleStartupMenu = (roleName: string, menuId: string) => {
  return defHttp.put<void>({
    url: `/api/platform/menus/startup/${menuId}/by-role`,
    data: {
      roleName: roleName,
    },
  });
};
