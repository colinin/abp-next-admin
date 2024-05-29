<template>
  <Node
    title="发起人"
    :is-root="true"
    :content="content"
    @selected="$emit('selected')"
    @insertNode="(type) => $emit('insertNode', type)"
    placeholder="所有人"
    header-bgc="#576a95"
    header-icon="el-icon-user-solid"
  >
    <template #headerIcon>
      <HomeOutlined />
    </template>
  </Node>
</template>

<script lang="ts">
  export default {
    name: 'root',
  };
</script>

<script setup lang="ts">
  import { computed } from 'vue';
  import { HomeOutlined } from '@ant-design/icons-vue';
  import Node from './Node.vue';

  defineEmits(['insertNode', 'selected']);
  const props = defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
  });

  const content = computed(() => {
    if (props.config.props?.assignedUser?.length > 0) {
      let texts: any[] = [];
      props.config.props.assignedUser.forEach((org) => texts.push(org.name));
      return String(texts).replaceAll(',', '、');
    } else {
      return '所有人';
    }
  });
</script>

<style scoped></style>
