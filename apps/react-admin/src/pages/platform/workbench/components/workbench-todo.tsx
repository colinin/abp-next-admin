import type React from "react";
import { Card, Checkbox, Empty } from "antd";
// Using custom type or interface from common-ui if available, else define locally
interface WorkbenchTodoItem {
	title: string;
	content: string;
	completed: boolean;
	date: string;
}

interface Props {
	items?: WorkbenchTodoItem[];
	title: string;
}

const WorkbenchTodo: React.FC<Props> = ({ items = [], title }) => {
	return (
		<Card title={title} className="min-h-[300px] shadow-sm">
			{items.length === 0 ? (
				<Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />
			) : (
				<ul className="divide-y divide-gray-100 w-full">
					{items.map((item) => (
						<li
							key={item.title}
							className={`flex justify-between gap-x-6 py-4 cursor-pointer hover:bg-gray-50 transition-colors px-2 ${
								item.completed ? "opacity-60" : ""
							}`}
						>
							<div className="flex min-w-0 items-start gap-x-4 flex-1">
								<Checkbox checked={item.completed} className="mt-1" />
								<div className="min-w-0 flex-auto">
									<p className={`text-sm font-semibold text-gray-900 ${item.completed ? "line-through" : ""}`}>
										{item.title}
									</p>
									<div
										className="mt-1 truncate text-xs text-gray-500"
										dangerouslySetInnerHTML={{ __html: item.content }}
									/>
								</div>
							</div>
							<div className="hidden shrink-0 sm:flex sm:flex-col sm:items-end">
								<span className="text-xs text-gray-400 mt-1">{item.date}</span>
							</div>
						</li>
					))}
				</ul>
			)}
		</Card>
	);
};

export default WorkbenchTodo;
