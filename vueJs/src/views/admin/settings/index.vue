<template>
  <el-tabs
    tab-position="left"
    style="height: 200px;"
  >
    <el-tab-pane
      label="系统配置"
    >
      系统配置
    </el-tab-pane>
    <el-tab-pane
      label="租户配置"
    >
      租户配置
    </el-tab-pane>
    <el-tab-pane
      label="用户配置"
    >
      用户配置
    </el-tab-pane>
  </el-tabs>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({
  name: 'Settings'
})
export default class extends Vue {

}
</script>

<style lang="scss">
.el-tabs__item {
  width: 160px;
}
</style>
