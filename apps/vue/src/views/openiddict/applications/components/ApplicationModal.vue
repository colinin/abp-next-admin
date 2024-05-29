<template>
  <BasicModal
    @register="registerModal"
    :title="L('Applications')"
    :can-fullscreen="false"
    :width="800"
    :height="500"
    :close-func="handleBeforeClose"
    @ok="handleSubmit"
  >
    <Form
      ref="formRef"
      :model="state.application"
      :rules="state.formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="state.activeTab" @change="handleTabChange">
        <!-- Basic -->
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem name="applicationType" :label="L('DisplayName:ApplicationType')">
            <Select
              default-value="web"
              :options="applicationTypes"
              v-model:value="state.application.applicationType"
            />
          </FormItem>
          <FormItem name="clientId" :label="L('DisplayName:ClientId')">
            <Input :disabled="state.isEdit" v-model:value="state.application.clientId" />
          </FormItem>
          <FormItem name="clientType" :label="L('DisplayName:ClientType')">
            <Select
              :disabled="state.isEdit"
              default-value="public"
              :options="clientTypes"
              v-model:value="state.application.clientType"
            />
          </FormItem>
          <FormItem v-if="getShowSecret" name="clientSecret" :label="L('DisplayName:ClientSecret')">
            <Input v-model:value="state.application.clientSecret" />
          </FormItem>
          <FormItem name="clientUri" :label="L('DisplayName:ClientUri')">
            <Input v-model:value="state.application.clientUri" />
          </FormItem>
          <FormItem name="logoUri" :label="L('DisplayName:LogoUri')">
            <Input v-model:value="state.application.logoUri" />
          </FormItem>
          <FormItem
            name="consentType"
            :label="L('DisplayName:ConsentType')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
          >
            <Select
              :options="consentTypes"
              default-value="explicit"
              v-model:value="state.application.consentType"
            />
          </FormItem>
        </TabPane>
        <!-- DisplayName -->
        <TabPane key="displayName" :tab="L('DisplayNames')">
          <FormItem name="displayName" :label="L('DisplayName:DefaultDisplayName')">
            <Input v-model:value="state.application.displayName" />
          </FormItem>
          <DisplayNameForm
            :displayNames="state.application.displayNames"
            @create="handleNewDisplayName"
            @delete="handleDelDisplayName"
          />
        </TabPane>
        <!-- Endpoints -->
        <TabPane key="endpoints">
          <template #tab>
            <Dropdown>
              <span
                >{{ L('Endpoints') }}
                <DownOutlined />
              </span>
              <template #overlay>
                <Menu @click="handleClickUrisMenu">
                  <MenuItem key="redirectUris">
                    {{ L('DisplayName:RedirectUris') }}
                  </MenuItem>
                  <MenuItem key="postLogoutRedirectUris">
                    {{ L('DisplayName:PostLogoutRedirectUris') }}
                  </MenuItem>
                </Menu>
              </template>
            </Dropdown>
          </template>
          <component
            :is="componentsRef[state.endPoint.component]"
            :uris="state.endPoint.uris"
            @create-redirect-uri="handleNewRedirectUri"
            @delete-redirect-uri="handleDelRedirectUri"
            @create-logout-uri="handleNewLogoutUri"
            @delete-logout-uri="handleDelLogoutUri"
          />
        </TabPane>
        <!-- Scopes -->
        <TabPane key="permissions" :tab="L('Scopes')">
          <ApplicationScope
            :scopes="state.application.scopes"
            :support-scopes="state.openIdConfiguration?.scopes_supported"
            @create="handleNewScope"
            @delete="handleDelScope"
          />
        </TabPane>
        <!-- Authorizations -->
        <TabPane key="authorizations" :tab="L('Authorizations')">
          <FormItem
            name="endpoints"
            :label="L('DisplayName:Endpoints')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
          >
            <Select :options="endpoints" mode="tags" v-model:value="state.application.endpoints" />
          </FormItem>
          <FormItem
            name="grantTypes"
            :label="L('DisplayName:GrantTypes')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
          >
            <Select
              :options="grantTypes"
              mode="tags"
              v-model:value="state.application.grantTypes"
            />
          </FormItem>
          <FormItem
            name="responseTypes"
            :label="L('DisplayName:ResponseTypes')"
            :label-col="{ span: 4 }"
            :wrapper-col="{ span: 20 }"
          >
            <Select
              :options="responseTypes"
              mode="tags"
              v-model:value="state.application.responseTypes"
            />
          </FormItem>
        </TabPane>
        <!-- Propertites -->
        <TabPane key="propertites" :tab="L('Propertites')">
          <PropertyForm
            :properties="state.application.properties"
            @create="handleNewProperty"
            @delete="handleDelProperty"
          />
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import type { FormInstance } from 'ant-design-vue/lib/form';
  import { cloneDeep } from 'lodash-es';
  import { computed, nextTick, ref, unref, reactive, shallowRef, onMounted, watch } from 'vue';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { Dropdown, Form, Menu, Input, Select, Tabs } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ApplicationState } from '../types/props';
  import { get, create, update} from '/@/api/openiddict/open-iddict-application';
  import { OpenIddictApplicationDto } from '/@/api/openiddict/open-iddict-application/model';
  import { discovery } from '/@/api/identity-server/discovery';
  import RedirectUri from './RedirectUri.vue';
  import PostLogoutRedirectUri from './PostLogoutRedirectUri.vue';
  import DisplayNameForm from '../../components/DisplayNames/DisplayNameForm.vue';
  import ApplicationScope from './ApplicationScope.vue';
  import PropertyForm from '../../components/Properties/PropertyForm.vue';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  const MenuItem = Menu.Item;

  const emits = defineEmits(['register', 'change']);
  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
  const { ruleCreator } = useValidation();
  const { createMessage, createConfirm } = useMessage();
  const componentsRef = shallowRef({
    redirectUris: RedirectUri,
    postLogoutRedirectUris: PostLogoutRedirectUri,
  });
  const formRef = ref<FormInstance>();
  const state = reactive<ApplicationState>({
    activeTab: 'basic',
    isEdit: false,
    entityChanged: false,
    application: {} as OpenIddictApplicationDto,
    formRules: {
      clientId: ruleCreator.fieldRequired({
        name: 'ClientId',
        prefix: 'DisplayName',
        resourceName: 'AbpOpenIddict',
        trigger: 'blur',
      }),
      clientSecret: ruleCreator.fieldRequired({
        name: 'ClientSecret',
        prefix: 'DisplayName',
        resourceName: 'AbpOpenIddict',
        trigger: 'blur',
      }),
      displayName: ruleCreator.fieldRequired({
        name: 'DisplayName',
        prefix: 'DisplayName',
        resourceName: 'AbpOpenIddict',
        trigger: 'blur',
      }),
    },
    endPoint: {
      component: '',
      uris: [],
    },
  });
  watch(
    () => state.application,
    () => {
      state.entityChanged = true;
    },
    {
      deep: true,
    },
  );
  const clientTypes = reactive([
    { label: 'public', value: 'public' },
    { label: 'confidential', value: 'confidential' },
  ]);
  const applicationTypes = reactive([
    { label: 'Web', value: 'web' },
    { label: 'Native', value: 'native' },
  ]);
  const consentTypes = reactive([
    { label: 'explicit', value: 'explicit' },
    { label: 'external', value: 'external' },
    { label: 'implicit', value: 'implicit' },
    { label: 'systematic', value: 'systematic' },
  ]);
  const endpoints = reactive([
    { label: 'authorization', value: 'authorization' },
    { label: 'token', value: 'token' },
    { label: 'logout', value: 'logout' },
    { label: 'device', value: 'device' },
    { label: 'revocation', value: 'revocation' },
    { label: 'introspection', value: 'introspection' },
  ]);
  const getShowSecret = computed(() => {
    return !state.isEdit && state.application.clientType === 'confidential';
  });
  const grantTypes = computed(() => {
    if (!state.openIdConfiguration) return [];
    const types = state.openIdConfiguration.grant_types_supported;
    return types.map((type) => {
      return {
        label: type,
        value: type,
      };
    });
  });
  const responseTypes = computed(() => {
    if (!state.openIdConfiguration) return [];
    const types = state.openIdConfiguration.response_types_supported;
    return types.map((type) => {
      return {
        label: type,
        value: type,
      };
    });
  });

  const [registerModal, { changeLoading, changeOkLoading, closeModal }] = useModalInner((data) => {
    nextTick(() => {
      fetch(data?.id);
    });
  });

  onMounted(initOpenidDiscovery);

  function initOpenidDiscovery() {
    discovery().then((openIdConfiuration) => {
      state.openIdConfiguration = openIdConfiuration;
    });
  }

  function fetch(id?: string) {
    state.activeTab = 'basic';
    state.isEdit = false;
    state.entityChanged = false;
    state.application = {} as OpenIddictApplicationDto;
    const form = unref(formRef);
    form?.resetFields();
    if (!id) {
      nextTick(() => {
        state.isEdit = false;
        state.entityChanged = false;
      });
      return;
    }
    changeLoading(true);
    get(id)
      .then((application) => {
        state.application = application;
        nextTick(() => {
          state.isEdit = true;
          state.entityChanged = false;
        });
      })
      .finally(() => {
        changeLoading(false);
      });
  }

  function handleTabChange(activeKey) {
    state.activeTab = activeKey;
    switch (activeKey) {
      case 'endpoints':
        if (!state.endPoint.component) {
          state.endPoint = {
            component: 'redirectUris',
            uris: state.application?.redirectUris,
          };
        }
        break;
    }
  }

  function handleNewLogoutUri(uri: string) {
    if (!state.application) {
      return;
    }
    state.application.postLogoutRedirectUris ??= [];
    state.application.postLogoutRedirectUris.push(uri);
  }

  function handleDelLogoutUri(uri: string) {
    if (!state.application || !state.application.postLogoutRedirectUris) {
      return;
    }
    const index = state.application.postLogoutRedirectUris.findIndex(
      (logoutUri) => logoutUri === uri,
    );
    index && state.application.postLogoutRedirectUris.splice(index);
  }

  function handleNewRedirectUri(uri: string) {
    if (!state.application) {
      return;
    }
    state.application.redirectUris ??= [];
    state.application.redirectUris.push(uri);
  }

  function handleDelRedirectUri(uri: string) {
    if (!state.application || !state.application.redirectUris) {
      return;
    }
    const index = state.application.redirectUris.findIndex((redirectUri) => redirectUri === uri);
    index && state.application.redirectUris.splice(index);
  }

  function handleNewDisplayName(record) {
    if (!state.application) {
      return;
    }
    state.application.displayNames ??= {};
    state.application.displayNames[record.culture] = record.displayName;
  }

  function handleDelDisplayName(record) {
    if (!state.application || !state.application.displayNames) {
      return;
    }
    delete state.application.displayNames[record.culture];
  }

  function handleNewProperty(record) {
    if (!state.application) {
      return;
    }
    state.application.properties ??= {};
    state.application.properties[record.key] = record.value;
  }

  function handleDelProperty(record) {
    if (!state.application || !state.application.properties) {
      return;
    }
    delete state.application.properties[record.key];
  }

  function handleNewScope(scopes: string[]) {
    if (!state.application) {
      return;
    }
    state.application.scopes ??= [];
    state.application.scopes.push(...scopes);
  }

  function handleDelScope(scopes: string[]) {
    if (!state.application || !state.application.scopes) {
      return;
    }
    state.application.scopes = state.application.scopes.filter((scope) => !scopes.includes(scope));
  }

  function handleClickUrisMenu(e) {
    state.endPoint = {
      component: e.key,
      uris: state.application[e.key],
    };
    state.activeTab = 'endpoints';
  }

  function handleBeforeClose(): Promise<boolean> {
    return new Promise((resolve) => {
      if (!state.entityChanged) {
        const form = unref(formRef);
        form?.resetFields();
        return resolve(true);
      }
      createConfirm({
        iconType: 'warning',
        title: L('AreYouSure'),
        content: L('AreYouSureYouWantToCancelEditingWarningMessage'),
        onOk: () => {
          const form = unref(formRef);
          form?.resetFields();
          resolve(true);
        },
        onCancel: () => {
          resolve(false);
        },
      });
    });
  }

  function handleSubmit() {
    const form = unref(formRef);
    form?.validate().then(() => {
      changeOkLoading(true);
      const api = state.application.id
        ? update(state.application.id, cloneDeep(state.application))
        : create(cloneDeep(state.application));
      api
        .then((res) => {
          createMessage.success(L('Successful'));
          emits('change', res);
          closeModal();
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }
</script>
