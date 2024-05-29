<template>
  <Node
    :title="config.name"
    :show-error="state.showError"
    :content="content"
    :error-info="state.errorInfo"
    @selected="$emit('selected')"
    @delNode="$emit('delNode')"
    @insertNode="(type) => $emit('insertNode', type)"
    placeholder="请设置抄送人"
    header-bgc="#3296fa"
  >
    <template #headerIcon>
      <SettingOutlined />
    </template>
  </Node>
</template>

<script lang="ts">
  export default {
    name: 'CC',
  };
</script>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { SettingOutlined } from '@ant-design/icons-vue';
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
    if (props.config.props.shouldAdd) {
      return '由发起人指定';
    } else if (props.config.props.assignedUser.length > 0) {
      let texts: string[] = [];
      props.config.props.assignedUser.forEach((org) => texts.push(org.name));
      return String(texts).replaceAll(',', '、');
    } else {
      return undefined;
    }
  });
  const state = reactive({
    showError: false,
    errorInfo: '',
  });
  //校验数据配置的合法性
  function validate(err: string[]) {
    state.showError = false;
    if (props.config.props.shouldAdd) {
      state.showError = false;
    } else if (props.config.props.assignedUser.length === 0) {
      state.showError = true;
      state.errorInfo = '请选择需要抄送的人员';
    }
    if (state.showError) {
      err.push(`抄送节点 ${props.config.name} 未设置抄送人`);
    }
    return !state.showError;
  }

  defineExpose({
    validate,
  });
</script>

<style scoped></style>
