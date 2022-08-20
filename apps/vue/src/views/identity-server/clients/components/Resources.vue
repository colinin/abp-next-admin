<template>
  <Transfer
    :dataSource="resources"
    :targetKeys="targetResources"
    :titles="[L('Assigned'), L('Available')]"
    :render="(item) => item.title"
    :list-style="getListStyle"
    @change="handleChange"
  />
</template>

<script lang="ts" setup>
  import { computed } from 'vue';
  import { Transfer } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  const emits = defineEmits(['change']);
  const props = defineProps({
    resources: {
      type: [Array] as PropType<{ key: string; title: string }[]>,
      required: true,
      default: () => [],
    },
    targetResources: { type: [Array] as PropType<string[]>, required: true, default: () => [] },
    listStyle: { type: Object, required: false },
  });

  const { L } = useLocalization('AbpIdentityServer');
  const defaultListStyle = {
    width: '48%',
    height: '500px',
    minHeight: '500px',
  };
  const getListStyle = computed(() => {
    return {...defaultListStyle, ...props.listStyle}
  });

  function handleChange(targetKeys, direction, moveKeys) {
    emits('change', targetKeys, direction, moveKeys);
  }
</script>
