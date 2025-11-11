<script lang="ts" setup>
import { useRouter } from 'vue-router';

import { Fallback, VbenButton } from '@vben/common-ui';
import { ArrowLeft, LogOut } from '@vben/icons';
import { $t } from '@vben/locales';
import { preferences } from '@vben/preferences';

import { Modal } from 'ant-design-vue';

import { useAuthStore } from '#/store';

defineOptions({ name: 'Fallback404Demo' });

const authStore = useAuthStore();
const { push } = useRouter();

// 返回首页
function back() {
  push(preferences.app.defaultHomePath);
}

// 退出登录
function logout() {
  Modal.confirm({
    centered: true,
    title: $t('common.logout'),
    content: $t('ui.widgets.logoutTip'),
    async onOk() {
      await authStore.logout();
    },
  });
}
</script>

<template>
  <Fallback status="404">
    <template #action>
      <div class="flex gap-2">
        <VbenButton size="lg" @click="back">
          <ArrowLeft class="mr-2 size-4" />
          {{ $t('common.backToHome') }}
        </VbenButton>
        <VbenButton size="lg" variant="destructive" @click="logout">
          <LogOut class="mr-2 size-4" />
          {{ $t('common.logout') }}
        </VbenButton>
      </div>
    </template>
  </Fallback>
</template>
