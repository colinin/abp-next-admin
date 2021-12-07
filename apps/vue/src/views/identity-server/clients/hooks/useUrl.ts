import type { Ref } from 'vue';

import { unref } from 'vue';
import { Client } from '/@/api/identity-server/model/clientsModel';

interface UseClaim {
  modelRef: Ref<Client>;
}

export function useUrl({ modelRef }: UseClaim) {
  function handleRedirectUriChange(direction, uri) {
    const model = unref(modelRef);
    switch (direction) {
      case 'add':
        model.redirectUris.push({ redirectUri: uri });
        break;
      case 'delete':
        const index = model.redirectUris.findIndex((item) => item.redirectUri === uri);
        model.redirectUris.splice(index, 1);
        break;
    }
  }

  function handleCorsOriginsChange(direction, origin) {
    const model = unref(modelRef);
    switch (direction) {
      case 'add':
        model.allowedCorsOrigins.push({ origin: origin });
        break;
      case 'delete':
        const index = model.allowedCorsOrigins.findIndex((item) => item.origin === origin);
        model.allowedCorsOrigins.splice(index, 1);
        break;
    }
  }

  function handleLogoutRedirectUris(direction, uri) {
    const model = unref(modelRef);
    switch (direction) {
      case 'add':
        model.postLogoutRedirectUris.push({ postLogoutRedirectUri: uri });
        break;
      case 'delete':
        const index = model.postLogoutRedirectUris.findIndex(
          (item) => item.postLogoutRedirectUri === uri
        );
        model.postLogoutRedirectUris.splice(index, 1);
        break;
    }
  }

  return {
    handleRedirectUriChange,
    handleCorsOriginsChange,
    handleLogoutRedirectUris,
  };
}
