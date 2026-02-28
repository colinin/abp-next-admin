import type React from "react";
import { useMemo } from "react";
import { Checkbox, Select } from "antd";
import { useTranslation } from "react-i18next";
import useAbpStore from "@/store/abpCoreStore";

interface ValueType {
	globalFeatureNames: string[];
	requiresAll: boolean;
}

interface GlobalFeatureStateCheckProps {
	value?: ValueType;
	onChange?: (value: ValueType) => void;
}

const GlobalFeatureStateCheck: React.FC<GlobalFeatureStateCheckProps> = ({
	value = { globalFeatureNames: [], requiresAll: false },
	onChange,
}) => {
	const { t: $t } = useTranslation();
	const application = useAbpStore((state) => state.application);

	const options = useMemo(() => {
		if (!application?.globalFeatures?.enabledFeatures) return [];
		return application.globalFeatures.enabledFeatures.map((f) => ({
			label: f,
			value: f,
		}));
	}, [application]);

	const triggerChange = (changedValue: Partial<ValueType>) => {
		onChange?.({ ...value, ...changedValue });
	};

	return (
		<div className="flex flex-col gap-4 w-full">
			<Checkbox checked={value.requiresAll} onChange={(e) => triggerChange({ requiresAll: e.target.checked })}>
				{$t("component.simple_state_checking.requireFeatures.requiresAll")}
			</Checkbox>
			<Select
				mode="tags"
				style={{ width: "100%" }}
				options={options}
				value={value.globalFeatureNames}
				onChange={(val) => triggerChange({ globalFeatureNames: val })}
				allowClear
			/>
		</div>
	);
};

export default GlobalFeatureStateCheck;
