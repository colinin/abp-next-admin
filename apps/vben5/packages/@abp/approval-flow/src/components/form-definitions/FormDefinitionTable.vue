<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type {
  FormDefinitionDto,
  FormFieldDto,
} from '../../types/approval-flow';

import { reactive, ref } from 'vue';

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

import { useApprovalFormsApi } from '../../api/useApprovalParticipantsApi';

const formFieldTypes = [
  { label: '单行文本', value: 'single-line-text' },
  { label: '多行文本', value: 'multi-line-text' },
  { label: '数字', value: 'number' },
  { label: '日期', value: 'date' },
  { label: '日期区间', value: 'date-range' },
  { label: '下拉选择', value: 'select' },
  { label: '单选', value: 'radio' },
  { label: '多选', value: 'multiple-select' },
  { label: '开关', value: 'switch' },
  { label: '附件', value: 'attachment' },
  { label: '说明', value: 'description' },
] as const;

const formFieldPermissions = [
  { label: '可编辑', value: 'E' },
  { label: '只读', value: 'R' },
  { label: '隐藏', value: 'H' },
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
  { field: 'name', minWidth: 220, title: '表单名称' },
  { field: 'code', minWidth: 180, title: '编码' },
  { field: 'version', title: '版本', width: 80 },
  { field: 'status', slots: { default: 'status' }, title: '状态', width: 100 },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: '操作',
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
      label: '搜索',
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
  { field: 'label', minWidth: 180, title: '显示名' },
  { field: 'key', minWidth: 160, title: '字段键' },
  { field: 'type', slots: { default: 'type' }, title: '类型', width: 120 },
  {
    field: 'permission',
    slots: { default: 'permission' },
    title: '权限',
    width: 90,
  },
  {
    field: 'required',
    slots: { default: 'required' },
    title: '必填',
    width: 80,
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: '操作',
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
    message.warning('请填写表单名称和编码');
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
    message.error('保存失败');
  } finally {
    saving.value = false;
  }
}

async function publish(target: FormDefinitionDto) {
  try {
    await formsApi.publishApi(target.id);
    message.success('发布成功');
    await load();
  } catch {
    message.error('发布失败');
  }
}

async function remove(target: FormDefinitionDto) {
  Modal.confirm({
    content: `确定删除表单「${target.name}」吗？`,
    onOk: async () => {
      await formsApi.deleteApi(target.id);
      await load();
    },
    title: '删除确认',
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
    message.warning('请填写字段键和显示名');
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
    message.error('字段保存失败');
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
    message.error('字段删除失败');
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
  <Card size="small" title="表单管理">
    <Grid table-title="表单定义">
      <template #toolbar-tools>
        <Button type="primary" @click="openCreate">新建表单</Button>
      </template>
      <template #status="{ row }">
        <Tag v-if="(row as FormDefinitionDto).isPublished" color="green">
          已发布
        </Tag>
        <Tag v-else>草稿</Tag>
      </template>
      <template #action="{ row }">
        <Space>
          <Button
            size="small"
            type="link"
            @click="openFields(row as FormDefinitionDto)"
          >
            字段
          </Button>
          <Button
            size="small"
            type="link"
            @click="publish(row as FormDefinitionDto)"
          >
            发布
          </Button>
          <Button
            size="small"
            type="link"
            @click="openEdit(row as FormDefinitionDto)"
          >
            编辑
          </Button>
          <Button
            danger
            size="small"
            type="link"
            @click="remove(row as FormDefinitionDto)"
          >
            删除
          </Button>
        </Space>
      </template>
    </Grid>

    <Modal
      v-model:open="editOpen"
      :confirm-loading="saving"
      :title="editTarget ? '编辑表单' : '新建表单'"
      @ok="submitEdit"
    >
      <Form layout="vertical">
        <FormItem label="表单名称" required>
          <Input v-model:value="form.name" />
        </FormItem>
        <FormItem label="表单编码" required>
          <Input v-model:value="form.code" :disabled="!!editTarget" />
        </FormItem>
        <FormItem v-if="!editTarget" label="版本">
          <InputNumber v-model:value="form.version" :min="1" />
        </FormItem>
        <FormItem label="描述">
          <Input.TextArea v-model:value="form.description" :rows="2" />
        </FormItem>
      </Form>
    </Modal>

    <Drawer
      v-model:open="fieldsOpen"
      :title="`字段管理 - ${fieldsTarget?.name ?? ''}`"
      :width="720"
    >
      <template #extra>
        <Button type="primary" @click="openAddField">添加字段</Button>
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
          <Tag v-if="(row as FormFieldDto).isRequired" color="red">必填</Tag>
        </template>
        <template #action="{ row }">
          <Space>
            <Button
              size="small"
              type="link"
              @click="openEditField(row as FormFieldDto)"
            >
              编辑
            </Button>
            <Button
              danger
              size="small"
              type="link"
              @click="removeField(row as FormFieldDto)"
            >
              删除
            </Button>
          </Space>
        </template>
      </VxeGrid>
    </Drawer>

    <Modal
      v-model:open="fieldOpen"
      :confirm-loading="saving"
      :title="fieldTarget ? '编辑字段' : '添加字段'"
      @ok="submitField"
    >
      <Form layout="vertical">
        <FormItem label="字段键" required>
          <Input v-model:value="fieldForm.key" :disabled="!!fieldTarget" />
        </FormItem>
        <FormItem label="显示名" required>
          <Input v-model:value="fieldForm.label" />
        </FormItem>
        <FormItem label="字段类型">
          <Select
            v-model:value="fieldForm.type"
            :options="formFieldTypes as any"
            :disabled="!!fieldTarget"
          />
        </FormItem>
        <FormItem label="字段权限">
          <Select
            v-model:value="fieldForm.permission"
            :options="formFieldPermissions as any"
          />
        </FormItem>
        <FormItem label="排序">
          <InputNumber v-model:value="fieldForm.order" :min="0" />
        </FormItem>
        <FormItem label="必填">
          <Switch v-model:checked="fieldForm.isRequired" />
        </FormItem>
        <FormItem label="说明">
          <Input v-model:value="fieldForm.hint" />
        </FormItem>
        <FormItem label="选项 JSON">
          <Input.TextArea v-model:value="fieldForm.optionsJson" :rows="2" />
        </FormItem>
      </Form>
    </Modal>
  </Card>
</template>
