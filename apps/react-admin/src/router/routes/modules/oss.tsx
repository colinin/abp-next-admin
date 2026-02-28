import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Outlet } from "react-router";

const OssContainer = lazy(() => import("@/pages/oss/containers/container-table"));
const OssObjects = lazy(() => import("@/pages/oss/objects/oss-management"));

const oss: AppRouteObject[] = [
	{
		order: 3,
		path: "oss",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.oss.title",
			icon: <Iconify icon="icon-park-outline:cloud-storage" size={24} />,
			key: "/oss",
		},

		children: [
			{
				path: "containers",
				element: <OssContainer />,
				meta: {
					icon: <Iconify icon="mdi:bucket-outline" />,
					label: "abp.oss.containers",
					key: "/oss/containers",
				},
			},
			{
				path: "objects",
				element: <OssObjects />,
				meta: {
					icon: <Iconify icon="mdi-light:file" />,
					label: "abp.oss.objects",
					key: "/oss/objects",
				},
			},
		],
	},
];

export default oss;
