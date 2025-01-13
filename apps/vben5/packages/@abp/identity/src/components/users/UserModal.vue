<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';
import type { TransferItem } from 'ant-design-vue/es/transfer';
import type { DataNode, EventDataNode } from 'ant-design-vue/es/tree';

import type { IdentityUserDto } from '../../types/users';

import { defineEmits, defineOptions, ref, toValue } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useSettings } from '@abp/core';
import {
  Checkbox,
  Form,
  Input,
  InputPassword,
  message,
  Tabs,
  Transfer,
  Tree,
} from 'ant-design-vue';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';
import { useUsersApi } from '../../api/useUsersApi';

defineOptions({
  name: 'UserModal',
});
const emits = defineEmits<{
  (event: 'change', data: IdentityUserDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

const defaultModel = {
  isActive: true,
} as IdentityUserDto;

const activedTab = ref('info');
const form = ref<FormInstance>();
/** 可分配的角色列表 */
const assignedRoles = ref<TransferItem[]>([]);
/** 组织机构 */
const organizationUnits = ref<DataNode[]>([]);
/** 已加载的组织机构Keys */
const loadedOuKeys = ref<string[]>([]);
/** 用户拥有的组织机构节点keys */
const checkedOuKeys = ref<string[]>([]);
/** 表单数据 */
const formModel = ref<IdentityUserDto>({ ...defaultModel });

const { isTrue } = useSettings();
const { hasAccessByCodes } = useAccess();
const {
  cancel,
  createApi,
  getApi,
  getAssignableRolesApi,
  getOrganizationUnitsApi,
  getRolesApi,
  updateApi,
} = useUsersApi();
const { getChildrenApi, getRootListApi } = useOrganizationUnitsApi();
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('User modal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const api = formModel.value.id
      ? updateApi(formModel.value.id, toValue(formModel))
      : createApi(toValue(formModel));
    modalApi.setState({ confirmLoading: true });
    api
      .then((res) => {
        message.success($t('AbpUi.SavedSuccessfully'));
        emits('change', res);
        modalApi.close();
      })
      .finally(() => {
        modalApi.setState({ confirmLoading: false });
      });
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      loadedOuKeys.value = [];
      assignedRoles.value = [];
      activedTab.value = 'info';
      organizationUnits.value = [];
      formModel.value = { ...defaultModel };
      modalApi.setState({
        loading: true,
        title: $t('AbpIdentity.NewUser'),
      });
      try {
        const userDto = modalApi.getData<IdentityUserDto>();
        const manageRolePolicy = checkManageRolePolicy();
        if (userDto?.id) {
          await Promise.all([
            initUserInfo(userDto.id),
            manageRolePolicy && initUserRoles(userDto.id),
            manageRolePolicy && initAssignableRoles(),
            checkManageOuPolicy() && initOrganizationUnitTree(userDto.id),
          ]);
          modalApi.setState({
            title: `${$t('AbpIdentity.Users')} - ${userDto.userName}`,
          });
          return;
        }
        manageRolePolicy && (await initAssignableRoles());
      } finally {
        modalApi.setState({
          loading: false,
        });
      }
    }
  },
  title: $t('AbpIdentity.Users'),
});

/** 检查管理角色权限 */
function checkManageRolePolicy() {
  return hasAccessByCodes(['AbpIdentity.Users.Update.ManageRoles']);
}

/** 检查管理组织机构权限 */
function checkManageOuPolicy() {
  return hasAccessByCodes(['AbpIdentity.Users.ManageOrganizationUnits']);
}

/**
 * 初始化用户信息
 * @param userId 用户id
 */
async function initUserInfo(userId: string) {
  const dto = await getApi(userId);
  formModel.value = dto;
  modalApi.setState({
    title: `${$t('AbpIdentity.Users')} - ${dto.userName}`,
  });
}

/**
 * 初始化用户角色
 * @param userId 用户id
 */
async function initUserRoles(userId: string) {
  const { items } = await getRolesApi(userId);
  formModel.value.roleNames = items.map((item) => item.name);
}

/** 初始化可用角色列表 */
async function initAssignableRoles() {
  const { items } = await getAssignableRolesApi();
  assignedRoles.value = items.map((item) => {
    return {
      key: item.name,
      title: item.name,
      ...item,
    };
  });
}

/**
 * 初始化组织机构树
 * @param userId 用户id
 */
async function initOrganizationUnitTree(userId: string) {
  const [ouResult, userOuResult] = await Promise.all([
    getRootListApi(),
    getOrganizationUnitsApi(userId),
  ]);
  organizationUnits.value = ouResult.items.map((item) => {
    return {
      isLeaf: false,
      key: item.id,
      title: item.displayName,
      children: [],
    };
  });
  checkedOuKeys.value = userOuResult.items.map((item) => item.id);
}

/** 加载组织机构树节点 */
async function onLoadOuChildren(node: EventDataNode) {
  const nodeKey = String(node.key);
  const { items } = await getChildrenApi({ id: nodeKey });
  node.dataRef!.isLeaf = items.length === 0;
  node.dataRef!.children = items.map((item): DataNode => {
    return {
      isLeaf: false,
      key: item.id,
      title: item.displayName,
      children: [],
    };
  });
  organizationUnits.value = [...organizationUnits.value];
  loadedOuKeys.value.push(nodeKey);
}
</script>

<template>
  <Modal>
    <Form
      ref="form"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="activedTab">
        <!-- 基本信息 -->
        <TabPane key="info" :tab="$t('AbpIdentity.UserInformations')">
          <FormItem :label="$t('AbpIdentity.DisplayName:IsActive')">
            <Checkbox v-model:checked="formModel.isActive">
              {{ $t('AbpIdentity.DisplayName:IsActive') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.UserName')"
            name="userName"
            required
          >
            <Input
              v-model:value="formModel.userName"
              :disabled="!isTrue('Abp.Identity.User.IsUserNameUpdateEnabled')"
            />
          </FormItem>
          <FormItem
            v-if="!formModel.id"
            :label="$t('AbpIdentity.Password')"
            name="password"
            required
          >
            <InputPassword v-model:value="formModel.password" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:Surname')"
            name="surname"
          >
            <Input v-model:value="formModel.surname" />
          </FormItem>
          <FormItem :label="$t('AbpIdentity.DisplayName:Name')" name="name">
            <Input v-model:value="formModel.name" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:Email')"
            name="email"
            required
          >
            <Input
              v-model:value="formModel.email"
              :disabled="!isTrue('Abp.Identity.User.IsEmailUpdateEnabled')"
            />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:PhoneNumber')"
            name="phoneNumber"
          >
            <Input v-model:value="formModel.phoneNumber" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:LockoutEnabled')"
            :label-col="{ span: 10 }"
          >
            <Checkbox v-model:checked="formModel.lockoutEnabled">
              {{ $t('AbpIdentity.DisplayName:LockoutEnabled') }}
            </Checkbox>
          </FormItem>
        </TabPane>
        <!-- 角色 -->
        <TabPane
          v-if="checkManageRolePolicy()"
          key="role"
          :tab="$t('AbpIdentity.Roles')"
        >
          <Transfer
            v-model:target-keys="formModel.roleNames"
            :data-source="assignedRoles"
            :list-style="{
              width: '47%',
              height: '338px',
            }"
            :render="(item) => item.title"
            :titles="[
              $t('AbpIdentityServer.Assigned'),
              $t('AbpIdentityServer.Available'),
            ]"
            class="tree-transfer"
          />
        </TabPane>
        <!-- 组织机构 -->
        <TabPane
          v-if="formModel.id && checkManageOuPolicy()"
          key="ou"
          :tab="$t('AbpIdentity.OrganizationUnits')"
        >
          <Tree
            :checked-keys="checkedOuKeys"
            :load-data="onLoadOuChildren"
            :loaded-keys="loadedOuKeys"
            :tree-data="organizationUnits"
            block-node
            check-strictly
            checkable
            disabled
          />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
