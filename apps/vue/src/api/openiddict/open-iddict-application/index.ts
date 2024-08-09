import { defHttp } from '/@/utils/http/axios';
import {
  OpenIddictApplicationDto,
  OpenIddictApplicationGetListInput,
  OpenIddictApplicationCreateDto,
  OpenIddictApplicationUpdateDto,
} from './model';

export const get = (id: string) => {
  return defHttp.get<OpenIddictApplicationDto>({
    url: `/api/openiddict/applications/${id}`,
  });
};

export const getList = (input: OpenIddictApplicationGetListInput) => {
  return defHttp.get<PagedResultDto<OpenIddictApplicationDto>>({
    url: '/api/openiddict/applications',
    params: input,
  });
};

export const create = (input: OpenIddictApplicationCreateDto) => {
  return defHttp.post<OpenIddictApplicationDto>({
    url: '/api/openiddict/applications',
    data: input,
  });
};

export const update = (id: string, input: OpenIddictApplicationUpdateDto) => {
  return defHttp.put<OpenIddictApplicationDto>({
    url: `/api/openiddict/applications/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/openiddict/applications/${id}`,
  });
};