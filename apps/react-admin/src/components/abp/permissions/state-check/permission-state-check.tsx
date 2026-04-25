import type React from "react";
import { useMemo } from "react";
import { Checkbox, TreeSelect, Spin } from "antd";
import { useTranslation } from "react-i18next";
import { useQuery } from "@tanstack/react-query";
import { getListApi as getPermissionsApi } from "@/api/management/permissions/definitions";
import { getListApi as getGroupsApi } from "@/api/management/permissions/groups";
import { listToTree } from "@/utils/tree";
import { localizationSerializer } from "@/utils/abp/localization-serializer";
import { useLocalizer } from "@/hooks/abp/use-localization";

interface ValueType {
	permissions: string[];
	requiresAll: boolean;
}

interface PermissionStateCheckProps {
	value?: ValueType;
	onChange?: (value: ValueType) => void;
}

const PermissionStateCheck: React.FC<PermissionStateCheckProps> = ({
	value = { permissions: [], requiresAll: false },
	onChange,
}) => {
	const { t: $t } = useTranslation();
	const { deserialize } = localizationSerializer();
	const { Lr } = useLocalizer();

	// 1. Fetch Data
	const { data: permissionData, isLoading } = useQuery({
		queryKey: ["permissionStateCheckData"],
		queryFn: async () => {
			const [groupsRes, permissionsRes] = await Promise.all([getGroupsApi(), getPermissionsApi()]);

			// Prepare localized permissions list
			const permissions = permissionsRes.items.map((p) => {
				const d = deserialize(p.displayName);
				return {
					...p,
					displayName: Lr(d.resourceName, d.name), // Localized Name
					name: p.name,
					parentName: p.parentName,
					groupName: p.groupName,
				};
			});

			// Prepare localized groups
			const groups = groupsRes.items.map((g) => {
				const d = deserialize(g.displayName);
				return {
					...g,
					displayName: Lr(d.resourceName, d.name),
					name: g.name,
				};
			});

			return { groups, permissions };
		},
		staleTime: Number.POSITIVE_INFINITY,
	});

	// 2. Construct Tree Data
	const treeData = useMemo(() => {
		if (!permissionData) return [];
		const { groups, permissions } = permissionData;

		return groups.map((group) => {
			// Find permissions belonging to this group
			const groupPermissions = permissions.filter((p) => p.groupName === group.name);

			// Build hierarchy for permissions (parent/child)
			// Standard listToTree usage: (list, config)
			const children = listToTree(groupPermissions, { id: "name", pid: "parentName" });

			// Return Group Node
			// Note: We use 'displayName' and 'name' which match the DTOs,
			// and map them using the 'fieldNames' prop on TreeSelect below.
			return {
				displayName: group.displayName,
				name: group.name,
				// UI props to make group unselectable/uncheckable
				checkable: false,
				selectable: false,
				disableCheckbox: true,
				children: children,
			};
		});
	}, [permissionData]);

	// 3. Map selected string[] to { label, value } objects for TreeSelect (Strict Mode)
	const treeValue = useMemo(() => {
		if (!permissionData || !value.permissions) return [];
		return value.permissions.map((name) => {
			const permission = permissionData.permissions.find((p) => p.name === name);
			return {
				label: permission?.displayName || name,
				value: name,
			};
		});
	}, [permissionData, value.permissions]);

	// 4. Handle Change
	const triggerChange = (changedValue: Partial<ValueType>) => {
		onChange?.({ ...value, ...changedValue });
	};

	const onTreeChange = (labeledValues: { label: React.ReactNode; value: string }[]) => {
		const names = labeledValues.map((item) => item.value);
		triggerChange({ permissions: names });
	};

	if (isLoading) return <Spin className="my-2" />;

	return (
		<div className="flex flex-col gap-2 w-full">
			<Checkbox checked={value.requiresAll} onChange={(e) => triggerChange({ requiresAll: e.target.checked })}>
				{$t("component.simple_state_checking.requirePermissions.requiresAll")}
			</Checkbox>

			<TreeSelect
				treeData={treeData}
				value={treeValue}
				onChange={onTreeChange}
				// Map DTO fields to AntD TreeSelect expected fields
				fieldNames={{ label: "displayName", value: "name", children: "children" }}
				allowClear
				treeCheckable
				treeCheckStrictly
				showCheckedStrategy={TreeSelect.SHOW_ALL}
				placeholder={$t("ui.placeholder.select")}
				style={{ width: "100%" }}
				treeDefaultExpandAll
			/>
		</div>
	);
};

export default PermissionStateCheck;
