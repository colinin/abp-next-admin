import { defHttp } from '/@/utils/http/axios';
import {
  OssObject,
  OssObjectCreate,
  OssObjectBulkDelete,
  OssObjectsResult,
  GetOssObjectRequest,
  GetOssObjectPagedRequest,
} from './model';
import { AxiosResponse } from 'axios';
import { isFunction } from '/@/utils/is';
import { UploadFileParams } from '/#/axios';
import { useAbpStoreWithOut } from '/@/store/modules/abp';
import { format } from '/@/utils/strings';
import { ContentTypeEnum } from '/@/enums/httpEnum';

export const uploadUrl = '/api/api/oss-management/objects/upload';
export const downloadUrl = '/api/api/files/static/{bucket}/p/{path}/{name}';

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
  return defHttp.get<Blob>({
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
  return new Promise<AxiosResponse<any>>(async (resolve, reject) => {
    function onPregress(progress: number, res: AxiosResponse) {
      // 回调上传进度
      if (isFunction(event)) {
        event({
          loaded: progress,
          total: totalSize,
        });
      }
      if (progress === totalSize) {
        if (!res.data) {
          let formatUrl = '/api/files/static/{bucket}/p/{path}/{name}';
          const abpStore = useAbpStoreWithOut();
          const { currentTenant } = abpStore.getApplication;
          if (currentTenant.id) {
            formatUrl = '/api/files/static/t/{tenantId}/{bucket}/p/{path}/{name}';
          }
          const path = encodeURIComponent(params.data?.path);
          res.data = {
            url: format(formatUrl, {
              bucket:  params.data?.bucket,
              tenantId: currentTenant.id,
              path: path,
              name: fileName,
            }),
          };
        }
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
      return defHttp
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

export const createObject = (input: OssObjectCreate, file?: Blob) => {
  const formData = new window.FormData();
  formData.append('bucket', input.bucket);
  formData.append('path', input.path);
  formData.append('fileName', input.object);
  formData.append('overwrite', String(input.overwrite));
  input.expirationTime && formData.append('expirationTime', input.expirationTime.toString());
  file && formData.append('file', file);
  return defHttp.post<OssObject>({
    url: '/api/oss-management/objects',
    headers: {
      'Content-type': ContentTypeEnum.FORM_DATA,
    },
    data: formData,
  });
};

export const deleteObject = (input: GetOssObjectRequest) => {
  return defHttp.delete<void>(
    {
      url: '/api/oss-management/objects',
      params: input,
    },
  );
};

export const bulkDeleteObject = (input: OssObjectBulkDelete) => {
  return defHttp.post<void>({
    url: '/api/oss-management/objects/bulk-delete',
    data: input,
  });
}

export const getObject = (input: GetOssObjectRequest) => {
  return defHttp.get<OssObject>({
    url: '/api/oss-management/objects',
    params: input,
  });
};

export const getObjects = (input: GetOssObjectPagedRequest) => {
  return defHttp.get<OssObjectsResult>({
    url: '/api/oss-management/containes/objects',
    params: input,
  });
};
