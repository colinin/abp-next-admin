import type React from "react";
import { useState } from "react";
import { Card, Select, Button, Tree, Empty, Spin } from "antd";
import { useTranslation } from "react-i18next";
import { useQuery } from "@tanstack/react-query";
import { getListApi as getContainersApi, getObjectsApi } from "@/api/oss/containes";
import FolderModal from "./folder-modal";

const { DirectoryTree } = Tree;

interface FolderNode {
	key: string;
	title: string;
	isLeaf?: boolean;
	children?: FolderNode[];
	dataRef?: { path?: string; name: string };
}

interface Props {
	onBucketChange: (bucket: string) => void;
	onFolderChange: (path: string) => void;
}

const FolderTree: React.FC<Props> = ({ onBucketChange, onFolderChange }) => {
	const { t: $t } = useTranslation();

	// -- State --
	const [bucket, setBucket] = useState<string>("");
	const [treeData, setTreeData] = useState<FolderNode[]>([]);

	// Controlled Tree State
	const [expandedKeys, setExpandedKeys] = useState<React.Key[]>([]);
	const [selectedKeys, setSelectedKeys] = useState<React.Key[]>([]);
	const [loadedKeys, setLoadedKeys] = useState<React.Key[]>([]);

	// Modal State
	const [modalVisible, setModalVisible] = useState(false);
	const [selectedPathForCreate, setSelectedPathForCreate] = useState<string>("");

	// -- 1. React Query for Containers --
	const { data: containerData, isLoading: isContainersLoading } = useQuery({
		queryKey: ["oss-containers-list"],
		queryFn: () => getContainersApi({ maxResultCount: 1000 }),
	});

	const containers = containerData?.containers || [];

	// -- Helpers --
	const rootNode: FolderNode = {
		key: "./",
		title: $t("AbpOssManagement.Objects:Root"),
		isLeaf: false,
		dataRef: { path: "", name: "./" },
		children: [],
	};

	// 3. FIX: Completely reset tree when bucket changes
	const handleBucketChange = (val: string) => {
		setBucket(val);
		onBucketChange(val);

		// Reset ALL list states to prevent stale paths
		setTreeData([rootNode]);
		setExpandedKeys([]);
		setLoadedKeys([]);
		setSelectedKeys([]);
		setSelectedPathForCreate(""); // Reset create path
		onFolderChange(""); // Reset file list path
	};

	const getFolders = async (bucketName: string, prefix: string) => {
		const { objects } = await getObjectsApi({
			bucket: bucketName,
			delimiter: "/",
			maxResultCount: 1000,
			prefix: prefix,
		});
		return objects
			.filter((f) => f.isFolder)
			.map((folder) => ({
				key: `${folder.path || ""}${folder.name}`,
				title: folder.name,
				isLeaf: false,
				dataRef: folder,
				children: [],
			}));
	};

	const updateTreeData = (list: FolderNode[], key: React.Key, children: FolderNode[]): FolderNode[] => {
		return list.map((node) => {
			if (node.key === key) {
				return { ...node, children };
			}
			if (node.children) {
				return { ...node, children: updateTreeData(node.children, key, children) };
			}
			return node;
		});
	};

	// -- Tree Event Handlers --

	const onLoadData = async ({ key, dataRef }: any) => {
		if (!bucket) return;

		let path = "";
		if (dataRef?.path) path += dataRef.path;
		if (dataRef?.name && dataRef.name !== "./") path += dataRef.name;

		try {
			const childFolders = await getFolders(bucket, path);
			setTreeData((origin) => updateTreeData(origin, key, childFolders));
			setLoadedKeys((prev) => [...prev, key]);
		} catch (error) {
			console.error(error);
			setTreeData((origin) => updateTreeData(origin, key, []));
		}
	};

	const onExpand = (keys: React.Key[], info: any) => {
		setExpandedKeys(keys);
		if (!info.expanded) {
			const nodeKey = info.node.key;
			setLoadedKeys((prev) => prev.filter((k) => k !== nodeKey));
		}
	};

	const onSelect = (keys: React.Key[], info: any) => {
		setSelectedKeys(keys);
		if (keys.length === 1) {
			const keyStr = keys[0].toString();
			// 1. Determine Path
			const nodePath = keyStr === "./" ? "" : keyStr;

			// 2. Pass path to parent (File List)
			onFolderChange(nodePath);

			// 3. FIX: Store specific path for "Create Folder" modal
			setSelectedPathForCreate(nodePath);
		}
	};

	const handleCreateFolder = () => {
		// If nothing selected, it stays empty (root), which is handled by default state
		setModalVisible(true);
	};

	const handleFolderCreated = () => {
		handleBucketChange(bucket); // TODO just re-fetch entire tree for simplicity  parent's other children (file-list.tsx) should also refresh (1), and the reverse is also true (2) and the same applies in reverse.
	};

	return (
		<>
			<Card title={$t("AbpOssManagement.Containers")} className="h-full flex flex-col">
				<div className="flex flex-col gap-2 flex-1">
					{isContainersLoading ? (
						<div className="flex justify-center p-4">
							<Spin />
						</div>
					) : (
						<Select
							placeholder={$t("AbpOssManagement.Containers:Select")}
							options={containers.map((c) => ({ label: c.name, value: c.name }))}
							value={bucket || undefined}
							onChange={handleBucketChange}
							className="w-full"
						/>
					)}

					{bucket ? (
						<>
							<Button block type="primary" ghost onClick={handleCreateFolder}>
								{$t("AbpOssManagement.Objects:CreateFolder")}
							</Button>
							<div className="overflow-auto flex-1 mt-2">
								<DirectoryTree
									key={bucket}
									blockNode
									treeData={treeData}
									// Controlled State
									expandedKeys={expandedKeys}
									selectedKeys={selectedKeys}
									loadedKeys={loadedKeys}
									// Handlers
									loadData={onLoadData}
									onSelect={onSelect}
									onExpand={onExpand}
									// Default
									defaultExpandedKeys={["./"]}
								/>
							</div>
						</>
					) : (
						<Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />
					)}
				</div>
			</Card>

			<FolderModal
				visible={modalVisible}
				bucket={bucket}
				path={selectedPathForCreate}
				onClose={() => setModalVisible(false)}
				onChange={handleFolderCreated}
			/>
		</>
	);
};

export default FolderTree;
