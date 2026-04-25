import type React from "react";
import { useMemo } from "react";
import { Checkbox, TreeSelect, Spin } from "antd";
import { useTranslation } from "react-i18next";
import { useQuery } from "@tanstack/react-query";
import { getListApi as getFeaturesApi } from "@/api/management/features/feature-definitions";
import { getListApi as getGroupsApi } from "@/api/management/features/feature-group-definitions";

import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";

import { listToTree } from "@/utils/tree";
import { valueTypeSerializer } from "../../string-value-type";

interface ValueType {
	featureNames: string[];
	requiresAll: boolean;
}

interface FeatureStateCheckProps {
	value?: ValueType;
	onChange?: (value: ValueType) => void;
}

const FeatureStateCheck: React.FC<FeatureStateCheckProps> = ({
	value = { featureNames: [], requiresAll: false },
	onChange,
}) => {
	const { t: $t } = useTranslation();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();

	// 1. Fetch Data
	const { data: featureData, isLoading } = useQuery({
		queryKey: ["featureStateCheckData"],
		queryFn: async () => {
			const [groupsRes, featuresRes] = await Promise.all([getGroupsApi(), getFeaturesApi()]);

			// Filter: Only BOOLEAN features are relevant for state checking
			const validFeatures = featuresRes.items.filter((item) => {
				if (item.valueType) {
					try {
						const vt = valueTypeSerializer.deserialize(item.valueType);
						return vt.validator.name === "BOOLEAN";
					} catch {
						return false;
					}
				}
				return true;
			});

			// Prepare localized features list for lookup and tree building
			const features = validFeatures.map((f) => {
				const d = deserialize(f.displayName);
				return {
					...f,
					title: Lr(d.resourceName, d.name), // Localized Title
					key: f.name,
					value: f.name,
				};
			});

			// Prepare localized groups
			const groups = groupsRes.items.map((g) => {
				const d = deserialize(g.displayName);
				return {
					...g,
					title: Lr(d.resourceName, d.name),
				};
			});

			return { groups, features };
		},
		staleTime: Number.POSITIVE_INFINITY, // Config data rarely changes
	});

	// 2. Construct Tree Data
	const treeData = useMemo(() => {
		if (!featureData) return [];
		const { groups, features } = featureData;

		return groups.map((group) => {
			// Find features belonging to this group
			const groupFeatures = features.filter((f) => f.groupName === group.name);

			// Build hierarchy for features (parent/child)
			const children = listToTree(groupFeatures, { id: "name", pid: "parentName" });

			// Return Group Node
			return {
				title: group.title,
				value: group.name,
				key: group.name,
				selectable: false, // Cannot select the group itself
				checkable: false, // Cannot check the group itself
				disableCheckbox: true, // Visual disabled checkbox
				children: children,
			};
		});
	}, [featureData]);

	// 3. Map selected string[] to { label, value }[] for TreeSelect (Required for treeCheckStrictly)
	const treeValue = useMemo(() => {
		if (!featureData || !value.featureNames) return [];
		return value.featureNames.map((name) => {
			const feature = featureData.features.find((f) => f.name === name);
			return {
				label: feature?.title || name,
				value: name,
			};
		});
	}, [featureData, value.featureNames]);

	// 4. Handle Change
	const triggerChange = (changedValue: Partial<ValueType>) => {
		onChange?.({ ...value, ...changedValue });
	};

	const onTreeChange = (labeledValues: { label: React.ReactNode; value: string }[]) => {
		// Extract just the feature names (strings) to send back
		const names = labeledValues.map((item) => item.value);
		triggerChange({ featureNames: names });
	};

	if (isLoading) return <Spin className="my-2" />;

	return (
		<div className="flex flex-col gap-2 w-full">
			<Checkbox checked={value.requiresAll} onChange={(e) => triggerChange({ requiresAll: e.target.checked })}>
				{$t("component.simple_state_checking.requireFeatures.requiresAll")}
			</Checkbox>

			<TreeSelect
				treeData={treeData}
				value={treeValue} // Pass objects
				onChange={onTreeChange} // Receive objects
				allowClear
				treeCheckable
				treeCheckStrictly // Decouples parent/child selection logic
				showCheckedStrategy={TreeSelect.SHOW_ALL} // Show every selected node explicitly
				placeholder={$t("ui.placeholder.select")}
				style={{ width: "100%" }}
				treeDefaultExpandAll
			/>
		</div>
	);
};

export default FeatureStateCheck;
