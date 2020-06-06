<template>
  <el-dropdown
    trigger="click"
    class="international"
    @command="handleSetLanguage"
  >
    <div>
      <svg-icon
        name="language"
        class="international-icon"
      />
    </div>
    <el-dropdown-menu slot="dropdown">
      <el-dropdown-item
        :disabled="language==='zh'"
        command="zh"
      >
        中文
      </el-dropdown-item>
      <el-dropdown-item
        :disabled="language==='en'"
        command="en"
      >
        English
      </el-dropdown-item>
    </el-dropdown-menu>
  </el-dropdown>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator'
import { AppModule } from '@/store/modules/app'
import { AbpConfigurationModule } from '@/store/modules/abp'

@Component({
  name: 'Login'
})
export default class extends Vue {
  get language() {
    return AppModule.language
  }

  /**
   * 监听abp配置状态,增强本地化
   */
  @Watch('$store.state.abpconfiguration.configuration')
  private onAbpConfigurationChanged() {
    const abpConfig = this.$store.state.abpconfiguration.configuration
    if (abpConfig) {
      const { twoLetterIsoLanguageName } = abpConfig.localization.currentCulture
      const resources: { [key: string]: any} = {}
      Object.keys(abpConfig.localization.values).forEach(key => {
        const resource = abpConfig.localization.values[key]

        if (typeof resource !== 'object') return

        Object.keys(resource).forEach(key2 => {
          if (/'{|{/g.test(resource[key2])) {
            resource[key2] = resource[key2].replace(/'{|{/g, '{{').replace(/}'|}/g, '}}')
          }
        })
        resources[key] = resource
      })
      this.$i18n.mergeLocaleMessage(twoLetterIsoLanguageName as string, resources)
      console.log(this.$i18n)
    }
  }

  private handleSetLanguage(lang: string) {
    AppModule.SetLanguage(lang)
    this.$i18n.locale = lang
    this.$message({
      message: 'Switch Language Success',
      type: 'success'
    })
  }
}
</script>
