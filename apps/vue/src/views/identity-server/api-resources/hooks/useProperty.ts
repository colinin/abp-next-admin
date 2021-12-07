import type { Ref } from 'vue';

import { unref } from 'vue';
import { ApiResource } from '/@/api/identity-server/model/apiResourcesModel';

interface UseProperty {
  resourceRef: Ref<ApiResource>;
}

export function useProperty({ resourceRef }: UseProperty) {
  function handleNewProperty(input) {
    const resource = unref(resourceRef);
    resource.properties.push(input);
  }

  function handleDeleteProperty(record) {
    const resource = unref(resourceRef);
    const index = resource.properties.findIndex(
      (item) => item.key === record.key && item.value === record.value
    );
    if (index >= 0) {
      resource.properties.splice(index, 1);
    }
  }

  return {
    handleNewProperty,
    handleDeleteProperty,
  };
}
