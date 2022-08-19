import { defAbpHttp } from '/@/utils/http/abp';
import {
  OpenIddictTokenDto,
  OpenIddictTokenGetListInput,
} from './model';
import { PagedResultDto } from '../../model/baseModel';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictToken';

// export const getById = (id: string) => {
//   return defAbpHttp.request<OpenIddictTokenDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetAsync',
//     params: {
//       id: id,
//     },
//   });
// };

export const getById = (id: string) => {
  return defAbpHttp.get<OpenIddictTokenDto>({
    url: `/api/openiddict/tokens/${id}`,
  }, {
    withToken: false
  });
};

// export const getList = (input: OpenIddictTokenGetListInput) => {
//   return defAbpHttp.pagedRequest<OpenIddictTokenDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetListAsync',
//     params: {
//       input: input,
//     },
//   });
// };

export const getList = (input: OpenIddictTokenGetListInput) => {
  return defAbpHttp.get<PagedResultDto<OpenIddictTokenDto>>({
    url: '/api/openiddict/tokens',
    params: input,
  }, {
    withToken: false
  });
};


// export const deleteById = (id: string) => {
//   return defAbpHttp.request<void>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'DeleteAsync',
//     params: {
//       id: id,
//     },
//   });
// };

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: `/api/openiddict/tokens/${id}`,
  }, {
    withToken: false
  });
};
