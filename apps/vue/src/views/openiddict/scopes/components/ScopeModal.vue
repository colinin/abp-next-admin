<template>
  <BasicModal
    @register="registerModal"
    :title="L('Scopes')"
    :can-fullscreen="false"
    :width="800"
    :height="500"
    :close-func="handleBeforeClose"
    @ok="handleSubmit"
  >
    <Form
      ref="formRef"
      :model="state.formModel"
      :rules="state.formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 16 }"
    >
      <Tabs v-model:active-key="state.activeTab">
        <!-- Basic -->
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem name="name" :label="L('DisplayName:Name')">
            <Input v-model:value="state.formModel.name" />
          </FormItem>
        </TabPane>
        <!-- DisplayName -->
        <TabPane key="description" :tab="L('Descriptions')">
          <FormItem name="description" :label="L('DisplayName:DefaultDescription')">
            <Input v-model:value="state.formModel.description" />
          </FormItem>
          <DisplayNameForm
            :displayNames="state.formModel.descriptions"
            @create="handleNewDescription"
            @delete="handleDelDescription"
          />
        </TabPane>
        <!-- DisplayName -->
        <TabPane key="displayName" :tab="L('DisplayNames')">
          <FormItem name="displayName" :label="L('DisplayName:DefaultDisplayName')">
            <Input v-model:value="state.formModel.displayName" />
          </FormItem>
          <DisplayNameForm
            :displayNames="state.formModel.displayNames"
            @create="handleNewDisplayName"
            @delete="handleDelDisplayName"
          />
        </TabPane>
        <!-- Resources -->
        <TabPane key="resources" :tab="L('Resources')">
          <PermissionForm
            :resources="getSupportResources"
            :targetResources="targetResources"
            @change="handleResourceChange"
          />
        </TabPane>
        <!-- Propertites -->
        <TabPane key="propertites" :tab="L('Propertites')">
          <PropertyForm
            :properties="state.formModel.properties"
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
  import { computed, nextTick, ref, unref, reactive, watch, onMounted } from 'vue';
  import { Form, Input, Tabs } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ScopeState } from '../types/props';
  import { get, create, update } from '/@/api/openiddict/open-iddict-scope';
  import { OpenIddictScopeDto } from '/@/api/openiddict/open-iddict-scope/model';
  import { discovery } from '/@/api/identity-server/discovery';
  import DisplayNameForm from '../../components/DisplayNames/DisplayNameForm.vue';
  import PropertyForm from '../../components/Properties/PropertyForm.vue';
  import PermissionForm from '../../components/Permissions/PermissionForm.vue';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;

  const emits = defineEmits(['register', 'change']);
  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
  const { ruleCreator } = useValidation();
  const { createMessage, createConfirm } = useMessage();
  const formRef = ref<FormInstance>();
  const state = reactive<ScopeState>({
    activeTab: 'basic',
    isEdit: false,
    entityChanged: false,
    formModel: {} as OpenIddictScopeDto,
    formRules: {
      name: ruleCreator.fieldRequired({
        name: 'Name',
        prefix: 'DisplayName',
        resourceName: 'AbpOpenIddict',
        trigger: 'blur',
      }),
    },
  });
  watch(
    () => state.formModel,
    () => {
      state.entityChanged = true;
    },
    {
      deep: true,
    },
  );
  const targetResources = computed(() => {
    if (!state.formModel.resources) return [];
    const targetScopes = getSupportResources.value.filter((item) =>
      state.formModel.resources?.some((scope) => scope === item.key),
    );
    return targetScopes.map((item) => item.key);
  });
  const getSupportResources = computed(() => {
    if (state.openIdConfiguration?.claims_supported) {
      const supportResources = state.openIdConfiguration?.claims_supported.map((claim) => {
        return {
          key: claim,
          title: claim,
        };
      });
      return supportResources;
    }
    return [];
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
    state.formModel = {} as OpenIddictScopeDto;
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
      .then((scope) => {
        state.formModel = scope;
        nextTick(() => {
          state.isEdit = true;
          state.entityChanged = false;
        });
      })
      .finally(() => {
        changeLoading(false);
      });
  }

  function handleResourceChange(_, direction, moveKeys: string[]) {
    switch (direction) {
      case 'left':
        moveKeys.forEach((key) => {
          const index = state.formModel.resources?.findIndex((r) => r === key);
          index && state.formModel.resources?.splice(index);
        });
        break;
      case 'right':
        state.formModel.resources ??= [];
        state.formModel.resources.push(...moveKeys);
        break;
    }
  }

  function handleNewDisplayName(record) {
    if (!state.formModel) {
      return;
    }
    state.formModel.displayNames ??= {};
    state.formModel.displayNames[record.culture] = record.displayName;
  }

  function handleDelDisplayName(record) {
    if (!state.formModel || !state.formModel.displayNames) {
      return;
    }
    delete state.formModel.displayNames[record.culture];
  }

  function handleNewDescription(record) {
    if (!state.formModel) {
      return;
    }
    state.formModel.descriptions ??= {};
    state.formModel.descriptions[record.culture] = record.displayName;
  }

  function handleDelDescription(record) {
    if (!state.formModel || !state.formModel.descriptions) {
      return;
    }
    delete state.formModel.descriptions[record.culture];
  }

  function handleNewProperty(record) {
    if (!state.formModel) {
      return;
    }
    state.formModel.properties ??= {};
    state.formModel.properties[record.key] = record.value;
  }

  function handleDelProperty(record) {
    if (!state.formModel || !state.formModel.properties) {
      return;
    }
    delete state.formModel.properties[record.key];
  }

  function handleBeforeClose(): Promise<boolean> {
    return new Promise((resolve) => {
      if (!state.entityChanged) {
        return resolve(true);
      }
      createConfirm({
        iconType: 'warning',
        title: L('AreYouSure'),
        content: L('AreYouSureYouWantToCancelEditingWarningMessage'),
        onOk: () => {
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
      console.log(state.formModel);
      const api = state.formModel.id
        ? update(state.formModel.id, cloneDeep(state.formModel))
        : create(cloneDeep(state.formModel));
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
