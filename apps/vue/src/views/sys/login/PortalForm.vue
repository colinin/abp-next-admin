<template>
  <template v-if="getShow">
    <LoginFormTitle class="enter-x" />
    <Form
      class="p-4 enter-x"
      :model="formData"
      :rules="getFormRules"
      ref="formRef"
      colon
      labelAlign="left"
      layout="vertical"
      @keypress.enter="handleLogin"
    >
    <FormItem name="userName" class="enter-x" :label="L('DisplayName:UserName')">
      <BInput
        size="large"
        v-model:value="formData.userName"
        :placeholder="L('DisplayName:UserName')"
        class="fix-auto-fill"
      />
    </FormItem>
    <FormItem name="password" class="enter-x" :label="L('DisplayName:Password')">
      <InputPassword
        size="large"
        visibilityToggle
        autocomplete="off"
        v-model:value="formData.password"
        :placeholder="L('DisplayName:Password')"
      />
    </FormItem>

      <FormItem class="enter-x">
        <Button type="primary" size="large" block @click="handleLogin" :loading="loading">
          {{ L('Login') }}
        </Button>
        <Button size="large" block class="mt-4" @click="handleBackLogin">
          {{ L('GoBack') }}
        </Button>
      </FormItem>
    </Form>
    <BasicModal @register="registerModal" :title="t('sys.login.loginToPortalTitle')" :height="400">
      <List item-layout="horizontal" :data-source="portalModel">
        <template #renderItem="{ item }">
          <ListItem>
            <ListItemMeta>
              <template #title>
                <Button type="text" @click="handleLoginTo(item.Id)" :loading="loading" :disabled="loading">{{ item.Name }}</Button>
              </template>
              <template #avatar>
                <Avatar :src="item.Logo" />
              </template>
            </ListItemMeta>
          </ListItem>
        </template>
      </List>
    </BasicModal>
    <TwoFactorModal @register="registerTwoFactorModal" />
  </template>
</template>
<script lang="ts" setup>
  import { reactive, ref, computed, unref, inject } from 'vue';
  import { Avatar, Form, Button, Input, List } from 'ant-design-vue';
  import { BasicModal, useModal } from '/@/components/Modal';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLoginState, useFormRules, useFormValid, LoginStateEnum } from './useLogin';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import { useUserStoreWithOut } from '/@/store/modules/user';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useGlobSetting } from '/@/hooks/setting';
  import { PortalLoginModel } from '/@/api/sys/model/userModel';
  import TwoFactorModal from './TwoFactorModal.vue';

  const FormItem = Form.Item;
  const InputPassword = Input.Password;
  const ListItem = List.Item;
  const ListItemMeta = ListItem.Meta;
  
  const { notification } = useMessage();
  const [registerModal, { openModal, closeModal }] = useModal();
  const [registerTwoFactorModal, { openModal: openTwoFactorModal }] = useModal();
  const { t } = useI18n();
  const { L } = useLocalization('AbpAccount');
  const { handleBackLogin, getLoginState } = useLoginState();
  const userStore = useUserStoreWithOut();
  const abpStore = useAbpStoreWithOut();

  const formRef = ref();
  const loading = ref(false);
  const portalModel = ref<PortalLoginModel[]>([]);

  const formData = reactive({
    userName: '',
    password: '',
  });

  const { validForm } = useFormValid(formRef);
  const { getFormRules } = useFormRules();
  const globSetting = useGlobSetting();
  const cookies = inject<any>('$cookies');

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.Portal);

  async function handleLoginTo(enterpriseId?: string) {
    const data = await validForm();
    if (!data) return;
    try {
      loading.value = true;
      closeModal();
      const userInfo = await userStore.login({
        username: data.userName,
        password: data.password,
        enterpriseId: enterpriseId,
        mode: 'none', //不要默认的错误提示
        isPortalLogin: true,
        loginCallback: () => {
          return new Promise((resolve) => {
            var currentTenant = abpStore.getApplication.currentTenant;
            if (currentTenant.id) {
              setTimeout(() => {
                cookies?.set(globSetting.multiTenantKey, currentTenant.id);
                return resolve();
              }, 100);
            } else {
              return resolve();
            }
          });
        },
      });
      if (userInfo) {
        notification.success({
          message: t('sys.login.loginSuccessTitle'),
          description: `${t('sys.login.loginSuccessDesc')}: ${
            userInfo.realName ?? userInfo.username
          }`,
          duration: 3,
        });
      }
    } catch (error: any) {
      if (error.userId && error.twoFactorToken) {
        openTwoFactorModal(true, {
          userId: error.userId,
          userName: data.userName,
          password: data.password,
        });
      } else {
        portalModel.value = JSON.parse(error as string) as PortalLoginModel[];
        openModal(true);
      }
      // createConfirm({
      //   iconType: 'info',
      //   title: '请选择登陆平台',
      //   content: '',
      //   okCancel: false,
      // });
    } finally {
      loading.value = false;
    }
  }

  async function handleLogin() {
    await handleLoginTo();
  }
</script>
