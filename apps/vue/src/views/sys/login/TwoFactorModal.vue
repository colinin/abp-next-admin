<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :width="400"
    :min-height="150"
    :show-ok-btn="false"
    :show-cancel-btn="false"
    :mask-closable="false"
    :title="t('sys.login.twoFactorFormTitle')"
  >
    <component
      :is="componentsRef[componentRef]"
      :user-info="userInfoRef"
      :provider="twoFactorProvider"
      @send-code="handleSendCode"
      @go-back="handleGoBack"
    />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { shallowRef, ref } from 'vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useI18n } from '/@/hooks/web/useI18n';
  import TwoFactorSwitch from './TwoFactorSwitch.vue';
  import TwoFactorCode from './TwoFactorCode.vue';

  const { t } = useI18n();
  const twoFactorProvider = ref('');
  const userInfoRef = ref<Recordable>({});
  const componentRef = ref('TwoFactorSwitch');
  const componentsRef = shallowRef({
    'TwoFactorSwitch': TwoFactorSwitch,
    'TwoFactorCode': TwoFactorCode,
  });
  const [registerModal] = useModalInner((data) => {
    userInfoRef.value = data;
    componentRef.value = 'TwoFactorSwitch';
  });

  function handleSendCode(provider: string) {
    twoFactorProvider.value = provider;
    componentRef.value = 'TwoFactorCode';
  }

  function handleGoBack() {
    componentRef.value = 'TwoFactorSwitch';
  }
</script>
