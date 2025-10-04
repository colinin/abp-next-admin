<script setup lang="ts">
import { useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';

import { buildUUID } from '@abp/core';
import { WWLoginRedirectType, WWLoginType } from '@wecom/jssdk';
import * as ww from '@wecom/jssdk';

import { userWorkWeixinJsSdkApi } from '../../api/userWorkWeixinJsSdkApi';

const emits = defineEmits<{
  /**
   * 用户扫码登录成功回调事件
   * @params code 企业微信授权码
   */
  (event: 'onLogin', code: string): void;
}>();
const wxLoginRef = useTemplateRef<Element>('wxLogin');

const { getAgentConfigApi } = userWorkWeixinJsSdkApi();

const [Modal, modalApi] = useVbenModal({
  onOpenChange(isOpen) {
    if (isOpen) {
      setTimeout(onInitLogin, 200);
    }
  },
});

async function onInitLogin() {
  try {
    modalApi.setState({ loading: true });
    const agentConfig = await getAgentConfigApi();
    ww.createWWLoginPanel({
      el: wxLoginRef.value!,
      params: {
        login_type: WWLoginType.corpApp,
        appid: agentConfig.corpId,
        agentid: agentConfig.agentId,
        // TODO: 是否应改为可配置式? 企业微信仅允许配置一个回调地址, 生产环境应配合反向代理服务器.
        redirect_uri: window.location.href,
        state: buildUUID(),
        redirect_type: WWLoginRedirectType.callback,
      },
      onLoginSuccess(res) {
        emits('onLogin', res.code);
      },
    });
  } finally {
    modalApi.setState({ loading: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpAccountOAuth.OAuth:WorkWeixin')">
    <div ref="wxLogin"></div>
  </Modal>
</template>

<style scoped></style>
