<template>
  <div>
    <slot name="insertFooter"></slot>
    <a-button v-bind="cancelButtonProps" @click="handleCancel" v-if="showCancelBtn">
      {{ getCancelText }}
    </a-button>
    <slot name="centerFooter"></slot>
    <a-button
      :type="okType"
      @click="handleOk"
      :loading="confirmLoading"
      v-bind="okButtonProps"
      v-if="showOkBtn"
    >
      {{ getOkText }}
    </a-button>
    <slot name="appendFooter"></slot>
  </div>
</template>
<script lang="ts">
  import { computed, defineComponent } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { basicProps } from '../props';
  export default defineComponent({
    name: 'BasicModalFooter',
    props: basicProps,
    emits: ['ok', 'cancel'],
    setup(props, { emit }) {
      const { t } = useI18n();

      const getOkText = computed(() => {
        return props.okText || t('common.okText');
      });

      const getCancelText = computed(() => {
        return props.cancelText || t('common.cancelText');
      });

      function handleOk(e: Event) {
        emit('ok', e);
      }

      function handleCancel(e: Event) {
        emit('cancel', e);
      }

      return { handleOk, handleCancel, getOkText, getCancelText };
    },
  });
</script>
