<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="userTitle"
    @ok="handleOk"
    :width="800"
    :min-height="400"
  >
    <Form
      ref="formElRef"
      :model="userRef"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:activeKey="activedTab">
        <TabPane key="info" :tab="L('UserInformations')">
          <FormItem name="userName" :label="L('UserName')">
            <BInput v-model:value="userRef.userName" />
          </FormItem>
          <FormItem v-if="!userRef.id" name="password" :label="L('Password')">
            <InputPassword v-model:value="userRef.password" />
          </FormItem>
          <FormItem name="surname" :label="L('DisplayName:Surname')">
            <BInput v-model:value="userRef.surname" />
          </FormItem>
          <FormItem name="name" :label="L('DisplayName:Name')">
            <BInput v-model:value="userRef.name" />
          </FormItem>
          <FormItem name="email" :label="L('DisplayName:Email')">
            <BInput v-model:value="userRef.email" />
          </FormItem>
          <FormItem name="phoneNumber" :label="L('DisplayName:PhoneNumber')">
            <BInput v-model:value="userRef.phoneNumber" />
          </FormItem>
          <FormItem :label="L('DisplayName:IsActive')">
            <Checkbox v-model:checked="userRef.isActive">{{ L('DisplayName:IsActive') }}</Checkbox>
          </FormItem>
          <FormItem :label="L('DisplayName:LockoutEnabled')">
            <Checkbox v-model:checked="userRef.lockoutEnabled">{{
              L('DisplayName:LockoutEnabled')
            }}</Checkbox>
          </FormItem>
        </TabPane>
        <TabPane key="role" :tab="L('Roles')">
          <Transfer
            :disabled="hasPermission('AbpIdentity.Users.ManageRoles')"
            class="tree-transfer"
            :dataSource="roleDataSource"
            :targetKeys="userRef.roleNames"
            :titles="[L('Assigned'), L('Available')]"
            :render="(item) => item.title"
            :list-style="{
              width: '47%',
              height: '338px',
            }"
            @change="handleRoleChange"
          />
        </TabPane>
        <TabPane key="organization-unit" :tab="L('OrganizationUnit')">
          <Tree
            checkable
            disabled
            :tree-data="getOrganizationUnitsTree"
            :checked-keys="hasInOrganizationUnitKeys"
            :field-names="{
              title: 'displayName',
              key: 'code',
            }"
          />
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Checkbox, Input, Form, Tabs, Transfer, Tree } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { FormActionType } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useUserForm } from '../hooks/useUserForm';
  import { useOrganizationUnit } from '../hooks/useOrganizationUnit';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  const InputPassword = Input.Password;

  const emits = defineEmits(['register', 'change']);

  const { createMessage } = useMessage();
  const { hasPermission } = usePermission();
  const { L } = useLocalization(['AbpIdentity', 'AbpIdentityServer']);
  const activedTab = ref('info');
  const userRef = ref<Recordable>({});
  const formElRef = ref<Nullable<FormActionType>>(null);
  const { handleSubmit, getUser, userTitle, formRules, roleDataSource, handleRoleChange } =
    useUserForm({
      userRef,
      formElRef,
    });
  const { getOrganizationUnitsTree, hasInOrganizationUnitKeys } = useOrganizationUnit({ userRef });
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner(async (val) => {
    activedTab.value = 'info';
    getUser(val.id);
  });

  function handleOk() {
    changeOkLoading(true);
    handleSubmit()
      .then((res) => {
        createMessage.success(L('Successful'));
        closeModal();
        emits('change', res);
      })
      .finally(() => {
        changeOkLoading(false);
      });
  }
</script>

<style scoped>
  .tree-transfer .ant-transfer-list:first-child {
    width: 100%;
    height: 400px;
    flex: none;
    justify-content: center;
    align-items: center;
  }
</style>
