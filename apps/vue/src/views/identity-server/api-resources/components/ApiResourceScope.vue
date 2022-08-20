<template>
  <Transfer
    :dataSource="supportedScopes"
    :targetKeys="targetScopes"
    :titles="[L('Assigned'), L('Available')]"
    :render="(item) => item.title"
    :list-style="{
      width: '293px',
      height: '338px',
    }"
    @change="handleChange"
  />
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Transfer } from 'ant-design-vue';
  import { discovery } from '/@/api/identity-server/identityServer';

  const emits = defineEmits(['change']);

  defineProps({
    targetScopes: {
      type: Object as PropType<string[]>,
      required: true,
    },
  });

  const { L } = useLocalization('AbpIdentityServer');
  const supportedScopes = ref<
    {
      key: string;
      title: string;
    }[]
  >([]);

  onMounted(() => {
    discovery().then((res) => {
      supportedScopes.value = res.scopes_supported.map((scope) => {
        return {
          key: scope,
          title: scope,
        };
      });
    });
  });

  function handleChange(targetKeys, direction, moveKeys) {
    emits('change', targetKeys, direction, moveKeys);
  }
</script>
