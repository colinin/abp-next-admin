import { defHttp } from '/@/utils/http/axios';
import {
  PackageCreateDto,
  PackageDto,
  PackageBlobUploadDto,
  PackageBlobDto,
  PackageBlobRemoveDto,
  PackageBlobDownloadInput,
  PackageGetLatestInput,
  PackageGetPagedListInput,
  PackageUpdateDto
} from './model';

export const CreateAsyncByInput = (input: PackageCreateDto) => {
  return defHttp.post<PackageDto>({
    url: `/api/platform/packages`,
    data: input,
  });
};
export const DeleteAsyncById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/packages/${id}`,
  });
};
export const UploadBlobAsyncByIdAndInput = (id: string, input: PackageBlobUploadDto) => {
  return defHttp.post<PackageBlobDto>({
    url: `/api/platform/packages/${id}/blob`,
    data: input,
  });
};
export const RemoveBlobAsyncByIdAndInput = (id: string, input: PackageBlobRemoveDto) => {
  return defHttp.delete<void>({
    url: `/api/platform/packages/${id}/blob?name=${input.name}`,
  });
};
export const DownloadBlobAsyncByIdAndInput = (id: string, input: PackageBlobDownloadInput) => {
  return defHttp.get<Blob>({
    url: `/api/platform/packages/${id}/blob?name=${input.name}`,
  },{
    withToken: false,
  });
};
export const GetAsyncById = (id: string) => {
  return defHttp.get<PackageDto>({
    url: `/api/platform/packages/${id}`,
  },{
    withToken: false,
  });
};
export const GetLatestAsyncByInput = (input: PackageGetLatestInput) => {
  return defHttp.get<PackageDto>({
    url: `/api/platform/packages/${input.name}/latest/${input.version}`,
  },{
    withToken: false,
  });
};
export const GetListAsyncByInput = (input: PackageGetPagedListInput) => {
  return defHttp.get<PagedResultDto<PackageDto>>({
    url: `/api/platform/packages`,
    params: input,
  });
};
export const UpdateAsyncByIdAndInput = (id: string, input: PackageUpdateDto) => {
  return defHttp.put<PackageDto>({
    url: `/api/platform/packages/${id}`,
    data: input,
  });
};
