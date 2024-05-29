import { defHttp } from '/@/utils/http/axios';
import {
  FeatureDefinitionDto,
  FeatureDefinitionCreateDto,
  FeatureDefinitionUpdateDto,
  FeatureDefinitionGetListInput,
} from './model';

export const create = (input: FeatureDefinitionCreateDto) => {
  return defHttp.post<FeatureDefinitionDto>({
    url: '/api/feature-management/definitions',
    data: input,
  });
};

export const deleteByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/feature-management/definitions/${name}`,
  });
};

export const getByName = (name: string) => {
  return defHttp.get<FeatureDefinitionDto>({
    url: `/api/feature-management/definitions/${name}`,
  });
};

export const getList = (input: FeatureDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<FeatureDefinitionDto>>({
    url: '/api/feature-management/definitions',
    params: input,
  });
};

export const update = (name: string, input: FeatureDefinitionUpdateDto) => {
  return defHttp.put<FeatureDefinitionDto>({
    url: `/api/feature-management/definitions/${name}`,
    data: input,
  });
};
