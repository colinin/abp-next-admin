<script setup lang="ts">
import type {
  AuthenticatorDto,
  AuthenticatorRecoveryCodeCountDto,
} from '../../types';

import { h, onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import {
  Alert,
  Button,
  Card,
  List,
  message,
  Modal,
  Skeleton,
} from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';
import AuthenticatorSteps from './AuthenticatorSteps.vue';

const ListItem = List.Item;
const ListItemMeta = List.Item.Meta;

const {
  getAuthenticatorApi,
  resetAuthenticatorApi,
  getRecoveryCodeCountApi,
  resetRecoveryCodeApi,
} = useProfileApi();

const authenticator = ref<AuthenticatorDto>();
const recoveryCodeCount = ref<AuthenticatorRecoveryCodeCountDto>();
async function onGetAuthenticator() {
  const dto = await getAuthenticatorApi();
  authenticator.value = dto;
  if (dto.isAuthenticated) {
    await onGetRecoveryCodeCount();
  }
}
async function onGetRecoveryCodeCount() {
  const dto = await getRecoveryCodeCountApi();
  recoveryCodeCount.value = dto;
}
async function onResetAuthenticator() {
  Modal.confirm({
    centered: true,
    content: $t('AbpAccount.ResetAuthenticatorWarning'),
    iconType: 'warning',
    onOk: async () => {
      await resetAuthenticatorApi();
      await onGetAuthenticator();
      message.success($t('AbpAccount.YourAuthenticatorIsSuccessfullyReset'));
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
async function onResetRecoveryCode() {
  Modal.confirm({
    centered: true,
    content: $t('AbpAccount.ResetRecoveryCodeWarning'),
    iconType: 'warning',
    onOk: async () => {
      const dto = await resetRecoveryCodeApi();
      Modal.info({
        title: $t('AbpAccount.RecoveryCode'),
        content: renderRecoveryCodes(dto.recoveryCodes),
      });
      await onGetRecoveryCodeCount();
      message.success($t('AbpAccount.YourRecoveryCodeIsSuccessfullyReset'));
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
function renderRecoveryCodes(recoveryCodes: string[]) {
  const renderColumn = (codes: string[]) =>
    h(
      'div',
      {
        class: 'm-2 text-xl font-bold text-blue-600 flex flex-col',
      },
      codes.map((code) => h('div', code)),
    );

  return h(
    'div',
    {
      class:
        'flex flex-col items-center justify-center rounded-lg bg-[#dac6c6]',
    },
    [
      h(Alert, {
        message: $t('AbpAccount.ResetRecoveryCodeDesc'),
        banner: true,
      }),
      renderColumn(recoveryCodes.slice(0, 5)),
      renderColumn(recoveryCodes.slice(5)),
    ],
  );
}
onMounted(onGetAuthenticator);
</script>

<template>
  <Card :title="$t('abp.account.settings.authenticatorSettings')">
    <AuthenticatorSteps
      v-if="authenticator?.isAuthenticated === false"
      :authenticator="authenticator"
      @done="onGetAuthenticator"
    />
    <List v-else-if="authenticator?.isAuthenticated === true">
      <ListItem>
        <template #extra>
          <Button type="primary" @click="onResetAuthenticator">
            {{ $t('AbpAccount.ResetAuthenticator') }}
          </Button>
        </template>
        <ListItemMeta
          :description="$t('AbpAccount.ResetAuthenticatorDesc')"
          :title="$t('AbpAccount.ResetAuthenticator')"
        />
      </ListItem>
      <ListItem v-if="recoveryCodeCount">
        <template #extra>
          <Button type="primary" @click="onResetRecoveryCode">
            {{ $t('AbpAccount.ResetRecoveryCode') }}
          </Button>
        </template>
        <ListItemMeta
          :description="
            $t('AbpAccount.RecoveryCodeCount', [
              recoveryCodeCount.availableNumber,
              recoveryCodeCount.recoveryCodeNumber,
            ])
          "
          :title="$t('AbpAccount.RecoveryCode')"
        />
      </ListItem>
    </List>
    <Skeleton v-else />
  </Card>
</template>

<style scoped></style>
