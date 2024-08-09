import { defHttp } from '/@/utils/http/axios';
import { 
  OpenIddictScopeCreateDto,
  OpenIddictScopeDto,
  OpenIddictScopeGetListInput,
  OpenIddictScopeUpdateDto,
} from './model';

export const create = (input: OpenIddictScopeCreateDto) => {
  return defHttp.post<OpenIddictScopeDto>({
    url: '/api/openiddict/scopes',
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/openiddict/scopes/${id}`
  });
};

export const get = (id: string) => {
  return defHttp.get<OpenIddictScopeDto>({
    url: `/api/openiddict/scopes/${id}`
  });
};

export const getList = (input: OpenIddictScopeGetListInput) => {
  return defHttp.get<PagedResultDto<OpenIddictScopeDto>>({
    url: '/api/openiddict/scopes',
    params: input,
  });
};

export const update = (id: string, input: OpenIddictScopeUpdateDto) => {
  return defHttp.put<OpenIddictScopeDto>({
    url: `/api/openiddict/scopes/${id}`,
    data: input,
  });
};