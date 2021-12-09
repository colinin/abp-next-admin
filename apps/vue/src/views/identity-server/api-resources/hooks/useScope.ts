import type { Ref } from 'vue';

import { computed, unref } from 'vue';
import { ApiResource } from '/@/api/identity-server/model/apiResourcesModel';

interface UseScope {
  resourceRef: Ref<ApiResource>;
}

export function useScope({ resourceRef }: UseScope) {
  const targetScopes = computed(() => {
    if (unref(resourceRef).scopes) {
      return unref(resourceRef).scopes.map((scope) => scope.scope);
    }
    return [];
  });

  function handleScopeChange(_, direction, moveKeys: string[]) {
    const resource = unref(resourceRef);
    switch (direction) {
      case 'left':
        moveKeys.forEach((key) => {
          const index = resource.scopes.findIndex((scope) => scope.scope === key);
          if (index >= 0) {
            resource.scopes.splice(index, 1);
          }
        });
        break;
      case 'right':
        moveKeys.forEach((key) => {
          resource.scopes.push({ scope: key });
        });
        break;
    }
  }

  return {
    targetScopes,
    handleScopeChange,
  };
}
