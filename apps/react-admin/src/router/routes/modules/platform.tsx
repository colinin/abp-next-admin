import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Navigate, Outlet } from "react-router";

const PlatformDataDictionaries = lazy(() => import("@/pages/platform/data-dictionaries/data-dictionary-table"));

const PlatformLayouts = lazy(() => import("@/pages/platform/layouts/layout-table"));

const PlatformMenus = lazy(() => import("@/pages/platform/menus/menu-table"));
const PlatformEmailMessages = lazy(() => import("@/pages/platform/messages/email/email-message-table"));
const PlatformSMSMessages = lazy(() => import("@/pages/platform/messages/sms/sms-message-table"));
const platform: AppRouteObject[] = [
	{
		order: 4,
		path: "platform",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.platform.title",
			icon: <Iconify icon="ep:platform" size={24} />,
			key: "/platform",
		},
		children: [
			{
				path: "data-dictionaries",
				element: <PlatformDataDictionaries />,
				meta: {
					icon: <Iconify icon="material-symbols:dictionary-outline" />,
					label: "abp.platform.dataDictionaries",
					key: "/platform/data-dictionaries",
				},
			},
			{
				path: "layouts",
				element: <PlatformLayouts />,
				meta: {
					icon: <Iconify icon="material-symbols-light:responsive-layout" />,
					label: "abp.platform.layouts",
					key: "/platform/layouts",
				},
			},
			{
				path: "menus",
				element: <PlatformMenus />,
				meta: {
					icon: <Iconify icon="material-symbols-light:menu" />,
					label: "abp.platform.menus",
					key: "/platform/menus",
				},
			},
			{
				path: "messages",
				meta: {
					icon: <Iconify icon="material-symbols:history" />,
					label: "abp.platform.messages.title",
					key: "/platform/messages",
				},
				children: [
					{
						index: true,
						element: <Navigate to="email" replace />,
					},
					{
						path: "email",
						element: <PlatformEmailMessages />,
						meta: {
							label: "abp.platform.messages.email",
							key: "/platform/messages/email",
							icon: <Iconify icon="material-symbols:attach-email-outline" />,
						},
					},
					{
						path: "sms",
						element: <PlatformSMSMessages />,
						meta: {
							label: "abp.platform.messages.sms",
							key: "/platform/messages/sms",
							icon: <Iconify icon="material-symbols:sms-outline" />,
						},
					},
				],
			},
		],
	},
];

export default platform;
