import type { SortOrder } from "#/abp-core";

import type { SortOrder as AntdSortOrder } from "antd/es/table/interface";

// type SortOrder = '' | 'asc' | 'desc' | null;
// AntdSortOrder : 'descend' | 'ascend' | null;

export const antdOrderToAbpOrder = (antdOrder: AntdSortOrder): SortOrder => {
	if (antdOrder) {
		return antdOrder === "ascend" ? "asc" : "desc";
	}
	return "";
};
