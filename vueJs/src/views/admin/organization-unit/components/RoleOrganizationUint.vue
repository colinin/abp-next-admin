<template>
  <div>
    <el-table
      row-key="id"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      :data="dataList"
    >
      <el-table-column
        :label="$t('roles.name')"
        prop="name"
        sortable
        width="350px"
        min-width="350px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>

          <el-tag
            v-if="row.isDefault"
            type="success"
          >
            {{ $t('roles.isDefault') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.isPublic')"
        prop="isPublic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isPublic ? 'success' : 'warning'"
          >
            {{ row.isPublic ? $t('roles.isPublic') : $t('roles.isPrivate') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.type')"
        prop="isStatic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isStatic ? 'info' : 'success'"
          >
            {{ row.isStatic ? $t('roles.system') : $t('roles.custom') }}
          </el-tag>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
    />
  </div>
</template>

<script lang="ts">
import { abpPagerFormat } from '@/utils'
import DataListMiXin from '@/mixins/DataListMiXin'
import { Prop, Watch } from 'vue-property-decorator'
import Component, { mixins } from 'vue-class-component'
import OrganizationUnitService, { OrganizationUnitGetRoleByPaged } from '@/api/organizationunit'
import Pagination from '@/components/Pagination/index.vue'

@Component({
  name: 'RoleOrganizationUint',
  components: {
    Pagination
  }
})
export default class extends mixins(DataListMiXin) {
  @Prop({ default: '' })
  private organizationUnitId?: string

  public dataFilter = new OrganizationUnitGetRoleByPaged()

  @Watch('organizationUnitId', { immediate: true })
  private onOrganizationUnitIdChanged() {
    this.dataList = new Array<any>()
    if (this.organizationUnitId) {
      this.dataFilter.id = this.organizationUnitId
      this.refreshPagedData()
    }
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    if (this.organizationUnitId) {
      return OrganizationUnitService.organizationUnitGetRoles(filter)
    }
    return this.getEmptyPagedList()
  }
}
</script>

<style lang="scss" scoped>

</style>
