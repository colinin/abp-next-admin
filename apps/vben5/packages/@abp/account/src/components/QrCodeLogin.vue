<script setup lang="ts">
import type { QrCodeUserInfoResult } from '../types/qrcode';

import { computed, onMounted, onUnmounted, ref } from 'vue';
import { useRouter } from 'vue-router';

import { $t } from '@vben/locales';

import { VbenButton } from '@vben-core/shadcn-ui';

import { useQRCode } from '@vueuse/integrations/useQRCode';
import { Spin } from 'ant-design-vue';

import { useScanQrCodeApi } from '../api';
import { QrCodeStatus } from '../types/qrcode';
import Title from './components/LoginTitle.vue';

interface Props {
  /**
   * @zh_CN 默认头像
   */
  defaultAvatar?: string;
  /**
   * @zh_CN 描述
   */
  description?: string;
  /**
   * @zh_CN 是否处于加载处理状态
   */
  loading?: boolean;
  /**
   * @zh_CN 登录路径
   */
  loginPath?: string;
  /**
   * @zh_CN 按钮文本
   */
  submitButtonText?: string;
  /**
   * @zh_CN 描述
   */
  subTitle?: string;
  /**
   * @zh_CN 标题
   */
  title?: string;
}

defineOptions({
  name: 'AccountQrCodeLogin',
});

const props = withDefaults(defineProps<Props>(), {
  defaultAvatar: '',
  description: '',
  loading: false,
  loginPath: '/auth/login',
  submitButtonText: '',
  subTitle: '',
  title: '',
});

const emits = defineEmits<{
  (event: 'confirm', key: string, tenantId?: string): void;
}>();

let interval: NodeJS.Timeout;
const router = useRouter();
const { checkCodeApi, generateApi } = useScanQrCodeApi();

const qrcodeInfo = ref<QrCodeUserInfoResult>({
  key: '',
  status: QrCodeStatus.Invalid,
});

const getQrCodeUrl = computed(() => {
  return `QRCODE_LOGIN:${qrcodeInfo.value.key}`;
});
const getScanedQrCode = computed(() => {
  return (
    qrcodeInfo.value.status === QrCodeStatus.Confirmed ||
    qrcodeInfo.value.status === QrCodeStatus.Scaned
  );
});

const qrcode = useQRCode(getQrCodeUrl, {
  errorCorrectionLevel: 'H',
  margin: 4,
});

function goToLogin() {
  router.push(props.loginPath);
}

async function onInit() {
  const loginCode = localStorage.getItem('login_qrocde');
  if (loginCode) {
    qrcodeInfo.value = {
      key: loginCode,
      status: QrCodeStatus.Created,
    };
  } else {
    const result = await generateApi();
    qrcodeInfo.value = {
      key: result.key,
      status: QrCodeStatus.Invalid,
    };
    localStorage.setItem('login_qrocde', result.key);
  }
  await onCheckCode();
  interval = setInterval(onCheckCode, 5000);
}

async function onCheckCode() {
  const result = await checkCodeApi(qrcodeInfo.value.key);
  if (result.status === QrCodeStatus.Invalid) {
    localStorage.removeItem('login_qrocde');
    interval && clearInterval(interval);
    await onInit();
    return;
  }
  qrcodeInfo.value = result;
  // 已确认登录
  if (result.status === QrCodeStatus.Confirmed) {
    interval && clearInterval(interval);
    localStorage.removeItem('login_qrocde');
    // 登录
    emits('confirm', result.key, result.tenantId);
  }
}

onMounted(onInit);

onUnmounted(() => {
  interval && clearInterval(interval);
});
</script>

<template>
  <div>
    <Title>
      <slot name="title">
        {{ title || $t('authentication.welcomeBack') }} 📱
      </slot>
      <template #desc>
        <span class="text-muted-foreground">
          <slot name="subTitle">
            {{ subTitle || $t('authentication.qrcodeSubtitle') }}
          </slot>
        </span>
      </template>
    </Title>

    <div class="flex-col-center mt-6">
      <template v-if="!getScanedQrCode">
        <img :src="qrcode" alt="qrcode" class="w-1/2" />
        <p class="text-muted-foreground mt-4 text-sm">
          <slot name="description">
            {{ description || $t('authentication.qrcodePrompt') }}
          </slot>
        </p>
      </template>
      <Spin v-else :tip="$t('abp.oauth.qrcodeLogin.scaned')">
        <div class="flex-row-center justify-items-center">
          <img
            :src="qrcodeInfo.picture ?? defaultAvatar"
            alt="qrcode"
            class="w-1/2" />
        </div>
      </Spin>
    </div>

    <VbenButton class="mt-4 w-full" variant="outline" @click="goToLogin()">
      {{ $t('common.back') }}
    </VbenButton>
  </div>
</template>
