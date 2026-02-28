import type React from "react";
import { useEffect, useState, useMemo } from "react";
import { Select, type SelectProps, Spin } from "antd";

function getNestedValue(obj: any, path: string) {
	if (!path) return undefined;
	return path.split(".").reduce((acc, part) => acc?.[part], obj);
}

export interface ApiSelectProps extends Omit<SelectProps, "options"> {
	/** Function to fetch data, returning a Promise */
	api?: (params?: any) => Promise<any>;
	/** Parameters to pass to the api function */
	params?: any;
	/** Key in the response object containing the array (e.g., 'items') */
	resultField?: string;
	/** Property name to use for the label */
	labelField?: string;
	/** Property name to use for the value */
	valueField?: string;
	/** Trigger fetch immediately on mount */
	immediate?: boolean;
}

const ApiSelect: React.FC<ApiSelectProps> = ({
	api,
	params,
	resultField = "items",
	labelField = "label",
	valueField = "value",
	immediate = true,
	...props
}) => {
	const [data, setData] = useState<any[]>([]);
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
			// Use helper instead of lodash.get
			const list = resultField ? getNestedValue(res, resultField) : res;

			if (Array.isArray(list)) {
				setData(list);
			} else if (Array.isArray(res)) {
				// Fallback: if extracting resultField failed but response itself is an array
				setData(res);
			}
		} catch (error) {
			console.error("ApiSelect fetch error:", error);
		} finally {
			setLoading(false);
		}
	};

	const options = useMemo(() => {
		return data.map((item) => ({
			...item,
			label: item[labelField],
			value: item[valueField],
		}));
	}, [data, labelField, valueField]);

	return (
		<Select
			loading={loading}
			options={options}
			notFoundContent={loading ? <Spin size="small" /> : null}
			onDropdownVisibleChange={(open) => {
				if (open && data.length === 0 && !loading) {
					fetchData();
				}
			}}
			{...props}
		/>
	);
};

export default ApiSelect;
