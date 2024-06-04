import type { Ref } from 'vue';

import { computed, ref, reactive, unref, watch } from 'vue';
import { cloneDeep } from 'lodash-es';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';

import { get, create, update } from '/@/api/identity-server/clients';
import { Client } from '/@/api/identity-server/clients/model';

interface UseModal {
  modelIdRef: Ref<string>;
  formElRef: Ref<any>;
  tabActivedKey: Ref<string>;
}

export function useModal({ modelIdRef, formElRef, tabActivedKey }: UseModal) {
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const { ruleCreator } = useValidation();
  const modelRef = ref<Client>({} as Client);

  const isEdit = computed(() => {
    if (unref(modelRef)?.id) {
      return true;
    }
    return false;
  });
  const formTitle = computed(() => {
    return isEdit.value
      ? L('Client:Name', [unref(modelRef).clientName] as Recordable)
      : L('Client:New');
  });
  const formRules = reactive({
    name: ruleCreator.fieldRequired({
      name: 'Name',
      resourceName: 'AbpIdentityServer',
      trigger: 'blur',
    }),
  });

  watch(
    () => unref(modelIdRef),
    (id) => {
      // 初始值
      modelRef.value = Object.assign({
        enabled: true,
        enableLocalLogin: true,
        allowOfflineAccess: true,
        protocolType: 'oidc',
        identityTokenLifetime: 300,
        accessTokenLifetime: 3600,
        authorizationCodeLifetime: 300,
        slidingRefreshTokenLifetime: 1296000,
        absoluteRefreshTokenLifetime: 2592000,
        refreshTokenUsage: 1,
        refreshTokenExpiration: 1,
        accessTokenType: 0,
        clientClaimsPrefix: 'client_',
        deviceCodeLifetime: 300,
        allowedScopes: [],
        clientSecrets: [],
        allowedCorsOrigins: [],
        redirectUris: [],
        postLogoutRedirectUris: [],
        identityProviderRestrictions: [],
        properties: [],
        claims: [],
        allowedGrantTypes: [],
      }) as Client;
      if (id) {
        get(id).then((res) => {
          modelRef.value = res;
        });
      }
    },
  );

  function handleChangeTab(activeKey) {
    tabActivedKey.value = activeKey;
  }

  function handleVisibleModal(visible: boolean) {
    if (!visible) {
      tabActivedKey.value = 'basic';
    }
  }

  function handleSubmit() {
    return new Promise<any>((resolve, reject) => {
      const formEl = unref(formElRef);
      formEl.validate().then(() => {
        const input = cloneDeep(unref(modelRef));
        const api = isEdit.value
          ? update(input.id, Object.assign(input))
          : create(Object.assign(input));
        api.then((res) => {
          createMessage.success(L('Successful'));
          resolve(res);
        }).catch((error) => {
          reject(error);
        });
      }).catch((error) => {
        reject(error);
      });
    });
  }

  return {
    isEdit,
    modelRef,
    formRules,
    formTitle,
    handleChangeTab,
    handleVisibleModal,
    handleSubmit,
  };
}
