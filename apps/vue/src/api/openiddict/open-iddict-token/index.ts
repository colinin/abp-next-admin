import { defHttp } from '/@/utils/http/axios';
import { OpenIddictTokenDto, OpenIddictTokenGetListInput,  } from './model';

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/openiddict/tokens/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<OpenIddictTokenDto>({
    url: `/api/openiddict/tokens/${id}`,
  });
};

export const getList = (input: OpenIddictTokenGetListInput) => {
  return defHttp.get<PagedResultDto<OpenIddictTokenDto>>({
    url: '/api/openiddict/tokens',
    params: input,
  });
};