import { defHttp } from '/@/utils/http/axios';
import {
  PermissionGroupDefinitionDto,
  PermissionGroupDefinitionCreateDto,
  PermissionGroupDefinitionUpdateDto,
  PermissionGroupDefinitionGetListInput,
} from './model';

export const CreateAsyncByInput = (input: PermissionGroupDefinitionCreateDto) => {
  return defHttp.post<PermissionGroupDefinitionDto>({
    url: '/api/permission-management/definitions/groups',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/permission-management/definitions/groups/${name}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<PermissionGroupDefinitionDto>({
    url: `/api/permission-management/definitions/groups/${name}`,
  });
};

export const GetListAsyncByInput = (input: PermissionGroupDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<PermissionGroupDefinitionDto>>({
    url: '/api/permission-management/definitions/groups',
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: PermissionGroupDefinitionUpdateDto) => {
  return defHttp.put<PermissionGroupDefinitionDto>({
    url: `/api/permission-management/definitions/groups/${name}`,
    data: input,
  });
};
