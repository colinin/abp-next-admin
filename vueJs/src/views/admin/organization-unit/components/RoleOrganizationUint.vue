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
        :label="$t('AbpIdentity.DisplayName:RoleName')"
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
            {{ $t('AbpIdentity.DisplayName:IsDefault') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:IsPublic')"
        prop="isPublic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.isPublic"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:IsStatic')"
        prop="isStatic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.isStatic"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        v-if="checkPermission(['AbpIdentity.Roles.ManageOrganizationUnits'])"
        :label="$t('AbpIdentity.Actions')"
        align="center"
        width="100px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleDeleteRole(row)"
          />
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
import { checkPermission } from '@/utils/permission'

import EventBusMiXin from '@/mixins/EventBusMiXin'
import DataListMiXin from '@/mixins/DataListMiXin'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'

import RoleApiService, { RoleGetPagedDto } from '@/api/roles'
import OrganizationUnitService from '@/api/organizationunit'

@Component({
  name: 'RoleOrganizationUint',
  components: {
    Pagination
  },
  methods: {
    checkPermission
  }
})
export default class extends Mixins(DataListMiXin, EventBusMiXin) {
  @Prop({ default: '' })
  private organizationUnitId!: string

  public dataFilter = new RoleGetPagedDto()

  @Watch('organizationUnitId', { immediate: true })
  private onOrganizationUnitIdChanged() {
    this.refreshPagedData()
  }

  mounted() {
    this.subscribe('onRoleOrganizationUintChanged', this.refreshPagedData)
  }

  destroyed() {
    this.unSubscribe('onRoleOrganizationUintChanged')
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(dataFilter: any) {
    if (this.organizationUnitId) {
      return OrganizationUnitService.getRoles(this.organizationUnitId, dataFilter)
    }
    return this.getEmptyPagedList()
  }

  private handleDeleteRole(row: any) {
    this.$confirm(this.l('AbpIdentity.OrganizationUnit:AreYouSureRemoveRole', { 0: row.name }),
      this.l('AbpIdentity.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            RoleApiService
              .removeOrganizationUnits(row.id, this.organizationUnitId)
              .then(() => {
                this.refreshPagedData()
              })
          }
        }
      })
  }
}
</script>

<style lang="scss" scoped>

</style>
