<template>
  <Transfer
    :dataSource="userClaims"
    :targetKeys="targetClaims"
    :titles="[L('Assigned'), L('Available')]"
    :render="(item) => item.title"
    :list-style="{
      width: '293px',
      height: '338px',
    }"
    @change="handleChange"
  />
</template>

<script lang="ts">
  import { defineComponent, onMounted, ref } from 'vue';
  import { Transfer } from 'ant-design-vue';
  import { getActivedList } from '/@/api/identity/claim';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  export default defineComponent({
    name: 'UserClaim',
    components: { Transfer },
    props: {
      targetClaims: { type: [Array] as PropType<string[]>, required: true, default: () => [] },
    },
    emits: ['change'],
    setup(_, { emit }) {
      const { L } = useLocalization('AbpIdentityServer');
      const userClaims = ref<
        {
          key: string;
          title: string;
        }[]
      >([]);

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
        emit('change', targetKeys, direction, moveKeys);
      }

      return {
        L,
        userClaims,
        handleChange,
      };
    },
  });
</script>
