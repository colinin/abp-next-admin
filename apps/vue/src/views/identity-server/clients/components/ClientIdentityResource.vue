<template>
  <Resources :resources="resources" :targetResources="targetResources" @change="handleChange" />
</template>

<script lang="ts">
  import { computed, defineComponent, ref, onMounted, toRefs } from 'vue';
  import Resources from './Resources.vue';
  import { getAssignableIdentityResources } from '/@/api/identity-server/clients';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import { useResource } from '../hooks/useResource';

  export default defineComponent({
    name: 'ClientIdentityResource',
    components: { Resources },
    props: {
      modelRef: {
        type: Object as PropType<Client>,
        required: true,
      },
    },
    setup(props) {
      const resources = ref<{ key: string; title: string }[]>([]);
      const targetResources = computed(() => {
        const targetScopes = resources.value.filter((item) =>
          props.modelRef.allowedScopes.some((scope) => scope.scope === item.key),
        );

        return targetScopes.map((item) => item.key);
      });

      onMounted(() => {
        getAssignableIdentityResources().then((res) => {
          resources.value = res.items.map((item) => {
            return {
              key: item,
              title: item,
            };
          });
        });
      });
      const { handleResourceChange } = useResource({ modelRef: toRefs(props).modelRef });

      function handleChange(_, direction, moveKeys: string[]) {
        switch (direction) {
          case 'left':
            handleResourceChange('delete', moveKeys);
            break;
          case 'right':
            handleResourceChange('add', moveKeys);
            break;
        }
      }

      return {
        resources,
        targetResources,
        handleChange,
      };
    },
  });
</script>
