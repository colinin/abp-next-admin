<script setup lang="ts">
import type { TwoFactorEnabledDto } from '../../types';
import type { UserInfo } from '../../types/user';

import { onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { Button, Card, List, Switch, Tag } from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';

defineProps<{
  userInfo: null | UserInfo;
}>();
const emits = defineEmits<{
  (event: 'changePassword'): void;
  (event: 'changePhoneNumber'): void;
  (event: 'validateEmail'): void;
}>();
const ListItem = List.Item;
const ListItemMeta = List.Item.Meta;

const { changeTwoFactorEnabledApi, getTwoFactorEnabledApi } = useProfileApi();
const twoFactor = ref<TwoFactorEnabledDto>();
const loading = ref(false);

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
            <Tag v-if="userInfo?.phoneNumberVerified" color="success">
              {{ $t('abp.account.settings.security.verified') }}
            </Tag>
            <Tag v-else color="warning">
              {{ $t('abp.account.settings.security.unVerified') }}
            </Tag>
          </template>
        </ListItemMeta>
      </ListItem>
      <!-- 邮件 -->
      <ListItem>
        <template #extra>
          <Button
            v-if="userInfo?.email && !userInfo?.emailVerified"
            type="link"
            @click="emits('validateEmail')"
          >
            {{ $t('AbpAccount.ClickToValidation') }}
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
