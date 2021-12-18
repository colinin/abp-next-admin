import { defHttp } from '/@/utils/http/axios';
import { OssObject } from './model/ossModel';
import { format } from '/@/utils/strings';
import { AxiosResponse } from 'axios';
import { ListResultDto } from '../model/baseModel';

enum Api {
  Upload = '/api/api/files/public/{path}/{name}',
  Get = '/api/api/files/public/p/{path}/{name}',
  GetList = '/api/files/public/search',
}

export const formatUrl = (url: string) => {
  // 格式化路径为公共目录
  return `/api/api/files/static/public/p/${url}`;
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
