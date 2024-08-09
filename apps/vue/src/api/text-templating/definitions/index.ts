import { defHttp } from '/@/utils/http/axios';
import {
  TextTemplateDefinitionDto,
  TextTemplateDefinitionCreateDto,
  TextTemplateDefinitionUpdateDto,
  TextTemplateDefinitionGetListInput
} from './model';

export const CreateAsyncByInput = (input: TextTemplateDefinitionCreateDto) => {
  return defHttp.post<TextTemplateDefinitionDto>({
    url: `/api/text-templating/template/definitions`,
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/text-templating/template/definitions/${name}`,
  });
};

export const GetByNameAsyncByName = (name: string) => {
  return defHttp.get<TextTemplateDefinitionDto>({
    url: `/api/text-templating/template/definitions/${name}`,
  });
};

export const GetListAsyncByInput = (input: TextTemplateDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<TextTemplateDefinitionDto>>({
    url: `/api/text-templating/template/definitions`,
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: TextTemplateDefinitionUpdateDto) => {
  return defHttp.put<TextTemplateDefinitionDto>({
    url: `/api/text-templating/template/definitions/${name}`,
    data: input,
  });
};
