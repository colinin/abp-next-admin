<template>
  <template v-if="getShow">
    <LoginFormTitle class="enter-x" />
    <Form
      class="p-4 enter-x"
      ref="formRef"
      :model="formData"
      :rules="getFormRules"
      colon
      labelAlign="left"
    >
      <FormItem>
        <MultiTenancyBox />
      </FormItem>
      <FormItem name="userName" class="enter-x" :label="L('DisplayName:UserName')">
        <BInput class="fix-auto-fill" size="large" v-model:value="formData.userName" />
      </FormItem>
      <FormItem name="emailAddress" class="enter-x" :label="L('DisplayName:Email')">
        <BInput size="large" v-model:value="formData.emailAddress" class="fix-auto-fill" />
      </FormItem>
      <FormItem name="password" class="enter-x" :label="L('DisplayName:Password')">
        <StrengthMeter size="large" v-model:value="formData.password" />
      </FormItem>

      <FormItem class="enter-x">
        <Button type="primary" size="large" block @click="handleRegister" :loading="loading">
          {{ L('Register') }}
        </Button>
      </FormItem>

      <FormItem class="enter-x">
        <Button size="large" block @click="setLoginState(LoginStateEnum.MOBILE_REGISTER)">
          {{ L('通过手机号注册') }}
        </Button>
      </FormItem>

      <ARow class="enter-x">
        <ACol :md="24" :xs="24">
          <span>{{ L('AlreadyRegistered') }}</span>
          <Button type="link" @click="handleBackLogin">
            {{ L('Login') }}
          </Button>
        </ACol>
      </ARow>
    </Form>
  </template>
</template>
<script lang="ts" setup>
  import { reactive, ref, unref, computed } from 'vue';
  import LoginFormTitle from './LoginFormTitle.vue';
  import { Form, Button, Row, Col } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { StrengthMeter } from '/@/components/StrengthMeter';
  import { MultiTenancyBox } from '/@/components/MultiTenancyBox';
  import { useLoginState, useFormRules, useFormValid, LoginStateEnum } from './useLogin';
  import { useGlobSetting } from '/@/hooks/setting';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { register } from '/@/api/account/accounts';

  const ACol = Col;
  const ARow = Row;
  const FormItem = Form.Item;
  const { L } = useLocalization('AbpAccount');
  const { handleBackLogin, getLoginState, setLoginState } = useLoginState();
  const glob = useGlobSetting();

  const formRef = ref();
  const loading = ref(false);

  const formData = reactive({
    userName: '',
    password: '',
    emailAddress: '',
    appName: glob.shortName,
  });

  const { getFormRules } = useFormRules();
  const { validForm } = useFormValid(formRef);

  const getShow = computed(() => unref(getLoginState) === LoginStateEnum.REGISTER);

  async function handleRegister() {
    const data = await validForm();
    if (!data) return;
    register(data).then(() => {
      formRef.value.resetFields();
      setLoginState(LoginStateEnum.LOGIN);
    });
  }
</script>
