<template>
  <CollapseContainer :title="L('BasicSettings')" :canExpan="false">
    <Row :gutter="24">
      <Col :span="14">
        <BasicForm @register="register" />
      </Col>
      <Col :span="10">
        <div class="change-avatar">
          <div class="mb-2">{{ L('Avatar') }}</div>
          <CropperAvatar
            :value="avatar"
            :btnText="L('AvatarChanged')"
            :btnProps="{ preIcon: 'ant-design:cloud-upload-outlined' }"
            width="150"
            :uploadApi="handleUploadAvatar"
            @change="updateAvatar"
          />
        </div>
      </Col>
    </Row>
    <Button
      type="primary"
      :loading="confirmButton.loading"
      :disabled="confirmButton.loading"
      @click="handleSubmit"
      >{{ confirmButton.title }}</Button
    >
  </CollapseContainer>
</template>
<script lang="ts" setup>
  import { Button, Row, Col } from 'ant-design-vue';
  import { computed, reactive, watch } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { CollapseContainer } from '/@/components/Container';
  import { CropperAvatar } from '/@/components/Cropper';
  import headerImg from '/@/assets/icons/64x64/color-user.png';
  import { useUserStore } from '/@/store/modules/user';
  import { upload } from '/@/api/oss-management/private';
  import { changeAvatar } from '/@/api/account/claims';
  import { update as updateProfile } from '/@/api/account/profiles';
  import { MyProfile, UpdateMyProfile } from '/@/api/account/model/profilesModel';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useProfile } from './useProfile';

  const emits = defineEmits(['profile-change']);
  const props = defineProps({
    profile: {
      type: Object as PropType<MyProfile>,
    }
  });

  const { getBaseSetschemas } = useProfile({ profile: props.profile });
  const userStore = useUserStore();
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpAccount');
  const [register, { getFieldsValue, setFieldsValue, validate }] = useForm({
    labelWidth: 120,
    schemas: getBaseSetschemas(),
    showActionButtonGroup: false,
  });
  const confirmButton = reactive({
    title: L('Submit'),
    loading: false,
  });
  const avatar = computed(() => {
    const { avatar } = userStore.getUserInfo;
    return avatar ?? headerImg;
  });

  watch(
    () => props.profile,
    (profile) => {
      if (profile) {
        setFieldsValue(profile);
      }
    },
    {
      immediate: true,
    },
  )

  function handleUploadAvatar(params: { file: Blob; name: string; filename: string }) {
    return new Promise<void>((resolve, reject) => {
      upload(params.file, 'avatar', params.filename)
        .then((res) => {
          const path = encodeURIComponent(res.data.path.substring(0, res.data.path.length - 1));
          changeAvatar({ avatarUrl: `${path}/${res.data.name}` })
            .then(() => {
              createMessage.success(L('Successful'));
              emits('profile-change');
              resolve({} as unknown as void);
            })
            .catch((err) => reject(err));
        })
        .catch((err) => reject(err));
    });
  }

  function updateAvatar(src: string) {
    const userinfo = userStore.getUserInfo;
    userinfo.avatar = src;
    userStore.setUserInfo(userinfo);
  }

  function handleSubmit() {
    validate().then(() => {
      confirmButton.loading = true;
      confirmButton.title = L('SavingWithThreeDot');
      updateProfile(getFieldsValue() as UpdateMyProfile)
        .then(() => {
          createMessage.success(L('PersonalSettingsSaved'));
          emits('profile-change');
        }).finally(() => {
          confirmButton.loading = false;
          confirmButton.title = L('Submit');
        });
    });
  }
</script>

<style lang="less" scoped>
  .change-avatar {
    img {
      display: block;
      margin-bottom: 15px;
      border-radius: 50%;
    }
  }
</style>
