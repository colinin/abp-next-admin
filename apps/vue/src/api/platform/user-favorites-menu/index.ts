import { defAbpHttp } from '/@/utils/http/abp';
import { UserFavoriteMenuDto, UserFavoriteMenuCreateDto, UserFavoriteMenuUpdateDto } from './model';

const remoteService = {
  name: 'Platform',
  controller: 'UserFavoriteMenu',
  replace: {
    framework: 'Vue Vben Admin',
  }
};

export const create = (userId: string, input: UserFavoriteMenuCreateDto) => {
  input.framework = remoteService.replace.framework;
  return defAbpHttp.request<UserFavoriteMenuDto>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'CreateAsync',
    params: {
      userId: userId,
    },
    data: {
      input: input,
    },
  });
};

export const createMyFavoriteMenu = (input: UserFavoriteMenuCreateDto) => {
  input.framework = remoteService.replace.framework;
  return defAbpHttp.request<UserFavoriteMenuDto>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'CreateMyFavoriteMenuAsync',
    data: {
      input: input,
    },
  });
};

export const del = (userId: string, menuId: string) => {
  return defAbpHttp.request<void>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'DeleteAsync',
    params: {
      userId: userId,
    },
    data: {
      input: {
        menuId: menuId,
      },
    },
  });
};

export const delMyFavoriteMenu = (menuId: string) => {
  return defAbpHttp.request<void>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'DeleteMyFavoriteMenuAsync',
    data: {
      input: {
        menuId: menuId,
      },
    },
  });
};

export const update = (userId: string, input: UserFavoriteMenuUpdateDto) => {
  return defAbpHttp.request<UserFavoriteMenuDto>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'UpdateAsync',
    params: {
      userId: userId,
    },
    data: {
      input: input,
    },
  });
};

export const updateMyFavoriteMenu = (input: UserFavoriteMenuUpdateDto) => {
  return defAbpHttp.request<UserFavoriteMenuDto>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'UpdateMyFavoriteMenuAsync',
    data: {
      input: input,
    },
  });
};

export const getList = (userId: string) => {
  return defAbpHttp.listRequest<UserFavoriteMenuDto>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'GetListAsync',
    params: {
      userId: userId,
      framework: remoteService.replace.framework,
    },
  });
};

export const getMyFavoriteMenuList = () => {
  return defAbpHttp.listRequest<UserFavoriteMenuDto>({
    service: remoteService.name,
    controller: remoteService.controller,
    action: 'GetMyFavoriteMenuListAsync',
    params: {
      framework: remoteService.replace.framework,
    },
  });
};
