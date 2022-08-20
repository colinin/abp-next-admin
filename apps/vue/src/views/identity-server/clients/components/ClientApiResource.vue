<template>
  <Resources :resources="resources" :targetResources="targetResources" @change="handleChange" />
</template>

<script lang="ts" setup>
  import { computed, ref, onMounted, toRefs } from 'vue';
  import Resources from './Resources.vue';
  import { getAssignableApiResources } from '/@/api/identity-server/clients';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import { useResource } from '../hooks/useResource';

  const props = defineProps({
    modelRef: {
      type: Object as PropType<Client>,
      required: true,
    },
  });

  const resources = ref<{ key: string; title: string }[]>([]);
  const targetResources = computed(() => {
    const targetScopes = resources.value.filter((item) =>
      props.modelRef.allowedScopes.some((scope) => scope.scope === item.key),
    );

    return targetScopes.map((item) => item.key);
  });

  onMounted(() => {
    getAssignableApiResources().then((res) => {
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
</script>
