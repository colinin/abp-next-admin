<template>
  <div class="role-reference-pane">
    <el-table
      ref="roleTable"
      v-loading="userLoading"
      row-key="id"
      :data="userList"
      border
      fit
      highlight-current-row
      max-height="250px"
      @row-click="onRowClick"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat } from '@/utils'
import { Component, Vue } from 'vue-property-decorator'
import RoleAppService, { RoleGetPagedDto } from '@/api/roles'

@Component({
  name: 'RoleReference',
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Vue {
  private roleQueryFilter = new RoleGetPagedDto()

  private hanldGetRoles() {
    // TODO 待完成
    RoleAppService.getRoles(this.roleQueryFilter).then(res => {
      console.log(res)
    })
  }
}
</script>

<style lang="stylus" scoped>

</style>
