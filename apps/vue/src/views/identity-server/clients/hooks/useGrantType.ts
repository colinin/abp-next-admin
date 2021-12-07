import type { Ref } from 'vue';

import { ref, unref, onMounted } from 'vue';
import { discovery } from '/@/api/identity-server/identityServer';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseGrantType {
  modelRef: Ref<Client>;
}

export function useGrantType({ modelRef }: UseGrantType) {
  const grantTypeOptions = ref<{ key: string; label: string; value: string }[]>([]);
  const allowGrantTypes = ref<string[]>([]);

  onMounted(() => {
    discovery().then((res) => {
      allowGrantTypes.value = res.grant_types_supported;
      reloadGrantTypeOptions();
    });
  });

  function reloadGrantTypeOptions() {
    const types = unref(allowGrantTypes);
    grantTypeOptions.value = types.map((item) => {
      return {
        key: item,
        label: item,
        value: item,
      };
    });
  }

  function handleGrantTypeChanged(direction, record) {
    const model = unref(modelRef);
    const index = model.allowedGrantTypes.findIndex((item) => item.grantType === record.grantType);
    switch (direction) {
      case 'add':
        if (index < 0) {
          model.allowedGrantTypes.push(record);
        }
        break;
      case 'delete':
        if (index >= 0) {
          model.allowedGrantTypes.splice(index, 1);
        }
        break;
    }
  }

  return {
    grantTypeOptions,
    handleGrantTypeChanged,
  };
}
