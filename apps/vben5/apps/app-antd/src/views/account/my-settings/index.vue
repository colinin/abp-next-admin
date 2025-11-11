<script lang="ts" setup>
import type { BindItem } from '@abp/account';

import { defineAsyncComponent, ref } from 'vue';

import { Page, useVbenModal } from '@vben/common-ui';
import { useRefresh } from '@vben/hooks';
import { $t } from '@vben/locales';

import { MySetting, useExternalLoginsApi } from '@abp/account';
import { message, Modal } from 'ant-design-vue';

defineOptions({
  name: 'Vben5AccountMySettings',
});

const { bindWorkWeixinApi, getExternalLoginsApi, removeExternalLoginApi } =
  useExternalLoginsApi();
const { refresh } = useRefresh();

const [WechatWorkUserBindModal, weComBindModal] = useVbenModal({
  connectedComponent: defineAsyncComponent(async () => {
    const component = await import('@abp/wechat');
    return component.WechatWorkUserBinder;
  }),
});
const externalLogins = ref<BindItem[]>([]);

async function onBindWorkWeixin(code: string) {
  weComBindModal.setState({ submitting: true });
  try {
    await bindWorkWeixinApi({ code });
    weComBindModal.close();
    message.success($t('AbpAccount.BindSuccessfully'));
    refresh();
  } finally {
    weComBindModal.setState({ submitting: false });
  }
}

async function onRemoveBind(provider: string, key: string) {
  Modal.confirm({
    title: $t('AbpUi.AreYouSure'),
    centered: true,
    content: $t('AbpAccount.CancelBindWarningMessage'),
    async onOk() {
      await removeExternalLoginApi({
        loginProvider: provider,
        providerKey: key,
      });
      message.success($t('AbpAccount.CancelBindSuccessfully'));
    },
  });
}

function onBind(provider: string) {
  switch (provider.toLocaleLowerCase()) {
    case 'workweixin': {
      weComBindModal.open();
    }
  }
}

async function onClick(provider: string, key?: string) {
  if (key) {
    await onRemoveBind(provider, key);
    return;
  }
  onBind(provider);
}

async function onInit() {
  const res = await getExternalLoginsApi();
  externalLogins.value = res.externalLogins.map((item) => {
    const userLogin = res.userLogins.find((x) => x.loginProvider === item.name);
    return {
      title: item.displayName,
      description: userLogin?.providerKey ?? $t('AbpAccount.UnBind'),
      buttons: [
        {
          title: userLogin?.providerKey
            ? $t('AbpAccount.CancelBind')
            : $t('AbpAccount.Bind'),
          type: 'link',
          click: () => onClick(item.name, userLogin?.providerKey),
        },
      ],
    };
  });
}
</script>

<template>
  <Page>
    <MySetting :bind-items="externalLogins" @on-bind-init="onInit" />
    <WechatWorkUserBindModal @on-login="onBindWorkWeixin" />
  </Page>
</template>
