import type { ListResultDto, PagedResultDto } from "#/abp-core";

import type {
	BackgroundJobDefinitionDto,
	BackgroundJobInfoBatchInput,
	BackgroundJobInfoCreateDto,
	BackgroundJobInfoDto,
	BackgroundJobInfoGetListInput,
	BackgroundJobInfoUpdateDto,
} from "#/tasks/job-infos";

import requestClient from "../request";

export function createApi(input: BackgroundJobInfoCreateDto): Promise<BackgroundJobInfoDto> {
	return requestClient.post<BackgroundJobInfoDto>("/api/task-management/background-jobs", input);
}

export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/task-management/background-jobs/${id}`);
}

export function bulkDeleteApi(input: BackgroundJobInfoBatchInput): Promise<void> {
	return requestClient.delete("/api/task-management/background-jobs/bulk-delete", {
		data: input,
	});
}

export function getApi(id: string): Promise<BackgroundJobInfoDto> {
	return requestClient.get<BackgroundJobInfoDto>(`/api/task-management/background-jobs/${id}`);
}

export function getPagedListApi(input?: BackgroundJobInfoGetListInput): Promise<PagedResultDto<BackgroundJobInfoDto>> {
	return requestClient.get<PagedResultDto<BackgroundJobInfoDto>>("/api/task-management/background-jobs", {
		params: input,
	});
}

export function pauseApi(id: string): Promise<void> {
	return requestClient.put(`/api/task-management/background-jobs/${id}/pause`);
}

export function bulkPauseApi(input: BackgroundJobInfoBatchInput): Promise<void> {
	return requestClient.put("/api/task-management/background-jobs/bulk-pause", input);
}

export function resumeApi(id: string): Promise<void> {
	return requestClient.put(`/api/task-management/background-jobs/${id}/resume`);
}

export function bulkResumeApi(input: BackgroundJobInfoBatchInput): Promise<void> {
	return requestClient.put("/api/task-management/background-jobs/bulk-resume", input);
}

export function triggerApi(id: string): Promise<void> {
	return requestClient.put(`/api/task-management/background-jobs/${id}/trigger`);
}

export function bulkTriggerApi(input: BackgroundJobInfoBatchInput): Promise<void> {
	return requestClient.put("/api/task-management/background-jobs/bulk-trigger", input);
}

export function stopApi(id: string): Promise<void> {
	return requestClient.put(`/api/task-management/background-jobs/${id}/stop`);
}

export function bulkStopApi(input: BackgroundJobInfoBatchInput): Promise<void> {
	return requestClient.put("/api/task-management/background-jobs/bulk-stop", input);
}

export function startApi(id: string): Promise<void> {
	return requestClient.put(`/api/task-management/background-jobs/${id}/start`);
}

export function bulkStartApi(input: BackgroundJobInfoBatchInput): Promise<void> {
	return requestClient.put("/api/task-management/background-jobs/bulk-start", input);
}

export function updateApi(id: string, input: BackgroundJobInfoUpdateDto): Promise<BackgroundJobInfoDto> {
	return requestClient.put<BackgroundJobInfoDto>(`/api/task-management/background-jobs/${id}`, input);
}

export function getDefinitionsApi(): Promise<ListResultDto<BackgroundJobDefinitionDto>> {
	return requestClient.get<ListResultDto<BackgroundJobDefinitionDto>>(
		"/api/task-management/background-jobs/definitions",
	);
}
