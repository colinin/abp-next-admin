import { defAbpHttp } from '/@/utils/http/abp';
import {
  OpenIddictApplicationDto,
  OpenIddictApplicationCreateDto,
  OpenIddictApplicationUpdateDto,
  OpenIddictApplicationGetListInput,
} from './model';
import { PagedResultDto } from '../../model/baseModel';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictApplication';

export const getById = (id: string) => {
  return defAbpHttp.request<OpenIddictApplicationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const getList = (input: OpenIddictApplicationGetListInput) => {
  return defAbpHttp.get<PagedResultDto<OpenIddictApplicationDto>>({
    url: '/api/openiddict/applications',
    params: input,
  }, {
    withToken: false
  });
};

// export const getList = (input: OpenIddictApplicationGetListInput) => {
//   return defAbpHttp.pagedRequest<OpenIddictApplicationDto>({
//     service: remoteServiceName,
//     controller: controllerName,
//     action: 'GetListAsync',
//     params: {
//       input: input,
//     },
//   });
// };

export const deleteById = (id: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    params: {
      id: id,
    },
  });
};

export const create = (input: OpenIddictApplicationCreateDto) => {
  return defAbpHttp.request<OpenIddictApplicationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    data: input,
  });
};

export const update = (input: OpenIddictApplicationUpdateDto) => {
  return defAbpHttp.request<OpenIddictApplicationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    data: input,
  });
};
