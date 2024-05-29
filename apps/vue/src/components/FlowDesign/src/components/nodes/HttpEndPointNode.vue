<template>
  <Node
    :title="config.name"
    :show-error="state.showError"
    :content="content"
    :error-info="state.errorInfo"
    @selected="$emit('selected')"
    @delNode="$emit('delNode')"
    @insertNode="(type) => $emit('insertNode', type)"
    placeholder="请设置触发活动的路径"
    header-bgc="#3296fa"
  >
    <template #headerIcon>
      <GlobalOutlined />
    </template>
  </Node>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { GlobalOutlined } from '@ant-design/icons-vue';
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
  const content = computed(() => {
    return props.config.props.path;
  });
  const state = reactive({
    showError: false,
    errorInfo: '',
  });

  function validate(err: string[]) {
    state.showError = false;
    if (isNullOrWhiteSpace(props.config.props.name)) {
      state.showError = true;
      state.errorInfo = '请设置活动的名称';
      return;
    }
    if (isNullOrWhiteSpace(props.config.props.path)) {
      state.showError = true;
      state.errorInfo = '请设置触发活动的请求地址';
      return;
    }
    if (Array.isArray(props.config.props.methods) && props.config.props.methods.length === 0) {
      state.showError = true;
      state.errorInfo = '请设置触发活动的请求方法';
      return;
    }
    if (state.showError) {
      err.push(`${props.config.name} 活动配置未设置完善`);
    }
    return !state.showError;
  }

  defineExpose({
    validate,
  });
</script>

<style scoped></style>
