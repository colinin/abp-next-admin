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
        :label="$t('AbpIdentity.DisplayName:UserName')"
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
        :label="$t('AbpIdentity.DisplayName:Name')"
        prop="name"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:Email')"
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
        :label="$t('AbpIdentity.DisplayName:PhoneNumber')"
        prop="phoneNumber"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.phoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.LockoutEnd')"
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
        :label="$t('AbpIdentity.CreationTime')"
        prop="creationTime"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.creationTime | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        v-if="checkPermission(['AbpIdentity.Users.ManageOrganizationUnits'])"
        :label="$t('AbpIdentity.Actions')"
        align="center"
        width="100px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleDeleteUser(row)"
          />
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="dataTotal > 0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
    />
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import DataListMiXin from '@/mixins/DataListMiXin'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'

import { checkPermission } from '@/utils/permission'
import { dateFormat, abpPagerFormat } from '@/utils'

import UserApiService, { UsersGetPagedDto } from '@/api/users'
import OrganizationUnitService from '@/api/organizationunit'

@Component({
  name: 'UserOrganizationUint',
  components: {
    Pagination
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends Mixins(DataListMiXin, EventBusMiXin) {
  @Prop({ default: '' })
  private organizationUnitId!: string

  public dataFilter = new UsersGetPagedDto()

  @Watch('organizationUnitId', { immediate: true })
  private onOrganizationUnitIdChanged() {
    this.refreshPagedData()
  }

  mounted() {
    this.subscribe('onUserOrganizationUintChanged', this.refreshPagedData)
  }

  destroyed() {
    this.unSubscribe('onUserOrganizationUintChanged')
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(dataFilter: any) {
    if (this.organizationUnitId) {
      return OrganizationUnitService.getUsers(this.organizationUnitId, dataFilter)
    }
    return this.getEmptyPagedList()
  }

  private handleDeleteUser(row: any) {
    this.$confirm(this.l('AbpIdentity.OrganizationUnit:AreYouSureRemoveUser', { 0: row.userName }),
      this.l('AbpIdentity.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            UserApiService
              .removeOrganizationUnit(row.id, this.organizationUnitId)
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
