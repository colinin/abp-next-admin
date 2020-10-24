<template>
  <div class="dashboard-editor-container">
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

import InstantMessage from '@/components/InstantMessage/index.vue'

@Component({
  name: 'DashboardAdmin',
  components: {
    InstantMessage
  }
})
export default class extends mixins(EventBusMiXin) {
  private onShowImDialogClick() {
    this.trigger('onShowImDialog')
  }
}
</script>

<style lang="scss" scoped>
.dashboard-editor-container {
  padding: 32px;
  background-color: rgb(12, 73, 165);
  position: relative;

  .github-corner {
    position: absolute;
    top: 0px;
    border: 0;
    right: 0;
  }

  .chart-wrapper {
    background: #fff;
    padding: 16px 16px 0;
    margin-bottom: 32px;
  }
}

@media (max-width:1024px) {
  .chart-wrapper {
    padding: 8px;
  }
}
</style>
