<template>
  <div class="app-container">
    <component :is="currentSettingForm" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { AbpModule } from '@/store/modules/abp'
import TenantSettingEditForm from './components/TenantSettingEditForm.vue'
import GlobalSettingEditForm from './components/GlobalSettingEditForm.vue'

@Component({
  name: 'Settings',
  components: {
    TenantSettingEditForm,
    GlobalSettingEditForm
  }
})
export default class extends Vue {
  private currentSettingForm = 'global-setting-editForm'

  get currentTenantId() {
    if (AbpModule.configuration.currentTenant.isAvailable) {
      return AbpModule.configuration.currentTenant.id
    }
    return ''
  }

  created() {
    if (this.currentTenantId) {
      this.currentSettingForm = 'tenant-setting-editForm'
    }
  }
}
</script>
