import { defHttp } from '/@/utils/http/axios';
import {
  TextTemplateContentDto,
  TextTemplateContentGetInput,
  TextTemplateRestoreInput,
  TextTemplateContentUpdateDto
} from './model';

export const GetAsyncByInput = (input: TextTemplateContentGetInput) => {
  let url = `/api/text-templating/templates/content/${input.name}`;
  if (input.culture) {
    url = `/api/text-templating/templates/content/${input.culture}/${input.name}`;
  }
  return defHttp.get<TextTemplateContentDto>({
    url,
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
