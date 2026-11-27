<script setup lang="ts">
import type { Notification } from '../../types/notifications';

import { ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';

import { MarkdownViewer } from '@abp/components/vditor';
import { getModalData } from '@abp/core';

const notification = ref<Notification>();

const [Modal, modalApi] = useVbenModal<Notification>({
  onOpenChange(isOpen) {
    if (isOpen) {
      notification.value = getModalData(modalApi);
    }
  },
  showConfirmButton: false,
});
</script>

<template>
  <Modal :title="notification?.title">
    <MarkdownViewer
      class="color: text-[#333]"
      :value="notification?.message as string"
    />
  </Modal>
</template>

<style lang="scss" scoped>
.vditor-reset {
  color: #333;
}
</style>
