import type React from "react";
import { useRef } from "react";
import { Button, Tag, Space, Modal, Dropdown, Card } from "antd";
import { DeleteOutlined, EllipsisOutlined, SendOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteApi, getPagedListApi, sendApi } from "@/api/platform/sms-messages";
import type { SmsMessageDto } from "#/platform/messages";
import { MessageStatus } from "#/platform/messages";
import { SmsMessagesPermissions } from "@/constants/platform/permissions";
import { hasAccessByCodes, withAccessChecker } from "@/utils/abp/access-checker";
import { formatToDateTime } from "@/utils/abp";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";

const SmsMessageTable: React.FC = () => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const queryClient = useQueryClient();
	const [modal, contextHolder] = Modal.useModal();

	const { mutateAsync: deleteMessage } = useMutation({
		mutationFn: deleteApi,
		onSuccess: () => {
			toast.success($t("AbpUi.DeletedSuccessfully"));
			actionRef.current?.reload();
		},
	});

	const { mutateAsync: sendMessage } = useMutation({
		mutationFn: sendApi,
		onSuccess: () => {
			toast.success($t("AppPlatform.SuccessfullySent"));
			actionRef.current?.reload();
		},
	});

	const handleDelete = (row: SmsMessageDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: () => deleteMessage(row.id),
		});
	};

	const handleSend = (row: SmsMessageDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AppPlatform.MessageWillBeReSendWarningMessage"),
			onOk: () => sendMessage(row.id),
		});
	};

	const statusOptions = [
		{ label: $t("AppPlatform.MessageStatus:Pending"), value: MessageStatus.Pending, color: "warning" },
		{ label: $t("AppPlatform.MessageStatus:Sent"), value: MessageStatus.Sent, color: "success" },
		{ label: $t("AppPlatform.MessageStatus:Failed"), value: MessageStatus.Failed, color: "error" },
	];

	const columns: ProColumns<SmsMessageDto>[] = [
		{
			title: $t("AppPlatform.DisplayName:Provider"),
			dataIndex: "provider",
			width: 150,
			sorter: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Status"),
			dataIndex: "status",
			width: 100,
			sorter: true,
			valueType: "select",
			fieldProps: {
				options: statusOptions,
			},
			render: (_, row) => {
				const option = statusOptions.find((o) => o.value === row.status);
				return <Tag color={option?.color}>{option?.label}</Tag>;
			},
		},
		{
			title: $t("AppPlatform.DisplayName:SendTime"),
			dataIndex: "sendTime",
			width: 180,
			sorter: true,
			valueType: "dateRange",
			render: (_, row) => formatToDateTime(row.sendTime),
		},
		{
			title: $t("AppPlatform.DisplayName:Content"),
			dataIndex: "content",
			width: 300,
			sorter: true,
			ellipsis: true,
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:Receiver"),
			dataIndex: "phoneNumber", // Search field name
			render: (_, row) => row.receiver,
			width: 150,
			sorter: true,
		},
		{
			title: $t("AppPlatform.DisplayName:SendCount"),
			dataIndex: "sendCount",
			width: 100,
			sorter: true,
			align: "center",
			hideInSearch: true,
		},
		{
			title: $t("AppPlatform.DisplayName:CreationTime"),
			dataIndex: "creationTime",
			width: 180,
			sorter: true,
			valueType: "date", // Hide in search if needed, usually implicitly handled by ProTable
			hideInSearch: true,
			render: (_, row) => formatToDateTime(row.creationTime),
		},
		{
			title: $t("AppPlatform.DisplayName:Reason"),
			dataIndex: "reason",
			width: 150,
			sorter: true,
			ellipsis: true,
			hideInSearch: true,
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 150,
			render: (_, record) => (
				<Space>
					{withAccessChecker(
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>,
						[SmsMessagesPermissions.Delete],
					)}

					<Dropdown
						menu={{
							items: [
								hasAccessByCodes([SmsMessagesPermissions.SendMessage])
									? {
											key: "send",
											label: $t("AppPlatform.SendMessage"),
											icon: <SendOutlined className="text-green-500" />,
											onClick: () => handleSend(record),
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
				<ProTable<SmsMessageDto>
					headerTitle={$t("AppPlatform.SmsMessages")}
					actionRef={actionRef}
					rowKey="id"
					columns={columns}
					search={{ labelWidth: "auto", defaultCollapsed: true }}
					request={async (params, sorter) => {
						const { current, pageSize, sendTime, ...filters } = params;
						const [beginSendTime, endSendTime] = sendTime || [];

						const sorting =
							sorter && Object.keys(sorter).length > 0
								? Object.keys(sorter)
										.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
										.join(", ")
								: undefined;

						const query = await queryClient.fetchQuery({
							queryKey: ["smsMessages", params, sorter],
							queryFn: () =>
								getPagedListApi({
									maxResultCount: pageSize,
									skipCount: ((current || 1) - 1) * (pageSize || 0),
									sorting: sorting,
									beginSendTime,
									endSendTime,
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
					scroll={{ x: "max-content" }}
				/>
			</Card>
		</>
	);
};

export default SmsMessageTable;
