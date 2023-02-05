import { defAbpHttp } from '/@/utils/http/abp';
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

const remoteServiceName = 'Platform';
const controllerName = 'Package';

export const CreateAsyncByInput = (input: PackageCreateDto) => {
  return defAbpHttp.request<PackageDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
    params: {
    },
    data: input,
  });
};
export const DeleteAsyncById = (id: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    uniqueName: 'DeleteAsyncById',
    params: {
      id: id,
    },
  });
};
export const UploadBlobAsyncByIdAndInput = (id: string, input: PackageBlobUploadDto) => {
  return defAbpHttp.request<PackageBlobDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UploadBlobAsync',
    uniqueName: 'UploadBlobAsyncByIdAndInput',
    params: id,
    data: input,
  });
};
export const RemoveBlobAsyncByIdAndInput = (id: string, input: PackageBlobRemoveDto) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'RemoveBlobAsync',
    uniqueName: 'RemoveBlobAsyncByIdAndInput',
    params: {
      id: id,
      input: input,
    },
  });
};
export const DownloadBlobAsyncByIdAndInput = (id: string, input: PackageBlobDownloadInput) => {
  return defAbpHttp.request<Blob>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DownloadBlobAsync',
    uniqueName: 'DownloadBlobAsyncByIdAndInput',
    params: {
      id: id,
      input: input,
    },
  },{
    withToken: false,
  });
};
export const GetAsyncById = (id: string) => {
  return defAbpHttp.request<PackageDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncById',
    params: {
      id: id,
    },
  },{
    withToken: false,
  });
};
export const GetLatestAsyncByInput = (input: PackageGetLatestInput) => {
  return defAbpHttp.request<PackageDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetLatestAsync',
    uniqueName: 'GetLatestAsyncByInput',
    params: {
      input: input,
    },
  },{
    withToken: false,
  });
};
export const GetListAsyncByInput = (input: PackageGetPagedListInput) => {
  return defAbpHttp.pagedRequest<PackageDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};
export const UpdateAsyncByIdAndInput = (id: string, input: PackageUpdateDto) => {
  return defAbpHttp.request<PackageDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    uniqueName: 'UpdateAsyncByIdAndInput',
    params: id,
    data: input,
  });
};
