<script setup lang="ts">
import type { Notification } from '../../types/notifications';

import { ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';

import { Tinymce } from '@abp/components/tinymce';

const notification = ref<Notification>();

const [Modal, modalApi] = useVbenModal({
  onOpenChange(isOpen) {
    if (isOpen) {
      notification.value = modalApi.getData<Notification>();
    }
  },
  showConfirmButton: false,
});
</script>

<template>
  <Modal :title="notification?.title">
    <Tinymce
      :value="notification?.message"
      :plugins="[]"
      :toolbar="[]"
      readonly
      menubar="''"
    />
  </Modal>
</template>

<style scoped></style>
