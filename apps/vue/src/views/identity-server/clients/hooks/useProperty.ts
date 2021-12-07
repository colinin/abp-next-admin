import type { Ref } from 'vue';

import { unref } from 'vue';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseProperty {
  modelRef: Ref<Client>;
}

export function useProperty({ modelRef }: UseProperty) {
  function handleNewProperty(input) {
    const model = unref(modelRef);
    model.properties.push(input);
  }

  function handleDeleteProperty(record) {
    const model = unref(modelRef);
    const index = model.properties.findIndex(
      (item) => item.key === record.key && item.value === record.value
    );
    if (index >= 0) {
      model.properties.splice(index, 1);
    }
  }

  return {
    handleNewProperty,
    handleDeleteProperty,
  };
}
