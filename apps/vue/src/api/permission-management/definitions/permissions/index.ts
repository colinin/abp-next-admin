import { defHttp } from '/@/utils/http/axios';
import {
  PermissionDefinitionDto,
  PermissionDefinitionCreateDto,
  PermissionDefinitionUpdateDto,
  PermissionDefinitionGetListInput,
} from './model';

export const CreateAsyncByInput = (input: PermissionDefinitionCreateDto) => {
  return defHttp.post<PermissionDefinitionDto>({
    url: '/api/permission-management/definitions',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/permission-management/definitions/${name}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<PermissionDefinitionDto>({
    url: `/api/permission-management/definitions/${name}`,
  });
};

export const GetListAsyncByInput = (input: PermissionDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<PermissionDefinitionDto>>({
    url: '/api/permission-management/definitions',
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: PermissionDefinitionUpdateDto) => {
  return defHttp.put<PermissionDefinitionDto>({
    url: `/api/permission-management/definitions/${name}`,
    data: input,
  });
};
