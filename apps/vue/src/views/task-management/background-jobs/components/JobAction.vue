<template>
  <div>
    <Card :title="L('Job:Actions')">
      <template #extra>
        <Dropdown :trigger="['click']">
          <template #overlay>
            <Menu @click="handleAddAction">
              <MenuItem v-for="action in actionDefinitions" :key="action.name">
                {{ action.displayName }}
              </MenuItem>
            </Menu>
          </template>
          <Button type="primary">
            {{ L('Job:AddAction') }}
            <DownOutlined />
          </Button>
        </Dropdown>
      </template>
      <Card v-for="actionForm in actionFormsRef" :title="actionForm.model.displayName ?? actionForm.model.name" :key="actionForm.key">
        <template #extra>
          <Button danger @click="handleDelAction(actionForm)">{{ L('Job:DeleteAction') }}</Button>
        </template>
        <BasicForm
          label-align="left"
          layout="vertical"
          :colon="true"
          :model="actionForm.model"
          :schemas="actionForm.schemas "
          :action-col-options="{
            span: 24,
          }"
          :show-reset-button="false"
          :submit-button-options="{
            text: L('Save'),
            loading: actionForm.submiting,
          }"
          @submit="(input) => handleSaveAction(actionForm, input)"
        />
      </Card>
    </Card>
  </div>
</template>

<script lang="ts" setup>
  import { ref, unref, watch } from 'vue';
  import { Button, Card, Dropdown, Menu, MenuItem } from 'ant-design-vue';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { BasicForm, FormSchema } from '/@/components/Form';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BackgroundJobAction, BackgroundJobActionDefinition, JobActionType } from '/@/api/task-management/model/backgroundJobActionModel';
  import { addAction, updateAction, deleteAction, getActions, getDefinitions as getActionDefinitions } from '/@/api/task-management/backgroundJobAction';
  import { buildUUID } from '/@/utils/uuid';

  const props = defineProps({
    jobId: {
      type: String,
      default: '',
    },
  });
  const { createConfirm } = useMessage();
  const { L } = useLocalization(['BackgroundTasks', 'TaskManagement', 'AbpUi']);
  const actionDefinitions = ref<BackgroundJobActionDefinition[]>([]);
  const actionFormsRef = ref<{
    key: string,
    submiting: boolean;
    model: BackgroundJobAction,
    schemas: FormSchema[],
  }[]>([]);

  watch(
    () => props.jobId,
    () => {
      fetchJobActionDefinitions();
    },
    {
      immediate: true,
    }
  )

  function fetchJobActionDefinitions(type?: JobActionType) {
    getActionDefinitions({ type: type }).then((res) => {
      actionDefinitions.value = res.items;
      actionFormsRef.value = [];
      fetchJobActions();
    })
  }

  function fetchJobActions() {
    if (props.jobId) {
      getActions(props.jobId).then((res) => {
        res.items.forEach((action) => {
          const actionDefinition = actionDefinitions.value.find(ad => ad.name === action.name);
          actionFormsRef.value.push({
            key: buildUUID(),
            submiting: false,
            model: {
              id: action.id,
              jobId: action.jobId,
              name: action.name,
              isEnabled: action.isEnabled,
              paramters:  action.paramters,
              displayName: actionDefinition?.displayName,
            },
            schemas: getFormSchemas(actionDefinition),
          });
        });
      });
    }
  }

  function getFormSchemas(action?: BackgroundJobActionDefinition) {
    const actionSchemas: FormSchema[] = [
      {
        field: 'id',
        label: 'id',
        component: 'Input',
        show: false,
        colProps: { span: 24 },
      },
      {
        field: 'jobId',
        label: 'jobId',
        component: 'Input',
        show: false,
        colProps: { span: 24 },
      },
      {
        field: 'name',
        label: 'name',
        component: 'Input',
        show: false,
        colProps: { span: 24 },
      },
      {
        field: 'isEnabled',
        label: '',
        component: 'Checkbox',
        colProps: { span: 24 },
        renderComponentContent: L('DisplayName:IsEnabled'),
      },
    ];
    if (action) {
      action.paramters.forEach((p) => {
        actionSchemas.push({
          field: `paramters.${p.name}`,
          label: p.displayName,
          component: 'InputTextArea',
          required: p.required,
          colProps: { span: 24 },
          componentProps: {
            rows: 1,
            showCount: true,
          },
        });
      });
    }
    return actionSchemas;
  }

  function handleAddAction(e) {
    const action = actionDefinitions.value.find(ad => ad.name === e.key);
    if (action) {
      const actionForms = unref(actionFormsRef);
      const paramters: ExtraPropertyDictionary = action.paramters.map((p) => {
        return {
          [p.name]: '',
        };
      });
      actionForms.push({
        key: buildUUID(),
        submiting: false,
        model: {
          id: '',
          jobId: props.jobId,
          name: action.name,
          displayName: action.displayName,
          isEnabled: true,
          paramters:  paramters,
        },
        schemas: getFormSchemas(action),
      });
    }
  }

  function handleSaveAction(form, input) {
    form.submiting = true;
    const api = input.id ? updateAction(input.id, input) : addAction(props.jobId, input);
    api.then((res) => {
      form.model.id = res;
    }).finally(() => {
      form.submiting = false;
    });
  }

  async function handleDelAction(form) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return new Promise((resolve, reject) => {
          if (form.model.id) {
            return deleteAction(form.model.id).then(() => {
              _deleteAction(form.key);
              return resolve(form.key);
            }).catch((error) => {
              return reject(error);
            });
          }
          _deleteAction(form.key);
          return resolve(form.key);
        });
      }
    });
  }

  function _deleteAction(key: string) {
    const index = actionFormsRef.value.findIndex(x => x.key === key);
    if (index >= 0) {
      actionFormsRef.value.splice(index, 1);
    }
  }

</script>
