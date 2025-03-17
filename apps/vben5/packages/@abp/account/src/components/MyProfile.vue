<script setup lang="ts">
import type { UploadChangeParam } from 'ant-design-vue';
import type { FileType } from 'ant-design-vue/es/upload/interface';

import type { ProfileDto } from '../types/profile';

import { computed, onMounted, reactive, ref, toValue } from 'vue';

import { $t } from '@vben/locales';
import { preferences } from '@vben/preferences';
import { useUserStore } from '@vben/stores';

import { useSettings } from '@abp/core';
import { UploadOutlined } from '@ant-design/icons-vue';
import {
  Avatar,
  Button,
  Card,
  Form,
  Input,
  Menu,
  message,
  Modal,
  Upload,
} from 'ant-design-vue';

import { useProfileApi } from '../api/useProfileApi';

const FormItem = Form.Item;

const selectedMenuKeys = ref<string[]>(['basic']);
const formModel = ref({} as ProfileDto);
const menuItems = reactive([
  {
    key: 'basic',
    label: $t('abp.account.settings.basicSettings'),
  },
]);
const userStore = useUserStore();
const { getApi, updateApi } = useProfileApi();
const { isTrue } = useSettings();

const avatar = computed(() => {
  return userStore.userInfo?.avatar ?? preferences.app.defaultAvatar;
});
async function onGet() {
  const profile = await getApi();
  formModel.value = profile;
}
function onAvatarChange(_param: UploadChangeParam) {
  console.warn('等待oss模块集成完成...');
}
function onBeforeUpload(_file: FileType) {
  console.warn('等待oss模块集成完成...');
  return false;
}
async function onSubmit() {
  Modal.confirm({
    centered: true,
    content: $t('AbpAccount.PersonalSettingsSaved'),
    onOk: async () => {
      const profile = await updateApi(toValue(formModel));
      message.success(
        $t('AbpAccount.PersonalSettingsChangedConfirmationModalTitle'),
      );
      formModel.value = profile;
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
onMounted(onGet);
</script>

<template>
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
        <Card
          v-if="selectedMenuKeys[0] === 'basic'"
          :bordered="false"
          :title="$t('abp.account.settings.basicSettings')"
        >
          <div class="flex flex-row">
            <div class="basis-2/4">
              <Form
                :label-col="{ span: 6 }"
                :model="formModel"
                :wrapper-col="{ span: 18 }"
              >
                <FormItem
                  :label="$t('AbpAccount.DisplayName:UserName')"
                  name="userName"
                  required
                >
                  <Input
                    v-model:value="formModel.userName"
                    :disabled="
                      !isTrue('Abp.Identity.User.IsUserNameUpdateEnabled')
                    "
                  />
                </FormItem>
                <FormItem
                  :label="$t('AbpAccount.DisplayName:Email')"
                  name="email"
                  required
                >
                  <Input
                    v-model:value="formModel.email"
                    :disabled="
                      !isTrue('Abp.Identity.User.IsEmailUpdateEnabled')
                    "
                    type="email"
                  />
                </FormItem>
                <FormItem
                  :label="$t('AbpAccount.DisplayName:PhoneNumber')"
                  name="phoneNumber"
                >
                  <Input v-model:value="formModel.phoneNumber" />
                </FormItem>
                <FormItem
                  :label="$t('AbpAccount.DisplayName:Surname')"
                  name="surname"
                >
                  <Input v-model:value="formModel.surname" />
                </FormItem>
                <FormItem
                  :label="$t('AbpAccount.DisplayName:Name')"
                  name="name"
                >
                  <Input v-model:value="formModel.name" />
                </FormItem>
                <FormItem>
                  <div class="flex flex-col items-center">
                    <Button
                      style="min-width: 100px"
                      type="primary"
                      @click="onSubmit"
                    >
                      {{ $t('AbpUi.Submit') }}
                    </Button>
                  </div>
                </FormItem>
              </Form>
            </div>
            <div class="basis-2/4">
              <div class="flex flex-col items-center">
                <p>{{ $t('AbpUi.ProfilePicture') }}</p>
                <Avatar :size="100">
                  <template #icon>
                    <img :src="avatar" alt="" />
                  </template>
                </Avatar>
                <Upload
                  :before-upload="onBeforeUpload"
                  :file-list="[]"
                  name="file"
                  @change="onAvatarChange"
                >
                  <Button class="mt-4">
                    <UploadOutlined />
                    {{ $t('abp.account.settings.changeAvatar') }}
                  </Button>
                </Upload>
              </div>
            </div>
          </div>
        </Card>
      </div>
    </div>
  </Card>
</template>

<style scoped></style>
