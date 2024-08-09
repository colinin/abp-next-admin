import { defHttp } from '/@/utils/http/axios';
import {
  TextTemplateContentDto,
  TextTemplateContentGetInput,
  TextTemplateRestoreInput,
  TextTemplateContentUpdateDto
} from './model';

export const GetAsyncByInput = (input: TextTemplateContentGetInput) => {
  return defHttp.get<TextTemplateContentDto>({
    url: `/api/text-templating/templates/content`,
    params: input,
  });
};

export const RestoreToDefaultAsyncByNameAndInput = (name: string, input: TextTemplateRestoreInput) => {
  return defHttp.put<void>({
    url: `/api/text-templating/templates/content/${name}/restore-to-default`,
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: TextTemplateContentUpdateDto) => {
  return defHttp.put<TextTemplateContentDto>({
    url: `/api/text-templating/templates/content/${name}`,
    data: input,
  });
};
