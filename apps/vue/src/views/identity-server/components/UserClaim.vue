<template>
  <Transfer
    :dataSource="userClaims"
    :targetKeys="targetClaims"
    :titles="[L('Assigned'), L('Available')]"
    :render="(item) => item.title"
    :list-style="getListStyle"
    @change="handleChange"
  />
</template>

<script lang="ts" setup>
  import { computed, onMounted, ref } from 'vue';
  import { Transfer } from 'ant-design-vue';
  import { getActivedList } from '/@/api/identity/claims';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  const emits = defineEmits(['change']);
  const props = defineProps({
    targetClaims: {
      type: [Array] as PropType<string[]>,
      required: true,
      default: () => [],
    },
    listStyle: {
      type: Object,
      required: false,
    },
  });

  const { L } = useLocalization('AbpIdentityServer');
  const userClaims = ref<
    {
      key: string;
      title: string;
    }[]
  >([]);
  const defaultListStyle = {
    width: '48%',
    height: '500px',
    minHeight: '500px',
  };

  const getListStyle = computed(() => {
    return { ...defaultListStyle, ...props.listStyle };
  });

  onMounted(() => {
    getActivedList().then((res) => {
      userClaims.value = res.items.map((item) => {
        return {
          key: item.name,
          title: item.name,
        };
      });
    });
  });

  function handleChange(targetKeys, direction, moveKeys) {
    emits('change', targetKeys, direction, moveKeys);
  }
</script>
