<template>
  <SettingForm :save-api="settingFormRef.saveApi" :setting-groups="group">
    <template #send-test-email="{ detail }">
      <FormItem name="testEmail" :label="detail.displayName" :extra="detail.description">
        <SearchInput
          :placeholder="L('TargetEmailAddress')"
          v-model:value="detail.value"
          @search="handleSendTestEmail"
          :loading="sendingEmail"
        >
          <template #enterButton>
            <Button type="primary">{{ L('Send') }}</Button>
          </template>
        </SearchInput>
      </FormItem>
    </template>
  </SettingForm>
</template>
<script lang="ts" setup>
  import { ref, onMounted } from 'vue';
  import { Button, Form, Input } from 'ant-design-vue';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { SettingForm } from '/@/components/SettingManagement';
  import { SettingGroup } from '/@/api/settings-management/settings/model';
  import {
    getCurrentTenantSettings,
    getGlobalSettings,
    setGlobalSettings,
    setCurrentTenantSettings,
    sendTestEmail,
  } from '/@/api/settings-management/settings';
  import { isEmail } from '/@/utils/is';

  interface ISettingForm {
    providerName: string;
    providerKey?: string;
    saveApi: (...args: any) => Promise<any>;
  }

  const FormItem = Form.Item;
  const SearchInput = Input.Search;

  const sendingEmail = ref(false);
  const group = ref<SettingGroup[]>([]);
  const settingFormRef = ref<ISettingForm>({
    providerName: 'G',
    providerKey: '',
    saveApi: setGlobalSettings,
  });
  const abpStore = useAbpStoreWithOut();
  const { createWarningModal, createMessage } = useMessage();
  const { L } = useLocalization(['AbpSettingManagement']);

  onMounted(() => {
    if (abpStore.getApplication.currentTenant.id) {
      settingFormRef.value = {
        providerName: 'T',
        providerKey: abpStore.getApplication.currentTenant.id,
        saveApi: setCurrentTenantSettings,
      };
      getCurrentTenantSettings().then((res) => {
        group.value = res.items;
      });
    } else {
      getGlobalSettings().then((res) => {
        group.value = res.items;
      });
    }
  });

  function handleSendTestEmail(emailAddress: string) {
    if (!isEmail(emailAddress)) {
      createWarningModal({
        title: L('ValidationErrorMessage'),
        content: L('ThisFieldIsNotAValidEmailAddress.'),
      });
      return;
    }
    sendingEmail.value = true;
    sendTestEmail(emailAddress)
      .then(() => {
        createMessage.success(L('SuccessfullySent'));
      })
      .finally(() => {
        sendingEmail.value = false;
      });
  }
</script>
