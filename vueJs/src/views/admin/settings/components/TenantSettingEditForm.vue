<template>
  <SettingEditForm
    :setting-groups="settingGroups"
    @onSettingSaving="onSettingSaving"
  />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import SettingEditForm from './SettingEditForm.vue'
import SettingService, { SettingGroup, SettingsUpdate } from '@/api/settings'

@Component({
  name: 'TenantSettingEditForm',
  components: {
    SettingEditForm
  }
})
export default class extends Vue {
  private settingGroups = new Array<SettingGroup>()

  mounted() {
    SettingService
      .getCurrentTenantSettings()
      .then(res => {
        this.settingGroups = res.items
      })
  }

  private onSettingSaving(settings: SettingsUpdate) {
    SettingService.setCurrentTenantSettings(settings).then(() => {
      this.$message.success(this.$t('AbpSettingManagement.SuccessfullySaved').toString())
    })
  }
}
</script>
