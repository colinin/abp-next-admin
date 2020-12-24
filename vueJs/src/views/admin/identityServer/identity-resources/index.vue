<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('global.queryFilter') }}</label>
      <el-input
        v-model="dataFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="refreshPagedData"
      >
        {{ $t('AbpIdentityServer.Search') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['AbpIdentityServer.IdentityResources.Create'])"
        @click="onShowEditForm('')"
      >
        {{ $t('AbpIdentityServer.Resource:New') }}
      </el-button>
    </div>

    <el-table
      v-loading="dataLoading"
      row-key="id"
      :data="dataList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('AbpIdentityServer.Name')"
        prop="name"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.DisplayName')"
        prop="displayName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.displayName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Resource:Enabled')"
        prop="enabled"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.enabled"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Description')"
        prop="description"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.creationTime')"
        prop="creationTime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag>
              {{ row.creationTime | datetimeFilter }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.lastModificationTime')"
        prop="lastModificationTime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag type="warning">
              {{ row.lastModificationTime | datetimeFilter }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Actions')"
        align="center"
        width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentityServer.IdentityResources.Update'])"
            size="mini"
            type="primary"
            @click="onShowEditForm(row.id)"
          >
            {{ $t('AbpIdentityServer.Resource:Edit') }}
          </el-button>
          <el-button
            :disabled="!checkPermission(['AbpIdentityServer.IdentityResources.Delete'])"
            size="mini"
            type="danger"
            @click="onDeleted(row)"
          >
            {{ $t('AbpIdentityServer.Resource:Delete') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
      @sort-change="handleSortChange"
    />

    <identity-resource-create-or-edit-form
      :id="selectId"
      :show-dialog="showEditDialog"
      @closed="onEditFormClosed"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, abpPagerFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import IdentityResourceService, { IdentityResource, IdentityResourceGetByPaged } from '@/api/identity-resources'

import IdentityResourceCreateOrEditForm from './components/IdentityResourceCreateOrEditForm.vue'

@Component({
  name: 'IdentityServerIdentityResource',
  components: {
    Pagination,
    IdentityResourceCreateOrEditForm
  },
  methods: {
    checkPermission
  },
  filters: {
    statusFilter(status: boolean) {
      if (status) {
        return 'success'
      }
      return 'warning'
    },
    datetimeFilter(val: string) {
      const date = new Date(val)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends mixins(DataListMiXin) {
  private selectId = ''
  private showEditDialog = false

  public dataFilter = new IdentityResourceGetByPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return IdentityResourceService.getList(filter)
  }

  private onShowEditForm(id: string) {
    this.selectId = id
    this.showEditDialog = true
  }

  private onDeleted(resource: IdentityResource) {
    this.$confirm(this.l('AbpIdentityServer.Resource:WillDelete', { 0: resource.name }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            IdentityResourceService
              .delete(resource.id)
              .then(() => {
                this.$message.success(this.l('global.successful'))
                this.refreshPagedData()
              })
          }
        }
      })
  }

  private onEditFormClosed(changed: boolean) {
    this.showEditDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }
}
</script>

<style lang="scss" scoped>
.roleItem {
  width: 40px;
}
.options {
  vertical-align: top;
  margin-left: 20px;
}
.el-dropdown + .el-dropdown {
  margin-left: 15px;
}
.el-icon-arrow-down {
  font-size: 12px;
}
</style>
