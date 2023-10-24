import { defHttp } from '/@/utils/http/axios';
import {
  FeatureDefinitionDto,
  FeatureDefinitionCreateDto,
  FeatureDefinitionUpdateDto,
  FeatureDefinitionGetListInput,
} from './model';

export const CreateAsyncByInput = (input: FeatureDefinitionCreateDto) => {
  return defHttp.post<FeatureDefinitionDto>({
    url: '/api/feature-management/definitions',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/feature-management/definitions/${name}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<FeatureDefinitionDto>({
    url: `/api/feature-management/definitions/${name}`,
  });
};

export const GetListAsyncByInput = (input: FeatureDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<FeatureDefinitionDto>>({
    url: '/api/feature-management/definitions',
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: FeatureDefinitionUpdateDto) => {
  return defHttp.put<FeatureDefinitionDto>({
    url: `/api/feature-management/definitions/${name}`,
    data: input,
  });
};
