import type { Ref } from 'vue';

import { unref } from 'vue';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseIdentityProvider {
  modelRef: Ref<Client>;
}

export function useIdentityProvider({ modelRef }: UseIdentityProvider) {
  function handleIdpChange(direction, record) {
    const model = unref(modelRef);
    const index = model.identityProviderRestrictions.findIndex(
      (item) => item.provider === record.provider
    );
    switch (direction) {
      case 'add':
        if (index < 0) {
          model.identityProviderRestrictions.push(record);
        }
        break;
      case 'delete':
        if (index >= 0) {
          model.identityProviderRestrictions.splice(index, 1);
        }
        break;
    }
  }

  function handleCheckedChange(e) {
    const model = unref(modelRef);
    model[e.target.id] = e.target.checked;
  }

  return {
    handleIdpChange,
    handleCheckedChange,
  };
}
