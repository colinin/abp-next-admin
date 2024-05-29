import { GetOssContainerPagedRequest, OssContainer, OssContainersResult } from './model';
import { defHttp } from '/@/utils/http/axios';

export const createContainer = (name: string) => {
    return defHttp.post<OssContainer>({
        url: `/api/oss-management/containes/${name}`,
    });
};

export const deleteContainer = (name: string) => {
    return defHttp.delete<void>({
        url: `/api/oss-management/containes/${name}`,
    });
};

export const getContainer = (name: string) => {
    return defHttp.get<OssContainer>({
        url: `/api/oss-management/containes/${name}`,
    });
};

export const getContainers = (input: GetOssContainerPagedRequest) => {
    return defHttp.get<OssContainersResult>({
        url: '/api/oss-management/containes',
        params: input,
    });
};