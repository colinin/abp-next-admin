import { defAbpHttp } from '/@/utils/http/abp';
import { 
  OpenIddictScopeCreateDto,
  OpenIddictScopeDto,
  OpenIddictScopeGetListInput,
  OpenIddictScopeUpdateDto,
} from './model';

// export const CreateAsyncByInput = (input: OpenIddictScopeCreateDto) => {
//   return defAbpHttp.request<OpenIddictScopeDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'CreateAsync',
//     uniqueName: 'CreateAsyncByInput',
//     data: input,
//   });
// };

export const create = (input: OpenIddictScopeCreateDto) => {
  return defAbpHttp.post<OpenIddictScopeDto>({
    url: '/api/openiddict/scopes',
    data: input,
  });
};

// export const DeleteAsyncById = (id: string) => {
//   return defAbpHttp.request<void>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'DeleteAsync',
//     uniqueName: 'DeleteAsyncById',
//     params: {
//       id: id,
//     },
//   });
// };

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: `/api/openiddict/scopes/${id}`
  });
};

// export const GetAsyncById = (id: string) => {
//   return defAbpHttp.request<OpenIddictScopeDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetAsync',
//     uniqueName: 'GetAsyncById',
//     params: {
//       id: id,
//     },
//   });
// };

export const get = (id: string) => {
  return defAbpHttp.get<OpenIddictScopeDto>({
    url: `/api/openiddict/scopes/${id}`
  });
};

// export const GetListAsyncByInput = (input: OpenIddictScopeGetListInput) => {
//   return defAbpHttp.pagedRequest<OpenIddictScopeDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetListAsync',
//     uniqueName: 'GetListAsyncByInput',
//     params: {
//       input: input,
//     },
//   });
// };

export const getList = (input: OpenIddictScopeGetListInput) => {
  return defAbpHttp.get<PagedResultDto<OpenIddictScopeDto>>({
    url: '/api/openiddict/scopes',
    params: input,
  });
};

// export const UpdateAsyncByIdAndInput = (id: string, input: OpenIddictScopeUpdateDto) => {
//   return defAbpHttp.request<OpenIddictScopeDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'UpdateAsync',
//     uniqueName: 'UpdateAsyncByIdAndInput',
//     params: {
//       id: id,
//     },
//     data: input,
//   });
// };

export const update = (id: string, input: OpenIddictScopeUpdateDto) => {
  return defAbpHttp.put<OpenIddictScopeDto>({
    url: `/api/openiddict/scopes/${id}`,
    data: input,
  });
};