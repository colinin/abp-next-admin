<template>
  <div>
    <Form layout="vertical">
      <FormItem label="⚙ 选择审批对象" name="text" class="user-type">
        <RadioGroup v-model:value="nodeProps.assignedType">
          <Radio v-for="t in state.approvalTypes" :value="t.type" :key="t.type">{{ t.name }}</Radio>
        </RadioGroup>
        <div v-if="nodeProps.assignedType === 'ASSIGN_USER'">
          <Button size="small" type="primary" @click="handleSelectUser" round>
            <template #icon>
              <PlusOutlined />
            </template>
            选择人员
          </Button>
          <OrgItems v-model:value="nodeProps.assignedUser" />
        </div>
        <div v-else-if="nodeProps.assignedType === 'SELF_SELECT'">
          <RadioGroup size="small" v-model:value="nodeProps.selfSelect.multiple">
            <RadioButton :value="false">自选一个人</RadioButton>
            <RadioButton :value="true">自选多个人</RadioButton>
          </RadioGroup>
        </div>
        <div v-else-if="nodeProps.assignedType === 'ROLE'">
          <Button size="small" type="primary" @click="handleSelectRole" round>
            <template #icon>
              <PlusOutlined />
            </template>
            选择系统角色
          </Button>
          <OrgItems v-model:value="nodeProps.role" />
        </div>
        <div v-else-if="nodeProps.assignedType === 'FORM_USER'">
          <FormItem label="选择表单联系人项" name="text" class="approve-end">
            <Select
              style="width: 80%"
              size="small"
              v-model:value="nodeProps.formUser"
              placeholder="请选择包含联系人的表单项"
            >
              <SelectOption v-for="op in forms" :value="op.id">{{ op.title }}</SelectOption>
            </Select>
          </FormItem>
        </div>
        <div v-else>
          <span class="item-desc">发起人自己作为审批人进行审批</span>
        </div>
      </FormItem>

      <Divider></Divider>
      <FormItem label="👤 审批人为空时" name="text" class="line-mode">
        <RadioGroup v-model:value="nodeProps.nobody.handler">
          <Radio value="TO_PASS">自动通过</Radio>
          <Radio value="TO_REFUSE">自动驳回</Radio>
          <Radio value="TO_ADMIN">转交审批管理员</Radio>
          <Radio value="TO_USER">转交到指定人员</Radio>
        </RadioGroup>

        <div style="margin-top: 10px" v-if="nodeProps.nobody.handler === 'TO_USER'">
          <Button size="small" type="primary" @click="handleSelectNoSetUser" round>
            <template #icon>
              <PlusOutlined />
            </template>
            选择人员</Button
          >
          <OrgItems v-model:value="nodeProps.assignedUser" />
        </div>
      </FormItem>

      <div v-if="showMode">
        <Divider />
        <FormItem label="👩‍👦‍👦 多人审批时审批方式" name="text" class="approve-mode">
          <RadioGroup v-model:value="nodeProps.mode">
            <Radio value="NEXT">会签 （按选择顺序审批，每个人必须同意）</Radio>
            <Radio value="AND">会签（可同时审批，每个人必须同意）</Radio>
            <Radio value="OR">或签（有一人同意即可）</Radio>
          </RadioGroup>
        </FormItem>
      </div>

      <Divider>高级设置</Divider>
      <FormItem label="✍ 审批同意时是否需要签字" name="text">
        <Switch
          checked-children="需要"
          un-checked-children="不用"
          v-model:checked="nodeProps.sign"
        ></Switch>
        <Tooltip
          class="item"
          effect="dark"
          content="如果全局设置了需要签字，则此处不生效"
          placement="top"
        >
          <QuestionOutlined style="margin-left: 10px; font-size: medium; color: #b0b0b1" />
        </Tooltip>
      </FormItem>
      <FormItem label="⏱ 审批期限（为 0 则不生效）" name="timeLimit">
        <InputGroup style="width: 180px" compact>
          <Input
            style="width: 100px"
            placeholder="时长"
            size="small"
            type="number"
            v-model:value="nodeProps.timeLimit.timeout.value"
          />
          <Select
            style="width: 75px"
            v-model:value="nodeProps.timeLimit.timeout.unit"
            size="small"
            placeholder="请选择"
          >
            <SelectOption value="D">天</SelectOption>
            <SelectOption value="H">小时</SelectOption>
            <SelectOption value="M">分钟</SelectOption>
          </Select>
        </InputGroup>
      </FormItem>
      <FormItem
        label="审批期限超时后执行"
        name="level"
        v-if="nodeProps.timeLimit.timeout.value > 0"
      >
        <RadioGroup v-model:value="nodeProps.timeLimit.handler.type">
          <Radio value="PASS">自动通过</Radio>
          <Radio value="REFUSE">自动驳回</Radio>
          <Radio value="NOTIFY">发送提醒</Radio>
        </RadioGroup>
        <div v-if="nodeProps.timeLimit.handler.type === 'NOTIFY'">
          <div style="color: #409eef; font-size: small">默认提醒当前审批人</div>
          <Switch
            checked-children="一次"
            un-checked-children="循环"
            v-model:checked="nodeProps.timeLimit.handler.notify.once"
          ></Switch>
          <span style="margin-left: 20px" v-if="!nodeProps.timeLimit.handler.notify.once">
            每隔
            <InputNumber
              :min="0"
              :max="10000"
              :step="1"
              size="small"
              v-model:value="nodeProps.timeLimit.handler.notify.hour"
            />
            小时提醒一次
          </span>
        </div>
      </FormItem>
      <FormItem label="🙅‍ 如果审批被驳回 👇">
        <RadioGroup v-model:value="nodeProps.refuse.type">
          <Radio value="TO_END">直接结束流程</Radio>
          <Radio value="TO_BEFORE">驳回到上级审批节点</Radio>
          <Radio value="TO_NODE">驳回到指定节点</Radio>
        </RadioGroup>
        <div v-if="nodeProps.refuse.type === 'TO_NODE'">
          <span>指定节点:</span>
          <Select
            style="margin-left: 10px; width: 150px"
            placeholder="选择跳转步骤"
            size="small"
            v-model:value="nodeProps.refuse.target"
          >
            <SelectOption v-for="(node, i) in nodeOptions" :key="i" :value="node.id">{{
              node.name
            }}</SelectOption>
          </Select>
        </div>
      </FormItem>
    </Form>
    <OrgPicker
      multiple
      :title="pickerTitle"
      :type="state.orgPickerType"
      ref="orgPickerRef"
      :selected="state.orgPickerSelected"
      @ok="selected"
    />
  </div>
</template>

<script setup lang="ts">
  import { computed, nextTick, reactive, ref, unref } from 'vue';
  import {
    Button,
    Divider,
    Form,
    Radio,
    Input,
    InputNumber,
    Select,
    Switch,
    Tooltip,
  } from 'ant-design-vue';
  import { PlusOutlined, QuestionOutlined } from '@ant-design/icons-vue';
  import { useFlowStoreWithOut } from '/@/store/modules/flow';
  import OrgPicker from '../OrgPicker.vue';
  import OrgItems from '../OrgItems.vue';

  const FormItem = Form.Item;
  const InputGroup = Input.Group;
  const RadioGroup = Radio.Group;
  const RadioButton = Radio.Button;
  const SelectOption = Select.Option;

  const props = defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
  });
  const orgPickerRef = ref<any>();
  const flowStore = useFlowStoreWithOut();

  const nodeProps = computed(() => {
    return flowStore.selectedNode.props;
  });

  const selectUser = computed(() => {
    return props.config.assignedUser || [];
  });

  const selectRole = computed(() => {
    return props.config.role || [];
  });

  const forms = computed(() => {
    return flowStore.design.formItems.filter((f) => {
      return f.name === 'UserPicker';
    });
  });

  const pickerTitle = computed(() => {
    switch (state.orgPickerType) {
      case 'user':
        return '请选择人员';
      case 'role':
        return '请选择系统角色';
      default:
        return undefined;
    }
  });

  const nodeOptions = computed(() => {
    let values: any[] = [];
    const excType = ['ROOT', 'EMPTY', 'CONDITION', 'CONDITIONS', 'CONCURRENT', 'CONCURRENTS'];
    flowStore.nodeMap.forEach((v) => {
      if (excType.indexOf(v.type) === -1) {
        values.push({ id: v.id, name: v.name });
      }
    });
    return values;
  });

  const showMode = computed(() => {
    const node = unref(nodeProps);
    switch (node.assignedType) {
      case 'ASSIGN_USER':
        return node.assignedUser.length > 0;
      case 'SELF_SELECT':
        return node.selfSelect.multiple;
      case 'FORM_USER':
        return true;
      case 'ROLE':
        return true;
      default:
        return false;
    }
  });

  const state = reactive({
    showOrgSelect: false,
    orgPickerSelected: [] as any[],
    orgPickerType: 'user',
    approvalTypes: [
      { name: '指定人员', type: 'ASSIGN_USER' },
      { name: '发起人自选', type: 'SELF_SELECT' },
      { name: '角色', type: 'ROLE' },
      { name: '发起人自己', type: 'SELF' },
      { name: '表单内联系人', type: 'FORM_USER' },
    ],
  });

  function handleSelectUser() {
    state.orgPickerSelected = unref(selectUser);
    state.orgPickerType = 'user';
    nextTick(() => {
      const orgPicker = unref(orgPickerRef);
      orgPicker?.show();
    });
  }

  function handleSelectNoSetUser() {
    state.orgPickerSelected = props.config.nobody.assignedUser;
    state.orgPickerType = 'user';
    nextTick(() => {
      const orgPicker = unref(orgPickerRef);
      orgPicker?.show();
    });
  }

  function handleSelectRole() {
    state.orgPickerSelected = unref(selectRole);
    state.orgPickerType = 'role';
    nextTick(() => {
      const orgPicker = unref(orgPickerRef);
      orgPicker?.show();
    });
  }

  function selected(select) {
    switch (state.orgPickerType) {
      case 'user':
        nodeProps.value.role = [];
        nodeProps.value.assignedUser = select;
        state.orgPickerSelected.length = 0;
        select.forEach((val) => state.orgPickerSelected.push(val));
        break;
      case 'role':
        nodeProps.value.assignedUser = [];
        nodeProps.value.role = select;
        state.orgPickerSelected.length = 0;
        select.forEach((val) => state.orgPickerSelected.push(val));
        break;
    }
  }

  function removeUserItem(index) {
    selectUser.value.splice(index, 1);
  }

  function removeRoleItem(index) {
    selectRole.value.splice(index, 1);
  }

  defineExpose({
    removeUserItem,
    removeRoleItem,
  });
</script>

<style lang="less" scoped>
  .user-type {
    :deep(.a-radio) {
      width: 110px;
      margin-top: 10px;
      margin-bottom: 20px;
    }
  }

  :deep(.line-mode) {
    .a-radio {
      width: 150px;
      margin: 5px;
    }
  }

  :deep(.a-form-item__label) {
    line-height: 25px;
  }

  :deep(.approve-mode) {
    .a-radio {
      float: left;
      width: 100%;
      display: block;
      margin-top: 15px;
    }
  }

  :deep(.approve-end) {
    position: relative;

    .a-radio-group {
      width: 160px;
    }

    .a-radio {
      margin-bottom: 5px;
      width: 100%;
    }

    .approve-end-leave {
      position: absolute;
      bottom: -5px;
      left: 150px;
    }
  }

  :deep(.a-divider--horizontal) {
    margin: 10px 0;
  }
</style>
