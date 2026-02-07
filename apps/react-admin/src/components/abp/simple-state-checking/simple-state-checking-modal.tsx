import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Select } from "antd";
import { useTranslation } from "react-i18next";
import GlobalFeatureStateCheck from "@/components/abp/features/state-check/global-feature-state-check";
import PermissionStateCheck from "@/components/abp/permissions/state-check/permission-state-check";
import FeatureStateCheck from "@/components/abp/features/state-check/feature-state-check";

interface SimpleStateCheckingModalProps {
	visible: boolean;
	onClose: () => void;
	onConfirm: (data: any) => void;
	record?: any; // The existing checker to edit
	options: { label: string; value: string; disabled: boolean }[];
}

const SimpleStateCheckingModal: React.FC<SimpleStateCheckingModalProps> = ({
	visible,
	onClose,
	onConfirm,
	record,
	options,
}) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [selectedType, setSelectedType] = useState<string | undefined>();

	useEffect(() => {
		if (visible) {
			if (record) {
				// Map record back to form values
				const name = record.name;
				setSelectedType(name);

				let value = {};
				switch (name) {
					case "F":
						value = { featureNames: record.featureNames, requiresAll: record.requiresAll };
						break;
					case "G":
						value = { globalFeatureNames: record.globalFeatureNames, requiresAll: record.requiresAll };
						break;
					case "P":
						value = { permissions: record.model?.permissions || record.permissions, requiresAll: record.requiresAll };
						break;
				}

				form.setFieldsValue({
					name: name,
					value: value,
				});
			} else {
				form.resetFields();
				setSelectedType(undefined);
			}
		}
	}, [visible, record, form]);

	const handleTypeChange = (val: string) => {
		setSelectedType(val);
		// Reset the value field when type changes
		let initialValue = {};
		if (val === "F") initialValue = { featureNames: [], requiresAll: false };
		if (val === "G") initialValue = { globalFeatureNames: [], requiresAll: false };
		if (val === "P") initialValue = { permissions: [], requiresAll: false };

		form.setFieldValue("value", initialValue);
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();

			// Transform form values back to ABP simple state checker structure
			const result: any = {
				T: values.name, // T is usually the discriminator in some serializations, checking logic below
			};

			// Mapping based on the Vue onSubmit logic:
			// A = RequiresAll (boolean)
			// T = Discriminator/Name
			// N = Names array

			const val = values.value || {};
			const checkerObj: any = {
				name: values.name,
				// 'A' property seems to map to requiresAll for the checker itself
				requiresAll: val.requiresAll,
			};

			// Specifically for serialization logic later
			// The Vue code returns: { A: boolean, T: string, N: string[] }
			const output: any = {
				A: val.requiresAll,
				T: values.name,
			};

			switch (values.name) {
				case "F":
					output.N = val.featureNames;
					break;
				case "G":
					output.N = val.globalFeatureNames;
					break;
				case "P":
					output.N = val.permissions;
					break;
			}

			onConfirm(output);
			onClose();
		} catch (e) {
			console.error(e);
		}
	};

	return (
		<Modal
			title={$t("component.simple_state_checking.title")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item name="name" label={$t("component.simple_state_checking.form.name")} rules={[{ required: true }]}>
					<Select
						options={options}
						onChange={handleTypeChange}
						disabled={!!record} // Cannot change type when editing
					/>
				</Form.Item>

				<Form.Item noStyle shouldUpdate={(prev, curr) => prev.name !== curr.name}>
					{({ getFieldValue }) => {
						const type = getFieldValue("name");

						if (type === "A") {
							// Authenticated check requires no extra config
							return null;
						}

						if (type === "F") {
							return (
								<Form.Item
									name="value"
									label={$t("component.simple_state_checking.requireFeatures.featureNames")}
									rules={[
										{
											validator: (_, val) =>
												val?.featureNames?.length > 0
													? Promise.resolve()
													: Promise.reject($t("component.simple_state_checking.requireFeatures.featureNames")),
										},
									]}
								>
									<FeatureStateCheck />
								</Form.Item>
							);
						}

						if (type === "G") {
							return (
								<Form.Item
									name="value"
									label={$t("component.simple_state_checking.requireFeatures.featureNames")}
									rules={[
										{
											validator: (_, val) =>
												val?.globalFeatureNames?.length > 0
													? Promise.resolve()
													: Promise.reject($t("component.simple_state_checking.requireFeatures.featureNames")),
										},
									]}
								>
									<GlobalFeatureStateCheck />
								</Form.Item>
							);
						}

						if (type === "P") {
							return (
								<Form.Item
									name="value"
									label={$t("component.simple_state_checking.requirePermissions.permissions")}
									rules={[
										{
											validator: (_, val) =>
												val?.permissions?.length > 0
													? Promise.resolve()
													: Promise.reject($t("component.simple_state_checking.requirePermissions.permissions")),
										},
									]}
								>
									<PermissionStateCheck />
								</Form.Item>
							);
						}

						return null;
					}}
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default SimpleStateCheckingModal;
