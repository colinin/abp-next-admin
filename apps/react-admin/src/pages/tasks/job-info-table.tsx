import type React from "react";
import { useRef, useState } from "react";
import { Button, Tag, Space, Dropdown, Modal, Checkbox, type FormInstance, Card } from "antd";
import {
	EditOutlined,
	DeleteOutlined,
	PlusOutlined,
	EllipsisOutlined,
	PauseCircleOutlined,
	PlayCircleOutlined,
	ThunderboltOutlined,
	StopOutlined,
} from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import ProTable, { type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useQueryClient } from "@tanstack/react-query";
import { formatToDateTime } from "@/utils/abp";
import {
	bulkDeleteApi,
	bulkStartApi,
	bulkStopApi,
	deleteApi,
	getPagedListApi,
	pauseApi,
	resumeApi,
	startApi,
	stopApi,
	triggerApi,
} from "@/api/tasks/job-infos";
import type { BackgroundJobInfoDto } from "#/tasks/job-infos";
import { JobSource, JobStatus } from "#/tasks/job-infos";
import { BackgroundJobsPermissions } from "@/constants/tasks/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import JobInfoDrawer from "./job-info-drawer";
import JobInfoDetailDrawer from "./job-info-detail-drawer";
import { toast } from "sonner";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";
import { useJobEnumsMap } from "@/hooks/abp/use-Job-enums-map";

const JobInfoTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const formRef = useRef<FormInstance>(); // Ref to control the search form
	const queryClient = useQueryClient();
	const { jobPriorityColor, jobPriorityMap, jobStatusColor, jobStatusMap, jobTypeMap } = useJobEnumsMap();
	const [modal, contextHolder] = Modal.useModal();

	// Drawer States
	const [createDrawerVisible, setCreateDrawerVisible] = useState(false);
	const [detailDrawerVisible, setDetailDrawerVisible] = useState(false);
	const [selectedJobId, setSelectedJobId] = useState<string | undefined>();
	const [selectedKeys, setSelectedKeys] = useState<React.Key[]>([]);

	// Status Logic
	const allowPauseStatus = [JobStatus.Queuing, JobStatus.Running, JobStatus.FailedRetry];
	const allowResumeStatus = [JobStatus.Paused, JobStatus.Stopped];
	const allowTriggerStatus = [JobStatus.Queuing, JobStatus.Running, JobStatus.Completed, JobStatus.FailedRetry];
	const allowStartStatus = [JobStatus.None, JobStatus.Stopped, JobStatus.FailedRetry];
	const allowStopStatus = [JobStatus.Queuing, JobStatus.Running];

	// --- Helper to handle manual filter triggers ---
	const onFilter = (field: string, value: any, shouldSubmit = true) => {
		if (formRef.current) {
			formRef.current.setFieldsValue({
				[field]: value,
			});
			if (shouldSubmit) {
				formRef.current.submit();
			}
		}
	};

	// Actions
	const handleCreate = () => {
		setSelectedJobId(undefined);
		setCreateDrawerVisible(true);
	};

	const handleEdit = (row: BackgroundJobInfoDto) => {
		setSelectedJobId(row.id);
		setCreateDrawerVisible(true);
	};

	const handleShow = (row: BackgroundJobInfoDto) => {
		setSelectedJobId(row.id);
		setDetailDrawerVisible(true);
	};

	const handleStatusChange = (job: BackgroundJobInfoDto, api: (id: string) => Promise<void>, warningMsg: string) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: warningMsg,
			onOk: async () => {
				await api(job.id);
				toast.success($t("AbpUi.SavedSuccessfully"));
				actionRef.current?.reload();
			},
		});
	};

	const handleDelete = (row: BackgroundJobInfoDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: async () => {
				await deleteApi(row.id);
				toast.success($t("AbpUi.DeletedSuccessfully"));
				actionRef.current?.reload();
			},
		});
	};

	const handleBulkAction = (api: (input: { jobIds: string[] }) => Promise<void>, warningMsg: string) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: warningMsg,
			onOk: async () => {
				await api({ jobIds: selectedKeys as string[] });
				toast.success($t("AbpUi.SavedSuccessfully"));
				actionRef.current?.reload();
				setSelectedKeys([]);
			},
		});
	};

	const columns: ProColumns<BackgroundJobInfoDto>[] = [
		{
			title: $t("TaskManagement.DisplayName:Group"),
			dataIndex: "group",
			width: 150,
			sorter: true,
			fixed: "left",
		},
		{
			title: $t("TaskManagement.DisplayName:Name"),
			dataIndex: "name",
			width: 300,
			sorter: true,
			fixed: "left",
			render: (_, row) => (
				<Button type="link" onClick={() => handleShow(row)}>
					{row.name}
				</Button>
			),
		},
		{
			title: $t("TaskManagement.DisplayName:Description"),
			dataIndex: "description",
			width: 350,
			sorter: true,
		},
		{
			title: $t("TaskManagement.DisplayName:CreationTime"),
			dataIndex: "creationTime",
			width: 150,
			sorter: true,
			render: (_, row) => formatToDateTime(row.creationTime),
		},
		{
			title: $t("TaskManagement.DisplayName:Status"),
			dataIndex: "status",
			width: 120,
			sorter: true,
			valueType: "select",
			fieldProps: {
				options: Object.keys(jobStatusMap).map((k) => ({ label: jobStatusMap[Number(k)], value: Number(k) })),
			},
			render: (_, row) => <Tag color={jobStatusColor[row.status]}>{jobStatusMap[row.status]}</Tag>,
		},
		{
			title: $t("TaskManagement.DisplayName:Result"),
			dataIndex: "result",
			width: 200,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("TaskManagement.DisplayName:LastRunTime"),
			dataIndex: "lastRunTime",
			width: 150,
			valueType: "dateRange",
			sorter: true,
			render: (_, row) => (row.lastRunTime ? formatToDateTime(row.lastRunTime) : ""),
		},
		{
			title: $t("TaskManagement.DisplayName:NextRunTime"),
			dataIndex: "nextRunTime",
			width: 150,
			sorter: true,
			hideInSearch: true,
			render: (_, row) => (row.nextRunTime ? formatToDateTime(row.nextRunTime) : ""),
		},
		{
			title: $t("TaskManagement.DisplayName:JobType"),
			dataIndex: "jobType",
			width: 150,
			sorter: true,
			valueType: "select",
			fieldProps: {
				options: Object.keys(jobTypeMap).map((k) => ({ label: jobTypeMap[Number(k)], value: Number(k) })),
			},
			render: (_, row) => <Tag color="blue">{jobTypeMap[row.jobType]}</Tag>,
		},
		{
			title: $t("TaskManagement.DisplayName:Priority"),
			dataIndex: "priority",
			width: 150,
			sorter: true,
			hideInSearch: true,
			render: (_, row) => <Tag color={jobPriorityColor[row.priority]}>{jobPriorityMap[row.priority]}</Tag>,
		},
		{
			title: $t("TaskManagement.DisplayName:Cron"),
			dataIndex: "cron",
			width: 150,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("TaskManagement.DisplayName:TriggerCount"),
			dataIndex: "triggerCount",
			width: 100,
			sorter: true,
			hideInSearch: true,
		},
		{
			title: $t("TaskManagement.DisplayName:TryCount"),
			dataIndex: "tryCount",
			width: 150,
			sorter: true,
			hideInSearch: true,
		},
		{
			// Custom search filters not shown in table
			title: $t("TaskManagement.DisplayName:BeginTime"),
			dataIndex: "time",
			valueType: "dateRange",
			hideInTable: true,
		},
		{
			title: $t("TaskManagement.DisplayName:IsAbandoned"),
			dataIndex: "isAbandoned",
			valueType: "checkbox",
			hideInTable: true,
			renderFormItem: () => (
				<Checkbox onChange={(e) => onFilter("isAbandoned", e.target.checked)}>
					{$t("TaskManagement.DisplayName:IsAbandoned")}
				</Checkbox>
			),
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 220,
			render: (_, row) => (
				<Space>
					{withAccessChecker(
						<Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(row)}>
							{$t("AbpUi.Edit")}
						</Button>,
						[BackgroundJobsPermissions.Update],
					)}

					{(row.source !== JobSource.System || hasAccessByCodes([BackgroundJobsPermissions.ManageSystemJobs])) &&
						withAccessChecker(
							<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(row)}>
								{$t("AbpUi.Delete")}
							</Button>,
							[BackgroundJobsPermissions.Delete],
						)}

					<Dropdown
						menu={{
							items: [
								allowPauseStatus.includes(row.status) && hasAccessByCodes([BackgroundJobsPermissions.Pause])
									? {
											key: "pause",
											label: $t("TaskManagement.BackgroundJobs:Pause"),
											icon: <PauseCircleOutlined />,
											onClick: () =>
												handleStatusChange(row, pauseApi, $t("TaskManagement.SelectJobWillBePauseMessage")),
										}
									: null,
								allowResumeStatus.includes(row.status) && hasAccessByCodes([BackgroundJobsPermissions.Resume])
									? {
											key: "resume",
											label: $t("TaskManagement.BackgroundJobs:Resume"),
											icon: <PlayCircleOutlined />,
											onClick: () =>
												handleStatusChange(row, resumeApi, $t("TaskManagement.SelectJobWillBeResumeMessage")),
										}
									: null,
								allowTriggerStatus.includes(row.status) && hasAccessByCodes([BackgroundJobsPermissions.Trigger])
									? {
											key: "trigger",
											label: $t("TaskManagement.BackgroundJobs:Trigger"),
											icon: <ThunderboltOutlined />,
											onClick: () =>
												handleStatusChange(row, triggerApi, $t("TaskManagement.SelectJobWillBeTriggerMessage")),
										}
									: null,
								allowStartStatus.includes(row.status) && hasAccessByCodes([BackgroundJobsPermissions.Start])
									? {
											key: "start",
											label: $t("TaskManagement.BackgroundJobs:Start"),
											icon: <PlayCircleOutlined />,
											onClick: () =>
												handleStatusChange(row, startApi, $t("TaskManagement.SelectJobWillBeStartMessage")),
										}
									: null,
								allowStopStatus.includes(row.status) && hasAccessByCodes([BackgroundJobsPermissions.Stop])
									? {
											key: "stop",
											label: $t("TaskManagement.BackgroundJobs:Stop"),
											icon: <StopOutlined />,
											onClick: () => handleStatusChange(row, stopApi, $t("TaskManagement.SelectJobWillBeStopMessage")),
										}
									: null,
							].filter(Boolean) as any,
						}}
					>
						<Button type="link" icon={<EllipsisOutlined />} />
					</Dropdown>
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<BackgroundJobInfoDto>
					headerTitle={$t("TaskManagement.BackgroundJobs")}
					actionRef={actionRef}
					formRef={formRef}
					rowKey="id"
					columns={columns}
					rowSelection={{
						selectedRowKeys: selectedKeys,
						onChange: (keys) => setSelectedKeys(keys),
					}}
					search={{ labelWidth: "auto", defaultCollapsed: true }}
					scroll={{ x: 2000 }} // Wide table due to many columns
					toolBarRender={() => [
						selectedKeys.length > 0 && hasAccessByCodes([BackgroundJobsPermissions.Start]) && (
							<Button
								key="start"
								ghost
								type="primary"
								icon={<PlayCircleOutlined />}
								onClick={() => handleBulkAction(bulkStartApi, $t("TaskManagement.SelectJobWillBeStartMessage"))}
							>
								{$t("TaskManagement.BackgroundJobs:Start")}
							</Button>
						),
						selectedKeys.length > 0 && hasAccessByCodes([BackgroundJobsPermissions.Stop]) && (
							<Button
								key="stop"
								ghost
								danger
								type="primary"
								icon={<StopOutlined />}
								onClick={() => handleBulkAction(bulkStopApi, $t("TaskManagement.SelectJobWillBeStopMessage"))}
							>
								{$t("TaskManagement.BackgroundJobs:Pause")}
							</Button>
						),
						hasAccessByCodes([BackgroundJobsPermissions.Create]) && (
							<Button key="create" type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
								{$t("TaskManagement.BackgroundJobs:AddNew")}
							</Button>
						),
						selectedKeys.length > 0 && hasAccessByCodes([BackgroundJobsPermissions.Delete]) && (
							<Button
								key="delete"
								danger
								type="primary"
								icon={<DeleteOutlined />}
								onClick={() =>
									handleBulkAction(bulkDeleteApi, $t("TaskManagement.MultipleSelectJobsWillBeDeletedMessage"))
								}
							>
								{$t("AbpUi.Delete")}
							</Button>
						),
					]}
					request={async (params, sorter) => {
						const { current, pageSize, time, lastRunTime, ...filters } = params;
						const [beginTime, endTime] = time || [];
						const [beginLastRunTime, endLastRunTime] = lastRunTime || [];

						const query = await queryClient.fetchQuery({
							queryKey: ["jobInfos", params, sorter],
							queryFn: () =>
								getPagedListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									sorting: sorter
										? Object.keys(sorter)
												.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
												.join(", ")
										: undefined,
									beginTime,
									endTime,
									beginLastRunTime,
									endLastRunTime,
									...filters,
								}),
						});
						return {
							data: query.items,
							total: query.totalCount,
							success: true,
						};
					}}
					pagination={{ defaultPageSize: 10, showSizeChanger: true }}
				/>
			</Card>
			<JobInfoDrawer
				visible={createDrawerVisible}
				onClose={() => setCreateDrawerVisible(false)}
				onChange={() => actionRef.current?.reload()}
				jobId={selectedJobId}
			/>

			<JobInfoDetailDrawer
				visible={detailDrawerVisible}
				onClose={() => setDetailDrawerVisible(false)}
				jobId={selectedJobId}
			/>
		</>
	);
};

export default JobInfoTable;
