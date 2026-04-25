import type {
	GetOssContainersInput,
	GetOssObjectsInput,
	OssContainerDto,
	OssContainersResultDto,
} from "#/oss/containes";
import type { OssObjectsResultDto } from "#/oss/objects";

import requestClient from "../request";

export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/oss-management/containes/${name}`);
}

export function getApi(name: string): Promise<OssContainerDto> {
	return requestClient.get<OssContainerDto>(`/api/oss-management/containes/${name}`);
}

export function getListApi(input?: GetOssContainersInput): Promise<OssContainersResultDto> {
	return requestClient.get<OssContainersResultDto>("/api/oss-management/containes", {
		params: input,
	});
}

export function getObjectsApi(input: GetOssObjectsInput): Promise<OssObjectsResultDto> {
	return requestClient.get<OssObjectsResultDto>("/api/oss-management/containes/objects", {
		params: input,
	});
}

export function createApi(name: string): Promise<OssContainerDto> {
	return requestClient.post<OssContainerDto>(`/api/oss-management/containes/${name}`);
}
