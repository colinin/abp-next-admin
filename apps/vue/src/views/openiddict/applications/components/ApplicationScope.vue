<template>
  <PermissionForm
    :resources="getSupportScopes"
    :targetResources="targetResources"
    @change="handleChange"
  />
</template>

<script lang="ts" setup>
  import { computed, ref, unref, onMounted } from 'vue';
  import { getList } from '/@/api/openiddict/open-iddict-scope';
  import PermissionForm from '../../components/Permissions/PermissionForm.vue';

  const emits = defineEmits(['create', 'delete']);
  const props = defineProps({
    scopes: {
      type: [Array] as PropType<string[]>,
    },
    supportScopes: {
      type: [Array] as PropType<string[]>,
    },
  });

  const resourcesRef = ref<{ key: string; title: string }[]>([]);
  const targetResources = computed(() => {
    if (!props.scopes) return [];
    const targetScopes = getSupportScopes.value.filter((item) =>
      props.scopes?.some((scope) => scope === item.key),
    );

    return targetScopes.map((item) => item.key);
  });
  const getSupportScopes = computed(() => {
    const resources = unref(resourcesRef);
    if (props.supportScopes) {
      const supportScopes = props.supportScopes.map((scope) => {
        return {
          key: scope,
          title: scope,
        };
      });
      resources.forEach((resource) => {
        if (!supportScopes.find((scope) => scope.key === resource.key)) {
          supportScopes.push(resource);
        }
      });
      return supportScopes;
    }
    return resources;
  });

  onMounted(() => {
    getList({
      skipCount: 0,
      maxResultCount: 100,
    }).then((res) => {
      resourcesRef.value = res.items.map((item) => {
        return {
          key: item.name,
          title: item.displayName ?? item.name,
        };
      });
    });
  });

  function handleChange(_, direction, moveKeys: string[]) {
    switch (direction) {
      case 'left':
        emits('delete', moveKeys);
        break;
      case 'right':
        emits('create', moveKeys);
        break;
    }
  }
</script>
