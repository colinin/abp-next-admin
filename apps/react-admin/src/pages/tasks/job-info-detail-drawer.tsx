import type React from "react";
import { useEffect, useState } from "react";
import { Drawer, Tabs, Descriptions, Tag, Checkbox, List, Button, Popconfirm, Empty, Spin } from "antd";
import { DeleteOutlined, CheckCircleOutlined, CloseCircleOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { getApi } from "@/api/tasks/job-infos";
import { deleteApi as deleteJobLogApi, getPagedListApi as getJobLogsApi } from "@/api/tasks/job-logs";
import type { BackgroundJobInfoDto, BackgroundJobLogDto } from "#/tasks";
import { JobType } from "#/tasks/job-infos";
import { formatToDateTime } from "@/utils/abp";
import PropertyTable from "@/components/abp/properties/property-table";
import { useJobEnumsMap } from "@/hooks/abp/use-Job-enums-map";

interface Props {
	visible: boolean;
	onClose: () => void;
	jobId?: string;
}

const JobInfoDetailDrawer: React.FC<Props> = ({ visible, onClose, jobId }) => {
	const { t: $t } = useTranslation();
	const { jobPriorityColor, jobPriorityMap, jobSourceMap, jobStatusColor, jobStatusMap, jobTypeMap } = useJobEnumsMap();

	const [activeTabKey, setActiveTabKey] = useState("basic");
	const [loading, setLoading] = useState(false);
	const [jobInfo, setJobInfo] = useState<BackgroundJobInfoDto>();
	const [jobLogs, setJobLogs] = useState<BackgroundJobLogDto[]>([]);

	// Pagination State
	const [pagination, setPagination] = useState({
		current: 1,
		pageSize: 10,
		total: 0,
	});

	useEffect(() => {
		if (visible && jobId) {
			setActiveTabKey("basic");
			fetchJobInfo(jobId);
		} else {
			setJobInfo(undefined);
			setJobLogs([]);
		}
	}, [visible, jobId]);

	const fetchJobInfo = async (id: string) => {
		try {
			setLoading(true);
			setJobLogs([]);
			const data = await getApi(id);
			setJobInfo(data);
		} finally {
			setLoading(false);
		}
	};

	const fetchJobLogs = async (page = 1, pageSize = 10) => {
		if (!jobInfo?.id) return;
		try {
			setLoading(true);
			const result = await getJobLogsApi({
				jobId: jobInfo.id,
				maxResultCount: pageSize,
				skipCount: (page - 1) * pageSize,
			});
			setJobLogs(result.items);
			setPagination({
				current: page,
				pageSize: pageSize,
				total: result.totalCount,
			});
		} finally {
			setLoading(false);
		}
	};

	const handleTabChange = (key: string) => {
		setActiveTabKey(key);
		if (key === "logs") {
			fetchJobLogs(1, pagination.pageSize);
		}
	};

	const handleDeleteLog = async (logId: string) => {
		try {
			setLoading(true);
			await deleteJobLogApi(logId);
			toast.success($t("AbpUi.DeletedSuccessfully"));
			fetchJobLogs(pagination.current, pagination.pageSize);
		} finally {
			setLoading(false);
		}
	};

	return (
		<Drawer
			title={$t("TaskManagement.BackgroundJobDetail")}
			open={visible}
			onClose={onClose}
			width="50%"
			destroyOnClose
		>
			<Spin spinning={loading}>
				{jobInfo ? (
					<Tabs activeKey={activeTabKey} onChange={handleTabChange}>
						{/* Basic Info Tab */}
						<Tabs.TabPane key="basic" tab={$t("TaskManagement.BasicInfo")}>
							<Descriptions column={2} bordered size="small" labelStyle={{ minWidth: "120px" }}>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Status")}>
									<Tag color={jobStatusColor[jobInfo.status]}>{jobStatusMap[jobInfo.status]}</Tag>
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Source")}>
									{jobSourceMap[jobInfo.source]}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:IsEnabled")}>
									<Checkbox disabled checked={jobInfo.isEnabled} />
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Priority")}>
									<Tag color={jobPriorityColor[jobInfo.priority]}>{jobPriorityMap[jobInfo.priority]}</Tag>
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Group")}>{jobInfo.group}</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Name")}>{jobInfo.name}</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Description")} span={2}>
									{jobInfo.description}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Type")} span={2}>
									{jobInfo.type}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:CreationTime")}>
									{formatToDateTime(jobInfo.creationTime)}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:LockTimeOut")}>
									{jobInfo.lockTimeOut}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:BeginTime")}>
									{formatToDateTime(jobInfo.beginTime)}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:EndTime")}>
									{jobInfo.endTime ? formatToDateTime(jobInfo.endTime) : ""}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:JobType")}>
									{jobTypeMap[jobInfo.jobType]}
								</Descriptions.Item>
								{jobInfo.jobType === JobType.Period ? (
									<Descriptions.Item label={$t("TaskManagement.DisplayName:Cron")}>{jobInfo.cron}</Descriptions.Item>
								) : (
									<Descriptions.Item label={$t("TaskManagement.DisplayName:Interval")}>
										{jobInfo.interval}
									</Descriptions.Item>
								)}
								<Descriptions.Item label={$t("TaskManagement.DisplayName:LastRunTime")}>
									{jobInfo.lastRunTime ? formatToDateTime(jobInfo.lastRunTime) : ""}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:NextRunTime")}>
									{jobInfo.nextRunTime ? formatToDateTime(jobInfo.nextRunTime) : ""}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:TriggerCount")}>
									{jobInfo.triggerCount}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:MaxCount")}>
									{jobInfo.maxCount}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:TryCount")}>
									{jobInfo.tryCount}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:MaxTryCount")}>
									{jobInfo.maxTryCount}
								</Descriptions.Item>
								<Descriptions.Item label={$t("TaskManagement.DisplayName:Result")} span={2}>
									<pre className="whitespace-pre-wrap break-words">{jobInfo.result}</pre>
								</Descriptions.Item>
							</Descriptions>
						</Tabs.TabPane>

						{/* Parameters Tab */}
						<Tabs.TabPane key="paramters" tab={$t("TaskManagement.Paramters")}>
							<PropertyTable data={jobInfo.args} disabled />
						</Tabs.TabPane>

						{/* Logs Tab */}
						<Tabs.TabPane key="logs" tab={$t("TaskManagement.BackgroundJobLogs")}>
							<List
								itemLayout="vertical"
								size="default"
								bordered
								dataSource={jobLogs}
								pagination={{
									...pagination,
									onChange: (page, pageSize) => fetchJobLogs(page, pageSize),
									showSizeChanger: true,
								}}
								renderItem={(item) => (
									<List.Item
										key={item.id}
										extra={
											<Popconfirm
												title={$t("AbpUi.AreYouSure")}
												description={$t("AbpUi.ItemWillBeDeletedMessage")}
												onConfirm={() => handleDeleteLog(item.id)}
											>
												<Button type="link" danger icon={<DeleteOutlined />} />
											</Popconfirm>
										}
									>
										<List.Item.Meta
											avatar={
												!item.exception ? (
													<CheckCircleOutlined style={{ fontSize: 24, color: "seagreen" }} />
												) : (
													<CloseCircleOutlined style={{ fontSize: 24, color: "orangered" }} />
												)
											}
											title={jobInfo.name}
											description={formatToDateTime(item.runTime)}
										/>
										<pre className="whitespace-pre-wrap break-words">{item.exception ?? item.message}</pre>
									</List.Item>
								)}
							/>
						</Tabs.TabPane>
					</Tabs>
				) : (
					<Empty />
				)}
			</Spin>
		</Drawer>
	);
};

export default JobInfoDetailDrawer;
