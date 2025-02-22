import type { Dictionary } from "#/abp-core";

export interface DisplayNameInfo {
	culture: string;
	displayName: string;
}

export interface DisplayNameProps {
	data?: Dictionary<string, string>;
	onChange?: (data: DisplayNameInfo) => void;
	onDelete?: (data: DisplayNameInfo) => void;
}
