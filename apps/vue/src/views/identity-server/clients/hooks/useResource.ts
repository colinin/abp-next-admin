import type { Ref } from 'vue';

import { unref } from 'vue';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseClaim {
  modelRef: Ref<Client>;
}

export function useResource({ modelRef }: UseClaim) {
  function handleResourceChange(direction, resources: string[]) {
    const model = unref(modelRef);
    switch (direction) {
      case 'add':
        model.allowedScopes.push(
          ...resources.map((item) => {
            return { scope: item };
          })
        );
        break;
      case 'delete':
        model.allowedScopes = model.allowedScopes.filter((item) => !resources.includes(item.scope));
        break;
    }
  }

  return {
    handleResourceChange,
  };
}
