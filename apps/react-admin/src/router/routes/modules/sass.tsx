import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Outlet } from "react-router";

const SassEdition = lazy(() => import("@/pages/saas/editions/edition-table"));
const SassTalents = lazy(() => import("@/pages/saas/tenants/tenant-table"));

const sass: AppRouteObject[] = [
	{
		order: 6,
		path: "/saas",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.saas.title",
			icon: <Iconify icon="ant-design:cloud-server-outlined" size={24} />,
			key: "/saas",
		},

		children: [
			{
				path: "editions",
				element: <SassEdition />,
				meta: {
					icon: <Iconify icon="icon-park-outline:multi-rectangle" />,
					label: "abp.saas.editions",
					key: "/saas/editions",
				},
			},
			{
				path: "tenants",
				element: <SassTalents />,
				meta: {
					icon: <Iconify icon="arcticons:tenantcloud-pro" />,
					label: "abp.saas.tenants",
					key: "/saas/tenants",
				},
			},
		],
	},
];

export default sass;
