<template>
  <SettingEditForm
    :settings="settings"
    @onSettingSaving="onSettingSaving"
  />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import SettingEditForm from './SettingEditForm.vue'
import SettingService, { Setting, SettingsUpdate } from '@/api/settings'

@Component({
  name: 'TenantSettingEditForm',
  components: {
    SettingEditForm
  }
})
export default class extends Vue {
  private settings = new Array<Setting>()

  mounted() {
    SettingService.getCurrentTenantSettings().then(res => {
      this.settings = res.items
    })
  }

  private onSettingSaving(settings: SettingsUpdate) {
    SettingService.setCurrentTenantSettings(settings).then(() => {
      this.$message.success(this.$t('AbpSettingManagement.SuccessfullySaved').toString())
    })
  }
}
</script>
