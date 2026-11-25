<script setup lang="ts">
import type { BindItem } from '../types/bind';
import type { ProfileDto, UpdateProfileDto } from '../types/profile';
import type { UserInfo } from '../types/user';

import { computed, defineAsyncComponent, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { useUserStore } from '@vben/stores';

import { Card, Menu, message, Modal } from 'ant-design-vue';

import { useProfileApi } from '../api/useProfileApi';

interface MenuItem {
  key: string;
  label: string;
}
const props = defineProps<{
  bindItems?: BindItem[];
  disableAuthenticator?: boolean;
  disableBind?: boolean;
  disableNotice?: boolean;
  disablePersonalData?: boolean;
  disableSecurity?: boolean;
  disableSession?: boolean;
  disableSystemSetting?: boolean;
}>();
const emits = defineEmits<{
  (event: 'onBindInit'): void;
  (event: 'onConfirm', params: Record<string, any>): void;
}>();
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
const UserSettings = defineAsyncComponent(
  () => import('./components/UserSettings.vue'),
);
const { getApi, updateApi } = useProfileApi();
const userStore = useUserStore();
const { query } = useRoute();

const selectedMenuKeys = ref<string[]>(['basic']);
const myProfile = ref({} as ProfileDto);
const basicMenuItems: MenuItem[] = [
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
  {
    key: 'system-settings',
    label: $t('abp.account.settings.systemSettings'),
  },
];
const getEnabledMenus = computed(() => {
  return basicMenuItems.filter((x) => {
    if (x.key === 'basic') return true;
    if (x.key === 'authenticator') return !props.disableAuthenticator;
    if (x.key === 'bind') return !props.disableBind;
    if (x.key === 'notice') return !props.disableNotice;
    if (x.key === 'personal-data') return !props.disablePersonalData;
    if (x.key === 'security') return !props.disableSecurity;
    if (x.key === 'session') return !props.disableSession;
    if (x.key === 'system-settings') return !props.disableSystemSetting;
    return true; // default case for any unexpected keys
  });
});
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
    role: userStore.userInfo.roles ?? [],
    sub: userStore.userInfo.userId,
    uniqueName: userStore.userInfo.username,
  };
});
const [EmailConfirmModal, emailConfirmModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./components/EmailConfirmModal.vue'),
  ),
});
const [ChangePasswordModal, changePasswordModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./components/ChangePasswordModal.vue'),
  ),
});
const [ChangePhoneNumberModal, changePhoneNumberModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./components/ChangePhoneNumberModal.vue'),
  ),
});
function onBindConfirm() {
  if (query?.confirmToken) {
    setTimeout(() => {
      emailConfirmModalApi.setData({
        email: myProfile.value.email,
        ...query,
      });
      emailConfirmModalApi.open();
    }, 300);
    emits('onConfirm', query);
  }
}
async function onGetProfile() {
  const profile = await getApi();
  myProfile.value = profile;
}
async function onPhoneNumberChange(phoneNumber: string) {
  userStore.$patch((state) => {
    state.userInfo && (state.userInfo.phoneNumber = phoneNumber);
  });
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
  changePasswordModalApi.open();
}
function onChangePhoneNumber() {
  changePhoneNumberModalApi.open();
}
onMounted(async () => {
  await onGetProfile();
  onBindConfirm();
});
</script>

<template>
  <div class="h-full min-h-0">
    <Card :bordered="false" class="h-full" :body-style="{ height: '100%' }">
      <div class="flex h-full min-h-0">
        <div class="basis-1/6 shrink-0 overflow-y-auto">
          <Menu
            v-model:selected-keys="selectedMenuKeys"
            :items="getEnabledMenus"
            mode="inline"
          />
        </div>
        <div class="settings-content min-h-0 flex-1 overflow-y-auto pl-4">
          <BasicSettings
            v-if="selectedMenuKeys[0] === 'basic'"
            :profile="myProfile"
            @submit="onUpdateProfile"
            @picture-change="onGetProfile"
          />
          <BindSettings
            v-else-if="selectedMenuKeys[0] === 'bind'"
            :items="bindItems"
            @on-init="emits('onBindInit')"
          />
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
          <UserSettings v-else-if="selectedMenuKeys[0] === 'system-settings'" />
        </div>
      </div>
    </Card>
    <EmailConfirmModal />
    <ChangePasswordModal />
    <ChangePhoneNumberModal @change="onPhoneNumberChange" />
  </div>
</template>

<style scoped>
.settings-content > :deep(.ant-card) {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.settings-content > :deep(.ant-card) > .ant-card-head {
  flex: none;
}

.settings-content > :deep(.ant-card) > .ant-card-body {
  flex: 1;
  min-height: 0;
  overflow-y: auto;
}
</style>
