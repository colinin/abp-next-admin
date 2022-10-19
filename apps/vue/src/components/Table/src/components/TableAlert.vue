<template>
  <Alert :class="prefixCls" type="info">
    <template #message>
      <div class="message-box">
        <span class="message-content">{{ message }}</span>
        <div class="message-button">
          <Button type="link" @click="handleDeSelect">{{ t('component.table.deSelected') }}</Button>
        </div>
      </div>
    </template>
  </Alert>
</template>

<script lang="ts" setup>
  import { Alert, Button } from 'ant-design-vue';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useI18n } from '/@/hooks/web/useI18n';

  defineProps({
    message: {
      type: String,
      default: '',
    },
  });
  const emits = defineEmits(['de-select']);

  const { t } = useI18n();
  const { prefixCls } = useDesign('basic-table-alert');

  function handleDeSelect() {
    emits('de-select');
  }
</script>

<style lang="less">
  @prefix-cls: ~'@{namespace}-basic-table-alert';

  .@{prefix-cls} {
    margin: 6px 0 3px 0;

    .message-box {
      position: relative;
      display: flex;
      align-items: center;
      word-wrap: break-word;

      .message-content {
        flex: 1;
        min-width: 0;
      }

      .message-button {
        margin-right: 0px;
      }
    }
  }
</style>
