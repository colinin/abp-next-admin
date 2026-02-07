import type { FeatureProvider, GetFeatureListResultDto, UpdateFeaturesDto } from "#/management/features/features";

import requestClient from "../../request";

/**
 * 删除功能
 * @param {FeatureProvider} provider 参数
 * @returns {Promise<void>}
 */
export function deleteApi(provider: FeatureProvider): Promise<void> {
	return requestClient.delete("/api/feature-management/features", {
		params: provider,
	});
}

/**
 * 查询功能
 * @param {FeatureProvider} provider 参数
 * @returns {Promise<GetFeatureListResultDto>} 功能实体数据传输对象
 */
export function getApi(provider: FeatureProvider): Promise<GetFeatureListResultDto> {
	return requestClient.get<GetFeatureListResultDto>("/api/feature-management/features", {
		params: provider,
	});
}

/**
 * 更新功能
 * @param {FeatureProvider} provider
 * @param {UpdateFeaturesDto} input 参数
 * @returns {Promise<void>}
 */
export function updateApi(provider: FeatureProvider, input: UpdateFeaturesDto): Promise<void> {
	return requestClient.put("/api/feature-management/features", input, {
		params: provider,
	});
}
