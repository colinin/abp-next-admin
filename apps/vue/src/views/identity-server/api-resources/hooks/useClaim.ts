import type { Ref } from 'vue';

import { computed, unref } from 'vue';
import { ApiResource } from '/@/api/identity-server/model/apiResourcesModel';

interface UseClaim {
  resourceRef: Ref<ApiResource>;
}

export function useClaim({ resourceRef }: UseClaim) {
  const targetClaims = computed(() => {
    if (unref(resourceRef).userClaims) {
      return unref(resourceRef).userClaims.map((claim) => claim.type);
    }
    return [];
  });

  function handleClaimChange(_, direction, moveKeys: string[]) {
    const resource = unref(resourceRef);
    switch (direction) {
      case 'left':
        moveKeys.forEach((key) => {
          const index = resource.userClaims.findIndex((claim) => claim.type === key);
          if (index >= 0) {
            resource.userClaims.splice(index, 1);
          }
        });
        break;
      case 'right':
        moveKeys.forEach((key) => {
          resource.userClaims.push({ type: key });
        });
        break;
    }
  }

  return {
    targetClaims,
    handleClaimChange,
  };
}
