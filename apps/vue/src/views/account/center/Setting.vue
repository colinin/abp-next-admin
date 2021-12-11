<template>
  <SettingForm :save-api="settingFormRef.saveApi" :setting-groups="group" tab-position="left" />
</template>
<script lang="ts" setup>
  import { ref, onMounted } from 'vue';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';

  import { SettingForm } from '/@/components/SettingManagement';
  import { SettingGroup } from '/@/api/settings/model/settingModel';
  import { getCurrentUserSettings, setCurrentUserSettings } from '/@/api/settings/settings';

  interface ISettingForm {
    providerName: string;
    providerKey?: string;
    saveApi: (...args: any) => Promise<any>;
  }

  const abpStore = useAbpStoreWithOut();
  const group = ref<SettingGroup[]>([]);
  const settingFormRef = ref<ISettingForm>({
    providerName: 'U',
    providerKey: abpStore.getApplication.currentUser.id,
    saveApi: setCurrentUserSettings,
  });

  onMounted(() => {
    getCurrentUserSettings().then((res) => {
      group.value = res.items;
    })
  });
</script>
