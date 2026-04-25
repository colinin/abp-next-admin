import { Suspense, lazy } from "react";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";
import { Outlet } from "react-router";

const WebhookGroupDefine = lazy(() => import("@/pages/webhooks/definitions/groups/webhook-group-definition-table"));

const WebhookDefine = lazy(() => import("@/pages/webhooks/definitions/webhooks/webhook-definition-table"));

const Subscriptions = lazy(() => import("@/pages/webhooks/subscriptions/webhook-subscription-table"));
const SendAttempts = lazy(() => import("@/pages/webhooks/send-attempts/webhook-send-attempt-table"));
const webhooks: AppRouteObject[] = [
	{
		order: 5,
		path: "webhooks",
		element: (
			<Suspense fallback={<CircleLoading />}>
				<Outlet />
			</Suspense>
		),
		meta: {
			label: "abp.webhooks.title",
			icon: <Iconify icon="material-symbols:webhook" size={24} />,
			key: "/webhooks",
		},
		children: [
			{
				path: "groups",
				element: <WebhookGroupDefine />,
				meta: {
					icon: <Iconify icon="lucide:group" />,
					label: "abp.webhooks.groups",
					key: "/webhooks/groups",
				},
			},
			{
				path: "definitions",
				element: <WebhookDefine />,
				meta: {
					icon: <Iconify icon="material-symbols:webhook" />,
					label: "abp.webhooks.definitions",
					key: "/webhooks/definitions",
				},
			},
			{
				path: "subscriptions",
				element: <Subscriptions />,
				meta: {
					icon: <Iconify icon="material-symbols:subscriptions" />,
					label: "abp.webhooks.subscriptions",
					key: "/webhooks/subscriptions",
				},
			},
			{
				path: "send-attempts",
				element: <SendAttempts />,
				meta: {
					icon: <Iconify icon="material-symbols:history" />,
					label: "abp.webhooks.sendAttempts",
					key: "/webhooks/send-attempts",
				},
			},
		],
	},
];

export default webhooks;
