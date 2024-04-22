import { defAbpHttp } from '/@/utils/http/abp';
import { OpenIddictTokenDto, OpenIddictTokenGetListInput,  } from './model';

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

export const DeleteAsyncById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: `/api/openiddict/tokens/${id}`,
  });
};

// export const GetAsyncById = (id: string) => {
//   return defAbpHttp.request<OpenIddictTokenDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetAsync',
//     uniqueName: 'GetAsyncById',
//     params: {
//       id: id,
//     },
//   });
// };

export const GetAsyncById = (id: string) => {
  return defAbpHttp.get<OpenIddictTokenDto>({
    url: `/api/openiddict/tokens/${id}`,
  });
};

// export const GetListAsyncByInput = (input: OpenIddictTokenGetListInput) => {
//   return defAbpHttp.pagedRequest<OpenIddictTokenDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetListAsync',
//     uniqueName: 'GetListAsyncByInput',
//     params: {
//       input: input,
//     },
//   });
// };

export const GetListAsyncByInput = (input: OpenIddictTokenGetListInput) => {
  return defAbpHttp.get<PagedResultDto<OpenIddictTokenDto>>({
    url: '/api/openiddict/tokens',
    params: input,
  });
};