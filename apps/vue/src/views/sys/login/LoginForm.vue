<template>
  <LoginFormTitle v-show="getShow" class="enter-x" />
  <Form
    class="p-4 enter-x"
    :model="formData"
    :rules="getFormRules"
    ref="formRef"
    v-show="getShow"
    colon
    labelAlign="left"
    @keypress.enter="handleLogin"
  >
    <FormItem>
      <MultiTenancyBox />
    </FormItem>
    <FormItem name="userName" class="enter-x" :label="t('AbpAccount.DisplayName:UserName')">
      <BInput
        size="large"
        v-model:value="formData.userName"
        :placeholder="t('AbpAccount.DisplayName:UserName')"
        class="fix-auto-fill"
      />
    </FormItem>
    <FormItem name="password" class="enter-x" :label="t('AbpAccount.DisplayName:Password')">
      <InputPassword
        size="large"
        visibilityToggle
        autocomplete="off"
        v-model:value="formData.password"
        :placeholder="t('AbpAccount.DisplayName:Password')"
      />
    </FormItem>

    <ARow class="enter-x">
      <ACol :span="12">
        <FormItem>
          <!-- No logic, you need to deal with it yourself -->
          <Checkbox v-model:checked="rememberMe" size="small">
            {{ t('sys.login.rememberMe') }}
          </Checkbox>
        </FormItem>
      </ACol>
      <ACol :span="12">
        <FormItem :style="{ 'text-align': 'right' }">
          <!-- No logic, you need to deal with it yourself -->
          <Button type="link" size="small" @click="setLoginState(LoginStateEnum.RESET_PASSWORD)">
            {{ t('sys.login.forgetPassword') }}
          </Button>
        </FormItem>
      </ACol>
    </ARow>

    <FormItem class="enter-x">
      <Button type="primary" size="large" block @click="handleLogin" :loading="loading">
        {{ t('sys.login.loginButton') }}
      </Button>
    </FormItem>
    <!-- <ARow class="enter-x">
      <ACol :md="12" :xs="24">
        <Button block @click="setLoginState(LoginStateEnum.MOBILE)">
          {{ t('sys.login.mobileSignInFormTitle') }}
        </Button>
      </ACol>
    </ARow> -->

    <ARow class="enter-x" v-if="settingProvider.isTrue('Abp.Account.IsSelfRegistrationEnabled')">
      <ACol :md="24" :xs="24">
        <span>{{ t('AbpAccount.AreYouANewUser') }}</span>
        <Button type="link" @click="setLoginState(LoginStateEnum.REGISTER)">
          {{ t('AbpAccount.Register') }}
        </Button>
      </ACol>
    </ARow>

    <Divider class="enter-x">{{ t('sys.login.otherSignIn') }}</Divider>

    <div class="flex justify-evenly enter-x" :class="`${prefixCls}-sign-in-way`">
      <SvgIcon
        v-if="getLoginState !== LoginStateEnum.SSO"
        name="idsv4"
        :style="{ cursor: 'pointer' }"
        :size="22"
        title="SSO"
        @click="login"
      />
      <UserOutlined
        v-if="getLoginState !== LoginStateEnum.LOGIN"
        :title="t('sys.login.passwordLogin')"
        @click="setLoginState(LoginStateEnum.LOGIN)"
      />
      <MobileOutlined
        v-if="getLoginState !== LoginStateEnum.MOBILE"
        :title="t('sys.login.phoneLogin')"
        @click="setLoginState(LoginStateEnum.MOBILE)"
      />
      <WechatOutlined
        v-if="getLoginState !== LoginStateEnum.WECHAT"
        :title="t('sys.login.wechatLogin')"
      />
    </div>
  </Form>
</template>
<script lang="ts" setup>
  import { reactive, ref, unref, computed } from 'vue';

  import { Checkbox, Form, Input, Row, Col, Button, Divider } from 'ant-design-vue';
  import { MobileOutlined, WechatOutlined, UserOutlined } from '@ant-design/icons-vue';
  import { SvgIcon } from '/@/components/Icon';
  import { Input as BInput } from '/@/components/Input';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { MultiTenancyBox } from '/@/components/MultiTenancyBox';

  import { useI18n } from '/@/hooks/web/useI18n';
  import { useMessage } from '/@/hooks/web/useMessage';

  import { useUserStore } from '/@/store/modules/user';
  import { LoginStateEnum, useLoginState, useFormRules, useFormValid } from './useLogin';
  import { useOidc } from './useOidc';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useSettings } from '/@/hooks/abp/useSettings';
  //import { onKeyStroke } from '@vueuse/core';

  const ACol = Col;
  const ARow = Row;
  const FormItem = Form.Item;
  const InputPassword = Input.Password;
  const { t } = useI18n();
  const { notification } = useMessage();
  const { prefixCls } = useDesign('login');
  const { settingProvider } = useSettings();
  const userStore = useUserStore();
  const { login } = useOidc();

  const { setLoginState, getLoginState } = useLoginState();
  const { getFormRules } = useFormRules();

  const formRef = ref();
  const loading = ref(false);
  const rememberMe = ref(false);

  const formData = reactive({
    userName: '',
    password: '',
  });

  const { validForm } = useFormValid(formRef);

  //onKeyStroke('Enter', handleLogin);

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.LOGIN);

  async function handleLogin() {
    const data = await validForm();
    if (!data) return;
    try {
      loading.value = true;
      const userInfo = await userStore.login({
        password: data.password,
        username: data.userName,
        mode: 'none', //不要默认的错误提示
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
    } finally {
      loading.value = false;
    }
  }
</script>
