<script setup lang="ts">
import { useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';

import { buildUUID, useFeatures } from '@abp/core';
import { WWLoginRedirectType, WWLoginType } from '@wecom/jssdk';
import * as ww from '@wecom/jssdk';
import { Empty } from 'ant-design-vue';

import { userWorkWeixinJsSdkApi } from '../../api/userWorkWeixinJsSdkApi';
import { WeixinWorkAuthEnable } from '../../constants/features';

const emits = defineEmits<{
  /**
   * 用户扫码登录成功回调事件
   * @params code 企业微信授权码
   */
  (event: 'onLogin', code: string): void;
}>();
const wxLoginRef = useTemplateRef<Element>('wxLogin');

const { isEnabled } = useFeatures();
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
    <div v-if="isEnabled([WeixinWorkAuthEnable])" ref="wxLogin"></div>
    <Empty
      v-else
      :description="
        $t('AbpFeature.Volo.Feature:010001', [
          $t('AbpAccountOAuth.Features:WeComOAuthEnable'),
        ])
      "
    />
  </Modal>
</template>

<style scoped></style>
