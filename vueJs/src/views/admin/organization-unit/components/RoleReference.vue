<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentity.OrganizationUnit:AddRole')"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed"
  >
    <el-form
      style="height: 500px;"
    >
      <el-table
        ref="roleTable"
        v-loading="dataLoading"
        row-key="id"
        :data="dataList"
        border
        fit
        highlight-current-row
        height="380px"
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

import OrganizationUnitService, { OrganizationUnitAddRole } from '@/api/organizationunit'
import { RoleGetPagedDto } from '@/api/roles'

import { dateFormat, abpPagerFormat } from '@/utils'
import { Table } from 'element-ui'

@Component({
  name: 'RoleReference',
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

  private ouAddRole = new OrganizationUnitAddRole()
  public dataFilter = new RoleGetPagedDto()

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
    return OrganizationUnitService.getUnaddedRoles(this.organizationUnitId, dataFilter)
  }

  private onSelectionChanged(selection: any[]) {
    this.ouAddRole.roleIds.length = 0
    selection.forEach(row => {
      if (!this.ouAddRole.isInOrganizationUnit(row.id)) {
        this.ouAddRole.addRole(row.id)
      }
    })
  }

  private onSave() {
    if (this.ouAddRole.roleIds.length > 0) {
      OrganizationUnitService
        .addRoles(this.organizationUnitId, this.ouAddRole)
        .then(() => {
          this.ouAddRole.roleIds.length = 0
          this.refreshPagedData()
          // 层次太深了,通过事件的方式来刷新列表
          this.trigger('onRoleOrganizationUintChanged')
        })
    }
  }

  private onFormClosed() {
    if (this.ouAddRole.roleIds.length > 0) {
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
    const roleTable = this.$refs.roleTable as Table
    roleTable.clearSelection()
    this.ouAddRole.roleIds.length = 0
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
