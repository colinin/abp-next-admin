import type React from "react";
import { useState } from "react";
import { Row, Col } from "antd";
import FolderTree from "./folder-tree";
import FileList from "./file-list";

const OssManagement: React.FC = () => {
	const [bucket, setBucket] = useState("");
	const [path, setPath] = useState("");

	const handleBucketChange = (val: string) => {
		setBucket(val);
		setPath("");
	};

	const handleFolderChange = (val: string) => {
		setPath(val);
	};

	return (
		<div className="p-4 h-full">
			<Row gutter={16} className="h-full">
				<Col span={6} className="h-full">
					<FolderTree onBucketChange={handleBucketChange} onFolderChange={handleFolderChange} />
				</Col>
				<Col span={18} className="h-full">
					<FileList bucket={bucket} path={path} />
				</Col>
			</Row>
		</div>
	);
};

export default OssManagement;
