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
        v-for="language in localization.languages"
        :key="language.cultureName"
        :disabled="localization.currentCulture.cultureName === language.cultureName"
        :command="language.cultureName"
      >
        {{ language.displayName }}
      </el-dropdown-item>
    </el-dropdown-menu>
  </el-dropdown>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { AppModule } from '@/store/modules/app'
import { AbpModule } from '@/store/modules/abp'

@Component({
  name: 'LangSelect'
})
export default class extends Vue {
  get localization() {
    return AbpModule.configuration.localization
  }

  private handleSetLanguage(lang: string) {
    AppModule.SetLanguage(lang)
    this.$i18n.locale = lang
    this.localization.currentCulture.cultureName = lang
    AbpModule.Initialize().then(() => {
      this.$message({
        message: 'Switch Language Success',
        type: 'success'
      })
      this.$emit('onLanguageSwitch')
    })
  }
}
</script>
