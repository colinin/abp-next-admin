import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Outlet } from "react-router";

const TextTemplatingDefinitions = lazy(() => import("@/pages/text-templating/template-definition-table"));

const textTemplating: AppRouteObject[] = [
	{
		order: 7,
		path: "text-templating",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.textTemplating.title",
			icon: <Iconify icon="tdesign:template" size={24} />,
			key: "/text-templating",
		},

		children: [
			{
				path: "definitions",
				element: <TextTemplatingDefinitions />,
				meta: {
					icon: <Iconify icon="qlementine-icons:template-16" />,
					label: "abp.textTemplating.definitions",
					key: "/text-templating/definitions",
				},
			},
		],
	},
];

export default textTemplating;
