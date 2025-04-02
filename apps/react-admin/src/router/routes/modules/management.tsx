import { Suspense, lazy } from "react";
import { Navigate, Outlet } from "react-router";

import { Iconify, SvgIcon } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";

// const ProfilePage = lazy(() => import("@/pages/management/user/profile"));
// const AccountPage = lazy(() => import("@/pages/management/user/account"));

// const OrganizationPage = lazy(() => import("@/pages/management/system/organization"));
// const PermissioPage = lazy(() => import("@/pages/management/system/permission"));

// const Blog = lazy(() => import("@/pages/management/blog"));

// Identity
const Users = lazy(() => import("@/pages/management/identity/users/user-table"));
const Roles = lazy(() => import("@/pages/management/identity/roles/role-table"));
const ClaimTypes = lazy(() => import("@/pages/management/identity/claim-types/claim-types-table"));
const SecurityLogs = lazy(() => import("@/pages/management/identity/security-logs/security-logs-table"));
const OrganizationUnits = lazy(() => import("@/pages/management/identity/organization-units/organization-unit-page"));
const IdentitySessions = lazy(() => import("@/pages/management/identity/sessions/session-table"));

// Permissions
const PermissionGroupDefinition = lazy(
	() => import("@/pages/management/permissions/definitions/permission-group-definition-table"),
);
const PermissionDefinitions = lazy(
	() => import("@/pages/management/permissions/permissions/permission-definition-table"),
);
// Auditing logs
const AuditingAuditLogs = lazy(() => import("@/pages/management/audit-logs/audit-log-table"));

// settings
const SettingDefinitions = lazy(() => import("@/pages/management/settings/definitions/setting-definition-table"));
const SystemSettings = lazy(() => import("@/pages/management/settings/settings/system-setting.tsx"));

// notifications
const MyNotifications = lazy(() => import("@/pages/management/notifications/my-notification-table"));

const management: AppRouteObject = {
	order: 2,
	path: "management",
	element: (
		<Suspense fallback={<CircleLoading />}>
			<Outlet />
		</Suspense>
	),
	meta: {
		label: "sys.menu.management",
		icon: <SvgIcon icon="ic-management" className="ant-menu-item-icon" size="24" />,
		key: "/management",
	},
	children: [
		{
			index: true,
			element: <Navigate to="user" replace />,
		},
		// {
		// 	path: "user",
		// 	meta: { label: "sys.menu.user.index", key: "/management/user" },
		// 	children: [
		// 		{
		// 			index: true,
		// 			element: <Navigate to="profile" replace />,
		// 		},
		// 		{
		// 			path: "profile",
		// 			element: <ProfilePage />,
		// 			meta: {
		// 				label: "sys.menu.user.profile",
		// 				key: "/management/user/profile",
		// 			},
		// 		},
		// 		{
		// 			path: "account",
		// 			element: <AccountPage />,
		// 			meta: {
		// 				label: "sys.menu.user.account",
		// 				key: "/management/user/account",
		// 			},
		// 		},
		// 	],
		// },
		// {
		// 	path: "system",
		// 	meta: { label: "sys.menu.system.index", key: "/management/system" },
		// 	children: [
		// 		{
		// 			path: "organization",
		// 			element: <OrganizationPage />,
		// 			meta: {
		// 				label: "sys.menu.system.organization",
		// 				key: "/management/system/organization",
		// 			},
		// 		},
		// 		{
		// 			path: "permission",
		// 			element: <PermissioPage />,
		// 			meta: {
		// 				label: "sys.menu.system.permission",
		// 				key: "/management/system/permission",
		// 			},
		// 		},
		// 	],
		// },
		// {
		// 	path: "blog",
		// 	element: <Blog />,
		// 	meta: { label: "sys.menu.blog", key: "/management/blog" },
		// },
		{
			path: "identity",
			meta: {
				label: "abp.manage.identity.title",
				key: "/management/identity",
				icon: <Iconify icon="teenyicons:id-outline" />,
			},
			children: [
				{
					index: true,
					element: <Navigate to="users" replace />,
				},
				{
					path: "users",
					element: <Users />,
					meta: {
						label: "abp.manage.identity.user",
						key: "/management/identity/users",
						icon: <Iconify icon="mdi:user-outline" />,
					},
				},
				{
					path: "roles",
					element: <Roles />,
					meta: {
						label: "abp.manage.identity.role",
						key: "/management/identity/roles",
						icon: <Iconify icon="carbon:user-role" />,
					},
				},
				{
					path: "claim-types",
					element: <ClaimTypes />,
					meta: {
						label: "abp.manage.identity.claimTypes",
						key: "/management/identity/claim-types",
						icon: <Iconify icon="la:id-card-solid" />,
					},
				},
				{
					path: "security-logs",
					element: <SecurityLogs />,
					meta: {
						label: "abp.manage.identity.securityLogs",
						key: "/management/identity/security-logs",
						icon: <Iconify icon="carbon:security" />,
					},
				},
				{
					path: "organization-units",
					element: <OrganizationUnits />,
					meta: {
						label: "abp.manage.identity.organizationUnits",
						key: "/management/identity/organization-units",
						icon: <Iconify icon="clarity:organization-line" />,
					},
				},
				{
					path: "sessions",
					element: <IdentitySessions />,
					meta: {
						label: "abp.manage.identity.sessions",
						key: "/management/identity/sessions",
						icon: <Iconify icon="carbon:prompt-session" />,
					},
				},
			],
		},
		{
			path: "permissions",
			meta: {
				label: "abp.manage.permissions.title",
				key: "/management/permissions",
				icon: <Iconify icon="arcticons:permissionsmanager" />,
			},
			children: [
				{
					index: true,
					element: <Navigate to="groups" replace />,
				},
				{
					path: "groups",
					element: <PermissionGroupDefinition />,
					meta: {
						label: "abp.manage.permissions.groups",
						key: "/management/permissions/groups",
						icon: <Iconify icon="lucide:group" />,
					},
				},
				{
					path: "definitions",
					element: <PermissionDefinitions />,
					meta: {
						label: "abp.manage.permissions.definitions",
						key: "/management/permissions/definitions",
						icon: <Iconify icon="icon-park-outline:permissions" />,
					},
				},
			],
		},
		{
			path: "settings",
			meta: {
				label: "abp.manage.settings.title",
				key: "/management/settings",
				icon: <Iconify icon="ic:outline-settings" />,
			},
			children: [
				{
					index: true,
					element: <Navigate to="definitions" replace />,
				},
				{
					path: "definitions",
					element: <SettingDefinitions />,
					meta: {
						label: "abp.manage.settings.definitions",
						key: "/management/settings/definitions",
						icon: <Iconify icon="codicon:settings" />,
					},
				},
				{
					path: "system",
					element: <SystemSettings />,
					meta: {
						label: "abp.manage.settings.system",
						key: "/management/settings/system",
						icon: <Iconify icon="tabler:settings-cog" />,
					},
				},
			],
		},
		{
			path: "audit-logs",
			element: <AuditingAuditLogs />,
			meta: {
				label: "abp.manage.identity.auditLogs",
				key: "/management/audit-logs",
				icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
			},
		},
		{
			path: "notifications",
			meta: {
				label: "abp.manage.notifications.title",
				key: "/management/notifications",
				icon: <Iconify icon="tabler:notification" />,
			},
			children: [
				{
					index: true,
					element: <Navigate to="my-notifilers" replace />,
				},
				{
					path: "my-notifilers",
					element: <MyNotifications />,
					meta: {
						label: "abp.manage.notifications.myNotifilers",
						key: "/management/notifications/my-notifilers",
						icon: <Iconify icon="ant-design:notification-outlined" />,
					},
				},
			],
		},
	],
};

export default management;
