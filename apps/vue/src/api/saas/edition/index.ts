import { defHttp } from '/@/utils/http/axios';
import { EditionCreateDto, EditionDto,EditionGetListInput, EditionUpdateDto,  } from './model';

export const CreateAsyncByInput = (input: EditionCreateDto) => {
  return defHttp.post<EditionDto>({
    url: `/api/saas/editions`,
    data: input,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/saas/editions/${id}`,
  });
};

export const GetAsyncById = (id: string) => {
  return defHttp.get<EditionDto>({
    url: `/api/saas/editions/${id}`,
  });
};

export const GetListAsyncByInput = (input: EditionGetListInput) => {
  return defHttp.get<PagedResultDto<EditionDto>>({
    url: `/api/saas/editions`,
    params: input,
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: EditionUpdateDto) => {
  return defHttp.put<EditionDto>({
    url: `/api/saas/editions/${id}`,
    data: input,
  });
};
