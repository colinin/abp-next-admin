import { defHttp } from '/@/utils/http/axios';
import { OpenIddictAuthorizationDto, OpenIddictAuthorizationGetListInput,  } from './model';

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/openiddict/authorizations/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<OpenIddictAuthorizationDto>({
    url: `/api/openiddict/authorizations/${id}`,
  });
};

export const getList = (input: OpenIddictAuthorizationGetListInput) => {
  return defHttp.get<PagedResultDto<OpenIddictAuthorizationDto>>({
    url: '/api/openiddict/authorizations',
    params: input,
  });
};