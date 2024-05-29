import { defAbpHttp } from '/@/utils/http/abp';
import {
  OpenIddictApplicationDto,
  OpenIddictApplicationGetListInput,
  OpenIddictApplicationCreateDto,
  OpenIddictApplicationUpdateDto,
} from './model';

// export const GetAsyncById = (id: string) => {
//   return defAbpHttp.request<OpenIddictApplicationDto>({
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
  return defAbpHttp.get<OpenIddictApplicationDto>({
    url: `/api/openiddict/applications/${id}`,
  });
};

// export const GetListAsyncByInput = (input: OpenIddictApplicationGetListInput) => {
//   return defAbpHttp.pagedRequest<OpenIddictApplicationDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetListAsync',
//     uniqueName: 'GetListAsyncByInput',
//     params: {
//       input: input,
//     },
//   });
// };

export const getList = (input: OpenIddictApplicationGetListInput) => {
  return defAbpHttp.get<PagedResultDto<OpenIddictApplicationDto>>({
    url: '/api/openiddict/applications',
    params: input,
  });
};

// export const CreateAsyncByInput = (input: OpenIddictApplicationCreateDto) => {
//   return defAbpHttp.request<OpenIddictApplicationDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'CreateAsync',
//     uniqueName: 'CreateAsyncByInput',
//     data: input,
//   });
// };

export const create = (input: OpenIddictApplicationCreateDto) => {
  return defAbpHttp.post<OpenIddictApplicationDto>({
    url: '/api/openiddict/applications',
    data: input,
  });
};

// export const UpdateAsyncByIdAndInput = (id: string, input: OpenIddictApplicationUpdateDto) => {
//   return defAbpHttp.request<OpenIddictApplicationDto>({
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

export const update = (id: string, input: OpenIddictApplicationUpdateDto) => {
  return defAbpHttp.put<OpenIddictApplicationDto>({
    url: `/api/openiddict/applications/${id}`,
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
    url: `/api/openiddict/applications/${id}`,
  });
};