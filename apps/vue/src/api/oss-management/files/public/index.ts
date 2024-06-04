import { OssObject } from '../../objects/model';
import { defHttp } from '/@/utils/http/axios';
import { AxiosResponse } from 'axios';

export const formatUrl = (url: string) => {
  // 格式化路径为公共目录
  return `/api/api/files/static/public/p/${url}`;
};

export const upload = (file: Blob, path: string, name: string) => {
  return new Promise<AxiosResponse<OssObject>>((resolve, reject) => {
    defHttp
      .uploadFile<OssObject>(
        {
          url: `/api/api/files/public/upload`,
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
    url: `/api/api/files/public/p/${path}/${name}`,
  });
};

export const getList = (input: { path?: string; filter?: string; maxResultCount?: number }) => {
  return defHttp.get<ListResultDto<OssObject>>({
    url: '/api/files/public/search',
    params: input,
  });
};
