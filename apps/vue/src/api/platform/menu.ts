import { defHttp } from '/@/utils/http/axios';
import { format } from '/@/utils/strings';
import {
  Menu,
  CreateMenu,
  UpdateMenu,
  GetAllMenuRequest,
  MenuListResult,
  GetMenuPagedRequest,
  MenuPagedResult,
  UserMenu,
  RoleMenu,
} from './model/menuModel';

enum Api {
  GetById = '/api/platform/menus/{id}',
  GetList = '/api/platform/menus',
  GetAll = '/api/platform/menus/all',
  GetUserMenus = '/api/platform/menus/by-user/{userId}/{framework}',
  SetUserMenus = 'api/platform/menus/by-user',
  GetRoleMenus = '/api/platform/menus/by-role/{role}/{framework}',
  SetRoleMenus = '/api/platform/menus/by-role',
  GetMyMenus = '/api/platform/menus/by-current-user',
  Create = '/api/platform/menus',
  Delete = '/api/platform/menus/{id}',
  Update = '/api/platform/menus/{id}',
}

export const create = (input: CreateMenu) => {
  return defHttp.post<Menu>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: UpdateMenu) => {
  return defHttp.put<Menu>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};

export const getById = (id: string) => {
  return defHttp.get<Menu>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getAll = (input: GetAllMenuRequest) => {
  return defHttp.get<MenuListResult>({
    url: Api.GetAll,
    params: input,
  });
};

export const getList = (input: GetMenuPagedRequest) => {
  return defHttp.get<MenuPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const getListByUser = (userId: string, framework: string) => {
  return defHttp.get<MenuListResult>({
    url: format(Api.GetUserMenus, { userId: userId, framework: framework }),
  });
};

export const getListByRole = (role: string, framework: string) => {
  return defHttp.get<MenuListResult>({
    url: format(Api.GetRoleMenus, { role: role, framework: framework }),
  });
};

export const setUserMenu = (input: UserMenu) => {
  return defHttp.put<void>({
    url: Api.SetUserMenus,
    data: input,
  });
};

export const setRoleMenu = (input: RoleMenu) => {
  return defHttp.put<void>({
    url: Api.SetRoleMenus,
    data: input,
  });
};
