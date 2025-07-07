<script setup lang="ts">
import type { UploadChangeParam } from 'ant-design-vue';
import type { FileType } from 'ant-design-vue/es/upload/interface';

import type { ProfileDto, UpdateProfileDto } from '../../types/profile';

import { computed, ref, toValue, watchEffect } from 'vue';

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
  message,
  Upload,
} from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';

const props = defineProps<{
  profile: ProfileDto;
}>();
const emits = defineEmits<{
  (event: 'pictureChange'): void;
  (event: 'submit', profile: UpdateProfileDto): void;
}>();
const FormItem = Form.Item;

const formModel = ref({} as ProfileDto);
const pictureState = ref<{
  file?: any;
  uploading: boolean;
}>({
  uploading: false,
});

const userStore = useUserStore();
const { isTrue } = useSettings();
const { changePictureApi, getPictureApi } = useProfileApi();

const avatar = computed(() => {
  return userStore.userInfo?.avatar ?? preferences.app.defaultAvatar;
});
async function onAvatarChange(_param: UploadChangeParam) {
  pictureState.value.uploading = true;
  try {
    await changePictureApi({
      file: pictureState.value.file,
    });
    if (userStore.userInfo?.avatar) {
      URL.revokeObjectURL(userStore.userInfo.avatar);
    }
    const picture = await getPictureApi();
    userStore.$patch((state) => {
      state.userInfo && (state.userInfo.avatar = URL.createObjectURL(picture));
    });
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('pictureChange');
  } finally {
    pictureState.value.uploading = false;
  }
}
function onBeforeUpload(file: FileType) {
  pictureState.value.file = file;
  return false;
}
function onSubmit() {
  emits('submit', toValue(formModel));
}
watchEffect(() => {
  formModel.value = { ...props.profile };
});
</script>

<template>
  <Card :bordered="false" :title="$t('abp.account.settings.basic.title')">
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
              :disabled="!isTrue('Abp.Identity.User.IsUserNameUpdateEnabled')"
              autocomplete="off"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpAccount.DisplayName:Email')"
            name="email"
            required
          >
            <Input
              v-model:value="formModel.email"
              :disabled="!isTrue('Abp.Identity.User.IsEmailUpdateEnabled')"
              autocomplete="off"
              type="email"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpAccount.DisplayName:Surname')"
            name="surname"
          >
            <Input v-model:value="formModel.surname" autocomplete="off" />
          </FormItem>
          <FormItem :label="$t('AbpAccount.DisplayName:Name')" name="name">
            <Input v-model:value="formModel.name" autocomplete="off" />
          </FormItem>
          <FormItem>
            <div class="flex flex-col items-center">
              <Button style="min-width: 100px" type="primary" @click="onSubmit">
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
            <Button class="mt-4" :loading="pictureState.uploading">
              <UploadOutlined />
              {{ $t('abp.account.settings.changeAvatar') }}
            </Button>
          </Upload>
        </div>
      </div>
    </div>
  </Card>
</template>

<style scoped></style>
