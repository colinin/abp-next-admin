import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Outlet } from "react-router";

const MySettings = lazy(() => import("@/pages/account/my-setting"));

const account: AppRouteObject[] = [
	{
		path: "account",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.account.title",
			icon: <Iconify icon="mdi:account-outline" size={24} />,
			key: "/account",
			hideMenu: true,
		},
		children: [
			{
				path: "my-settings",
				element: <MySettings />,
				meta: {
					icon: <Iconify icon="tdesign:user-setting" />,
					label: "abp.account.settings.title",
					key: "/account/my-settings",
				},
			},
		],
	},
];

export default account;
