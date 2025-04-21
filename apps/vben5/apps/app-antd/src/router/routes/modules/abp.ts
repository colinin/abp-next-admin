import type { RouteRecordRaw } from 'vue-router';

import { $t } from '#/locales';

const routes: RouteRecordRaw[] = [
  {
    meta: {
      icon: 'https://abp.io/assets/favicon.ico/favicon-16x16.png',
      keepAlive: true,
      order: 1000,
      title: $t('abp.title'),
    },
    name: 'AbpFramework',
    path: '/abp',
    children: [
      {
        meta: {
          title: $t('abp.manage.title'),
          icon: 'arcticons:activity-manager',
        },
        name: 'Manage',
        path: '/manage',
        children: [
          {
            meta: {
              title: $t('abp.manage.identity.title'),
              icon: 'teenyicons:id-outline',
            },
            name: 'Identity',
            path: '/manage/identity',
            children: [
              {
                component: () => import('#/views/identity/users/index.vue'),
                name: 'Users',
                path: '/manage/identity/users',
                meta: {
                  title: $t('abp.manage.identity.user'),
                  icon: 'mdi:user-outline',
                },
              },
              {
                component: () => import('#/views/identity/roles/index.vue'),
                name: 'Roles',
                path: '/manage/identity/roles',
                meta: {
                  title: $t('abp.manage.identity.role'),
                  icon: 'carbon:user-role',
                },
              },
              {
                component: () =>
                  import('#/views/identity/claim-types/index.vue'),
                name: 'ClaimTypes',
                path: '/manage/identity/claim-types',
                meta: {
                  title: $t('abp.manage.identity.claimTypes'),
                  icon: 'la:id-card-solid',
                },
              },
              {
                component: () =>
                  import('#/views/identity/security-logs/index.vue'),
                name: 'SecurityLogs',
                path: '/manage/identity/security-logs',
                meta: {
                  title: $t('abp.manage.identity.securityLogs'),
                  icon: 'carbon:security',
                },
              },
              {
                component: () =>
                  import('#/views/identity/organization-units/index.vue'),
                name: 'OrganizationUnits',
                path: '/manage/identity/organization-units',
                meta: {
                  title: $t('abp.manage.identity.organizationUnits'),
                  icon: 'clarity:organization-line',
                },
              },
              {
                component: () => import('#/views/identity/sessions/index.vue'),
                name: 'IdentitySessions',
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
            name: 'PermissionManagement',
            path: '/manage/permissions',
            children: [
              {
                meta: {
                  title: $t('abp.manage.permissions.groups'),
                  icon: 'lucide:group',
                },
                name: 'PermissionGroupDefinitions',
                path: '/manage/permissions/groups',
                component: () => import('#/views/permissions/groups/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.permissions.definitions'),
                  icon: 'icon-park-outline:permissions',
                },
                name: 'PermissionDefinitions',
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
            name: 'FeatureManagement',
            path: '/manage/features',
            children: [
              {
                meta: {
                  title: $t('abp.manage.features.groups'),
                  icon: 'lucide:group',
                },
                name: 'FeatureGroupDefinitions',
                path: '/manage/features/groups',
                component: () => import('#/views/features/groups/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.features.definitions'),
                  icon: 'pajamas:feature-flag',
                },
                name: 'FeatureDefinitions',
                path: '/manage/features/definitions',
                component: () =>
                  import('#/views/features/definitions/index.vue'),
              },
            ],
          },
          {
            meta: {
              title: $t('abp.manage.settings.title'),
              icon: 'ic:outline-settings',
            },
            name: 'SettingManagement',
            path: '/manage/settings',
            children: [
              {
                meta: {
                  title: $t('abp.manage.settings.definitions'),
                  icon: 'codicon:settings',
                },
                name: 'SettingDefinitions',
                path: '/manage/settings/definitions',
                component: () =>
                  import('#/views/settings/definitions/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.settings.system'),
                  icon: 'tabler:settings-cog',
                },
                name: 'SystemSettings',
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
            name: 'LocalizationManagement',
            path: '/manage/localization',
            children: [
              {
                meta: {
                  title: $t('abp.manage.localization.resources'),
                  icon: 'grommet-icons:resources',
                },
                name: 'LocalizationResources',
                path: '/manage/localization/resources',
                component: () =>
                  import('#/views/localization/resources/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.localization.languages'),
                  icon: 'cil:language',
                },
                name: 'LocalizationLanguages',
                path: '/manage/localization/languages',
                component: () =>
                  import('#/views/localization/languages/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.localization.texts'),
                  icon: 'mi:text',
                },
                name: 'LocalizationTexts',
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
            name: 'DataProtectionManagement',
            path: '/manage/data-protection',
            children: [
              {
                meta: {
                  title: $t('abp.manage.dataProtection.entityTypeInfos'),
                  icon: 'iconamoon:type',
                  keepAlive: true,
                },
                name: 'EntityTypeInfos',
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
            name: 'AuditingAuditLogs',
            path: '/manage/audit-logs',
            component: () => import('#/views/auditing/audit-logs/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.loggings'),
              icon: 'icon-park-outline:log',
            },
            name: 'AuditingLoggings',
            path: '/manage/sys-logs',
            component: () => import('#/views/auditing/loggings/index.vue'),
          },
          {
            meta: {
              title: $t('abp.manage.notifications.title'),
              icon: 'tabler:notification',
            },
            name: 'Notifications',
            path: '/manage/notifications',
            children: [
              {
                meta: {
                  title: $t('abp.manage.notifications.myNotifilers'),
                  icon: 'ant-design:notification-outlined',
                },
                name: 'MyNotifications',
                path: '/manage/notifications/my-notifilers',
                component: () =>
                  import('#/views/notifications/my-notifilers/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.notifications.groups'),
                  icon: 'lucide:group',
                },
                name: 'NotificationGroupDefinitions',
                path: '/manage/notifications/groups',
                component: () =>
                  import('#/views/notifications/groups/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.manage.notifications.definitions'),
                  icon: 'nimbus:notification',
                },
                name: 'NotificationDefinitions',
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
        name: 'Saas',
        path: '/saas',
        children: [
          {
            meta: {
              title: $t('abp.saas.editions'),
              icon: 'icon-park-outline:multi-rectangle',
            },
            name: 'SaasEditions',
            path: '/saas/editions',
            component: () => import('#/views/saas/editions/index.vue'),
          },
          {
            meta: {
              title: $t('abp.saas.tenants'),
              icon: 'arcticons:tenantcloud-pro',
            },
            name: 'SaasTenants',
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
        name: 'OpenIddict',
        path: '/openiddict',
        children: [
          {
            meta: {
              title: $t('abp.openiddict.applications'),
              icon: 'carbon:application',
            },
            name: 'OpenIddictApplications',
            path: '/openiddict/applications',
            component: () =>
              import('#/views/openiddict/applications/index.vue'),
          },
          {
            meta: {
              title: $t('abp.openiddict.authorizations'),
              icon: 'arcticons:ente-authenticator',
            },
            name: 'OpenIddictAuthorizations',
            path: '/openiddict/authorizations',
            component: () =>
              import('#/views/openiddict/authorizations/index.vue'),
          },
          {
            meta: {
              title: $t('abp.openiddict.scopes'),
              icon: 'et:scope',
            },
            name: 'OpenIddictScopes',
            path: '/openiddict/scopes',
            component: () => import('#/views/openiddict/scopes/index.vue'),
          },
          {
            meta: {
              title: $t('abp.openiddict.tokens'),
              icon: 'oui:token-key',
            },
            name: 'OpenIddictTokens',
            path: '/openiddict/tokens',
            component: () => import('#/views/openiddict/tokens/index.vue'),
          },
        ],
      },
      {
        name: 'Account',
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
            name: 'MySettings',
            path: '/account/my-settings',
            component: () => import('#/views/account/my-settings/index.vue'),
          },
        ],
      },
      {
        name: 'Platform',
        path: '/platform',
        meta: {
          title: $t('abp.platform.title'),
          icon: 'ep:platform',
        },
        children: [
          {
            meta: {
              title: $t('abp.platform.messages.title'),
              icon: 'tabler:message-cog',
            },
            name: 'PlatformMessages',
            path: '/platform/messages',
            children: [
              {
                meta: {
                  title: $t('abp.platform.messages.email'),
                  icon: 'material-symbols:attach-email-outline',
                },
                name: 'PlatformEmailMessages',
                path: '/platform/messages/email',
                component: () =>
                  import('#/views/platform/messages/email/index.vue'),
              },
              {
                meta: {
                  title: $t('abp.platform.messages.sms'),
                  icon: 'material-symbols:sms-outline',
                },
                name: 'PlatformSmsMessages',
                path: '/platform/messages/sms',
                component: () =>
                  import('#/views/platform/messages/sms/index.vue'),
              },
            ],
          },
        ],
      },
      {
        name: 'TaskManagement',
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
            name: 'TaskManagementJobInfos',
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
        name: 'WebhooksManagement',
        path: '/webhooks',
        children: [
          {
            meta: {
              title: $t('abp.webhooks.groups'),
              icon: 'lucide:group',
            },
            name: 'WebhookGroupDefinitions',
            path: '/webhooks/groups',
            component: () => import('#/views/webhooks/groups/index.vue'),
          },
        ],
      },
      {
        name: 'AbpDemo',
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
            name: 'DemoBooks',
            path: '/abp/demos/books',
            component: () => import('#/views/demos/books/index.vue'),
          },
        ],
      },
    ],
  },
];

export default routes;
