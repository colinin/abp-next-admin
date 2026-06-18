<script lang="ts" setup>
import type { NotificationItem } from '@vben/layouts';

import { computed, defineAsyncComponent, ref, watch } from 'vue';
import { useRouter } from 'vue-router';

import { AuthenticationLoginExpiredModal, useVbenModal } from '@vben/common-ui';
import { useWatermark } from '@vben/hooks';
import { createIconifyIcon } from '@vben/icons';
import {
  BasicLayout,
  LockScreen,
  Notification,
  UserDropdown,
} from '@vben/layouts';
import { preferences } from '@vben/preferences';
import { useAccessStore, useTabbarStore, useUserStore } from '@vben/stores';

import { useAbpStore } from '@abp/core';

import { useSessions } from '#/hooks/useSessions';
import { $t } from '#/locales';
import { useAuthStore } from '#/store';
import LoginForm from '#/views/_core/authentication/login.vue';

const UserSettingsIcon = createIconifyIcon('tdesign:user-setting');
const UserLinkIcon = createIconifyIcon('material-symbols-light:link');

const notifications = ref<NotificationItem[]>([]);

useSessions();

const router = useRouter();
const abpStore = useAbpStore();
const userStore = useUserStore();
const authStore = useAuthStore();
const accessStore = useAccessStore();
const tabbarStore = useTabbarStore();
const { destroyWatermark, updateWatermark } = useWatermark();
const showDot = computed(() =>
  notifications.value.some((item) => !item.isRead),
);

const [LinkUserModal, linkUserModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(async () => {
    const res = await import('@abp/account');
    return res.UserLinkModal;
  }),
});

const menus = computed(() => [
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
]);

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

function handleMakeAll() {
  notifications.value.forEach((item) => (item.isRead = true));
}

async function handleLinkUser(token: string) {
  const { currentUser, currentTenant } = abpStore.application!;
  const extraQueryParams: Record<string, string> = {
    LinkUserId: currentUser.id!,
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
watch(
  () => preferences.app.watermark,
  async (enable) => {
    if (enable) {
      await updateWatermark({
        content: `${userInfo.value?.username}`,
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
  <BasicLayout @clear-preferences-and-logout="handleLogout">
    <template #user-dropdown>
      <div class="flex items-center pr-[4px]">
        <UserDropdown
          :avatar
          :description="description"
          :menus
          :tag-text="userInfo?.email"
          :text="userInfo?.realName"
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
        @make-all="handleMakeAll"
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
