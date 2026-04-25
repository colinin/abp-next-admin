import type { BulkDeleteOssObjectInput, CreateOssObjectInput, GetOssObjectInput, OssObjectDto } from "#/oss/objects";

import requestClient from "../request";

export function createApi(input: CreateOssObjectInput): Promise<OssObjectDto> {
	const formData = new window.FormData();
	formData.append("bucket", input.bucket);
	formData.append("fileName", input.fileName);
	formData.append("overwrite", String(input.overwrite));
	input.expirationTime && formData.append("expirationTime", input.expirationTime.toString());
	input.path && formData.append("path", input.path);
	input.file && formData.append("file", input.file);

	return requestClient.post<OssObjectDto>("/api/oss-management/objects", formData, {
		headers: {
			"Content-Type": "multipart/form-data;charset=utf-8",
		},
	});
}

export function generateUrlApi(input: GetOssObjectInput): Promise<string> {
	// return requestClient.get<string>("/api/oss-management/objects/generate-url", { TODO update
	return requestClient.get<string>("/api/oss-management/objects/download", {
		params: input,
	});
}

export function deleteApi(input: BulkDeleteOssObjectInput): Promise<void> {
	return requestClient.delete("/api/oss-management/objects", {
		params: input,
	});
}
