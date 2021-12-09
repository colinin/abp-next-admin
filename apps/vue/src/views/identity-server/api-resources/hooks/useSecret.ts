import type { Ref } from 'vue';

import { unref } from 'vue';
import { ApiResource } from '/@/api/identity-server/model/apiResourcesModel';

interface UseSecret {
  resourceRef: Ref<ApiResource>;
}

export function useSecret({ resourceRef }: UseSecret) {
  function handleNewSecret(input) {
    const resource = unref(resourceRef);
    resource.secrets.push(input);
  }

  function handleDeleteSecret(record) {
    const resource = unref(resourceRef);
    const index = resource.secrets.findIndex(
      (item) => item.type === record.type && item.value === record.value
    );
    if (index >= 0) {
      resource.secrets.splice(index, 1);
    }
  }

  return {
    handleNewSecret,
    handleDeleteSecret,
  };
}
