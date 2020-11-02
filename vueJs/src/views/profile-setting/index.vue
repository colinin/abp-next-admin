<template>
  <el-tabs
    tab-position="left"
    style="margin: 30px;"
  >
    <el-tab-pane label="基本设置">
      <my-profile />
    </el-tab-pane>
    <el-tab-pane label="安全设置">
      安全设置
    </el-tab-pane>
    <el-tab-pane label="账号绑定">
      账号绑定
    </el-tab-pane>
    <el-tab-pane label="消息通知">
      <my-notifier />
    </el-tab-pane>
  </el-tabs>
</template>

<script lang="ts">
import MyProfile from './components/MyProfile.vue'
import MyNotifier from './components/MyNotifier.vue'
import { Component, Vue } from 'vue-property-decorator'

@Component({
  name: 'ProfileSettingEditForm',
  components: {
    MyProfile,
    MyNotifier
  }
})
export default class extends Vue {
}
</script>
