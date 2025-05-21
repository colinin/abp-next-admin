import type { RouteRecordRaw } from 'vue-router';

import { $t } from '#/locales';

const routes: RouteRecordRaw[] = [
  {
    meta: {
      title: $t('abp.manage.title'),
      icon: 'arcticons:activity-manager',
    },
    name: 'Vben5Manage',
    path: '/manage',
    children: [
      {
        meta: {
          title: $t('abp.manage.identity.title'),
          icon: 'teenyicons:id-outline',
        },
        name: 'Vben5Identity',
        path: '/manage/identity',
        children: [
          {
            component: () => import('#/views/identity/users/index.vue'),
            name: 'Vben5IdentityUsers',
            path: '/manage/identity/users',
            meta: {
              title: $t('abp.manage.identity.user'),
              icon: 'mdi:user-outline',
            },
          },
          {
            component: () => import('#/views/identity/roles/index.vue'),
            name: 'Vben5IdentityRoles',
            path: '/manage/identity/roles',
            meta: {
              title: $t('abp.manage.identity.role'),
              icon: 'carbon:user-role',
            },
          },
          {
            component: () => import('#/views/identity/claim-types/index.vue'),
            name: 'Vben5IdentityClaimTypes',
            path: '/manage/identity/claim-types',
            meta: {
              title: $t('abp.manage.identity.claimTypes'),
              icon: 'la:id-card-solid',
            },
          },
          {
            component: () => import('#/views/identity/security-logs/index.vue'),
            name: 'Vben5IdentitySecurityLogs',
            path: '/manage/identity/security-logs',
            meta: {
              title: $t('abp.manage.identity.securityLogs'),
              icon: 'carbon:security',
            },
          },
          {
            component: () =>
              import('#/views/identity/organization-units/index.vue'),
            name: 'Vben5IdentityOrganizationUnits',
            path: '/manage/identity/organization-units',
            meta: {
              title: $t('abp.manage.identity.organizationUnits'),
              icon: 'clarity:organization-line',
            },
          },
          {
            component: () => import('#/views/identity/sessions/index.vue'),
            name: 'Vben5IdentitySessions',
            path: '/manage/identity/sessions',
            meta: {
              title: $t('abp.manage.identity.sessions'),
              icon: 'carbon:prompt-session',
            },
          },
        ],
      },
      {
        meta: {
          title: $t('abp.manage.permissions.title'),
          icon: 'arcticons:permissionsmanager',
        },
        name: 'Vben5Permissions',
        path: '/manage/permissions',
        children: [
          {
            meta: {
              title: $t('abp.manage.permissions.groups'),
              icon: 'lucide:group',
            },
            name: 'Vben5PermissionsGroupDefinitions',
            path: '/manage/permissions/groups',
            component: () => import('#/views/permissions/groups/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.permissions.definitions'),
              icon: 'icon-park-outline:permissions',
            },
            name: 'Vben5PermissionsDefinitions',
            path: '/manage/permissions/definitions',
            component: () =>
              import('#/views/permissions/definitions/index.vue'),
          },
        ],
      },
      {
        meta: {
          title: $t('abp.manage.features.title'),
          icon: 'ant-design:gold-outlined',
        },
        name: 'Vben5Features',
        path: '/manage/features',
        children: [
          {
            meta: {
              title: $t('abp.manage.features.groups'),
              icon: 'lucide:group',
            },
            name: 'Vben5FeaturesGroupDefinitions',
            path: '/manage/features/groups',
            component: () => import('#/views/features/groups/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.features.definitions'),
              icon: 'pajamas:feature-flag',
            },
            name: 'Vben5FeaturesDefinitions',
            path: '/manage/features/definitions',
            component: () => import('#/views/features/definitions/index.vue'),
          },
        ],
      },
      {
        meta: {
          title: $t('abp.manage.settings.title'),
          icon: 'ic:outline-settings',
        },
        name: 'Vben5Settings',
        path: '/manage/settings',
        children: [
          {
            meta: {
              title: $t('abp.manage.settings.definitions'),
              icon: 'codicon:settings',
            },
            name: 'Vben5SettingsDefinitions',
            path: '/manage/settings/definitions',
            component: () => import('#/views/settings/definitions/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.settings.system'),
              icon: 'tabler:settings-cog',
            },
            name: 'Vben5SettingsSystem',
            path: '/manage/settings/system',
            component: () => import('#/views/settings/system/index.vue'),
          },
        ],
      },
      {
        meta: {
          title: $t('abp.manage.localization.title'),
          icon: 'ion:globe-outline',
        },
        name: 'Vben5Localizations',
        path: '/manage/localization',
        children: [
          {
            meta: {
              title: $t('abp.manage.localization.resources'),
              icon: 'grommet-icons:resources',
            },
            name: 'Vben5LocalizationsResources',
            path: '/manage/localization/resources',
            component: () => import('#/views/localization/resources/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.localization.languages'),
              icon: 'cil:language',
            },
            name: 'Vben5LocalizationsLanguages',
            path: '/manage/localization/languages',
            component: () => import('#/views/localization/languages/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.localization.texts'),
              icon: 'mi:text',
            },
            name: 'Vben5LocalizationsTexts',
            path: '/manage/localization/texts',
            component: () => import('#/views/localization/texts/index.vue'),
          },
        ],
      },
      {
        meta: {
          title: $t('abp.manage.dataProtection.title'),
          icon: 'icon-park-outline:protect',
        },
        name: 'Vben5DataProtection',
        path: '/manage/data-protection',
        children: [
          {
            meta: {
              title: $t('abp.manage.dataProtection.entityTypeInfos'),
              icon: 'iconamoon:type',
              keepAlive: true,
            },
            name: 'Vben5DataProtectionEntityTypeInfos',
            path: '/manage/data-protection/entity-type-infos',
            component: () =>
              import('#/views/data-protection/entity-type-infos/index.vue'),
          },
        ],
      },
      {
        meta: {
          title: $t('abp.manage.auditLogs'),
          icon: 'fluent-mdl2:compliance-audit',
        },
        name: 'Vben5AuditingAuditLogs',
        path: '/manage/audit-logs',
        component: () => import('#/views/auditing/audit-logs/index.vue'),
      },
      {
        meta: {
          title: $t('abp.manage.loggings'),
          icon: 'icon-park-outline:log',
        },
        name: 'Vben5AuditingLoggings',
        path: '/manage/sys-logs',
        component: () => import('#/views/auditing/loggings/index.vue'),
      },
      {
        meta: {
          title: $t('abp.manage.notifications.title'),
          icon: 'tabler:notification',
        },
        name: 'Vben5Notifications',
        path: '/manage/notifications',
        children: [
          {
            meta: {
              title: $t('abp.manage.notifications.myNotifilers'),
              icon: 'ant-design:notification-outlined',
            },
            name: 'Vben5NotificationsMyNotifilers',
            path: '/manage/notifications/my-notifilers',
            component: () =>
              import('#/views/notifications/my-notifilers/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.notifications.groups'),
              icon: 'lucide:group',
            },
            name: 'Vben5NotificationsGroupDefinitions',
            path: '/manage/notifications/groups',
            component: () => import('#/views/notifications/groups/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.notifications.definitions'),
              icon: 'nimbus:notification',
            },
            name: 'Vben5NotificationsDefinitions',
            path: '/manage/notifications/definitions',
            component: () =>
              import('#/views/notifications/definitions/index.vue'),
          },
        ],
      },
    ],
  },
  {
    meta: {
      title: $t('abp.saas.title'),
      icon: 'ant-design:cloud-server-outlined',
    },
    name: 'Vben5Saas',
    path: '/saas',
    children: [
      {
        meta: {
          title: $t('abp.saas.editions'),
          icon: 'icon-park-outline:multi-rectangle',
        },
        name: 'Vben5SaasEditions',
        path: '/saas/editions',
        component: () => import('#/views/saas/editions/index.vue'),
      },
      {
        meta: {
          title: $t('abp.saas.tenants'),
          icon: 'arcticons:tenantcloud-pro',
        },
        name: 'Vben5SaasTenants',
        path: '/saas/tenants',
        component: () => import('#/views/saas/tenants/index.vue'),
      },
    ],
  },
  {
    meta: {
      title: $t('abp.openiddict.title'),
      icon: 'mdi:openid',
    },
    name: 'Vben5OpenIddict',
    path: '/openiddict',
    children: [
      {
        meta: {
          title: $t('abp.openiddict.applications'),
          icon: 'carbon:application',
        },
        name: 'Vben5OpenIddictApplications',
        path: '/openiddict/applications',
        component: () => import('#/views/openiddict/applications/index.vue'),
      },
      {
        meta: {
          title: $t('abp.openiddict.authorizations'),
          icon: 'arcticons:ente-authenticator',
        },
        name: 'Vben5OpenIddictAuthorizations',
        path: '/openiddict/authorizations',
        component: () => import('#/views/openiddict/authorizations/index.vue'),
      },
      {
        meta: {
          title: $t('abp.openiddict.scopes'),
          icon: 'et:scope',
        },
        name: 'Vben5OpenIddictScopes',
        path: '/openiddict/scopes',
        component: () => import('#/views/openiddict/scopes/index.vue'),
      },
      {
        meta: {
          title: $t('abp.openiddict.tokens'),
          icon: 'oui:token-key',
        },
        name: 'Vben5OpenIddictTokens',
        path: '/openiddict/tokens',
        component: () => import('#/views/openiddict/tokens/index.vue'),
      },
    ],
  },
  {
    name: 'Vben5Account',
    path: '/account',
    meta: {
      title: $t('abp.account.title'),
      icon: 'mdi:account-outline',
      hideInMenu: true,
    },
    children: [
      {
        meta: {
          title: $t('abp.account.settings.title'),
          icon: 'tdesign:user-setting',
        },
        name: 'Vben5AccountMySettings',
        path: '/account/my-settings',
        component: () => import('#/views/account/my-settings/index.vue'),
      },
    ],
  },
  {
    name: 'Vben5Platform',
    path: '/platform',
    meta: {
      title: $t('abp.platform.title'),
      icon: 'ep:platform',
    },
    children: [
      {
        meta: {
          title: $t('abp.platform.dataDictionaries'),
          icon: 'material-symbols:dictionary-outline',
        },
        name: 'Vben5PlatformDataDictionaries',
        path: '/platform/data-dictionaries',
        component: () => import('#/views/platform/data-dictionaries/index.vue'),
      },
      {
        meta: {
          title: $t('abp.platform.layouts'),
          icon: 'material-symbols-light:responsive-layout',
        },
        name: 'Vben5PlatformLayouts',
        path: '/platform/layouts',
        component: () => import('#/views/platform/layouts/index.vue'),
      },
      {
        meta: {
          title: $t('abp.platform.menus'),
          icon: 'material-symbols-light:menu',
        },
        name: 'Vben5PlatformMenus',
        path: '/platform/menus',
        component: () => import('#/views/platform/menus/index.vue'),
      },
      {
        meta: {
          title: $t('abp.platform.messages.title'),
          icon: 'tabler:message-cog',
        },
        name: 'Vben5PlatformMessages',
        path: '/platform/messages',
        children: [
          {
            meta: {
              title: $t('abp.platform.messages.email'),
              icon: 'material-symbols:attach-email-outline',
            },
            name: 'Vben5PlatformEmailMessages',
            path: '/platform/messages/email',
            component: () =>
              import('#/views/platform/messages/email/index.vue'),
          },
          {
            meta: {
              title: $t('abp.platform.messages.sms'),
              icon: 'material-symbols:sms-outline',
            },
            name: 'Vben5PlatformSmsMessages',
            path: '/platform/messages/sms',
            component: () => import('#/views/platform/messages/sms/index.vue'),
          },
        ],
      },
    ],
  },
  {
    name: 'Vben5Tasks',
    path: '/task-management',
    meta: {
      title: $t('abp.tasks.title'),
      icon: 'eos-icons:background-tasks',
    },
    children: [
      {
        meta: {
          title: $t('abp.tasks.jobInfo.title'),
          icon: 'eos-icons:job',
        },
        name: 'Vben5TasksJobInfos',
        path: '/task-management/background-jobs',
        component: () => import('#/views/tasks/job-infos/index.vue'),
      },
    ],
  },
  {
    meta: {
      title: $t('abp.webhooks.title'),
      icon: 'material-symbols:webhook',
    },
    name: 'Vben5Webhooks',
    path: '/webhooks',
    children: [
      {
        meta: {
          title: $t('abp.webhooks.groups'),
          icon: 'lucide:group',
        },
        name: 'Vben5WebhooksGroupDefinitions',
        path: '/webhooks/groups',
        component: () => import('#/views/webhooks/groups/index.vue'),
      },
      {
        meta: {
          title: $t('abp.webhooks.definitions'),
          icon: 'material-symbols:webhook',
        },
        name: 'Vben5WebhooksDefinitions',
        path: '/webhooks/definitions',
        component: () => import('#/views/webhooks/definitions/index.vue'),
      },
      {
        meta: {
          title: $t('abp.webhooks.subscriptions'),
          icon: 'material-symbols:subscriptions',
        },
        name: 'Vben5WebhooksSubscriptions',
        path: '/webhooks/subscriptions',
        component: () => import('#/views/webhooks/subscriptions/index.vue'),
      },
      {
        meta: {
          title: $t('abp.webhooks.sendAttempts'),
          icon: 'material-symbols:history',
        },
        name: 'Vben5WebhooksSendAttempts',
        path: '/webhooks/send-attempts',
        component: () => import('#/views/webhooks/send-attempts/index.vue'),
      },
    ],
  },
  {
    meta: {
      title: $t('abp.textTemplating.title'),
      icon: 'tdesign:template',
    },
    name: 'Vben5TextTemplating',
    path: '/text-templating',
    children: [
      {
        meta: {
          title: $t('abp.textTemplating.definitions'),
          icon: 'qlementine-icons:template-16',
        },
        name: 'Vben5TextTemplatingDefinitions',
        path: '/text-templating/definitions',
        component: () =>
          import('#/views/text-templating/definitions/index.vue'),
      },
    ],
  },
  {
    name: 'Vben5AbpDemo',
    path: '/abp/demos',
    meta: {
      title: $t('abp.demo.title'),
      icon: 'carbon:demo',
    },
    children: [
      {
        meta: {
          title: $t('abp.demo.books'),
          icon: 'mingcute:book-line',
        },
        name: 'Vben5AbpDemoBooks',
        path: '/abp/demos/books',
        component: () => import('#/views/demos/books/index.vue'),
      },
    ],
  },
];

export default routes;
