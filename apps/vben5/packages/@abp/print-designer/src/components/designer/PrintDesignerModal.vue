<script setup lang="ts">
import type {
  PrintDesignerInstance,
  PrintDesignerOptions,
} from '../../types/print-designer';

import { useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import PrintDesigner from './index.vue';

const props = defineProps<PrintDesignerOptions>();

const designer = useTemplateRef<PrintDesignerInstance>('print-designer');

const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  fullscreen: true,
  fullscreenButton: false,
  footer: false,
  title: $t('PrintDesigner.Templates:Print'),
  async onOpened() {
    const { templateId, openMarket } = modalApi.getData();
    if (openMarket === true) {
      await designer.value?.designerApi?.openMarket();
      return;
    }
    if (templateId) {
      await designer.value?.setCurrentTemplte(templateId);
    }
  },
});
</script>

<template>
  <Modal>
    <PrintDesigner ref="print-designer" v-bind="props" />
  </Modal>
</template>

<style scoped></style>
