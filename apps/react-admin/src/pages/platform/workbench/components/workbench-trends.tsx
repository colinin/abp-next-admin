import type React from "react";
import { Card, Empty, Avatar } from "antd";
import { Iconify } from "@/components/icon";

interface WorkbenchTrendItem {
	avatar?: string; // Icon string or image url
	title: string;
	content: string;
	date: string;
}

interface Props {
	items?: WorkbenchTrendItem[];
	title: string;
}

const WorkbenchTrends: React.FC<Props> = ({ items = [], title }) => {
	return (
		<Card title={title} className="h-full">
			{items.length === 0 ? (
				<Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />
			) : (
				<ul className="divide-y divide-gray-100 w-full">
					{items.map((item, index) => (
						<li
							key={`${item.title}-${index}`}
							className="flex justify-between gap-x-6 py-4 px-2 hover:bg-gray-50 transition-colors"
						>
							<div className="flex min-w-0 items-center gap-x-4">
								{/* Check if avatar is icon string or url */}
								{item.avatar?.includes(":") ? (
									<Avatar
										icon={<Iconify icon={item.avatar} />}
										className="bg-blue-100 text-blue-600 flex-none"
										size="large"
									/>
								) : (
									<Avatar src={item.avatar} size="large" className="flex-none" />
								)}

								<div className="min-w-0 flex-auto">
									<p className="text-sm font-semibold text-gray-900 leading-6">{item.title}</p>
									<div
										className="mt-1 truncate text-xs text-gray-500"
										dangerouslySetInnerHTML={{ __html: item.content }}
									/>
								</div>
							</div>
							<div className="hidden shrink-0 sm:flex sm:flex-col sm:items-end justify-center">
								<span className="text-xs text-gray-400">{item.date}</span>
							</div>
						</li>
					))}
				</ul>
			)}
		</Card>
	);
};

export default WorkbenchTrends;
