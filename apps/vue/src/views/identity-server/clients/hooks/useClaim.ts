import type { Ref } from 'vue';

import { unref } from 'vue';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseClaim {
  modelRef: Ref<Client>;
}

export function useClaim({ modelRef }: UseClaim) {
  function handleClaimChange(direction, record) {
    const model = unref(modelRef);
    switch (direction) {
      case 'add':
        model.claims.push(record);
        break;
      case 'delete':
        const index = modelRef.value.claims.findIndex(
          (item) => item.type === record.type && item.value === record.value
        );
        model.claims.splice(index, 1);
        break;
    }
  }

  function handleCheckedChange(e) {
    const model = unref(modelRef);
    model[e.target.id] = e.target.checked;
  }

  return {
    handleClaimChange,
    handleCheckedChange,
  };
}
