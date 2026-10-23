<script setup lang="ts">
import type { ApprovalFlowModel } from '../../types/approval-flow';

import { computed, onMounted, reactive, ref } from 'vue';

import {
  Button,
  Card,
  Descriptions,
  DescriptionsItem,
  Input,
  message,
  Result,
  Select,
  Space,
  Switch,
  Tag,
} from 'ant-design-vue';

import { useApprovalTemplatesApi } from '../../api/useApprovalFlowApi';
import { useApprovalFormsApi } from '../../api/useApprovalParticipantsApi';
import ApprovalFlowDesigner from './ApprovalFlowDesigner.vue';
import FormDesigner from './FormDesigner.vue';

const props = defineProps<{
  templateId?: null | string;
}>();

const emit = defineEmits<{
  cancel: [];
  done: [string];
  'update:templateId': [string];
}>();

const templatesApi = useApprovalTemplatesApi();
const formsApi = useApprovalFormsApi();

const step = ref(0);
const maxVisited = ref(0);
const currentTemplateId = ref<null | string>(props.templateId ?? null);
const loading = ref(false);
const saving = ref(false);
const published = ref(false);
const useForm = ref(true);
const formId = ref<null | string>(null);
const formName = ref('');
const formCode = ref('');
const formDesigner = ref<InstanceType<typeof FormDesigner> | null>(null);
const flowDesigner = ref<InstanceType<typeof ApprovalFlowDesigner> | null>(
  null,
);
const forms = ref<Array<{ label: string; value: string }>>([]);
const summary = reactive({ approvals: 0, cc: 0, nodes: 0 });

const basic = reactive({
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

const steps = [
  { title: '基础设置' },
  { title: '表单设计' },
  { title: '流程设计' },
  { title: '发布' },
];

onMounted(async () => {
  await Promise.all([loadForms(), loadTemplate()]);
});

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
  if (!currentTemplateId.value) {
    return;
  }

  loading.value = true;
  try {
    const template = await templatesApi.getApi(currentTemplateId.value);
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
  } catch {
    message.error('审批模板加载失败');
  } finally {
    loading.value = false;
  }
}

async function saveBasic(): Promise<boolean> {
  if (!basic.displayName.trim()) {
    message.warning('请填写模板名称');
    return false;
  }

  saving.value = true;
  try {
    if (currentTemplateId.value) {
      await templatesApi.updateApi(currentTemplateId.value, {
        businessKeyHint: basic.businessKeyHint || null,
        category: basic.category || null,
        description: basic.description || null,
        displayName: basic.displayName.trim(),
        icon: basic.icon || null,
      });
    } else {
      if (!basic.name.trim()) {
        message.warning('请填写模板编码');
        return false;
      }
      const created = await templatesApi.createApi({
        businessKeyHint: basic.businessKeyHint || null,
        category: basic.category || null,
        description: basic.description || null,
        displayName: basic.displayName.trim(),
        icon: basic.icon || null,
        name: basic.name.trim(),
      });
      currentTemplateId.value = created.id;
      emit('update:templateId', created.id);
      formCode.value = created.name;
      formName.value = created.displayName;
    }
    return true;
  } catch {
    message.error('基础信息保存失败');
    return false;
  } finally {
    saving.value = false;
  }
}

async function saveForm(): Promise<boolean> {
  if (!useForm.value) {
    formId.value = null;
    return true;
  }

  const savedId = await formDesigner.value?.save();
  if (!savedId) {
    return false;
  }
  formId.value = savedId;
  return true;
}

async function saveFlow(): Promise<boolean> {
  const templateId = currentTemplateId.value;
  const json = flowDesigner.value?.getModelJson();
  if (!templateId || !json) {
    message.warning('流程设计尚未准备好');
    return false;
  }

  const model = JSON.parse(json) as ApprovalFlowModel;
  model.formDefinitionId = useForm.value ? formId.value : null;

  saving.value = true;
  try {
    await templatesApi.saveDesignApi(templateId, {
      flowDefinitionJson: JSON.stringify(model),
    });
    summary.nodes = model.nodes.length;
    summary.approvals = model.nodes.filter(
      (node) => node.kind === 'approval',
    ).length;
    summary.cc = model.nodes.filter((node) => node.kind === 'cc').length;
    return true;
  } catch {
    message.error('流程设计保存失败');
    return false;
  } finally {
    saving.value = false;
  }
}

async function next() {
  if (step.value === 0) {
    if (await saveBasic()) {
      goTo(1);
    }
    return;
  }

  if (step.value === 1) {
    if (await saveForm()) {
      goTo(2);
    }
    return;
  }

  if (step.value === 2 && (await saveFlow())) {
    goTo(3);
  }
}

function goTo(target: number) {
  step.value = target;
  maxVisited.value = Math.max(maxVisited.value, target);
}

function goStep(target: number) {
  if (target === step.value) {
    return;
  }
  if (target > maxVisited.value) {
    return;
  }
  step.value = target;
}

function cancel() {
  emit('cancel');
}

async function publish() {
  const templateId = currentTemplateId.value;
  if (!templateId) {
    return;
  }

  saving.value = true;
  try {
    await templatesApi.publishApi(templateId, {
      formDefinitionId: useForm.value ? formId.value : null,
    });
    published.value = true;
    message.success('审批模板已发布');
  } catch {
    message.error('发布失败');
  } finally {
    saving.value = false;
  }
}

function onExistingFormChange(value: any) {
  formId.value = value ? String(value) : null;
  useForm.value = true;
}

function finish() {
  if (currentTemplateId.value) {
    emit('done', currentTemplateId.value);
  }
}
</script>

<template>
  <div class="workflow-shell">
    <header class="top-nav">
      <div class="nav-left">
        <span class="process-title">{{
          basic.displayName || '新建审批流程'
        }}</span>
        <Tag v-if="published" color="green">已发布</Tag>
      </div>

      <div class="nav-steps" :class="`step-${step}`">
        <div class="step-active-bg" aria-hidden="true"></div>
        <div
          v-for="(item, index) in steps"
          :key="item.title"
          class="step-item"
          :class="{ active: step === index, done: step > index }"
          @click="goStep(index)"
        >
          <span class="step-index">{{ index + 1 }}</span>
          <span class="step-text">{{ item.title }}</span>
        </div>
      </div>

      <div class="nav-right">
        <Button @click="cancel">取消</Button>
        <Button
          v-if="step === 3 && !published"
          :loading="saving"
          type="primary"
          @click="publish"
        >
          确认发布
        </Button>
        <Button
          v-else-if="published && step === 3"
          type="primary"
          @click="finish"
        >
          完成
        </Button>
        <Button v-else :loading="saving" type="primary" @click="next">
          下一步
        </Button>
      </div>
    </header>

    <section class="content-panel">
      <div v-if="published" class="result-wrap">
        <Result
          status="success"
          sub-title="审批模板已发布，可在审批模板列表中直接发起审批。"
          title="发布成功"
        >
          <template #extra>
            <Button type="primary" @click="finish">完成</Button>
          </template>
        </Result>
      </div>

      <div v-else class="step-page" :class="{ 'is-flow': step === 2 }">
        <div v-if="step === 0" class="basic-setting-page">
          <div class="basic-setting-wrap" :class="{ 'is-loading': loading }">
            <div class="setting-form">
              <div class="setting-item">
                <label>流程编号</label>
                <Input
                  v-model:value="basic.name"
                  :disabled="!!currentTemplateId"
                  placeholder="例如：leave-approval"
                />
              </div>
              <div class="setting-item">
                <label>审批名称</label>
                <Input
                  v-model:value="basic.displayName"
                  placeholder="例如：请假审批"
                />
              </div>
              <div class="setting-item">
                <label>选择分组</label>
                <Input
                  v-model:value="basic.category"
                  placeholder="例如：人事"
                />
              </div>
              <div class="setting-item">
                <label>模板图标</label>
                <Input v-model:value="basic.icon" placeholder="可选" />
              </div>
              <div class="setting-item">
                <label>业务键提示</label>
                <Input
                  v-model:value="basic.businessKeyHint"
                  placeholder="发起时业务键的填写提示"
                />
              </div>
              <div class="setting-item is-textarea">
                <label>审批说明</label>
                <Input.TextArea v-model:value="basic.description" :rows="4" />
              </div>
            </div>
          </div>
        </div>

        <div v-else-if="step === 1" class="form-step">
          <div class="mb-3 flex flex-wrap items-center gap-4">
            <Space>
              <span class="text-sm text-gray-600">启用表单</span>
              <Switch v-model:checked="useForm" />
            </Space>
            <Space>
              <span class="text-sm text-gray-600">选择已有表单</span>
              <Select
                :disabled="!useForm"
                :options="formOptions"
                :value="formId ?? ''"
                class="w-72"
                @update:value="onExistingFormChange"
              />
            </Space>
            <Tag v-if="formId" color="green">已绑定表单</Tag>
          </div>

          <FormDesigner
            v-if="useForm"
            ref="formDesigner"
            v-model:code="formCode"
            v-model:form-id="formId"
            v-model:name="formName"
          />
          <Result
            v-else
            status="info"
            sub-title="本审批不需要填写表单，将直接进入流程。"
            title="未启用表单"
          />
        </div>

        <ApprovalFlowDesigner
          v-else-if="step === 2 && currentTemplateId"
          ref="flowDesigner"
          embedded
          :form-definition-id="formId"
          :template-id="currentTemplateId"
        />

        <Card v-else class="setting-card" title="发布确认">
          <Descriptions bordered :column="2" size="small">
            <DescriptionsItem label="模板">
              {{ basic.displayName }}
            </DescriptionsItem>
            <DescriptionsItem label="编码">{{ basic.name }}</DescriptionsItem>
            <DescriptionsItem label="分类">
              {{ basic.category || '-' }}
            </DescriptionsItem>
            <DescriptionsItem label="表单">
              {{ useForm ? (formId ? '已配置' : '未保存') : '未启用' }}
            </DescriptionsItem>
            <DescriptionsItem label="节点数">
              {{ summary.nodes }}
            </DescriptionsItem>
            <DescriptionsItem label="审批节点">
              {{ summary.approvals }}
            </DescriptionsItem>
            <DescriptionsItem label="抄送节点">
              {{ summary.cc }}
            </DescriptionsItem>
            <DescriptionsItem label="状态">待发布</DescriptionsItem>
          </Descriptions>
          <div class="publish-tip">
            点击右上角「确认发布」后，系统会根据当前流程设计自动生成并发布 Elsa
            工作流定义，无需手动填写定义版本 Id。
          </div>
        </Card>
      </div>
    </section>
  </div>
</template>

<style scoped>
.workflow-shell {
  position: fixed;
  inset: 0;
  z-index: 1200;
  display: flex;
  flex-direction: column;
  background: #f5f6f8;
}

.top-nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 60px;
  padding: 0 24px;
  background: #fff;
  border-bottom: 1px solid #ebeef5;
}

.nav-left {
  display: flex;
  gap: 10px;
  align-items: center;
  min-width: 240px;
}

.process-title {
  font-size: 16px;
  font-weight: 600;
  color: #303133;
}

.nav-steps {
  display: flex;
  gap: 28px;
  align-items: center;
}

.step-item {
  display: flex;
  gap: 8px;
  align-items: center;
  color: #909399;
  cursor: pointer;
  user-select: none;
}

.step-item.active,
.step-item.done {
  color: #3296fa;
}

.step-index {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 22px;
  font-size: 12px;
  color: #fff;
  background: #c0c4cc;
  border-radius: 50%;
}

.step-item.active .step-index,
.step-item.done .step-index {
  background: #3296fa;
}

.step-text {
  font-size: 14px;
}

.nav-right {
  display: flex;
  gap: 10px;
  justify-content: flex-end;
  min-width: 200px;
}

.content-panel {
  flex: 1;
  padding: 20px 24px;
  overflow: auto;
}

.step-page {
  max-width: 980px;
  margin: 0 auto;
}

.step-page.is-flow {
  max-width: none;
  height: calc(100vh - 100px);
  margin: 0;
}

.setting-card {
  background: #fff;
  border-radius: 10px;
}

.form-step {
  padding: 16px;
  background: #fff;
  border-radius: 10px;
}

.result-wrap {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  background: #fff;
  border-radius: 10px;
}

.publish-tip {
  margin-top: 16px;
  font-size: 13px;
  color: #909399;
}

/* AntFlow-style step bar */
.top-nav {
  height: 56px;
  color: #fff;
  background: linear-gradient(90deg, #409eff 0%, #1677ff 100%);
  border-bottom: none;
  box-shadow: 0 2px 8px rgb(22 119 255 / 25%);
}

.process-title {
  font-size: 15px;
  color: #fff;
}

.step-item,
.step-item.active,
.step-item.done {
  color: rgb(255 255 255 / 78%);
}

.step-item.active {
  font-weight: 600;
  color: #fff;
}

.step-index {
  width: 20px;
  height: 20px;
  color: #fff;
  background: rgb(255 255 255 / 18%);
  border: 1px solid rgb(255 255 255 / 55%);
}

.step-item.active .step-index {
  color: #1677ff;
  background: #fff;
  border-color: #fff;
}

.step-item.done .step-index {
  color: #1677ff;
  background: rgb(255 255 255 / 88%);
  border-color: transparent;
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

.basic-setting-page {
  min-height: 100%;
  padding: 0 64px;
}

.basic-setting-wrap {
  box-sizing: border-box;
  min-width: 320px;
  max-width: 760px;
  min-height: calc(100vh - 120px);
  padding: 26px 64px;
  margin: 0 auto;
  background: #fff;
}

.setting-form {
  display: flex;
  flex-direction: column;
  gap: 18px;
  max-width: 520px;
  margin: 0 auto;
}

.setting-item {
  display: grid;
  grid-template-columns: 90px minmax(0, 1fr);
  gap: 12px;
  align-items: center;
}

.setting-item > label {
  font-size: 14px;
  color: #4f5b6f;
  text-align: right;
}

.setting-item.is-textarea {
  align-items: flex-start;
}

.top-nav {
  display: grid;
  grid-template-columns: minmax(160px, 260px) minmax(420px, 1fr) auto;
  gap: 16px;
  height: 65px;
  padding: 0 18px;
  background: linear-gradient(90deg, #2d82da 0%, #3f97eb 100%);
  box-shadow: 0 6px 16px rgb(34 101 189 / 24%);
}

.nav-steps {
  position: relative;
  display: flex;
  width: min(560px, 100%);
  min-width: 420px;
  height: 65px;
  margin: 0 auto;
  overflow: hidden;
}

.step-active-bg {
  position: absolute;
  top: 0;
  left: 0;
  width: calc(100% / 4);
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

.nav-steps.step-3 .step-active-bg {
  transform: translateX(300%);
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
}

.step-text {
  font-size: 15px;
}

.step-index {
  background: rgb(255 255 255 / 8%);
  border: 1px solid #fff;
}

.step-item.active .step-index {
  background: rgb(255 255 255 / 24%);
}
</style>
