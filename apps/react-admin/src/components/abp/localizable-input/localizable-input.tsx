import { useCallback, useEffect, useMemo, useState } from "react";
import { Form, Input, Select } from "antd";
import type { DefaultOptionType } from "antd/es/select";
import { useTranslation } from "react-i18next";
import { isNullOrWhiteSpace } from "@/utils/string";
import type { LocalizableStringInfo } from "#/abp-core/global";
import useAbpStore from "@/store/abpCoreStore";
import { localizationSerializer } from "@/utils/abp/localization-serializer";

interface Props {
	allowClear?: boolean;
	disabled?: boolean;
	value?: string;
	onChange?: (value: string) => void;
}

interface State {
	displayName?: string;
	displayNames: DefaultOptionType[];
	resourceName?: string;
}

const LocalizableInput: React.FC<Props> = ({ allowClear, disabled, value, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const abpStore = useAbpStore();
	const { deserialize, serialize } = localizationSerializer();

	const [state, setState] = useState<State>({
		displayNames: [],
		displayName: undefined,
		resourceName: undefined,
	});

	const isFixed = useMemo(() => {
		return state.resourceName === "Fixed";
	}, [state.resourceName]);

	const resources = useMemo(() => {
		if (!abpStore.localization) {
			return [];
		}

		const sources = Object.keys(abpStore.localization.resources).map((key) => ({
			label: key,
			value: key,
		}));

		return [
			{
				label: $t("component.localizable_input.resources.fiexed.group"),
				options: [
					{
						label: $t("component.localizable_input.resources.fiexed.label"),
						value: "Fixed",
					},
				],
				value: "F",
			},
			{
				label: $t("component.localizable_input.resources.localization.group"),
				options: sources,
				value: "R",
			},
		];
	}, [abpStore.localization, $t]);

	const triggerDisplayNameChange = useCallback(
		(displayName?: string) => {
			if (!displayName) return;

			let updateValue = "";
			if (isFixed) {
				updateValue = `F:${displayName}`;
			} else if (!isNullOrWhiteSpace(state.resourceName)) {
				const info: LocalizableStringInfo = {
					name: displayName,
					resourceName: state.resourceName ?? "",
				};
				updateValue = serialize(info);
			}

			onChange?.(updateValue);
			form.setFieldValue("localizableInput", updateValue);
		},
		[isFixed, state.resourceName, serialize, onChange, form],
	);

	const handleDisplayNameChange = (value?: string) => {
		setState((prev) => ({ ...prev, displayName: value }));
		triggerDisplayNameChange(value);
	};

	const localizationResources = abpStore.localization?.resources;
	const handleResourceChange = useCallback(
		(value?: string, triggerChanged = false) => {
			const newDisplayNames: DefaultOptionType[] = [];

			if (value && localizationResources?.[value]) {
				Object.keys(localizationResources[value].texts).forEach((key) => {
					const labelText = localizationResources[value]?.texts[key];
					newDisplayNames.push({
						label: labelText ?? key,
						value: key,
					});
				});
			}

			setState((prev) => ({
				...prev,
				displayNames: newDisplayNames,
				displayName: undefined,
				resourceName: value,
			}));

			if (triggerChanged) {
				triggerDisplayNameChange(undefined);
			}
		},
		[localizationResources, triggerDisplayNameChange],
	);
	useEffect(() => {
		if (value) {
			const info = deserialize(value);
			if (state.resourceName !== info.resourceName) {
				handleResourceChange(isNullOrWhiteSpace(info.resourceName) ? undefined : info.resourceName, false);
			}
			if (state.displayName !== info.name) {
				setState((prev) => ({
					...prev,
					displayName: isNullOrWhiteSpace(info.name) ? undefined : info.name,
				}));
			}
		}
	}, [value]);

	return (
		<div className="w-full">
			<Form.Item noStyle>
				<Input.Group>
					<div className="flex flex-row gap-4">
						<div className="basis-2/5">
							<Select
								value={state.resourceName}
								allowClear={allowClear}
								disabled={disabled}
								options={resources}
								placeholder={$t("component.localizable_input.placeholder")}
								className="w-full"
								onChange={(value) => handleResourceChange(value?.toString(), true)}
							/>
						</div>
						<div className="basis-3/5">
							{isFixed ? (
								<Input
									allowClear={allowClear}
									disabled={disabled}
									placeholder={$t("component.localizable_input.resources.fiexed.placeholder")}
									value={state.displayName}
									autoComplete="off"
									className="w-full"
									onChange={(e) => handleDisplayNameChange(e.target.value)}
								/>
							) : (
								<Select
									allowClear={allowClear}
									disabled={disabled}
									options={state.displayNames}
									placeholder={$t("component.localizable_input.resources.localization.placeholder")}
									value={state.displayName}
									className="w-full"
									onChange={(value) => handleDisplayNameChange(value?.toString())}
								/>
							)}
						</div>
					</div>
				</Input.Group>
			</Form.Item>
		</div>
	);
};

export default LocalizableInput;
