import type { PagedResultDto } from "#/abp-core";

import type { BackgroundJobLogDto, BackgroundJobLogGetListInput } from "#/tasks/job-logs";

import requestClient from "../request";

export function getApi(id: string): Promise<BackgroundJobLogDto> {
	return requestClient.get<BackgroundJobLogDto>(`/api/task-management/background-jobs/logs/${id}`);
}

export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/task-management/background-jobs/logs/${id}`);
}

export function getPagedListApi(input?: BackgroundJobLogGetListInput): Promise<PagedResultDto<BackgroundJobLogDto>> {
	return requestClient.get<PagedResultDto<BackgroundJobLogDto>>("/api/task-management/background-jobs/logs", {
		params: input,
	});
}
