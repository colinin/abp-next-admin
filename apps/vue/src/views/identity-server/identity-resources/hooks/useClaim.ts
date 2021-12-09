import type { Ref } from 'vue';

import { computed, unref } from 'vue';
import { IdentityResource } from '/@/api/identity-server/model/identityResourcesModel';

interface UseClaim {
  modelRef: Ref<IdentityResource>;
}

export function useClaim({ modelRef }: UseClaim) {
  const targetClaims = computed(() => {
    if (unref(modelRef).userClaims) {
      return unref(modelRef).userClaims.map((claim) => claim.type);
    }
    return [];
  });

  function handleClaimChange(_, direction, moveKeys: string[]) {
    const model = unref(modelRef);
    switch (direction) {
      case 'left':
        moveKeys.forEach((key) => {
          const index = model.userClaims.findIndex((claim) => claim.type === key);
          if (index >= 0) {
            model.userClaims.splice(index, 1);
          }
        });
        break;
      case 'right':
        moveKeys.forEach((key) => {
          model.userClaims.push({ type: key });
        });
        break;
    }
  }

  return {
    targetClaims,
    handleClaimChange,
  };
}
