<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentity.OrganizationUnit:AddMember')"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed"
  >
    <el-form
      ref="formUserReference"
      :model="dataFilter"
      style="height: 500px;"
    >
      <el-form-item
        prop="filter"
        label="搜索"
        label-width="70%"
      >
        <el-input
          v-model="dataFilter.filter"
          @input="refreshPagedData"
        />
      </el-form-item>
      <el-table
        ref="userTable"
        v-loading="dataLoading"
        row-key="id"
        :data="dataList"
        border
        fit
        highlight-current-row
        height="330px"
        style="width: 100%;"
        @sort-change="handleSortChange"
        @selection-change="onSelectionChanged"
      >
        <el-table-column
          type="selection"
          width="50"
          align="center"
          reserve-selection
        />
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
          :label="$t('AbpIdentity.DisplayName:Surname')"
          prop="surname"
          width="110px"
          align="center"
        >
          <template slot-scope="{row}">
            <span>{{ row.surname }}</span>
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
      </el-table>

      <pagination
        :total="dataTotal"
        :page.sync="currentPage"
        :limit.sync="pageSize"
        @pagination="refreshPagedData"
      />

      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed"
        >
          {{ $t('AbpIdentityServer.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpIdentityServer.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import DataListMiXin from '@/mixins/DataListMiXin'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'

import OrganizationUnitService, { OrganizationUnitAddUser } from '@/api/organizationunit'
import { UsersGetPagedDto } from '@/api/users'

import { dateFormat, abpPagerFormat } from '@/utils'
import { Form, Table } from 'element-ui'

@Component({
  name: 'UserReference',
  components: {
    Pagination
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Mixins(DataListMiXin, EventBusMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private organizationUnitId!: string

  private ouAddUser = new OrganizationUnitAddUser()
  public dataFilter = new UsersGetPagedDto()

  @Watch('showDialog')
  private onShowDialogChanged() {
    if (this.showDialog) {
      this.refreshPagedData()
    }
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(dataFilter: any) {
    return OrganizationUnitService.getUnaddedUsers(this.organizationUnitId, dataFilter)
  }

  private onSelectionChanged(selection: any[]) {
    this.ouAddUser.userIds.length = 0
    selection.forEach(row => {
      if (!this.ouAddUser.isInOrganizationUnit(row.id)) {
        this.ouAddUser.addUser(row.id)
      }
    })
  }

  private onSave() {
    if (this.ouAddUser.userIds.length > 0) {
      OrganizationUnitService
        .addUsers(this.organizationUnitId, this.ouAddUser)
        .then(() => {
          this.ouAddUser.userIds.length = 0
          this.refreshPagedData()
          // 层次太深了,通过事件的方式来刷新列表
          this.trigger('onUserOrganizationUintChanged')
        })
    }
  }

  private onFormClosed() {
    if (this.ouAddUser.userIds.length > 0) {
      this.$confirm(this.l('AbpIdentity.AreYouSureYouWantToCancelEditingWarningMessage'),
        this.l('AbpIdentity.AreYouSure'), {
          callback: (action) => {
            if (action === 'confirm') {
              this.resetFields()
              this.$emit('closed')
            }
          }
        })
    } else {
      this.resetFields()
      this.$emit('closed')
    }
  }

  private resetFields() {
    const formUserReference = this.$refs.formUserReference as Form
    formUserReference.resetFields()
    const userTable = this.$refs.userTable as Table
    userTable.clearSelection()
    this.ouAddUser.userIds.length = 0
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
  width:100px;
}
.cancel {
  position: absolute;
  right: 120px;
  width:100px;
}
</style>
