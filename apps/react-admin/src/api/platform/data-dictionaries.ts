import type { ListResultDto, PagedResultDto } from "#/abp-core";

import type {
	DataCreateDto,
	DataDto,
	DataItemCreateDto,
	DataItemUpdateDto,
	DataMoveDto,
	DataUpdateDto,
	GetDataListInput,
} from "#/platform/data-dictionaries";

import requestClient from "../request";

export function createApi(input: DataCreateDto): Promise<DataDto> {
	return requestClient.post<DataDto>("/api/platform/datas", input);
}

export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/platform/datas/${id}`);
}

export function createItemApi(id: string, input: DataItemCreateDto): Promise<void> {
	return requestClient.post(`/api/platform/datas/${id}/items`, input);
}

export function deleteItemApi(id: string, name: string): Promise<void> {
	return requestClient.delete(`/api/platform/datas/${id}/items/${name}`);
}

export function getApi(id: string): Promise<DataDto> {
	return requestClient.get<DataDto>(`/api/platform/datas/${id}`);
}

export function getByNameApi(name: string): Promise<DataDto> {
	return requestClient.get<DataDto>(`/api/platform/datas/by-name/${name}`);
}

export function getAllApi(): Promise<ListResultDto<DataDto>> {
	return requestClient.get<ListResultDto<DataDto>>("/api/platform/datas/all");
}

export function getPagedListApi(input?: GetDataListInput): Promise<PagedResultDto<DataDto>> {
	return requestClient.get<PagedResultDto<DataDto>>("/api/platform/datas", {
		params: input,
	});
}

export function moveApi(id: string, input: DataMoveDto): Promise<DataDto> {
	return requestClient.put<DataDto>(`/api/platform/datas/${id}/move`, input);
}

export function updateApi(id: string, input: DataUpdateDto): Promise<DataDto> {
	return requestClient.put<DataDto>(`/api/platform/datas/${id}`, input);
}

export function updateItemApi(id: string, name: string, input: DataItemUpdateDto): Promise<void> {
	return requestClient.put(`/api/platform/datas/${id}/items/${name}`, input);
}
