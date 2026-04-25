import type React from "react";
import { useRef, useState, useEffect } from "react";
import { Button, Card, Modal, Space } from "antd";
import { DeleteOutlined, DownloadOutlined, UploadOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { ProTable, type ProColumns, type ActionType } from "@ant-design/pro-table";
import { getObjectsApi } from "@/api/oss/containes";
import { deleteApi, generateUrlApi } from "@/api/oss/objects";
import type { OssObjectDto } from "#/oss/objects";
import { formatToDateTime } from "@/utils/abp";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { OssObjectPermissions } from "@/constants/oss/permissions";
import FileUploadModal from "./file-upload-modal";
import { antdOrderToAbpOrder } from "@/utils/abp/sort-order";

interface Props {
	bucket: string;
	path: string;
}

const FileList: React.FC<Props> = ({ bucket, path }) => {
	const { t: $t } = useTranslation();
	const actionRef = useRef<ActionType>();
	const [modal, contextHolder] = Modal.useModal();
	const [uploadModalVisible, setUploadModalVisible] = useState(false);

	// Reload table when bucket or path changes
	useEffect(() => {
		if (bucket) {
			actionRef.current?.reload();
		}
	}, [bucket, path]);

	const handleUpload = () => {
		setUploadModalVisible(true);
	};

	const handleDelete = (row: OssObjectDto) => {
		modal.confirm({
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessageWithFormat", { 0: row.name }),
			onOk: async () => {
				await deleteApi({
					bucket,
					object: row.name,
					path: row.path,
				});
				toast.success($t("AbpUi.DeletedSuccessfully"));
				actionRef.current?.reload();
			},
		});
	};

	const handleDownload = async (row: OssObjectDto) => {
		const url = await generateUrlApi({
			bucket,
			mD5: false,
			object: row.name,
			path: row.path,
		});
		const link = document.createElement("a");
		link.style.display = "none";
		link.href = url;
		link.setAttribute("download", row.name);
		document.body.appendChild(link);
		link.click();
		document.body.removeChild(link);
	};

	const formatSize = (size?: number) => {
		if (!size) return "";
		const kb = 1024;
		const mb = kb * 1024;
		const gb = mb * 1024;

		if (size > gb) return `${(size / gb).toFixed(2)} GB`;
		if (size > mb) return `${(size / mb).toFixed(2)} MB`;
		return `${(size / kb).toFixed(2)} KB`;
	};

	const columns: ProColumns<OssObjectDto>[] = [
		{
			title: $t("AbpOssManagement.DisplayName:Name"),
			dataIndex: "name",
			minWidth: 150,
			sorter: true,
		},
		{
			title: $t("AbpOssManagement.DisplayName:FileType"),
			dataIndex: "isFolder",
			minWidth: 150,
			sorter: true,
			render: (val) => (val ? $t("AbpOssManagement.DisplayName:Folder") : $t("AbpOssManagement.DisplayName:Standard")),
		},
		{
			title: $t("AbpOssManagement.DisplayName:Size"),
			dataIndex: "size",
			minWidth: 150,
			sorter: true,
			render: (_, row) => formatSize(row.size),
		},
		{
			title: $t("AbpOssManagement.DisplayName:CreationDate"),
			dataIndex: "creationDate",
			minWidth: 150,
			sorter: true,
			render: (_, row) => formatToDateTime(row.creationDate),
		},
		{
			title: $t("AbpOssManagement.DisplayName:LastModifiedDate"),
			dataIndex: "lastModifiedDate",
			minWidth: 150,
			sorter: true,
			render: (_, row) => formatToDateTime(row.lastModifiedDate),
		},
		{
			title: $t("AbpUi.Actions"),
			valueType: "option",
			fixed: "right",
			width: 200,
			render: (_, record) => (
				<Space>
					{!record.isFolder && hasAccessByCodes([OssObjectPermissions.Download]) && (
						<Button type="link" icon={<DownloadOutlined />} onClick={() => handleDownload(record)}>
							{$t("AbpOssManagement.Objects:Download")}
						</Button>
					)}
					{hasAccessByCodes([OssObjectPermissions.Delete]) && (
						<Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>
							{$t("AbpUi.Delete")}
						</Button>
					)}
				</Space>
			),
		},
	];

	return (
		<>
			{contextHolder}
			<Card>
				<ProTable<OssObjectDto>
					headerTitle={$t("AbpOssManagement.FileList")}
					actionRef={actionRef}
					rowKey="name"
					columns={columns}
					search={false}
					toolBarRender={() => [
						path && (
							<Button key="upload" type="primary" icon={<UploadOutlined />} onClick={handleUpload}>
								{$t("AbpOssManagement.Objects:UploadFile")}
							</Button>
						),
					]}
					request={async (params, sorter) => {
						if (!bucket) return { data: [], success: true };

						const sorting =
							sorter && Object.keys(sorter).length > 0
								? Object.keys(sorter)
										.map((key) => `${key} ${antdOrderToAbpOrder(sorter[key])}`)
										.join(", ")
								: undefined;

						// Note: Prefix logic handles path filtering
						// Logic for path: if path is ./ or empty, prefix might be empty or root logic
						const prefix = path === "./" ? "" : path;

						const res = await getObjectsApi({
							bucket,
							maxResultCount: params.pageSize,
							skipCount: ((params.current || 1) - 1) * (params.pageSize || 10),
							prefix: prefix,
							sorting,
							// You might need delimiter here if listing "current folder only"
							// delimiter: '/'
						});

						return {
							data: res.objects,
							total: res.maxKeys,
							success: true,
						};
					}}
					pagination={{
						defaultPageSize: 10,
						showSizeChanger: true,
					}}
				/>
			</Card>
			<FileUploadModal
				visible={uploadModalVisible}
				bucket={bucket}
				path={path === "./" ? "" : path}
				onClose={() => setUploadModalVisible(false)}
				onFileUploaded={() => actionRef.current?.reload()}
			/>
		</>
	);
};

export default FileList;
