import type { PagedResultDto } from "#/abp-core";

import type { LayoutCreateDto, LayoutDto, LayoutGetPagedListInput, LayoutUpdateDto } from "#/platform/layouts";

import requestClient from "../request";

export function createApi(input: LayoutCreateDto): Promise<LayoutDto> {
	return requestClient.post<LayoutDto>("/api/platform/layouts", input);
}

export function getPagedListApi(input?: LayoutGetPagedListInput): Promise<PagedResultDto<LayoutDto>> {
	return requestClient.get<PagedResultDto<LayoutDto>>("/api/platform/layouts", {
		params: input,
	});
}

export function getApi(id: string): Promise<LayoutDto> {
	return requestClient.get<LayoutDto>(`/api/platform/layouts/${id}`);
}

export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/platform/layouts/${id}`);
}

export function updateApi(id: string, input: LayoutUpdateDto): Promise<LayoutDto> {
	return requestClient.put<LayoutDto>(`/api/platform/layouts/${id}`, input);
}
