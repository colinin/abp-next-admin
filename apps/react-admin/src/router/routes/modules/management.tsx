import { Suspense, lazy } from "react";
import { Navigate, Outlet } from "react-router";

import { Iconify, SvgIcon } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";


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

// Features
const FeatureGroupDefinition = lazy(
	() => import("@/pages/management/features/definitions-groups/feature-group-definition-table"),
);
const FeatureDefinitions = lazy(
	() => import("@/pages/management/features/definitions-features/feature-definition-table"),
);

// Auditing logs
const AuditingAuditLogs = lazy(() => import("@/pages/management/audit-logs/audit-log-table"));
const Loggings = lazy(() => import("@/pages/management/loggings/logging-table"));

// settings
const SettingDefinitions = lazy(() => import("@/pages/management/settings/definitions/setting-definition-table"));
const SystemSettings = lazy(() => import("@/pages/management/settings/settings/system-setting.tsx"));

// notifications
const MyNotifications = lazy(() => import("@/pages/management/notifications/my-notifications/my-notification-table"));
const NotificationsGroupDefinition = lazy(
	() => import("@/pages/management/notifications/definitions/groups/notification-group-definition-table"),
);
const NotificationsDefinition = lazy(
	() => import("@/pages/management/notifications/definitions/notifications/notification-definition-table"),
);

// Localization
const Languages = lazy(() => import("@/pages/management/localization/languages/localization-language-table"));
const Resources = lazy(() => import("@/pages/management/localization/resources/localization-resource-table"));
const Texts = lazy(() => import("@/pages/management/localization/texts/localization-text-table"));

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
			path: "localization",
			meta: {
				label: "abp.manage.localization.title",
				key: "/management/localization",
				icon: <Iconify icon="ion:globe-outline" />,
			},
			children: [
				{
					index: true,
					element: <Navigate to="resources" replace />,
				},
				{
					path: "resources",
					element: <Resources />,
					meta: {
						label: "abp.manage.localization.resources",
						key: "/management/localization/resources",
						icon: <Iconify icon="grommet-icons:resources" />,
					},
				},
				{
					path: "languages",
					element: <Languages />,
					meta: {
						label: "abp.manage.localization.languages",
						key: "/management/localization/languages",
						icon: <Iconify icon="cil:language" />,
					},
				},
				{
					path: "texts",
					element: <Texts />,
					meta: {
						label: "abp.manage.localization.texts",
						key: "/management/localization/texts",
						icon: <Iconify icon="mi:text" />,
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
			path: "features",
			meta: {
				label: "abp.manage.features.title",
				key: "/management/features",
				icon: <Iconify icon="ant-design:gold-outlined" />,
			},
			children: [
				{
					index: true,
					element: <Navigate to="definitions" replace />,
				},
				{
					path: "definitions",
					element: <FeatureDefinitions />,
					meta: {
						label: "abp.manage.features.definitions",
						key: "/management/features/definitions",
						icon: <Iconify icon="pajamas:feature-flag" />,
					},
				},
				{
					path: "groups",
					element: <FeatureGroupDefinition />,
					meta: {
						label: "abp.manage.features.groups",
						key: "/management/features/groups",
						icon: <Iconify icon="lucide:group" />,
					},
				},
			],
		},
		{
			path: "audit-logs",
			element: <AuditingAuditLogs />,
			meta: {
				label: "abp.manage.auditLogs",
				key: "/management/audit-logs",
				icon: <Iconify icon="fluent-mdl2:compliance-audit" />,
			},
		},
		{
			path: "sys-logs",
			element: <Loggings />,
			meta: {
				label: "abp.manage.loggings",
				key: "/management/sys-logs",
				icon: <Iconify icon="icon-park-outline:log" />,
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
					element: <Navigate to="my-notifications" replace />,
				},
				{
					path: "my-notifications",
					element: <MyNotifications />,
					meta: {
						label: "abp.manage.notifications.myNotifilers",
						key: "/management/notifications/my-notifications",
						icon: <Iconify icon="ant-design:notification-outlined" />,
					},
				},
				{
					path: "groups",
					element: <NotificationsGroupDefinition />,
					meta: {
						label: "abp.manage.notifications.groups",
						key: "/management/notifications/groups",
						icon: <Iconify icon="lucide:group" />,
					},
				},
				{
					path: "definitions",
					element: <NotificationsDefinition />,
					meta: {
						label: "abp.manage.notifications.definitions",
						key: "/management/notifications/definitions",
						icon: <Iconify icon="nimbus:notification" />,
					},
				},
			],
		},
	],
};

export default management;
