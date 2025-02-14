<script setup lang="ts">
import type { EmailMessageDto } from '../../../types/messages';

import { ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';

import { Tinymce } from '@abp/components/tinymce';

const messageContent = ref('');

const [Modal, modalApi] = useVbenModal({
  onOpenChange(isOpen) {
    if (isOpen) {
      const dto = modalApi.getData<EmailMessageDto>();
      messageContent.value = dto.content;
    }
  },
  showConfirmButton: false,
});
</script>

<template>
  <Modal :title="$t('AppPlatform.EmailMessages')">
    <Tinymce
      :value="messageContent"
      :plugins="[]"
      :toolbar="[]"
      readonly
      menubar="''"
    />
  </Modal>
</template>

<style scoped></style>
