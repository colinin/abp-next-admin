import { defHttp } from '/@/utils/http/axios';
import {
  IUserData,
  UserLookupSearchRequest,
  UserLookupCountRequest,
} from './model';

export const findById = (id: string) => {
  return defHttp.get<IUserData>({
    url: `/api/identity/users/lookup/${id}`,
  });
};

export const findByUserName = (userName: string) => {
  return defHttp.get<IUserData>({
    url: `/api/identity/users/lookup/by-username/${userName}`,
  });
};

export const search = (input: UserLookupSearchRequest) => {
  return defHttp.get<ListResultDto<IUserData>>({
    url: '/api/identity/users/lookup/search',
    params: input,
  });
};

export const getCount = (input: UserLookupCountRequest) => {
  return defHttp.get<number>({
    url: '/api/identity/users/lookup/count',
    params: input,
  });
};
