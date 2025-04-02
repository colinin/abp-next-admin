import type { Dictionary } from "#/abp-core";

interface PropertyInfo {
	key: string;
	value: string;
}
interface PropertyProps {
	allowDelete?: boolean;
	allowEdit?: boolean;
	data?: Dictionary<string, string>;
	disabled?: boolean;
	onChange?: (data: PropertyInfo) => void;
	onDelete?: (data: PropertyInfo) => void;
}

export type { PropertyInfo, PropertyProps };
