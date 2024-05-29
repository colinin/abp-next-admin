<template>
  <Node
    :title="config.name"
    :show-error="state.showError"
    :content="content"
    :error-info="state.errorInfo"
    @selected="$emit('selected')"
    @delNode="$emit('delNode')"
    @insertNode="(type) => $emit('insertNode', type)"
    placeholder="请设置延时时间"
    header-bgc="#f25643"
  >
    <template #headerIcon>
      <ClockCircleOutlined />
    </template>
  </Node>
</template>

<script lang="ts">
  export default {
    name: 'Delay',
  };
</script>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { ClockCircleOutlined } from '@ant-design/icons-vue';
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
  const state = reactive({
    showError: false,
    errorInfo: '',
  });

  const content = computed(() => {
    if (props.config.props.type === 'FIXED') {
      return `等待 ${props.config.props.time} ${getName(props.config.props.unit)}`;
    } else if (props.config.props.type === 'AUTO') {
      return `至当天 ${props.config.props.dateTime}`;
    } else {
      return undefined;
    }
  });

  function getName(unit: string) {
    switch (unit) {
      case 'D':
        return '天';
      case 'H':
        return '小时';
      case 'M':
        return '分钟';
      default:
        return '未知';
    }
  }

  function validate(err: string[]) {
    state.showError = false;
    try {
      if (props.config.props.type === 'AUTO') {
        if ((props.config.props.dateTime || '') === '') {
          state.showError = true;
          state.errorInfo = '请选择时间点';
        }
      } else {
        if (props.config.props.time <= 0) {
          state.showError = true;
          state.errorInfo = '请设置延时时长';
        }
      }
    } catch (e) {
      state.showError = true;
      state.errorInfo = '配置出现问题';
    }
    if (state.showError) {
      err.push(`${props.config.name} 未设置延时规则`);
    }
    return !state.showError;
  }

  defineExpose({
    validate,
  });
</script>

<style scoped></style>
