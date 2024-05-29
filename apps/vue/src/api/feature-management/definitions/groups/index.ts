import { defHttp } from '/@/utils/http/axios';
import {
  FeatureGroupDefinitionDto,
  FeatureGroupDefinitionCreateDto,
  FeatureGroupDefinitionUpdateDto,
  FeatureGroupDefinitionGetListInput,
} from './model';

export const create = (input: FeatureGroupDefinitionCreateDto) => {
  return defHttp.post<FeatureGroupDefinitionDto>({
    url: '/api/feature-management/definitions/groups',
    data: input,
  });
};

export const deleteByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/feature-management/definitions/groups/${name}`,
  });
};

export const getByName = (name: string) => {
  return defHttp.get<FeatureGroupDefinitionDto>({
    url: `/api/feature-management/definitions/groups/${name}`,
  });
};

export const getList = (input: FeatureGroupDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<FeatureGroupDefinitionDto>>({
    url: '/api/feature-management/definitions/groups',
    params: input,
  });
};

export const update = (name: string, input: FeatureGroupDefinitionUpdateDto) => {
  return defHttp.put<FeatureGroupDefinitionDto>({
    url: `/api/feature-management/definitions/groups/${name}`,
    data: input,
  });
};
