<script setup lang="ts">
import type { IdentitySessionDto } from '@abp/identity';

import { onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { UserSessionTable } from '@abp/identity';
import { Card, message, Modal } from 'ant-design-vue';

import { useMySessionApi } from '../../api/useMySessionApi';

const { cancel, getSessionsApi, revokeSessionApi } = useMySessionApi();

const sessions = ref<IdentitySessionDto[]>([]);

async function getSessions() {
  const { items } = await getSessionsApi();
  sessions.value = items;
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
      await getSessions();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

onMounted(getSessions);
</script>

<template>
  <Card :bordered="false" :title="$t('abp.account.settings.sessionSettings')">
    <UserSessionTable :sessions="sessions" @revoke="onRevoke" />
  </Card>
</template>

<style scoped></style>
