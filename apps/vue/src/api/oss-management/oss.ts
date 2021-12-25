import { defAbpHttp } from '/@/utils/http/abp';
import {
  OssObject,
  OssObjectCreate,
  OssObjectBulkDelete,
  OssContainer,
  GetOssObjectRequest,
  GetOssObjectPagedRequest,
  OssObjectsResult,
  GetOssContainerPagedRequest,
  OssContainersResult,
} from './model/ossModel';
import { format } from '/@/utils/strings';
import { AxiosResponse } from 'axios';
import { isFunction } from '/@/utils/is';
import { UploadFileParams } from '/#/axios';

enum Api {
  CreateObject = '/api/oss-management/objects',
  DeleteObject = '/api/oss-management/objects',
  BulkDeleteObject = '/api/oss-management/objects/bulk-delete',
  GetObject = '/api/oss-management/objects',
  GetObjects = '/api/oss-management/containes/objects',
  CreateContainer = '/api/oss-management/containes/{name}',
  DeleteContainer = '/api/oss-management/containes/{name}',
  GetContainer = '/api/oss-management/containes/{name}',
  GetContainers = '/api/oss-management/containes',
  UploadObject = '/api/api/oss-management/objects/upload',
  DownloadObject = '/api/api/files/static/{bucket}/p/{path}/{name}',
}

export const uploadUrl = Api.UploadObject;
export const downloadUrl = Api.DownloadObject;

export function generateOssUrl(bucket: string, path: string, object: string) {
  if (path) {
    // 对 Path部分的 URL 编码
    path = encodeURIComponent(path);
    if (path !== '.%2F' && path.endsWith('%2F')) {
      path = path.substring(0, path.length - 3);
    }
  }
  return format(downloadUrl, { bucket: bucket, path: path, name: object });
}

export const downloadBlob = (bucket: string, path: string, object: string) => {
  return defAbpHttp.get<Blob>({
    url: generateOssUrl(bucket, path, object),
    headers: {
      accept: 'application/json',
    },
    responseType: 'blob',
  }, {
    apiUrl: '/api'
  });
};

/**
 * 分片上传文件
 * @param params 文件参数
 * @param event 事件参数
 * @returns 上传成功后响应
 */
export const uploadObject = (params: UploadFileParams, event: any) => {
  // 常规块大小
  const chunkSize: number = params.chunkSize ?? 2 * 1024 * 1024;
  // 块总数
  const totalChunks = Math.ceil(params.file.size / chunkSize);
  // 总大小
  const totalSize = params.file.size;
  // 文件名
  const fileName = params.filename ?? (params.file as File).name;
  // 已完成大小
  let loadedSize = 0;
  // 返回包装的结果
  return new Promise<AxiosResponse<void>>(async (resolve, reject) => {
    function onPregress(progress: number, res: AxiosResponse) {
      // 回调上传进度
      if (isFunction(event)) {
        event({
          loaded: progress,
          total: totalSize,
        });
      }
      if (progress === totalSize) {
        resolve(res);
      }
    }
    function uploadFunc(num: number) {
      // 计算分片索引
      const nextSize = Math.min(num * chunkSize, params.file.size);
      // 切割文件
      const fileData = params.file.slice((num - 1) * chunkSize, nextSize);
      // 当前分片大小
      const currentChunksSize = fileData.size;
      const requestConfig = {
        url: uploadUrl,
        // 超时时间设置长时间
        timeout: 600000,
      };
      const requestData = {
        file: fileData,
        chunkNumber: num,
        chunkSize: chunkSize,
        currentChunkSize: currentChunksSize,
        totalChunks: totalChunks,
        totalSize: totalSize,
        bucket: params.data?.bucket,
        path: params.data?.path,
        fileName: fileName,
      };
      return defAbpHttp
        .uploadFile<void>(requestConfig, {
          data: requestData,
          file: fileData,
        })
        .then((res) => {
          // 当前进度
          loadedSize += currentChunksSize;
          onPregress(loadedSize, res);
          return Promise.resolve(res);
        })
        .catch((error) => {
          return Promise.reject(error);
        });
    }
    // TODO: 支持一次上传多个分片文件
    for (let num = 1; num <= totalChunks; num++) {
      try {
        // 上传第一个分片采用同步的方式，如果出现错误其他分片不再上传
        const res = await uploadFunc(num);
        onPregress(loadedSize, res);
      } catch (error) {
        return reject(error);
      }
    }
  });
};

export const createContainer = (name: string) => {
  return defAbpHttp.post<OssContainer>({
    url: format(Api.CreateContainer, { name: name }),
  });
};

export const deleteContainer = (name: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetContainer, { name: name }),
  });
};

export const getContainer = (name: string) => {
  return defAbpHttp.get<OssContainer>({
    url: format(Api.GetContainer, { name: name }),
  });
};

export const getContainers = (input: GetOssContainerPagedRequest) => {
  return defAbpHttp.get<OssContainersResult>({
    url: Api.GetContainers,
    params: input,
  });
};

export const createObject = (input: OssObjectCreate) => {
  return defAbpHttp.post<OssObject>({
    url: Api.CreateObject,
    data: input,
  });
};

export const deleteObject = (input: GetOssObjectRequest) => {
  return defAbpHttp.delete<void>(
    {
      url: Api.DeleteObject,
      params: input,
    },
    {
      joinParamsToUrl: true,
    },
  );
};

export const bulkDeleteObject = (input: OssObjectBulkDelete) => {
  return defAbpHttp.post<void>({
    url: Api.BulkDeleteObject,
    params: input,
  });
}

export const getObject = (input: GetOssObjectRequest) => {
  return defAbpHttp.get<OssObject>({
    url: Api.GetObject,
    params: input,
  });
};

export const getObjects = (input: GetOssObjectPagedRequest) => {
  return defAbpHttp.get<OssObjectsResult>({
    url: Api.GetObjects,
    params: input,
  });
};
