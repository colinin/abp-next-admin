<template>
  <Alert :class="prefixCls" type="info" :message="message">
    <template #closeText>
      <Button type="link" @click="handleDeSelect">{{ t('component.table.deSelected') }}</Button>
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
  const emits = defineEmits(['onDeSelect']);

  const { t } = useI18n();
  const { prefixCls } = useDesign('basic-table-alert');

  function handleDeSelect() {
    emits('onDeSelect');
  }
</script>

<style lang="less">
  @prefix-cls: ~'@{namespace}-basic-table-alert';

  .@{prefix-cls} {
    margin: 6px 0 3px 0;
  }
</style>