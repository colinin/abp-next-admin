<template>
  <div class="dashboard-container">
    <component :is="currentRole" />
    IM测试页
    <el-button @click="onShowImDialogClick">
      打开IM
    </el-button>
    <instant-message />
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import Component, { mixins } from 'vue-class-component'
import { UserModule } from '@/store/modules/user'
import AdminDashboard from './admin/index.vue'
import EditorDashboard from './editor/index.vue'
import InstantMessage from '@/components/InstantMessage/index.vue'

@Component({
  name: 'Dashboard',
  components: {
    AdminDashboard,
    EditorDashboard,
    InstantMessage
  }
})
export default class extends mixins(EventBusMiXin) {
  private currentRole = 'admin-dashboard'

  get roles() {
    return UserModule.roles
  }

  created() {
    if (!this.roles.includes('admin')) {
      this.currentRole = 'editor-dashboard'
    }
  }

  private onShowImDialogClick() {
    this.trigger('onShowImDialog')
  }
}
</script>
