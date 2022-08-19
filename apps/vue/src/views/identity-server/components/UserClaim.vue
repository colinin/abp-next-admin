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

<script lang="ts">
  import { computed, defineComponent, onMounted, ref } from 'vue';
  import { Transfer } from 'ant-design-vue';
  import { getActivedList } from '/@/api/identity/claim';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  export default defineComponent({
    name: 'UserClaim',
    components: { Transfer },
    props: {
      targetClaims: { type: [Array] as PropType<string[]>, required: true, default: () => [] },
      listStyle: { type: Object, required: false },
    },
    emits: ['change'],
    setup(props, { emit }) {
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
        return {...defaultListStyle, ...props.listStyle}
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
        emit('change', targetKeys, direction, moveKeys);
      }

      return {
        L,
        getListStyle,
        userClaims,
        handleChange,
      };
    },
  });
</script>
