import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Outlet } from "react-router";

const BackgroundJobs = lazy(() => import("@/pages/tasks/job-info-table"));

const tasks: AppRouteObject[] = [
	{
		order: 4,
		path: "task-management",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.tasks.title",
			icon: <Iconify icon="eos-icons:background-tasks" size={24} />,
			key: "/task-management",
		},

		children: [
			{
				path: "background-jobs",
				element: <BackgroundJobs />,
				meta: {
					icon: <Iconify icon="eos-icons:job" />,
					label: "abp.tasks.jobInfo.title",
					key: "/task-management/background-jobs",
				},
			},
		],
	},
];

export default tasks;
