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
import { Component, Vue } from 'vue-property-decorator'
import { AppModule } from '@/store/modules/app'
import { AbpModule } from '@/store/modules/abp'

@Component({
  name: 'Login'
})
export default class extends Vue {
  get language() {
    return AppModule.language
  }

  private async handleSetLanguage(lang: string) {
    AppModule.SetLanguage(lang)
    this.$i18n.locale = lang
    await AbpModule.LoadAbpConfiguration()
    this.$message({
      message: 'Switch Language Success',
      type: 'success'
    })
  }
}
</script>
