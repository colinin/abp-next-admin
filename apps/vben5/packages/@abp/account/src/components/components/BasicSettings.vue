<script setup lang="ts">
import type { ProfileDto, UpdateProfileDto } from '../../types/profile';

import { computed, ref, toValue, watchEffect } from 'vue';

import { $t } from '@vben/locales';
import { preferences } from '@vben/preferences';
import { useUserStore } from '@vben/stores';

import { CropperAvatar } from '@abp/components/cropper';
import { buildUUID, useSettings } from '@abp/core';
import { Button, Card, Form, Input, message } from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';

type ApiFunParams = { file: Blob; fileName?: string; name: string };

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
const { changePictureApi } = useProfileApi();

const avatar = computed(() => {
  return userStore.userInfo?.avatar ?? preferences.app.defaultAvatar;
});
async function onUploadAvatar(params: ApiFunParams) {
  pictureState.value.uploading = true;
  try {
    const fileName = `${buildUUID()}.${params.file.type.slice(6)}`;
    const file = new File([params.file], fileName, {
      type: params.file.type,
    });
    await changePictureApi({ file });
    if (userStore.userInfo?.avatar) {
      URL.revokeObjectURL(userStore.userInfo.avatar);
    }
    message.success($t('AbpUi.SavedSuccessfully'));
  } finally {
    pictureState.value.uploading = false;
  }
}
async function onAvatarChange(url: string) {
  userStore.$patch((state) => {
    state.userInfo && (state.userInfo.avatar = url);
  });
  emits('pictureChange');
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
          <!-- <Avatar :size="100">
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
          </Upload> -->
          <div class="mb-8 block rounded-[50%]">
            <CropperAvatar
              :value="avatar"
              :btn-text="$t('AbpAccount.AvatarChanged')"
              width="150"
              :upload-api="onUploadAvatar"
              @change="onAvatarChange"
            />
          </div>
        </div>
      </div>
    </div>
  </Card>
</template>

<style scoped></style>
