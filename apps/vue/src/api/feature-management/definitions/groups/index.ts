import { defHttp } from '/@/utils/http/axios';
import {
  FeatureGroupDefinitionDto,
  FeatureGroupDefinitionCreateDto,
  FeatureGroupDefinitionUpdateDto,
  FeatureGroupDefinitionGetListInput,
} from './model';

export const CreateAsyncByInput = (input: FeatureGroupDefinitionCreateDto) => {
  return defHttp.post<FeatureGroupDefinitionDto>({
    url: '/api/feature-management/definitions/groups',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/feature-management/definitions/groups/${name}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<FeatureGroupDefinitionDto>({
    url: `/api/feature-management/definitions/groups/${name}`,
  });
};

export const GetListAsyncByInput = (input: FeatureGroupDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<FeatureGroupDefinitionDto>>({
    url: '/api/feature-management/definitions/groups',
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: FeatureGroupDefinitionUpdateDto) => {
  return defHttp.put<FeatureGroupDefinitionDto>({
    url: `/api/feature-management/definitions/groups/${name}`,
    data: input,
  });
};
