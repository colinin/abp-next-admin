<script lang="ts" setup>
import type { NotificationItem } from '@vben/layouts';

import { computed, defineAsyncComponent, ref, watch } from 'vue';
import { useRouter } from 'vue-router';

import {
  AuthenticationLoginExpiredModal,
  confirm,
  useVbenModal,
} from '@vben/common-ui';
import { useWatermark } from '@vben/hooks';
import { createIconifyIcon } from '@vben/icons';
import {
  BasicLayout,
  LockScreen,
  Notification,
  UserDropdown,
} from '@vben/layouts';
import { preferences, usePreferences } from '@vben/preferences';
import { useAccessStore, useTabbarStore, useUserStore } from '@vben/stores';

import { useAbpStore } from '@abp/core';

import { useSessions } from '#/hooks/useSessions';
import { $t } from '#/locales';
import { useAuthStore } from '#/store';
import LoginForm from '#/views/_core/authentication/login.vue';

const UserSettingsIcon = createIconifyIcon('tdesign:user-setting');
const UserLinkIcon = createIconifyIcon('material-symbols:link');
const BckLoginIcon = createIconifyIcon('mingcute:back-line');

const notifications = ref<NotificationItem[]>([]);

useSessions();

const router = useRouter();
const abpStore = useAbpStore();
const userStore = useUserStore();
const authStore = useAuthStore();
const accessStore = useAccessStore();
const tabbarStore = useTabbarStore();
const { destroyWatermark, updateWatermark } = useWatermark();
const { isDark } = usePreferences();
const showDot = computed(() =>
  notifications.value.some((item) => !item.isRead),
);

const [LinkUserModal, linkUserModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(async () => {
    const res = await import('@abp/account');
    return res.UserLinkModal;
  }),
});

const menus = computed(() => {
  const basicMenus = [
    {
      handler: () => {
        linkUserModalApi.open();
      },
      icon: UserLinkIcon,
      text: $t('abp.account.linkAccount'),
    },
    {
      handler: () => {
        router.replace('/account/my-settings');
      },
      icon: UserSettingsIcon,
      text: $t('abp.account.settings.title'),
    },
  ];
  if (abpStore.application?.currentUser.impersonatorUserId) {
    basicMenus.push({
      handler: handleBackUserLogin,
      icon: BckLoginIcon,
      text: $t('abp.account.revertImpersonation'),
    });
  }
  return basicMenus;
});

const userInfo = computed(() => {
  return userStore.userInfo;
});

const description = computed(() => {
  if (abpStore.application?.currentTenant.name && userInfo.value?.username) {
    return `${abpStore.application.currentTenant.name}/${userInfo.value.username}`;
  }
  return userInfo.value?.username;
});

const avatar = computed(() => {
  return userInfo.value?.avatar ?? preferences.app.defaultAvatar;
});

async function handleLogout() {
  await authStore.logout(false);
}

function handleNoticeClear() {
  notifications.value = [];
}

function markRead(id: number | string) {
  const item = notifications.value.find((item) => item.id === id);
  if (item) {
    item.isRead = true;
  }
}

function remove(id: number | string) {
  notifications.value = notifications.value.filter((item) => item.id !== id);
}

function handleMakeAll() {
  notifications.value.forEach((item) => (item.isRead = true));
}

async function handleLinkUser(token: string) {
  if (!abpStore.application) return;
  const { currentUser, currentTenant } = abpStore.application;
  if (!currentUser.id) return;
  const extraQueryParams: Record<string, string> = {
    LinkUserId: currentUser.id,
    LinkToken: token,
  };
  if (currentTenant.id) {
    extraQueryParams.LinkTenantId = currentTenant.id;
  }
  // 跳转登录页,交由后端处理
  await authStore.oidcLogin({
    prompt: 'login',
    extraQueryParams,
  });
}

async function handleLinkLogin(userId: string, tenantId?: string) {
  await authStore.linkUseLogin(userId, tenantId, () => {
    tabbarStore.closeAllTabs(router);
    window.location.replace('/');
  });
}

async function handleBackUserLogin() {
  confirm({
    beforeClose: async (scope) => {
      if (scope.isConfirm) {
        await authStore.impersonationUserLogin({}, () => {
          tabbarStore.closeAllTabs(router);
          window.location.replace('/');
        });
      }
      return true;
    },
    centered: true,
    content: $t('AbpAccount.RevertImpersonationWarning'),
    title: $t('AbpUi.AreYouSure'),
  });
}

const viewAll = () => {};

const handleClick = (item: NotificationItem) => {
  // 如果通知项有链接，点击时跳转
  if (item.link) {
    navigateTo(item.link, item.query, item.state);
  }
};

function navigateTo(
  link: string,
  query?: Record<string, any>,
  state?: Record<string, any>,
) {
  if (link.startsWith('http://') || link.startsWith('https://')) {
    // 外部链接，在新标签页打开
    window.open(link, '_blank');
  } else {
    // 内部路由链接，支持 query 参数和 state
    router.push({
      path: link,
      query: query || {},
      state,
    });
  }
}

watch(
  () => ({
    enable: preferences.app.watermark,
    content: preferences.app.watermarkContent,
    isDark: isDark.value,
  }),
  async ({ enable, content, isDark: isDarkValue }) => {
    if (enable) {
      const watermarkColor = isDarkValue
        ? 'rgba(255, 255, 255, 0.12)'
        : 'rgba(0, 0, 0, 0.12)';

      await updateWatermark({
        advancedStyle: {
          colorStops: [
            {
              color: watermarkColor,
              offset: 0,
            },
            {
              color: watermarkColor,
              offset: 1,
            },
          ],
          type: 'linear',
        },
        content:
          content ||
          `${userStore.userInfo?.username} - ${userStore.userInfo?.realName}`,
      });
    } else {
      destroyWatermark();
    }
  },
  {
    immediate: true,
  },
);
</script>

<template>
  <BasicLayout
    :avatar
    :text="userStore.userInfo?.realName"
    @clear-preferences-and-logout="handleLogout"
    @logout="handleLogout"
  >
    <template #user-dropdown>
      <div class="flex items-center pr-[4px]">
        <UserDropdown
          :avatar
          :menus
          :text="userInfo?.realName"
          :description="description"
          :tag-text="userInfo?.email"
          @clear-preferences-and-logout="handleLogout"
          @logout="handleLogout"
        />
        <span class="mb-1 flex items-center text-sm font-medium">
          {{ description }}
        </span>
      </div>
      <LinkUserModal @link="handleLinkUser" @login="handleLinkLogin" />
    </template>
    <template #notification>
      <Notification
        :dot="showDot"
        :notifications="notifications"
        @clear="handleNoticeClear"
        @read="(item) => item.id && markRead(item.id)"
        @remove="(item) => item.id && remove(item.id)"
        @make-all="handleMakeAll"
        @on-click="handleClick"
        @view-all="viewAll"
      />
    </template>
    <template #extra>
      <AuthenticationLoginExpiredModal
        v-model:open="accessStore.loginExpired"
        :avatar
      >
        <LoginForm />
      </AuthenticationLoginExpiredModal>
    </template>
    <template #lock-screen>
      <LockScreen :avatar @to-login="handleLogout" />
    </template>
  </BasicLayout>
</template>
