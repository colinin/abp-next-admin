<template>
  <el-tabs
    tab-position="left"
  >
    <el-tab-pane
      :label="$t('settings.globalSetting')"
    >
      <GlobalSettingEditForm />
    </el-tab-pane>
    <el-tab-pane
      :label="$t('settings.tenantSetting')"
    >
      {{ $t('settings.tenantSetting') }}
    </el-tab-pane>
    <el-tab-pane
      :label="$t('settings.userSetting')"
    >
      {{ $t('settings.userSetting') }}
    </el-tab-pane>
  </el-tabs>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

import GlobalSettingEditForm from './components/GlobalSettingEditForm.vue'

@Component({
  name: 'Settings',
  components: {
    GlobalSettingEditForm
  }
})
export default class extends Vue {}
</script>

<style lang="scss" scoped>
.el-tabs__item {
  width: 160px;
}
</style>
