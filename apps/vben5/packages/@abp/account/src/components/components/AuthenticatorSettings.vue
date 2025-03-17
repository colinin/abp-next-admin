<script setup lang="ts">
import type { AuthenticatorDto } from '../../types';

import { onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { Button, Card, List, message, Modal, Skeleton } from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';
import AuthenticatorSteps from './AuthenticatorSteps.vue';

const ListItem = List.Item;
const ListItemMeta = List.Item.Meta;

const { getAuthenticatorApi, resetAuthenticatorApi } = useProfileApi();

const authenticator = ref<AuthenticatorDto>();
async function onGet() {
  const dto = await getAuthenticatorApi();
  authenticator.value = dto;
}
async function onReset() {
  Modal.confirm({
    centered: true,
    content: $t('AbpAccount.ResetAuthenticatorWarning'),
    iconType: 'warning',
    onOk: async () => {
      await resetAuthenticatorApi();
      await onGet();
      message.success($t('AbpAccount.YourAuthenticatorIsSuccessfullyReset'));
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
onMounted(onGet);
</script>

<template>
  <Card
    :bordered="false"
    :title="$t('abp.account.settings.authenticatorSettings')"
  >
    <AuthenticatorSteps
      v-if="authenticator?.isAuthenticated === false"
      :authenticator="authenticator"
      @done="onGet"
    />
    <List v-else-if="authenticator?.isAuthenticated === true">
      <ListItem>
        <template #extra>
          <Button type="primary" @click="onReset">
            {{ $t('AbpAccount.ResetAuthenticator') }}
          </Button>
        </template>
        <ListItemMeta
          :description="$t('AbpAccount.ResetAuthenticatorDesc')"
          :title="$t('AbpAccount.ResetAuthenticator')"
        />
      </ListItem>
    </List>
    <Skeleton v-else />
  </Card>
</template>

<style scoped></style>
