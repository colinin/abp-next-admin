import { defAbpHttp } from '/@/utils/http/abp';
import { OpenIddictAuthorizationDto, OpenIddictAuthorizationGetListInput,  } from './model';

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
    url: `/api/openiddict/authorizations/${id}`,
  });
};

// export const GetAsyncById = (id: string) => {
//   return defAbpHttp.request<OpenIddictAuthorizationDto>({
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
  return defAbpHttp.get<OpenIddictAuthorizationDto>({
    url: `/api/openiddict/authorizations/${id}`,
  });
};

// export const GetListAsyncByInput = (input: OpenIddictAuthorizationGetListInput) => {
//   return defAbpHttp.pagedRequest<OpenIddictAuthorizationDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetListAsync',
//     uniqueName: 'GetListAsyncByInput',
//     params: {
//       input: input,
//     },
//   });
// };

export const getList = (input: OpenIddictAuthorizationGetListInput) => {
  return defAbpHttp.get<PagedResultDto<OpenIddictAuthorizationDto>>({
    url: '/api/openiddict/authorizations',
    params: input,
  });
};