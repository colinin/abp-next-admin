<template>
  <Transfer
    :dataSource="resources"
    :targetKeys="targetResources"
    :titles="[L('Assigned'), L('Available')]"
    :render="(item) => item.title"
    :list-style="{
      width: '465px',
      height: '438px',
    }"
    @change="handleChange"
  />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Transfer } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  export default defineComponent({
    name: 'Resources',
    components: { Transfer },
    props: {
      resources: {
        type: [Array] as PropType<{ key: string; title: string }[]>,
        required: true,
        default: () => [],
      },
      targetResources: { type: [Array] as PropType<string[]>, required: true, default: () => [] },
    },
    emits: ['change'],
    setup(_, { emit }) {
      const { L } = useLocalization('AbpIdentityServer');

      function handleChange(targetKeys, direction, moveKeys) {
        emit('change', targetKeys, direction, moveKeys);
      }

      return {
        L,
        handleChange,
      };
    },
  });
</script>
