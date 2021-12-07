import { defAbpHttp } from '/@/utils/http/abp';
import {
  IUserData,
  UserLookupSearchRequest,
  UserLookupSearchResult,
  UserLookupCountRequest,
} from './model/userLookupModel';
import { format } from '/@/utils/strings';

enum Api {
  Search = '/api/identity/users/lookup/search',
  GetCount = '/api/identity/users/lookup/count',
  FindById = '/api/identity/users/lookup/{id}',
  FindByUserName = '/api/identity/users/lookup/by-username/{userName}',
}

export const findById = (id: string) => {
  return defAbpHttp.get<IUserData>({
    url: format(Api.FindById, { id: id }),
  });
};

export const findByUserName = (userName: string) => {
  return defAbpHttp.get<IUserData>({
    url: format(Api.FindByUserName, { userName: userName }),
  });
};

export const search = (input: UserLookupSearchRequest) => {
  return defAbpHttp.get<UserLookupSearchResult>({
    url: Api.Search,
    params: input,
  });
};

export const getCount = (input: UserLookupCountRequest) => {
  return defAbpHttp.get<number>({
    url: Api.GetCount,
    params: input,
  });
};
