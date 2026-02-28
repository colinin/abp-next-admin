import type React from "react";
import { Avatar } from "antd";
import { useTranslation } from "react-i18next";

interface Props {
	avatar?: string;
	notifierCount?: number;
	text?: string;
	children?: React.ReactNode; // For title
	description?: React.ReactNode;
}

const WorkbenchHeader: React.FC<Props> = ({ avatar, text, notifierCount = 0, children, description }) => {
	const { t: $t } = useTranslation();

	return (
		<div className="card-box p-4 py-6 lg:flex bg-white rounded-md shadow-sm border border-gray-100">
			<Avatar size={80} src={avatar} alt={text} className="flex-none" />
			<div className="flex flex-col justify-center md:ml-6 md:mt-0 flex-1">
				<h1 className="text-md font-semibold md:text-xl text-gray-800">{children}</h1>
				{description && <span className="text-gray-500 mt-1">{description}</span>}
			</div>
			<div className="mt-4 flex flex-1 justify-end md:mt-0 items-center">
				<div className="flex flex-col justify-center text-right">
					<span className="text-gray-500">{$t("workbench.header.notifier.title")}</span>
					<span className="text-2xl font-medium">{$t("workbench.header.notifier.count", { 0: notifierCount })}</span>
				</div>
			</div>
		</div>
	);
};

export default WorkbenchHeader;
