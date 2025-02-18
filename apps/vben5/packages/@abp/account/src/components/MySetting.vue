<script setup lang="ts">
import type { ProfileDto, UpdateProfileDto } from '../types/profile';
import type { UserInfo } from '../types/user';

import { computed, defineAsyncComponent, onMounted, reactive, ref } from 'vue';
import { useRoute } from 'vue-router';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { useUserStore } from '@vben/stores';

import { Card, Menu, message, Modal } from 'ant-design-vue';

import { useProfileApi } from '../api/useProfileApi';

const AuthenticatorSettings = defineAsyncComponent(
  () => import('./components/AuthenticatorSettings.vue'),
);
const BasicSettings = defineAsyncComponent(
  () => import('./components/BasicSettings.vue'),
);
const BindSettings = defineAsyncComponent(
  () => import('./components/BindSettings.vue'),
);
const NoticeSettings = defineAsyncComponent(
  () => import('./components/NoticeSettings.vue'),
);
const SecuritySettings = defineAsyncComponent(
  () => import('./components/SecuritySettings.vue'),
);
const SessionSettings = defineAsyncComponent(
  () => import('./components/SessionSettings.vue'),
);
const PersonalDataSettings = defineAsyncComponent(
  () => import('./components/PersonalDataSettings.vue'),
);
const { getApi, updateApi } = useProfileApi();
const userStore = useUserStore();
const { query } = useRoute();

const selectedMenuKeys = ref<string[]>(['basic']);
const myProfile = ref({} as ProfileDto);
const menuItems = reactive([
  {
    key: 'basic',
    label: $t('abp.account.settings.basic.title'),
  },
  {
    key: 'security',
    label: $t('abp.account.settings.security.title'),
  },
  {
    key: 'bind',
    label: $t('abp.account.settings.bindSettings'),
  },
  {
    key: 'session',
    label: $t('abp.account.settings.sessionSettings'),
  },
  {
    key: 'notice',
    label: $t('abp.account.settings.noticeSettings'),
  },
  {
    key: 'authenticator',
    label: $t('abp.account.settings.authenticatorSettings'),
  },
  {
    key: 'personal-data',
    label: $t('abp.account.settings.personalDataSettings'),
  },
]);
const getUserInfo = computed((): null | UserInfo => {
  if (!userStore.userInfo) {
    return null;
  }
  return {
    email: userStore.userInfo.email,
    emailVerified: userStore.userInfo.emailVerified,
    name: userStore.userInfo.name,
    phoneNumber: userStore.userInfo.phoneNumber,
    phoneNumberVerified: userStore.userInfo.phoneNumberVerified,
    preferredUsername: userStore.userInfo.username,
    role: userStore.userInfo.roles!,
    sub: userStore.userInfo.userId,
    uniqueName: userStore.userInfo.username,
  };
});
const [EmailConfirmModal, emailConfirmModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./components/EmailConfirmModal.vue'),
  ),
});
function onEmailConfirm() {
  if (query?.confirmToken) {
    emailConfirmModalApi.setData({
      email: myProfile.value.email,
      ...query,
    });
    emailConfirmModalApi.open();
  }
}
async function onGetProfile() {
  const profile = await getApi();
  myProfile.value = profile;
}
async function onUpdateProfile(input: UpdateProfileDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpAccount.PersonalSettingsSaved'),
    onOk: async () => {
      await updateApi(input);
      message.success(
        $t('AbpAccount.PersonalSettingsChangedConfirmationModalTitle'),
      );
      // 刷新页面重载用户信息
      window.location.reload();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
function onChangePassword() {
  // TODO: onChangePassword 暂时未实现!
  console.warn('onChangePassword 暂时未实现!');
}
function onChangePhoneNumber() {
  // TODO: onChangePhoneNumber 暂时未实现!
  console.warn('onChangePhoneNumber 暂时未实现!');
}
onMounted(async () => {
  await onGetProfile();
  onEmailConfirm();
});
</script>

<template>
  <div>
    <Card>
      <div class="flex">
        <div class="basis-1/6">
          <Menu
            v-model:selected-keys="selectedMenuKeys"
            :items="menuItems"
            mode="inline"
          />
        </div>
        <div class="basis-5/6">
          <BasicSettings
            v-if="selectedMenuKeys[0] === 'basic'"
            :profile="myProfile"
            @submit="onUpdateProfile"
          />
          <BindSettings v-else-if="selectedMenuKeys[0] === 'bind'" />
          <SecuritySettings
            v-else-if="selectedMenuKeys[0] === 'security'"
            :user-info="getUserInfo"
            @change-password="onChangePassword"
            @change-phone-number="onChangePhoneNumber"
          />
          <NoticeSettings v-else-if="selectedMenuKeys[0] === 'notice'" />
          <AuthenticatorSettings
            v-else-if="selectedMenuKeys[0] === 'authenticator'"
          />
          <SessionSettings v-else-if="selectedMenuKeys[0] === 'session'" />
          <PersonalDataSettings
            v-else-if="selectedMenuKeys[0] === 'personal-data'"
          />
        </div>
      </div>
    </Card>
    <EmailConfirmModal />
  </div>
</template>

<style scoped></style>
