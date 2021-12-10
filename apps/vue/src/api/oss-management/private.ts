import { defHttp } from '/@/utils/http/axios';
import { FileShare, FileShareInput, MyFileShare, OssObject } from './model/ossModel';
import { format } from '/@/utils/strings';
import { AxiosResponse } from 'axios';
import { ListResultDto } from '../model/baseModel';

enum Api {
  Upload = '/api/api/files/private',
  Get = '/api/api/files/private/p/{path}/{name}',
  GetList = '/api/files/private/search',
  Share = '/api/files/private/share',
}

export const formatUrl = (url: string) => {
  // 格式化路径为用户目录
  return `/api/api/files/static/users/p/${url}`;
};

export const upload = (file: Blob, path: string, name: string) => {
  return new Promise<AxiosResponse<OssObject>>((resolve, reject) => {
    defHttp
      .uploadFile<OssObject>(
        {
          url: Api.Upload,
        },
        {
          data: { path: path, object: name },
          file: file,
        },
      )
      .then((res: AxiosResponse<any>) => {
        resolve(res);
      })
      .catch((err) => {
        reject(err);
      });
  });
  // return defHttp.uploadFile<OssObject>(
  //   {
  //     url: format(Api.Upload, { path: path, name: name }),
  //     data: { file: file },
  //     method: 'POST',
  //     headers: {
  //       'Content-Type': ContentTypeEnum.FORM_DATA,
  //     },
  //   },
  //   {
  //     formatDate: false,
  //   });
};

export const get = (path: string, name: string) => {
  return defHttp.get<Blob>({
    url: format(Api.Get, { name: name, path: path }),
  });
};

export const getList = (input: { path?: string; filter?: string; maxResultCount?: number }) => {
  return defHttp.get<ListResultDto<OssObject>>({
    url: Api.GetList,
    params: input,
  });
};

export const share = (input: FileShareInput) => {
  return defHttp.post<FileShare>({
    url: Api.Share,
    data: input,
  });
};

export const getShareList = () => {
  return defHttp.get<ListResultDto<MyFileShare>>({
    url: Api.Share,
  });
};
