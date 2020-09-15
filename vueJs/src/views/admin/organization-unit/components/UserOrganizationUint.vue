<template>
  <el-table
    row-key="id"
    border
    fit
    highlight-current-row
    style="width: 100%;"
    :data="dataList"
  >
    <el-table-column
      :label="$t('users.userName')"
      prop="userName"
      sortable
      width="110px"
      align="center"
    >
      <template slot-scope="{row}">
        <span>{{ row.userName }}</span>
      </template>
    </el-table-column>
    <el-table-column
      :label="$t('users.name')"
      prop="name"
      width="110px"
      align="center"
    >
      <template slot-scope="{row}">
        <span>{{ row.name }}</span>
      </template>
    </el-table-column>
    <el-table-column
      :label="$t('users.email')"
      prop="email"
      sortable
      min-width="180"
      align="center"
    >
      <template slot-scope="{row}">
        <span>{{ row.email }}</span>
      </template>
    </el-table-column>
    <el-table-column
      :label="$t('users.phoneNumber')"
      prop="phoneNumber"
      width="140px"
      align="center"
    >
      <template slot-scope="{row}">
        <span>{{ row.phoneNumber }}</span>
      </template>
    </el-table-column>
    <el-table-column
      :label="$t('users.lockoutEnd')"
      prop="lockoutEnd"
      sortable
      width="140px"
      align="center"
    >
      <template slot-scope="{row}">
        <span>{{ row.lockoutEnd | dateTimeFilter }}</span>
      </template>
    </el-table-column>
    <el-table-column
      :label="$t('users.creationTime')"
      prop="creationTime"
      sortable
      width="140px"
      align="center"
    >
      <template slot-scope="{row}">
        <span>{{ row.creationTime | dateTimeFilter }}</span>
      </template>
    </el-table-column>
  </el-table>
</template>

<script lang="ts">
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import { Prop, Watch } from 'vue-property-decorator'
import { dateFormat } from '@/utils'
import OrganizationUnitService from '@/api/organizationunit'

@Component({
  name: 'UserOrganizationUint',
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends mixins(DataListMiXin) {
  @Prop({ default: '' })
  private organizationUnitId?: string

  @Watch('organizationUnitId', { immediate: true })
  private onOrganizationUnitIdChanged() {
    this.dataList = new Array<any>()
    if (this.organizationUnitId) {
      this.refreshData()
    }
  }

  protected getList() {
    if (this.organizationUnitId) {
      return OrganizationUnitService.organizationUnitGetUsers(this.organizationUnitId)
    }
    return this.getEmptyList()
  }
}
</script>

<style lang="scss" scoped>

</style>
