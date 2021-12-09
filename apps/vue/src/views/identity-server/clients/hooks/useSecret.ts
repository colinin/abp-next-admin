import type { Ref } from 'vue';

import { unref } from 'vue';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseSecret {
  modelRef: Ref<Client>;
}

export function useSecret({ modelRef }: UseSecret) {
  function handleSecretChange(direction, record) {
    const model = unref(modelRef);
    switch (direction) {
      case 'add':
        model.clientSecrets.push(record);
        break;
      case 'delete':
        const index = model.clientSecrets.findIndex(
          (item) => item.type === record.type && item.value === record.value
        );
        model.clientSecrets.splice(index, 1);
        break;
    }
  }

  function handleRequiredChange(e) {
    const model = unref(modelRef);
    model.requireClientSecret = e.target.checked;
  }

  return {
    handleSecretChange,
    handleRequiredChange,
  };
}
