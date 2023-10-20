import { defHttp } from '/@/utils/http/axios';
import {
  SettingDefinitionDto,
  SettingDefinitionCreateDto,
  SettingDefinitionUpdateDto,
  SettingDefinitionGetListInput,
 } from './model';


export const GetAsyncByName = (name: string) => {
  return defHttp.get<SettingDefinitionDto>({
    url: `/api/setting-management/settings/definitions/${name}`,
  });
};

export const GetListAsyncByInput = (input: SettingDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<SettingDefinitionDto>>({
    url: `/api/setting-management/settings/definitions`,
    params: input,
  });
}

export const CreateAsyncByInput = (input: SettingDefinitionCreateDto) => {
  return defHttp.post<SettingDefinitionDto>({
    url: `/api/setting-management/settings/definitions`,
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: SettingDefinitionUpdateDto) => {
  return defHttp.put<SettingDefinitionDto>({
    url: `/api/setting-management/settings/definitions/${name}`,
    data: input,
  });
};

export const DeleteOrRestoreAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/setting-management/settings/definitions/${name}`,
  });
};
