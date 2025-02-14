<script setup lang="ts">
import type { TwoFactorEnabledDto } from '../../types';
import type { UserInfo } from '../../types/user';

import { computed, onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { Button, Card, List, Switch, Tag } from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';

defineProps<{
  userInfo: null | UserInfo;
}>();
const emits = defineEmits<{
  (event: 'changePassword'): void;
  (event: 'changePhoneNumber'): void;
}>();
const ListItem = List.Item;
const ListItemMeta = List.Item.Meta;

const {
  changeTwoFactorEnabledApi,
  getTwoFactorEnabledApi,
  sendEmailConfirmLinkApi,
} = useProfileApi();
const twoFactor = ref<TwoFactorEnabledDto>();
const loading = ref(false);

const sendMailInternal = ref(0);
const getSendMailLoading = computed(() => {
  return sendMailInternal.value > 0;
});
const getSendMailTitle = computed(() => {
  if (sendMailInternal.value > 0) {
    return `${sendMailInternal.value} s`;
  }
  return $t('AbpAccount.ClickToValidation');
});

async function onGet() {
  const dto = await getTwoFactorEnabledApi();
  twoFactor.value = dto;
}
async function onTwoFactorChange(enabled: boolean) {
  try {
    loading.value = true;
    await changeTwoFactorEnabledApi({ enabled });
  } catch {
    twoFactor.value!.enabled = !enabled;
  } finally {
    loading.value = false;
  }
}
async function onValidateEmail(email: string) {
  sendMailInternal.value = 60;
  try {
    await sendEmailConfirmLinkApi({
      appName: 'VueVben5',
      email,
      returnUrl: window.location.href,
    });
    setInterval(() => {
      if (sendMailInternal.value <= 0) {
        return;
      }
      sendMailInternal.value -= 1;
    }, 1000);
  } catch {
    sendMailInternal.value = 0;
  }
}
onMounted(onGet);
</script>

<template>
  <Card :bordered="false" :title="$t('abp.account.settings.security.title')">
    <List item-layout="horizontal">
      <!-- 密码 -->
      <ListItem>
        <template #extra>
          <Button type="link" @click="emits('changePassword')">
            {{ $t('AbpUi.Edit') }}
          </Button>
        </template>
        <ListItemMeta
          :description="$t('abp.account.settings.security.passwordDesc')"
        >
          <template #title>
            <a href="https://www.antdv.com/">{{
              $t('abp.account.settings.security.password')
            }}</a>
          </template>
        </ListItemMeta>
      </ListItem>
      <!-- 手机号码 -->
      <ListItem>
        <template #extra>
          <Button type="link" @click="emits('changePhoneNumber')">
            {{ $t('AbpUi.Edit') }}
          </Button>
        </template>
        <ListItemMeta>
          <template #title>
            {{ $t('abp.account.settings.security.phoneNumber') }}
          </template>
          <template #description>
            {{ userInfo?.phoneNumber }}
            <template v-if="!userInfo?.phoneNumber">
              <Tag color="warning">
                {{ $t('abp.account.settings.security.unSet') }}
              </Tag>
            </template>
            <template v-else>
              <Tag v-if="userInfo?.phoneNumberVerified" color="success">
                {{ $t('abp.account.settings.security.verified') }}
              </Tag>
              <Tag v-else color="warning">
                {{ $t('abp.account.settings.security.unVerified') }}
              </Tag>
            </template>
          </template>
        </ListItemMeta>
      </ListItem>
      <!-- 邮件 -->
      <ListItem>
        <template #extra>
          <Button
            v-if="userInfo?.email && !userInfo?.emailVerified"
            :disabled="getSendMailLoading"
            type="link"
            @click="onValidateEmail(userInfo.email)"
          >
            {{ getSendMailTitle }}
          </Button>
        </template>
        <ListItemMeta>
          <template #title>
            {{ $t('abp.account.settings.security.email') }}
          </template>
          <template #description>
            {{ userInfo?.email }}
            <Tag v-if="userInfo?.emailVerified" color="success">
              {{ $t('abp.account.settings.security.verified') }}
            </Tag>
            <Tag v-else color="warning">
              {{ $t('abp.account.settings.security.unVerified') }}
            </Tag>
          </template>
        </ListItemMeta>
      </ListItem>
      <!-- 二次认证 -->
      <ListItem v-if="twoFactor">
        <template #extra>
          <Switch
            v-model:checked="twoFactor.enabled"
            :loading="loading"
            @change="(checked) => onTwoFactorChange(Boolean(checked))"
          />
        </template>
        <ListItemMeta
          :description="$t('AbpAccount.TwoFactor')"
          :title="$t('AbpAccount.TwoFactor')"
        />
      </ListItem>
    </List>
  </Card>
</template>

<style scoped></style>
