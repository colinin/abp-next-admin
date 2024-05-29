<template>
  <Node
    :title="config.name"
    :show-error="state.showError"
    :content="content"
    :error-info="state.errorInfo"
    @selected="$emit('selected')"
    @delNode="$emit('delNode')"
    @insertNode="(type) => $emit('insertNode', type)"
    placeholder="请设置审批人"
    header-bgc="#ff943e"
  >
    <template #headerIcon>
      <UserOutlined />
    </template>
  </Node>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { UserOutlined } from '@ant-design/icons-vue';
  import { useFlowStoreWithOut } from '/@/store/modules/flow';
  import Node from './Node.vue';

  defineEmits(['delNode', 'insertNode', 'selected']);
  const props = defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
  });
  const content = computed(() => {
    console.log('props', props);
    const config = props.config.props;
    switch (config.assignedType) {
      case 'ASSIGN_USER':
        if (config.assignedUser.length > 0) {
          let texts: string[] = [];
          config.assignedUser.forEach((org) => texts.push(org.name));
          return String(texts).replaceAll(',', '、');
        } else {
          return '请指定审批人';
        }
      case 'SELF':
        return '发起人自己';
      case 'SELF_SELECT':
        return config.selfSelect.multiple ? '发起人自选多人' : '发起人自选一人';
      case 'FORM_USER':
        if (!config.formUser || config.formUser === '') {
          return '表单内联系人（未选择）';
        } else {
          let text = getFormItemById(config.formUser);
          if (text && text.title) {
            return `表单（${text.title}）内的人员`;
          } else {
            return '该表单已被移除😥';
          }
        }
      case 'ROLE':
        if (config.role.length > 0) {
          let texts: string[] = [];
          config.role.forEach((org) => texts.push(org.name));
          return String(texts).replaceAll(',', '、');
        } else {
          return '指定角色（未设置）';
        }
      default:
        return '未知设置项😥';
    }
  });
  const state = reactive({
    showError: false,
    errorInfo: '',
    validate: {} as any,
  });
  const flowStore = useFlowStoreWithOut();

  function getFormItemById(id: any) {
    return flowStore.design.formItems.find((item) => item.id === id);
  }

  function validate(err: string[]) {
    try {
      return (state.showError =
        !state.validate[`validate_${props.config.props.assignedType}`](err));
    } catch (e) {
      return true;
    }
  }

  function validate_ASSIGN_USER(err: string[]) {
    if (props.config.props.assignedUser.length > 0) {
      return true;
    } else {
      state.errorInfo = '请指定审批人员';
      err.push(`${props.config.name} 未指定审批人员`);
      return false;
    }
  }

  function validate_SELF_SELECT(err: string[]) {
    console.log(err);
    return true;
  }

  function validate_LEADER_TOP(err: string[]) {
    console.log(err);
    return true;
  }

  function validate_LEADER(err: string[]) {
    console.log(err);
    return true;
  }

  function validate_ROLE(err: string[]) {
    if (props.config.props.role.length <= 0) {
      state.errorInfo = '请指定负责审批的系统角色';
      err.push(`${props.config.name} 未指定审批角色`);
      return false;
    }
    return true;
  }

  function validate_SELF(err: string[]) {
    console.log(err);
    return true;
  }

  function validate_FORM_USER(err: string[]) {
    if (props.config.props.formUser === '') {
      state.errorInfo = '请指定表单中的人员组件';
      err.push(`${props.config.name} 审批人为表单中人员，但未指定`);
      return false;
    }
    return true;
  }

  function validate_REFUSE(err: string[]) {
    console.log(err);
    return true;
  }

  defineExpose({
    validate,
    validate_ASSIGN_USER,
    validate_SELF_SELECT,
    validate_LEADER_TOP,
    validate_LEADER,
    validate_ROLE,
    validate_SELF,
    validate_FORM_USER,
    validate_REFUSE,
  });
</script>

<style scoped></style>
