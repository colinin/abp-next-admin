import type React from "react";
import { useEffect, useState } from "react";
import { Drawer, Form, Input, Select, Checkbox, InputNumber, DatePicker, Tabs, Space, Button, Spin } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import dayjs from "dayjs";
import { createApi, updateApi, getApi, getDefinitionsApi } from "@/api/tasks/job-infos";
import type { BackgroundJobInfoDto, BackgroundJobDefinitionDto } from "#/tasks/job-infos";
import { JobPriority, JobType } from "#/tasks/job-infos";
import { formatToDate } from "@/utils/abp";
import PropertyTable from "@/components/abp/properties/property-table";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: BackgroundJobInfoDto) => void;
	jobId?: string; // If provided, edit mode
}

const JobInfoDrawer: React.FC<Props> = ({ visible, onClose, onChange, jobId }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const [activeTab, setActiveTab] = useState("basic");
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);
	const [jobDefinitions, setJobDefinitions] = useState<BackgroundJobDefinitionDto[]>([]);
	const [formModel, setFormModel] = useState<Partial<BackgroundJobInfoDto>>({});

	// Options
	const jobTypeOptions = [
		{ label: $t("BackgroundTasks.JobType:Once"), value: JobType.Once },
		{ label: $t("BackgroundTasks.JobType:Period"), value: JobType.Period },
		{ label: $t("BackgroundTasks.JobType:Persistent"), value: JobType.Persistent },
	];

	const jobPriorityOptions = [
		{ label: $t("BackgroundTasks.Priority:Low"), value: JobPriority.Low },
		{ label: $t("BackgroundTasks.Priority:BelowNormal"), value: JobPriority.BelowNormal },
		{ label: $t("BackgroundTasks.Priority:Normal"), value: JobPriority.Normal },
		{ label: $t("BackgroundTasks.Priority:AboveNormal"), value: JobPriority.AboveNormal },
		{ label: $t("BackgroundTasks.Priority:High"), value: JobPriority.High },
	];

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			form.resetFields();
			if (jobId) {
				fetchJob(jobId);
			} else {
				fetchDefinitions();
				// Set Defaults
				const initial = {
					args: {},
					beginTime: formatToDate(new Date()),
					isEnabled: true,
					jobType: JobType.Once,
					priority: JobPriority.Normal,
				};
				setFormModel(initial);
				form.setFieldsValue({
					...initial,
					beginTime: dayjs(), // AntD needs dayjs object
				});
			}
		}
	}, [visible, jobId]);

	const fetchJob = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			setFormModel(dto);
			form.setFieldsValue({
				...dto,
				beginTime: dto.beginTime ? dayjs(dto.beginTime) : null,
				endTime: dto.endTime ? dayjs(dto.endTime) : null,
			});
		} finally {
			setLoading(false);
		}
	};

	const fetchDefinitions = async () => {
		const { items } = await getDefinitionsApi();
		setJobDefinitions(items);
	};

	const handleJobDefineChange = (jobName: string) => {
		const jobDefine = jobDefinitions.find((x) => x.name === jobName);
		if (jobDefine) {
			const params: Record<string, any> = {};
			jobDefine.paramters.forEach((param) => {
				params[param.name] = undefined;
			});

			const newModel = { ...formModel, args: params, type: jobDefine.name };
			setFormModel(newModel);
			form.setFieldsValue({ type: jobDefine.name }); // Sync specific field
		}
	};

	const handleParamChange = (record: { key: string; value: any }) => {
		setFormModel((prev) => ({
			...prev,
			args: { ...prev.args, [record.key]: record.value },
		}));
	};

	const handleDeleteParam = (record: { key: string }) => {
		setFormModel((prev) => {
			const newArgs = { ...prev.args };
			delete newArgs[record.key];
			return { ...prev, args: newArgs };
		});
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const submitData = {
				...formModel,
				...values,
				// Convert Dayjs back to string
				beginTime: values.beginTime ? values.beginTime.format("YYYY-MM-DD") : null,
				endTime: values.endTime ? values.endTime.format("YYYY-MM-DD") : null,
				args: formModel.args, // Args managed separately via PropertyTable
			};

			const api = jobId ? updateApi(jobId, submitData) : createApi(submitData);

			const res = await api;
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		} catch (e) {
			console.error(e);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Drawer
			title={
				jobId
					? `${$t("TaskManagement.BackgroundJobs:Edit")} - ${formModel.name}`
					: $t("TaskManagement.BackgroundJobs:AddNew")
			}
			open={visible}
			onClose={onClose}
			width={800}
			extra={
				<Space>
					<Button onClick={onClose}>{$t("AbpUi.Cancel")}</Button>
					<Button type="primary" loading={submitting} onClick={handleSubmit}>
						{$t("AbpUi.Save")}
					</Button>
				</Space>
			}
		>
			<Spin spinning={loading}>
				<Form form={form} layout="horizontal" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }} autoComplete="off">
					<Tabs activeKey={activeTab} onChange={setActiveTab}>
						<Tabs.TabPane key="basic" tab={$t("TaskManagement.BasicInfo")}>
							{!jobId && (
								<Form.Item label={$t("TaskManagement.BackgroundJobs")} extra={$t("TaskManagement.BackgroundJobs")}>
									<Select
										allowClear
										options={jobDefinitions}
										fieldNames={{ label: "displayName", value: "name" }}
										onChange={handleJobDefineChange}
									/>
								</Form.Item>
							)}

							<Form.Item name="isEnabled" valuePropName="checked" label={$t("TaskManagement.DisplayName:IsEnabled")}>
								<Checkbox>{$t("TaskManagement.DisplayName:IsEnabled")}</Checkbox>
							</Form.Item>

							<Form.Item name="nodeName" label={$t("TaskManagement.DisplayName:NodeName")}>
								<Input autoComplete="off" data-lpignore="true" />
							</Form.Item>

							<Form.Item name="group" label={$t("TaskManagement.DisplayName:Group")} rules={[{ required: true }]}>
								<Input autoComplete="off" />
							</Form.Item>

							<Form.Item name="name" label={$t("TaskManagement.DisplayName:Name")} rules={[{ required: true }]}>
								<Input autoComplete="off" />
							</Form.Item>

							<Form.Item
								name="type"
								label={$t("TaskManagement.DisplayName:Type")}
								extra={$t("TaskManagement.Description:Type")}
								rules={[{ required: true }]}
							>
								<Input.TextArea autoSize={{ minRows: 3 }} />
							</Form.Item>

							<Form.Item
								name="beginTime"
								label={$t("TaskManagement.DisplayName:BeginTime")}
								rules={[{ required: true }]}
							>
								<DatePicker className="w-full" showTime />
							</Form.Item>

							<Form.Item name="endTime" label={$t("TaskManagement.DisplayName:EndTime")}>
								<DatePicker className="w-full" showTime />
							</Form.Item>

							<Form.Item
								name="jobType"
								label={$t("TaskManagement.DisplayName:JobType")}
								extra={$t("TaskManagement.Description:JobType")}
							>
								<Select options={jobTypeOptions} allowClear />
							</Form.Item>

							<Form.Item noStyle shouldUpdate={(prev, curr) => prev.jobType !== curr.jobType}>
								{({ getFieldValue }) =>
									getFieldValue("jobType") === JobType.Period ? (
										<Form.Item
											name="cron"
											label={$t("TaskManagement.DisplayName:Cron")}
											extra={$t("TaskManagement.Description:Cron")}
											rules={[{ required: true }]}
										>
											<Input />
										</Form.Item>
									) : (
										<Form.Item
											name="interval"
											label={$t("TaskManagement.DisplayName:Interval")}
											extra={$t("TaskManagement.Description:Interval")}
											rules={[{ required: true }]}
										>
											<InputNumber className="w-full" />
										</Form.Item>
									)
								}
							</Form.Item>

							<Form.Item
								name="maxCount"
								label={$t("TaskManagement.DisplayName:MaxCount")}
								extra={$t("TaskManagement.Description:MaxCount")}
							>
								<InputNumber className="w-full" />
							</Form.Item>

							<Form.Item
								name="maxTryCount"
								label={$t("TaskManagement.DisplayName:MaxTryCount")}
								extra={$t("TaskManagement.Description:MaxTryCount")}
							>
								<InputNumber className="w-full" />
							</Form.Item>

							<Form.Item
								name="priority"
								label={$t("TaskManagement.DisplayName:Priority")}
								extra={$t("TaskManagement.Description:Priority")}
							>
								<Select options={jobPriorityOptions} allowClear />
							</Form.Item>

							<Form.Item
								name="lockTimeOut"
								label={$t("TaskManagement.DisplayName:LockTimeOut")}
								extra={$t("TaskManagement.Description:LockTimeOut")}
							>
								<InputNumber className="w-full" />
							</Form.Item>

							<Form.Item name="description" label={$t("TaskManagement.DisplayName:Description")}>
								<Input.TextArea autoSize={{ minRows: 3 }} />
							</Form.Item>
						</Tabs.TabPane>

						<Tabs.TabPane key="paramters" tab={$t("TaskManagement.Paramters")}>
							<PropertyTable data={formModel.args} onChange={handleParamChange} onDelete={handleDeleteParam} />
						</Tabs.TabPane>
					</Tabs>
				</Form>
			</Spin>
		</Drawer>
	);
};

export default JobInfoDrawer;
