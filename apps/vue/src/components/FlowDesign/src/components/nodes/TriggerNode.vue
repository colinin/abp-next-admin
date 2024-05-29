<template>
  <Node
    :title="config.name"
    :show-error="state.showError"
    :error-info="state.errorInfo"
    @selected="$emit('selected')"
    @delNode="$emit('delNode')"
    @insertNode="(type) => $emit('insertNode', type)"
    placeholder="请设置触发器"
    header-bgc="#47bc82"
  >
    <template #headerIcon>
      <SettingOutlined />
    </template>
  </Node>
</template>

<script lang="ts">
  export default {
    name: 'Trigger',
  };
</script>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { SettingOutlined } from '@ant-design/icons-vue';
  import { isNullOrWhiteSpace } from '/@/utils/strings';
  import Node from './Node.vue';

  defineEmits(['delNode', 'insertNode', 'selected']);
  const props = defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
  });

  computed(() => props.config);

  const state = reactive({
    showError: false,
    errorInfo: '',
  });

  function validate(err: string[]) {
    state.showError = false;
    if (props.config.props.type === 'WEBHOOK') {
      if (!isNullOrWhiteSpace(props.config.props.http.url)) {
        state.showError = false;
      } else {
        state.showError = true;
        state.errorInfo = '请设置WEBHOOK的URL地址';
      }
    } else if (props.config.props.type === 'EMAIL') {
      if (
        isNullOrWhiteSpace(props.config.props.email.subject) ||
        props.config.props.email.to.length === 0 ||
        isNullOrWhiteSpace(props.config.props.email.content)
      ) {
        state.showError = true;
        state.errorInfo = '请设置邮件发送配置';
      } else {
        state.showError = false;
      }
    }
    if (state.showError) {
      err.push(`${props.config.name} 触发动作未设置完善`);
    }
    return !state.showError;
  }

  defineExpose({
    validate,
  });
</script>

<style scoped></style>
