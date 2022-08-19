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

<script lang="ts">
  import { computed, defineComponent } from 'vue';
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
      listStyle: { type: Object, required: false },
    },
    emits: ['change'],
    setup(props, { emit }) {
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
        emit('change', targetKeys, direction, moveKeys);
      }

      return {
        L,
        getListStyle,
        handleChange,
      };
    },
  });
</script>
