<template>
  <BasicModal
    @register="registerModal"
    :width="800"
    :height="400"
    :title="modalTitle"
    :mask-closable="false"
    @ok="handleSubmit"
  >
    <Form ref="formElRef" :colon="true" layout="vertical" :model="modelRef" :rules="modelRules">
      <FormItem name="isActive">
        <Checkbox v-model:checked="modelRef.isActive">{{ L('DisplayName:IsActive') }}</Checkbox>
      </FormItem>
      <FormItem name="tenantId" :label="L('DisplayName:TenantId')">
        <Select v-model:value="modelRef.tenantId">
          <SelectOption v-for="tenant in tenantsRef" :key="tenant.id" :value="tenant.id">{{
            tenant.name
          }}</SelectOption>
        </Select>
      </FormItem>
      <FormItem name="webhookUri" required :label="L('DisplayName:WebhookUri')">
        <Input v-model:value="modelRef.webhookUri" autocomplete="off" />
      </FormItem>
      <FormItem name="description" :label="L('DisplayName:Description')">
        <Textarea
          v-model:value="modelRef.description"
          :show-count="true"
          :auto-size="{ minRows: 3 }"
        />
      </FormItem>
      <FormItem name="secret" :label="L('DisplayName:Secret')">
        <InputPassword v-model:value="modelRef.secret" autocomplete="off" />
      </FormItem>
      <FormItem name="webhooks" :label="L('DisplayName:Webhooks')">
        <Select v-model:value="modelRef.webhooks" mode="multiple" :filterOption="optionFilter">
          <SelectGroup
            v-for="group in webhooksGroupRef"
            :key="group.name"
            :label="group.displayName"
          >
            <SelectOption
              v-for="option in group.webhooks"
              :key="option.name"
              :value="option.name"
              :displayName="option.displayName"
            >
              <Tooltip placement="right">
                <template #title>
                  {{ option.description }}
                </template>
                {{ option.displayName }}
              </Tooltip>
            </SelectOption>
          </SelectGroup>
        </Select>
      </FormItem>
      <FormItem name="headers" :label="L('DisplayName:Headers')">
        <CodeEditor style="height: 300px" :mode="MODE.JSON" v-model:value="modelRef.headers" />
      </FormItem>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, reactive, unref, onMounted, nextTick } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { Checkbox, Form, Select, Tooltip, Input, InputPassword, Textarea } from 'ant-design-vue';
  import { isString } from '/@/utils/is';
  import { CodeEditor, MODE } from '/@/components/CodeEditor';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { TenantDto } from '/@/api/saas/tenant/model';
  import { GetListAsyncByInput as getTenants } from '/@/api/saas/tenant';
  import {
    GetAsyncById,
    CreateAsyncByInput,
    UpdateAsyncByIdAndInput,
    GetAllAvailableWebhooksAsync,
  } from '/@/api/webhooks/subscriptions';
  import { WebhookSubscription, WebhookAvailableGroup } from '/@/api/webhooks/subscriptions/model';

  const FormItem = Form.Item;
  const SelectGroup = Select.OptGroup;
  const SelectOption = Select.Option;

  const emit = defineEmits(['change', 'register']);
  const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);
  const { ruleCreator } = useValidation();
  const { createMessage } = useMessage();
  const formElRef = ref<any>();
  const tenantsRef = ref<TenantDto[]>([]);
  const webhooksGroupRef = ref<WebhookAvailableGroup[]>([]);
  const modelRef = ref<WebhookSubscription>(getDefaultModel());
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner((model) => {
    fetchModel(model.id);
    nextTick(() => {
      const formEl = unref(formElRef);
      formEl?.clearValidate();
    });
  });
  const isEditModal = computed(() => {
    if (modelRef.value.id) {
      return true;
    }
    return false;
  });
  const modalTitle = computed(() => {
    if (!isEditModal.value) {
      return L('Subscriptions:AddNew');
    }
    return L('Subscriptions:Edit');
  });
  const modelRules = reactive({
    webhookUri: [
      ...ruleCreator.fieldRequired({
        name: 'WebhookUri',
        resourceName: 'WebhooksManagement',
        prefix: 'DisplayName',
      }),
      ...ruleCreator.fieldMustBeStringWithMaximumLength({
        name: 'WebhookUri',
        resourceName: 'WebhooksManagement',
        prefix: 'DisplayName',
        length: 255,
        type: 'string',
      }),
    ],
    secret: ruleCreator.fieldMustBeStringWithMaximumLength({
      name: 'Secret',
      resourceName: 'WebhooksManagement',
      prefix: 'DisplayName',
      length: 128,
      type: 'string',
    }),
    // headers: ruleCreator.defineValidator({
    //   trigger: 'change',
    //   validator: (_, value: any) => {
    //     console.log(String(value));
    //     if (!value) {
    //       return Promise.resolve();
    //     }
    //     if (isJson(value)) {
    //       return Promise.resolve();
    //     }
    //     return Promise.reject(L('InvalidHeaders'));
    //   },
    // }),
  });

  onMounted(() => {
    fetchTenants();
    fetchAvailableWebhooks();
  });

  function fetchAvailableWebhooks() {
    GetAllAvailableWebhooksAsync().then((res) => {
      webhooksGroupRef.value = res.items;
    });
  }

  function fetchTenants() {
    getTenants({
      skipCount: 0,
      maxResultCount: 100,
      sorting: undefined,
    }).then((res) => {
      tenantsRef.value = res.items;
    });
  }

  function fetchModel(id: string) {
    if (!id) {
      modelRef.value = getDefaultModel();
      return;
    }
    GetAsyncById(id).then((res) => {
      modelRef.value = res;
    });
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then(() => {
      const model = unref(modelRef);
      if (isString(model.headers)) {
        model.headers = JSON.parse(model.headers);
      }
      changeOkLoading(true);
      const api = isEditModal.value
        ? UpdateAsyncByIdAndInput(model.id, Object.assign(model))
        : CreateAsyncByInput(Object.assign(model));
      api
        .then(() => {
          createMessage.success(L('Successful'));
          formEl?.resetFields();
          closeModal();
          emit('change');
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }

  function optionFilter(onputValue: string, option: any) {
    if (option.displayName) {
      return option.displayName.includes(onputValue);
    }
    if (option.label) {
      return option.label.includes(onputValue);
    }
    return true;
  }

  function getDefaultModel(): WebhookSubscription {
    return {
      id: '',
      webhooks: [],
      webhookUri: '',
      headers: {},
      secret: '',
      isActive: true,
      creatorId: '',
      creationTime: new Date(),
    };
  }
</script>
