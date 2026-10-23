<script setup lang="ts">
import type { FormFieldDto } from '../../types/approval-flow';
import type { AntFlowNode } from './types';

import {
  computed,
  defineAsyncComponent,
  nextTick,
  onMounted,
  reactive,
  ref,
  watch,
} from 'vue';

import {
  Button,
  Card,
  Descriptions,
  DescriptionsItem,
  message,
  Modal,
  Result,
  Select,
  Space,
  Spin,
  Switch,
  Tag,
} from 'ant-design-vue';

import { useApprovalTemplatesApi } from '../../api/useApprovalFlowApi';
import { useApprovalFormsApi } from '../../api/useApprovalParticipantsApi';
import FormDesigner from '../designer/FormDesigner.vue';
import BasicSetting from './BasicSetting.vue';
import { oldToAntFlow } from './model';

const props = withDefaults(
  defineProps<{
    readonly?: boolean;
    templateId?: null | string;
  }>(),
  {
    readonly: false,
    templateId: null,
  },
);

const emit = defineEmits<{
  cancel: [];
  done: [string];
  'update:templateId': [string];
}>();

/**
 * FlowDesign is imported lazily: the canvas (and its drawers) only ships with
 * the bundle once the wizard is opened.
 */
const FlowDesign = defineAsyncComponent(() => import('./FlowDesign.vue'));

interface BasicDraft {
  businessKeyHint: string;
  category: string;
  description: string;
  displayName: string;
  icon: string;
  name: string;
}

interface FlowDesignExpose {
  getData: () => AntFlowNode | null;
  validatePublish?: () => Array<{
    key: string;
    label: string;
    message: string;
  }>;
}

const templatesApi = useApprovalTemplatesApi();
const formsApi = useApprovalFormsApi();

const steps = [
  { title: '基础设置' },
  { title: '表单设计' },
  { title: '流程设计' },
];

const currentStep = ref(0);
const currentTemplateId = ref<null | string>(props.templateId ?? null);
const loading = ref(false);
const saving = ref(false);
const publishing = ref(false);
const published = ref(false);
const useForm = ref(true);
const formId = ref<null | string>(null);
const flowFormFields = ref<FormFieldDto[]>([]);
const formName = ref('');
const formCode = ref('');
const forms = ref<Array<{ label: string; value: string }>>([]);
const antFlowRoot = ref<AntFlowNode | null>(null);
const formDesignerRef = ref<InstanceType<typeof FormDesigner> | null>(null);
const flowDesignRef = ref<FlowDesignExpose | null>(null);

const basic = reactive<BasicDraft>({
  businessKeyHint: '',
  category: '',
  description: '',
  displayName: '',
  icon: '',
  name: '',
});

const formOptions = computed(() => [
  { label: '新建表单', value: '' },
  ...forms.value,
]);

const pageTitle = computed(() => basic.displayName || '新建审批流程');

const boundFormLabel = computed(
  () => forms.value.find((item) => item.value === formId.value)?.label ?? '',
);

onMounted(async () => {
  await Promise.all([loadForms(), loadTemplate()]);
});

watch(
  () => props.templateId,
  async (value) => {
    if (!value || value === currentTemplateId.value) {
      return;
    }
    currentTemplateId.value = value;
    await loadTemplate();
  },
);

async function loadForms() {
  try {
    const result = await formsApi.getListApi();
    forms.value = (result.items ?? []).map((item) => ({
      label: `${item.name} (v${item.version})`,
      value: item.id,
    }));
  } catch {
    forms.value = [];
  }
}

async function loadTemplate() {
  const templateId = currentTemplateId.value;
  if (!templateId) {
    antFlowRoot.value = null;
    return;
  }

  loading.value = true;
  try {
    const template = await templatesApi.getApi(templateId);
    basic.name = template.name;
    basic.displayName = template.displayName;
    basic.description = template.description ?? '';
    basic.category = template.category ?? '';
    basic.icon = template.icon ?? '';
    basic.businessKeyHint = template.businessKeyHint ?? '';
    formId.value = template.formDefinitionId ?? null;
    useForm.value = !!template.formDefinitionId;
    if (!template.formDefinitionId) {
      formName.value = template.displayName;
      formCode.value = template.name;
    }
    published.value = template.isPublished;
    try {
      const parsed = template.flowDefinitionJson
        ? JSON.parse(template.flowDefinitionJson)
        : null;
      antFlowRoot.value = parsed?.nodeType
        ? parsed
        : oldToAntFlow(template.flowDefinitionJson);
    } catch {
      antFlowRoot.value = oldToAntFlow(template.flowDefinitionJson);
    }
    await nextTick();
    formDesignerRef.value?.loadForm?.(formId.value, true);
  } catch {
    message.error('审批模板加载失败');
  } finally {
    loading.value = false;
  }
}

function toNullable(value: string): null | string {
  const text = value.trim();
  return text.length > 0 ? text : null;
}

function onBasicUpdate(value: BasicDraft) {
  Object.assign(basic, value);
}

function onFormChange(value: unknown) {
  formId.value = value ? String(value) : null;
  useForm.value = true;
}

function syncFlowFields() {
  if (!useForm.value) {
    flowFormFields.value = [];
    return;
  }
  const draftFields = formDesignerRef.value?.getFields?.();
  if (draftFields && draftFields.length > 0) {
    flowFormFields.value = draftFields;
  }
}

function goStep(target: number) {
  if (target < 0 || target >= steps.length) {
    return;
  }
  if (target === 2) {
    syncFlowFields();
  }
  currentStep.value = target;
}

async function saveBasic(): Promise<boolean> {
  if (!basic.displayName.trim()) {
    message.warning('请填写审批名称');
    return false;
  }

  try {
    if (currentTemplateId.value) {
      await templatesApi.updateApi(currentTemplateId.value, {
        businessKeyHint: toNullable(basic.businessKeyHint),
        category: toNullable(basic.category),
        description: toNullable(basic.description),
        displayName: basic.displayName.trim(),
        icon: toNullable(basic.icon),
      });
    } else {
      if (!basic.name.trim()) {
        message.warning('请填写流程编号');
        return false;
      }

      const created = await templatesApi.createApi({
        businessKeyHint: toNullable(basic.businessKeyHint),
        category: toNullable(basic.category),
        description: toNullable(basic.description),
        displayName: basic.displayName.trim(),
        icon: toNullable(basic.icon),
        name: basic.name.trim(),
      });

      currentTemplateId.value = created.id;
      emit('update:templateId', created.id);
      if (!formName.value) {
        formName.value = created.displayName;
      }
      if (!formCode.value) {
        formCode.value = created.name;
      }
    }
    return true;
  } catch {
    message.error('基础信息保存失败');
    return false;
  }
}

async function saveForm(publishForm = true): Promise<boolean> {
  if (!useForm.value) {
    formId.value = null;
    return true;
  }
  if (props.readonly) {
    return true;
  }

  // `FormDesigner.save()` returns the current definition untouched when it is
  // not dirty, which is what keeps published forms from being republished on
  // every template save.
  const designer = formDesignerRef.value;
  if (!designer) {
    return !!formId.value;
  }

  const savedId = await designer.save(publishForm);
  if (!savedId) {
    return false;
  }
  formId.value = savedId;
  return true;
}

function getFlowRoot(): AntFlowNode | null {
  return flowDesignRef.value?.getData() ?? antFlowRoot.value;
}

watch(
  formId,
  async (id) => {
    if (!id) {
      flowFormFields.value = [];
      return;
    }
    try {
      const form = await formsApi.getApi(id);
      flowFormFields.value = form.fields ?? [];
    } catch {
      flowFormFields.value = [];
    }
  },
  { immediate: true },
);
async function saveDesign(): Promise<boolean> {
  const templateId = currentTemplateId.value;
  if (!templateId) {
    return false;
  }

  const root = getFlowRoot();
  if (!root) {
    currentStep.value = 2;
    message.warning('请先完成流程设计');
    return false;
  }

  try {
    await templatesApi.saveDesignApi(templateId, {
      flowDefinitionJson: JSON.stringify(root),
      formDefinitionId: useForm.value ? formId.value : null,
    });
    return true;
  } catch {
    message.error('流程设计保存失败');
    return false;
  }
}

async function save() {
  if (props.readonly) {
    return;
  }

  saving.value = true;
  try {
    if (!(await saveBasic())) {
      return;
    }
    if (!(await saveForm(false))) {
      return;
    }
    if (!(await saveDesign())) {
      return;
    }
    message.success('审批流程已保存');
  } finally {
    saving.value = false;
  }
}

async function publish() {
  if (props.readonly) {
    return;
  }

  publishing.value = true;
  try {
    if (!(await saveBasic())) {
      return;
    }
    if (!(await saveForm())) {
      return;
    }

    const errors = flowDesignRef.value?.validatePublish?.() ?? [];
    if (errors.length > 0) {
      currentStep.value = 2;
      message.warning(`流程设计还有 ${errors.length} 个节点未完善`);
      return;
    }

    if (!(await saveDesign())) {
      return;
    }

    const templateId = currentTemplateId.value;
    if (!templateId) {
      return;
    }

    await templatesApi.publishApi(templateId, {
      formDefinitionId: useForm.value ? formId.value : null,
    });
    published.value = true;
    message.success('审批模板已发布');
    emit('done', templateId);
  } catch {
    message.error('发布失败');
  } finally {
    publishing.value = false;
  }
}

function cancel() {
  emit('cancel');
}
</script>

<template>
  <Modal
    :body-style="{ padding: 0 }"
    :closable="false"
    :footer="null"
    :mask-closable="false"
    :open="true"
    width="100vw"
    wrap-class-name="approval-wizard-modal"
    @cancel="cancel"
  >
    <div class="wizard-shell">
      <header class="top-nav">
        <div class="nav-left">
          <span class="process-title" :title="pageTitle">{{ pageTitle }}</span>
          <Tag v-if="published" color="green">已发布</Tag>
          <Tag v-if="readonly" color="blue">只读</Tag>
        </div>

        <div
          class="nav-steps"
          :class="`step-${currentStep}`"
          aria-label="审批流程步骤"
          role="list"
        >
          <div aria-hidden="true" class="step-active-bg"></div>
          <div
            v-for="(item, index) in steps"
            :key="item.title"
            class="step-item"
            :class="{
              active: currentStep === index,
              done: currentStep > index,
            }"
            role="listitem"
            @click="goStep(index)"
          >
            <span class="step-index">{{ index + 1 }}</span>
            <span class="step-text">{{ item.title }}</span>
          </div>
        </div>

        <div class="nav-right">
          <Button class="ghost-btn" @click="cancel">
            {{ readonly ? '关闭' : '取消' }}
          </Button>
          <template v-if="!readonly">
            <Button class="ghost-btn" :loading="saving" @click="save">
              保存
            </Button>
            <Button
              class="publish-btn"
              :loading="publishing"
              type="primary"
              @click="publish"
            >
              发布
            </Button>
          </template>
        </div>
      </header>

      <section class="content-panel">
        <Spin :spinning="loading">
          <div v-show="currentStep === 0" class="step-page">
            <BasicSetting
              :disabled="readonly"
              :is-edit="!!currentTemplateId"
              :model-value="basic"
              @update:model-value="onBasicUpdate"
            />
          </div>

          <div v-show="currentStep === 1" class="step-page">
            <Card class="form-step" :bordered="false">
              <div class="form-toolbar">
                <Space>
                  <span class="toolbar-label">启用表单</span>
                  <Switch v-model:checked="useForm" :disabled="readonly" />
                </Space>
                <Space>
                  <span class="toolbar-label">选择已有表单</span>
                  <Select
                    class="form-select"
                    :disabled="readonly || !useForm"
                    :options="formOptions"
                    :value="formId ?? ''"
                    @change="onFormChange"
                  />
                </Space>
                <Tag v-if="formId" color="green">已绑定表单</Tag>
                <Tag v-else-if="useForm" color="orange">未保存</Tag>
              </div>

              <template v-if="useForm">
                <FormDesigner
                  v-if="!readonly"
                  :key="formId ?? 'new'"
                  ref="formDesignerRef"
                  v-model:code="formCode"
                  v-model:form-id="formId"
                  v-model:name="formName"
                />
                <Card v-else size="small" title="表单信息">
                  <Descriptions bordered :column="1" size="small">
                    <DescriptionsItem label="表单名称">
                      {{ formName || boundFormLabel || '未命名表单' }}
                    </DescriptionsItem>
                    <DescriptionsItem label="表单编码">
                      {{ formCode || '-' }}
                    </DescriptionsItem>
                    <DescriptionsItem label="表单 ID">
                      {{ formId || '未绑定' }}
                    </DescriptionsItem>
                  </Descriptions>
                </Card>
              </template>
              <Result
                v-else
                status="info"
                sub-title="本审批不需要填写表单，将直接进入流程。"
                title="未启用表单"
              />
            </Card>
          </div>

          <div v-show="currentStep === 2" class="step-page is-flow">
            <FlowDesign
              ref="flowDesignRef"
              v-model:value="antFlowRoot"
              :form-fields="flowFormFields"
              :readonly="readonly"
            />
          </div>
        </Spin>
      </section>
    </div>
  </Modal>
</template>

<style scoped>
@media (max-width: 1100px) {
  .top-nav {
    grid-template-columns: minmax(120px, 200px) minmax(360px, 1fr) auto;
    gap: 10px;
    height: 56px;
    padding: 0 12px;
  }

  .nav-steps {
    min-width: 360px;
    height: 56px;
  }

  .process-title {
    max-width: 160px;
    font-size: 16px;
  }

  .step-page.is-flow,
  .step-page.is-flow :deep(.review-warp),
  .step-page.is-flow :deep(.flow-viewport) {
    height: calc(100vh - 112px);
  }
}

@media (max-width: 860px) {
  .top-nav {
    grid-template-columns: minmax(100px, 160px) minmax(280px, 1fr) auto;
  }

  .nav-right {
    gap: 8px;
  }

  .nav-right :deep(.ant-btn) {
    padding: 0 10px;
  }

  .step-text {
    font-size: 13px;
  }

  .step-index {
    width: 20px;
    height: 20px;
    font-size: 11px;
  }
}

@media (max-width: 640px) {
  .form-step {
    margin: 8px;
  }

  .form-select {
    width: 100%;
  }
}

.wizard-shell {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background: linear-gradient(180deg, #eef4ff 0%, #f9fbff 56%, #fff 100%);
}

/* ===== AntFlow-style top bar ===== */
.top-nav {
  position: sticky;
  top: 0;
  z-index: 30;
  display: grid;
  grid-template-columns: minmax(160px, 260px) minmax(420px, 1fr) auto;
  gap: 16px;
  align-items: center;
  height: 65px;
  padding: 0 18px;
  overflow: auto hidden;
  background: linear-gradient(90deg, #2d82da 0%, #3f97eb 100%);
  box-shadow: 0 6px 16px rgb(34 101 189 / 24%);
}

.nav-left {
  display: flex;
  gap: 8px;
  align-items: center;
  min-width: 0;
  color: #fff;
  white-space: nowrap;
}

.process-title {
  max-width: 200px;
  overflow: hidden;
  text-overflow: ellipsis;
  font-size: 18px;
  font-weight: 600;
  letter-spacing: 0.5px;
  white-space: nowrap;
}

.nav-steps {
  position: relative;
  display: flex;
  width: min(480px, 100%);
  min-width: 420px;
  height: 65px;
  margin: 0 auto;
  overflow: hidden;
}

.step-active-bg {
  position: absolute;
  top: 0;
  left: 0;
  width: calc(100% / 3);
  height: 100%;
  background: rgb(76 118 230 / 36%);
  transition: transform 0.28s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.step-active-bg::after {
  position: absolute;
  bottom: -1px;
  left: 50%;
  width: 0;
  height: 0;
  content: '';
  border-right: 7px solid transparent;
  border-bottom: 8px solid #dfe7f6;
  border-left: 7px solid transparent;
  transform: translateX(-50%);
}

.nav-steps.step-1 .step-active-bg {
  transform: translateX(100%);
}

.nav-steps.step-2 .step-active-bg {
  transform: translateX(200%);
}

.step-item {
  position: relative;
  z-index: 1;
  display: flex;
  flex: 1;
  gap: 6px;
  align-items: center;
  justify-content: center;
  color: #fff;
  cursor: pointer;
  user-select: none;
}

.step-index {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 24px;
  height: 24px;
  font-size: 12px;
  line-height: 1;
  color: #fff;
  background: rgb(255 255 255 / 8%);
  border: 1px solid #fff;
  border-radius: 50%;
  transition: background-color 0.22s;
}

.step-item.active .step-index {
  background: rgb(255 255 255 / 24%);
}

.step-text {
  font-size: 15px;
  font-weight: 500;
  white-space: nowrap;
}

.nav-right {
  display: flex;
  gap: 10px;
  align-items: center;
  justify-content: flex-end;
  white-space: nowrap;
}

.nav-right :deep(.ant-btn) {
  color: #fff;
  background: rgb(255 255 255 / 14%);
  border-color: rgb(255 255 255 / 55%);
}

.nav-right :deep(.ant-btn:hover) {
  color: #1677ff;
  background: #fff;
  border-color: #fff;
}

.nav-right :deep(.ant-btn-primary) {
  color: #1677ff;
  background: #fff;
  border-color: #fff;
}

.nav-right :deep(.ant-btn-primary:hover) {
  color: #2f87df;
  background: #f4f8ff;
  border-color: #f4f8ff;
}

.nav-right :deep(.publish-btn) {
  min-width: 82px;
  font-weight: 600;
}

/* ===== Content ===== */
.content-panel {
  position: relative;
  flex: 1;
  min-height: 0;
}

.step-page {
  width: 100%;
}

.step-page.is-flow {
  height: calc(100vh - 121px);
  min-height: 600px;
  overflow: hidden;
}

.step-page.is-flow :deep(.review-warp) {
  height: calc(100vh - 121px);
  min-height: 600px;
  padding: 0;
}

.step-page.is-flow :deep(.flow-viewport) {
  height: calc(100vh - 121px);
  min-height: 600px;
}

.form-step {
  max-width: 1100px;
  min-height: calc(100vh - 160px);
  margin: 20px auto;
  background: #fff;
  border-radius: 8px;
}

.form-toolbar {
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
  align-items: center;
  padding-bottom: 12px;
  margin-bottom: 12px;
  border-bottom: 1px solid #ebeef5;
}

.toolbar-label {
  font-size: 13px;
  color: #4f5b6f;
}

.form-select {
  width: 280px;
}
</style>
<style>
.approval-wizard-modal {
  padding: 0;
}

.approval-wizard-modal .ant-modal {
  top: 0;
  max-width: 100vw;
  padding-bottom: 0;
  margin: 0;
}

.approval-wizard-modal .ant-modal-content {
  min-height: 100vh;
  border-radius: 0;
}

.approval-wizard-modal .ant-modal-body {
  padding: 0;
}
</style>
