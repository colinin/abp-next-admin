<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type {
  FormDefinitionDto,
  FormFieldDto,
} from '../../types/approval-flow';

import { reactive, ref } from 'vue';

import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import {
  Button,
  Card,
  Drawer,
  Form,
  FormItem,
  Input,
  InputNumber,
  message,
  Modal,
  Select,
  Space,
  Switch,
  Tag,
} from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import { useApprovalFormsApi } from '../../api/useApprovalFormsApi';

const formFieldTypes = [
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:SingleLineText'),
    value: 'single-line-text',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:MultiLineText'),
    value: 'multi-line-text',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Number'),
    value: 'number',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Date'),
    value: 'date',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:DateRange'),
    value: 'date-range',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Select'),
    value: 'select',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Radio'),
    value: 'radio',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:MultipleSelect'),
    value: 'multiple-select',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Switch'),
    value: 'switch',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Attachment'),
    value: 'attachment',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldType:Description'),
    value: 'description',
  },
] as const;

const formFieldPermissions = [
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldPermission:Editable'),
    value: 'E',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldPermission:ReadOnly'),
    value: 'R',
  },
  {
    label: $t('ElsaNextApprovalFlow.FormDefinitions:FieldPermission:Hidden'),
    value: 'H',
  },
] as const;

const formsApi = useApprovalFormsApi();

const editOpen = ref(false);
const editTarget = ref<FormDefinitionDto | null>(null);
const saving = ref(false);
const form = reactive({
  name: '',
  code: '',
  version: 1,
  description: '',
});

const fieldsOpen = ref(false);
const fieldsTarget = ref<FormDefinitionDto | null>(null);
const fields = ref<FormFieldDto[]>([]);

const fieldOpen = ref(false);
const fieldTarget = ref<FormFieldDto | null>(null);
const fieldForm = reactive({
  key: '',
  label: '',
  type: 'single-line-text',
  permission: 'E',
  order: 0,
  isRequired: false,
  hint: '',
  optionsJson: '',
});

const columns: VxeGridProps<FormDefinitionDto>['columns'] = [
  {
    field: 'name',
    minWidth: 220,
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Name'),
  },
  {
    field: 'code',
    minWidth: 180,
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Code'),
  },
  {
    field: 'version',
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Version'),
    width: 80,
  },
  {
    field: 'status',
    slots: { default: 'status' },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Status'),
    width: 100,
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Actions'),
    width: 260,
  },
];

const formOptions: VbenFormProps = {
  collapsed: false,
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('ElsaNextApprovalFlow.FormDefinitions:Search'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<FormDefinitionDto> = {
  columns,
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await formsApi.getListApi({
          sorting,
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    custom: true,
    refresh: true,
    refreshOptions: { code: 'query' },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<FormDefinitionDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const fieldColumns: VxeGridProps<FormFieldDto>['columns'] = [
  {
    field: 'label',
    minWidth: 180,
    title: $t('ElsaNextApprovalFlow.FormDefinitions:DisplayName'),
  },
  {
    field: 'key',
    minWidth: 160,
    title: $t('ElsaNextApprovalFlow.FormDefinitions:FieldKey'),
  },
  {
    field: 'type',
    slots: { default: 'type' },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Type'),
    width: 120,
  },
  {
    field: 'permission',
    slots: { default: 'permission' },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Permission'),
    width: 90,
  },
  {
    field: 'required',
    slots: { default: 'required' },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Required'),
    width: 80,
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:Actions'),
    width: 150,
  },
];

async function load() {
  await gridApi.query();
}

function openCreate() {
  editTarget.value = null;
  Object.assign(form, { name: '', code: '', version: 1, description: '' });
  editOpen.value = true;
}

function openEdit(target: FormDefinitionDto) {
  editTarget.value = target;
  Object.assign(form, {
    name: target.name,
    code: target.code,
    version: target.version,
    description: target.description ?? '',
  });
  editOpen.value = true;
}

async function submitEdit() {
  if (!form.name || !form.code) {
    message.warning($t('ElsaNextApprovalFlow.FormDefinitions:NameRequired'));
    return;
  }

  saving.value = true;
  try {
    await (editTarget.value
      ? formsApi.updateApi(editTarget.value.id, {
          name: form.name,
          description: form.description || null,
        })
      : formsApi.createApi({
          name: form.name,
          code: form.code,
          version: form.version,
          description: form.description || null,
        }));

    editOpen.value = false;
    await load();
  } catch {
    message.error($t('ElsaNextApprovalFlow.FormDefinitions:SaveFailed'));
  } finally {
    saving.value = false;
  }
}

async function publish(target: FormDefinitionDto) {
  try {
    await formsApi.publishApi(target.id);
    message.success($t('ElsaNextApprovalFlow.FormDefinitions:PublishSuccess'));
    await load();
  } catch {
    message.error($t('ElsaNextApprovalFlow.FormDefinitions:PublishFailed'));
  }
}

async function remove(target: FormDefinitionDto) {
  Modal.confirm({
    content: $t('ElsaNextApprovalFlow.FormDefinitions:DeleteConfirm').replace(
      '{0}',
      target.name,
    ),
    onOk: async () => {
      await formsApi.deleteApi(target.id);
      await load();
    },
    title: $t('ElsaNextApprovalFlow.FormDefinitions:DeleteTitle'),
  });
}

async function openFields(target: FormDefinitionDto) {
  const detail = await formsApi.getApi(target.id);
  fieldsTarget.value = detail;
  fields.value = detail.fields ?? [];
  fieldsOpen.value = true;
}

function openAddField() {
  fieldTarget.value = null;
  Object.assign(fieldForm, {
    key: '',
    label: '',
    type: 'single-line-text',
    permission: 'E',
    order: fields.value.length,
    isRequired: false,
    hint: '',
    optionsJson: '',
  });
  fieldOpen.value = true;
}

function openEditField(field: FormFieldDto) {
  fieldTarget.value = field;
  Object.assign(fieldForm, {
    key: field.key,
    label: field.label,
    type: field.type,
    permission: field.permission,
    order: field.order,
    isRequired: field.isRequired,
    hint: field.hint ?? '',
    optionsJson: field.optionsJson ?? '',
  });
  fieldOpen.value = true;
}

async function submitField() {
  if (!fieldsTarget.value) {
    return;
  }
  if (!fieldForm.key || !fieldForm.label) {
    message.warning($t('ElsaNextApprovalFlow.FormDefinitions:FieldRequired'));
    return;
  }

  saving.value = true;
  try {
    if (fieldTarget.value) {
      const detail = await formsApi.updateFieldApi(
        fieldsTarget.value.id,
        fieldTarget.value.id,
        {
          label: fieldForm.label,
          permission: fieldForm.permission,
          order: fieldForm.order,
          isRequired: fieldForm.isRequired,
          hint: fieldForm.hint || null,
          optionsJson: fieldForm.optionsJson || null,
        },
      );
      fields.value = detail.fields ?? [];
    } else {
      const detail = await formsApi.addFieldApi(fieldsTarget.value.id, {
        key: fieldForm.key,
        label: fieldForm.label,
        type: fieldForm.type,
        permission: fieldForm.permission,
        order: fieldForm.order,
        isRequired: fieldForm.isRequired,
        hint: fieldForm.hint || null,
        optionsJson: fieldForm.optionsJson || null,
      });
      fields.value = detail.fields ?? [];
    }

    fieldOpen.value = false;
    await load();
  } catch {
    message.error($t('ElsaNextApprovalFlow.FormDefinitions:FieldSaveFailed'));
  } finally {
    saving.value = false;
  }
}

async function removeField(field: FormFieldDto) {
  if (!fieldsTarget.value) {
    return;
  }

  try {
    const detail = await formsApi.removeFieldApi(
      fieldsTarget.value.id,
      field.id,
    );
    fields.value = detail.fields ?? [];
    await load();
  } catch {
    message.error($t('ElsaNextApprovalFlow.FormDefinitions:FieldDeleteFailed'));
  }
}

function typeLabel(type: string): string {
  return formFieldTypes.find((item) => item.value === type)?.label ?? type;
}

function permissionLabel(permission: string): string {
  return (
    formFieldPermissions.find((item) => item.value === permission)?.label ??
    permission
  );
}
</script>

<template>
  <Card
    size="small"
    :title="$t('ElsaNextApprovalFlow.FormDefinitions:ManageTitle')"
  >
    <Grid :table-title="$t('ElsaNextApprovalFlow.FormDefinitions:Title')">
      <template #toolbar-tools>
        <Button type="primary" @click="openCreate">
          {{ $t('ElsaNextApprovalFlow.FormDefinitions:NewForm') }}
        </Button>
      </template>
      <template #status="{ row }">
        <Tag v-if="(row as FormDefinitionDto).isPublished" color="green">
          {{ $t('ElsaNextApprovalFlow.FormDefinitions:Published') }}
        </Tag>
        <Tag v-else>{{ $t('ElsaNextApprovalFlow.FormDefinitions:Draft') }}</Tag>
      </template>
      <template #action="{ row }">
        <Space>
          <Button
            size="small"
            type="link"
            @click="openFields(row as FormDefinitionDto)"
          >
            {{ $t('ElsaNextApprovalFlow.FormDefinitions:Field') }}
          </Button>
          <Button
            size="small"
            type="link"
            @click="publish(row as FormDefinitionDto)"
          >
            {{ $t('ElsaNextApprovalFlow.FormDefinitions:Publish') }}
          </Button>
          <Button
            size="small"
            type="link"
            @click="openEdit(row as FormDefinitionDto)"
          >
            {{ $t('ElsaNextApprovalFlow.FormDefinitions:Edit') }}
          </Button>
          <Button
            danger
            size="small"
            type="link"
            @click="remove(row as FormDefinitionDto)"
          >
            {{ $t('ElsaNextApprovalFlow.FormDefinitions:Delete') }}
          </Button>
        </Space>
      </template>
    </Grid>

    <Modal
      v-model:open="editOpen"
      :confirm-loading="saving"
      :title="
        editTarget
          ? $t('ElsaNextApprovalFlow.FormDefinitions:EditForm')
          : $t('ElsaNextApprovalFlow.FormDefinitions:CreateForm')
      "
      @ok="submitEdit"
    >
      <Form layout="vertical">
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:FormName')"
          required
        >
          <Input v-model:value="form.name" />
        </FormItem>
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:FormCode')"
          required
        >
          <Input v-model:value="form.code" :disabled="!!editTarget" />
        </FormItem>
        <FormItem
          v-if="!editTarget"
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:Version')"
        >
          <InputNumber v-model:value="form.version" :min="1" />
        </FormItem>
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:Description')"
        >
          <Input.TextArea v-model:value="form.description" :rows="2" />
        </FormItem>
      </Form>
    </Modal>

    <Drawer
      v-model:open="fieldsOpen"
      :title="
        $t('ElsaNextApprovalFlow.FormDefinitions:FieldManage').replace(
          '{0}',
          fieldsTarget?.name ?? '',
        )
      "
      :width="720"
    >
      <template #extra>
        <Button type="primary" @click="openAddField">
          {{ $t('ElsaNextApprovalFlow.FormDefinitions:AddField') }}
        </Button>
      </template>

      <VxeGrid
        :columns="fieldColumns"
        :data="fields"
        :pager-config="{ enabled: false }"
        border
        size="small"
      >
        <template #type="{ row }">
          {{ typeLabel((row as FormFieldDto).type) }}
        </template>
        <template #permission="{ row }">
          {{ permissionLabel((row as FormFieldDto).permission) }}
        </template>
        <template #required="{ row }">
          <Tag v-if="(row as FormFieldDto).isRequired" color="red">
            {{ $t('ElsaNextApprovalFlow.FormDefinitions:Required') }}
          </Tag>
        </template>
        <template #action="{ row }">
          <Space>
            <Button
              size="small"
              type="link"
              @click="openEditField(row as FormFieldDto)"
            >
              {{ $t('ElsaNextApprovalFlow.FormDefinitions:Edit') }}
            </Button>
            <Button
              danger
              size="small"
              type="link"
              @click="removeField(row as FormFieldDto)"
            >
              {{ $t('ElsaNextApprovalFlow.FormDefinitions:Delete') }}
            </Button>
          </Space>
        </template>
      </VxeGrid>
    </Drawer>

    <Modal
      v-model:open="fieldOpen"
      :confirm-loading="saving"
      :title="
        fieldTarget
          ? $t('ElsaNextApprovalFlow.FormDefinitions:EditField')
          : $t('ElsaNextApprovalFlow.FormDefinitions:AddField')
      "
      @ok="submitField"
    >
      <Form layout="vertical">
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:FieldKey')"
          required
        >
          <Input v-model:value="fieldForm.key" :disabled="!!fieldTarget" />
        </FormItem>
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:DisplayName')"
          required
        >
          <Input v-model:value="fieldForm.label" />
        </FormItem>
        <FormItem :label="$t('ElsaNextApprovalFlow.FormDefinitions:FieldType')">
          <Select
            v-model:value="fieldForm.type"
            :options="formFieldTypes as any"
            :disabled="!!fieldTarget"
          />
        </FormItem>
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:FieldPermission')"
        >
          <Select
            v-model:value="fieldForm.permission"
            :options="formFieldPermissions as any"
          />
        </FormItem>
        <FormItem :label="$t('ElsaNextApprovalFlow.FormDefinitions:Sort')">
          <InputNumber v-model:value="fieldForm.order" :min="0" />
        </FormItem>
        <FormItem :label="$t('ElsaNextApprovalFlow.FormDefinitions:Required')">
          <Switch v-model:checked="fieldForm.isRequired" />
        </FormItem>
        <FormItem :label="$t('ElsaNextApprovalFlow.FormDefinitions:Hint')">
          <Input v-model:value="fieldForm.hint" />
        </FormItem>
        <FormItem
          :label="$t('ElsaNextApprovalFlow.FormDefinitions:OptionsJson')"
        >
          <Input.TextArea v-model:value="fieldForm.optionsJson" :rows="2" />
        </FormItem>
      </Form>
    </Modal>
  </Card>
</template>
