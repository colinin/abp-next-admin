<template>
  <el-tabs
    tab-position="left"
  >
    <el-tab-pane
      v-if="!currentTenantId"
      :label="$t('settings.globalSetting')"
    >
      <GlobalSettingEditForm />
    </el-tab-pane>
    <el-tab-pane
      v-if="currentTenantId"
      :label="$t('settings.tenantSetting')"
    >
      <TenantSettingEditForm
        :tenant-id="currentTenantId"
      />
    </el-tab-pane>
    <el-tab-pane
      :label="$t('settings.userSetting')"
    >
      <UserSettingEditForm
        :user-id="currentUserId"
      />
    </el-tab-pane>
  </el-tabs>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { UserModule } from '@/store/modules/user'
import { AbpModule } from '@/store/modules/abp'
import UserSettingEditForm from './components/UserSettingEditForm.vue'
import TenantSettingEditForm from './components/TenantSettingEditForm.vue'
import GlobalSettingEditForm from './components/GlobalSettingEditForm.vue'

@Component({
  name: 'Settings',
  components: {
    UserSettingEditForm,
    TenantSettingEditForm,
    GlobalSettingEditForm
  }
})
export default class extends Vue {
  get currentTenantId() {
    if (AbpModule.configuration.currentTenant.isAvailable) {
      return AbpModule.configuration.currentTenant.id
    }
    return ''
  }

  get currentUserId() {
    return UserModule.id
  }
}
</script>

<style lang="scss" scoped>
.el-tabs__item {
  width: 160px;
}
</style>
