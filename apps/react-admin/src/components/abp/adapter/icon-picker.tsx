import type React from "react";
import { useMemo } from "react";
import { Input, type InputProps } from "antd";
import { Iconify } from "@/components/icon"; // Assumes you have this component

export interface IconPickerProps extends Omit<InputProps, "onChange"> {
	value?: string;
	onChange?: (value: string) => void;
}

const IconPicker: React.FC<IconPickerProps> = ({ value, onChange, ...props }) => {
	// A helper to render the icon preview
	const iconPreview = useMemo(() => {
		if (!value) return null;
		return (
			<div className="flex items-center justify-center w-6 h-6 text-xl">
				<Iconify icon={value} />
			</div>
		);
	}, [value]);

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		onChange?.(e.target.value);
	};

	return (
		<Input
			value={value}
			onChange={handleChange}
			placeholder="Type icon code (e.g. mdi:home)"
			allowClear
			// Display the icon at the start of the input
			addonBefore={iconPreview}
			{...props}
		/>
	);
};

export default IconPicker;
