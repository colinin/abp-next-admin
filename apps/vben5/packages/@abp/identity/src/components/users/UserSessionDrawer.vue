<script setup lang="ts">
import type { IdentitySessionDto, IdentityUserDto } from '../../types';

import { defineAsyncComponent, ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message, Modal } from 'ant-design-vue';

import { useUserSessionsApi } from '../../api/useUserSessionsApi';

const SessionTable = defineAsyncComponent(
  () => import('../sessions/SessionTable.vue'),
);

const sessions = ref<IdentitySessionDto[]>([]);

const { cancel, getSessionsApi, revokeSessionApi } = useUserSessionsApi();

const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-[800px]',
  onBeforeClose: cancel,
  onCancel() {
    drawerApi.close();
  },
  onConfirm: async () => {},
  onOpenChange: async (isOpen: boolean) => {
    isOpen && (await onRefresh());
  },
  title: $t('AbpIdentity.IdentitySessions'),
});
async function onRefresh() {
  try {
    drawerApi.setState({ loading: true });
    const dto = drawerApi.getData<IdentityUserDto>();
    const { items } = await getSessionsApi({ userId: dto.id });
    sessions.value = items;
  } finally {
    drawerApi.setState({ loading: false });
  }
}
async function onRevoke(session: IdentitySessionDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.SessionWillBeRevokedMessage'),
    iconType: 'warning',
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await revokeSessionApi(session.sessionId);
      message.success($t('AbpIdentity.SuccessfullyRevoked'));
      await onRefresh();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Drawer>
    <SessionTable :sessions="sessions" @revoke="onRevoke" />
  </Drawer>
</template>

<style scoped></style>
