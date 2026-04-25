import type React from "react";
import { useEffect, useState } from "react";
import { TreeSelect, type TreeSelectProps, Spin } from "antd";

function getNestedValue(obj: any, path: string) {
	if (!path) return undefined;
	return path.split(".").reduce((acc, part) => acc?.[part], obj);
}

export interface ApiTreeSelectProps extends Omit<TreeSelectProps, "treeData"> {
	api?: (params?: any) => Promise<any>;
	params?: any;
	resultField?: string;
	immediate?: boolean;
	fieldNames?: { label: string; value: string; children: string };
}

const ApiTreeSelect: React.FC<ApiTreeSelectProps> = ({
	api,
	params,
	resultField,
	immediate = true,
	fieldNames = { label: "label", value: "value", children: "children" },
	...props
}) => {
	const [treeData, setTreeData] = useState<any[]>([]);
	const [loading, setLoading] = useState(false);

	useEffect(() => {
		if (immediate && api) {
			fetchData();
		}
	}, [JSON.stringify(params)]);

	const fetchData = async () => {
		if (!api) return;
		setLoading(true);
		try {
			const res = await api(params);

			let list = res;
			if (resultField) {
				list = getNestedValue(res, resultField);
			}

			if (Array.isArray(list)) {
				setTreeData(list);
			}
		} catch (error) {
			console.error("ApiTreeSelect fetch error:", error);
		} finally {
			setLoading(false);
		}
	};

	return (
		<TreeSelect
			loading={loading}
			treeData={treeData}
			fieldNames={fieldNames}
			dropdownStyle={{ maxHeight: 400, overflow: "auto" }}
			notFoundContent={loading ? <Spin size="small" /> : null}
			onDropdownVisibleChange={(open) => {
				if (open && treeData.length === 0 && !loading) {
					fetchData();
				}
			}}
			{...props}
		/>
	);
};

export default ApiTreeSelect;
