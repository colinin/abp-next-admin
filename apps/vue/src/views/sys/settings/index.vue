<template>
  <SettingForm :save-api="settingFormRef.saveApi" :setting-groups="group" />
</template>
<script lang="ts" setup>
  import { ref, onMounted } from 'vue';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';

  import { SettingForm } from '/@/components/SettingManagement';
  import { SettingGroup } from '/@/api/settings/model/settingModel';
  import {
    getCurrentTenantSettings,
    getGlobalSettings,
    setGlobalSettings,
    setCurrentTenantSettings,
  } from '/@/api/settings/settings';

  interface ISettingForm {
    providerName: string;
    providerKey?: string;
    saveApi: (...args: any) => Promise<any>;
  }

  const group = ref<SettingGroup[]>([]);
  const settingFormRef = ref<ISettingForm>({
    providerName: 'G',
    providerKey: '',
    saveApi: setGlobalSettings,
  });
  const abpStore = useAbpStoreWithOut();

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
</script>
