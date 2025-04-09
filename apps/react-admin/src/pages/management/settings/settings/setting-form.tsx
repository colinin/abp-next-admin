import { useEffect, useState } from "react";
import { Card, Form, Tabs, Collapse, Input, InputNumber, DatePicker, Select, Checkbox, Button } from "antd";
import { useTranslation } from "react-i18next";
import {
	ValueType,
	type SettingDetail,
	type SettingGroup,
	type SettingsUpdateInput,
} from "#/management/settings/settings";
import { formatToDate } from "@/utils/abp";
import { SettingOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { toast } from "sonner";

interface Props {
	getApi: () => Promise<SettingGroup[]>;
	submitApi: (input: SettingsUpdateInput) => Promise<void>;
	onChange: (data: SettingsUpdateInput) => void;
	slots?: {
		[key: string]: React.FC<{
			detail: SettingDetail;
			onChange: (setting: SettingDetail) => void;
		}>;
	};
}

const defaultModel: SettingsUpdateInput = {
	settings: [],
};

const SettingForm: React.FC<Props> = ({ getApi, submitApi, onChange, slots }) => {
	const { t: $t } = useTranslation();
	// const [form] = Form.useForm();
	const [activeTab, setActiveTab] = useState(0);
	const [submitting, setSubmitting] = useState(false);
	const [settingGroups, setSettingGroups] = useState<SettingGroup[]>([]);
	const [settingsUpdateInput, setSettingsUpdateInput] = useState<SettingsUpdateInput>(defaultModel);

	useEffect(() => {
		fetchSettings();
	}, []);

	const fetchSettings = async () => {
		const groups = await getApi();
		setSettingGroups(groups);
	};

	const getExpandedCollapseKeys = (group: SettingGroup) => {
		return group.settings.map((setting) => setting.displayName);
	};

	const handleSubmit = async () => {
		if (settingsUpdateInput.settings.length === 0) return;

		try {
			setSubmitting(true);
			await submitApi(settingsUpdateInput);
			onChange(settingsUpdateInput);
			toast.success($t("AbpSettingManagement.SuccessfullySaved"));
		} finally {
			setSubmitting(false);
		}
	};

	const handleCheckChange = (setting: SettingDetail) => {
		setting.value = setting.value === "true" ? "false" : "true";
		handleValueChange(setting);
	};

	const handleDateChange = (date: any, setting: SettingDetail) => {
		setting.value = dayjs.isDayjs(date) ? formatToDate(date) : "";
		handleValueChange(setting);
	};

	const handleValueChange = (setting: SettingDetail) => {
		if (setting.valueType === ValueType.NoSet) return;

		setSettingsUpdateInput((prev) => {
			const newSettings = [...prev.settings];
			const index = newSettings.findIndex((s) => s.name === setting.name);

			if (index === -1) {
				newSettings.push({
					name: setting.name,
					value: String(setting.value),
				});
			} else {
				newSettings[index] = {
					...newSettings[index],
					value: String(setting.value),
				};
			}

			return { settings: newSettings };
		});
	};

	const renderSettingInput = (detail: SettingDetail) => {
		// 如果存在自定义槽位，优先使用槽位
		if (detail.slot && slots?.[detail.slot]) {
			const SlotComponent = slots[detail.slot];
			// return <SlotComponent detail={detail} onChange={(_)=>{}} />;
			return <SlotComponent detail={detail} onChange={handleValueChange} />;
		}

		switch (detail.valueType) {
			case ValueType.String:
				return detail.isEncrypted ? (
					<Input.Password
						value={detail.value}
						placeholder={detail.description}
						onChange={(e) => {
							detail.value = e.target.value;
							handleValueChange(detail);
						}}
					/>
				) : (
					<Input
						value={detail.value}
						placeholder={detail.description}
						onChange={(e) => {
							detail.value = e.target.value;
							handleValueChange(detail);
						}}
					/>
				);
			case ValueType.Number:
				return !detail.isEncrypted ? (
					<InputNumber
						className="w-full"
						value={detail.value}
						placeholder={detail.description}
						onChange={(value) => {
							detail.value = String(value);
							handleValueChange(detail);
						}}
					/>
				) : null;
			case ValueType.Date:
				return (
					<DatePicker
						className="w-full"
						placeholder={detail.description}
						value={detail.value ? dayjs(detail.value, "YYYY-MM-DD") : null}
						onChange={(date) => handleDateChange(date, detail)}
					/>
				);
			case ValueType.Option:
				return (
					<Select
						value={detail.value}
						onChange={(value) => {
							detail.value = value;
							handleValueChange(detail);
						}}
					>
						{detail.options?.map((option) => (
							<Select.Option key={option.value} value={option.value} disabled={option.value === detail.value}>
								{option.name}
							</Select.Option>
						))}
					</Select>
				);
			case ValueType.Boolean:
				return (
					<Checkbox checked={detail.value === "true"} onChange={() => handleCheckChange(detail)}>
						{detail.displayName}
					</Checkbox>
				);
			default:
				return null;
		}
	};

	return (
		<Card
			title={$t("AbpSettingManagement.Settings")}
			extra={
				settingsUpdateInput.settings.length > 0 && (
					<Button
						type="primary"
						icon={<SettingOutlined />}
						loading={submitting}
						onClick={handleSubmit}
						className="w-[100px]"
					>
						{$t("AbpUi.Submit")}
					</Button>
				)
			}
		>
			<Form labelCol={{ span: 5 }} wrapperCol={{ span: 15 }}>
				<Tabs activeKey={String(activeTab)} onChange={(key) => setActiveTab(Number(key))}>
					{settingGroups.map((group, index) => (
						// biome-ignore lint: reason
						<Tabs.TabPane key={index} tab={group.displayName}>
							<Collapse defaultActiveKey={getExpandedCollapseKeys(group)}>
								{group.settings.map((setting) => (
									<Collapse.Panel key={setting.displayName} header={setting.displayName}>
										{setting.details.map((detail) => (
											<Form.Item key={detail.name} label={detail.displayName} extra={detail.description}>
												{renderSettingInput(detail)}
											</Form.Item>
										))}
									</Collapse.Panel>
								))}
							</Collapse>
						</Tabs.TabPane>
					))}
				</Tabs>
			</Form>
		</Card>
	);
};

export default SettingForm;
