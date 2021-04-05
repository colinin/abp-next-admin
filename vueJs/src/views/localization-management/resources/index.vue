<template>
  <div class="app-container">
    <div class="filter-container">
      <el-card>
        <div style="margin-top: 15px;">
          <el-row style="width: 100%;">
            <el-col :span="20">
              <el-input
                v-model="dataFilter.filter"
                :placeholder="$t('LocalizationManagement.SearchFilter')"
              >
                <el-button
                  slot="append"
                  icon="el-icon-search"
                  @click="refreshPagedData"
                />
              </el-input>
            </el-col>
            <el-col
              :span="4"
              style="text-align: right;"
            >
              <el-button
                class="create-new"
                type="success"
                @click="handleCreate"
              >
                <i class="ivu-icon ivu-icon-md-add" />
                {{ $t('LocalizationManagement.Resource:AddNew') }}
              </el-button>
            </el-col>
          </el-row>
        </div>
      </el-card>
    </div>

    <el-card>
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
          :label="$t('LocalizationManagement.DisplayName:Enable')"
          prop="enable"
          sortable
          width="100px"
          align="center"
        >
          <template slot-scope="{row}">
            <el-switch
              v-model="row.enable"
              disabled
              active-color="#13ce66"
              inactive-color="#ff4949"
            />
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:Name')"
          prop="name"
          sortable
          width="250px"
        >
          <template slot-scope="{row}">
            <span>{{ row.name }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:DisplayName')"
          prop="displayName"
          sortable
          width="300px"
        >
          <template slot-scope="{row}">
            <span>{{ row.displayName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:Description')"
          prop="description"
          sortable
          width="300px"
        >
          <template slot-scope="{row}">
            <span>{{ row.description }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('operaActions')"
          align="center"
          width="180px"
        >
          <template slot-scope="{row}">
            <el-tooltip
              effect="dark"
              :content="$t('LocalizationManagement.Edit')"
              placement="top"
            >
              <el-button
                size="mini"
                type="primary"
                icon="el-icon-edit"
                @click="handleModify(row)"
              />
            </el-tooltip>
            <el-tooltip
              effect="dark"
              :content="$t('LocalizationManagement.Delete')"
              placement="top"
            >
              <el-button
                size="mini"
                type="danger"
                icon="el-icon-delete"
                @click="handleDelete(row)"
              />
            </el-tooltip>
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
    </el-card>

    <ResourceDialog
      :resource-id="editResource.id"
      :show-dialog="showEditDialog"
      @closed="showEditDialog=false"
    />
  </div>
</template>

<script lang="ts">
import { Component, Mixins } from 'vue-property-decorator'
import DataListMiXin from '@/mixins/DataListMiXin'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'
import Pagination from '@/components/Pagination/index.vue'
import ResourceDialog from './components/ResourceDialog.vue'

import {
  service,
  controller,
  Resource,
  GetResourcesInput
} from './types'

import { dateFormat, abpPagerFormat } from '@/utils/index'

@Component({
  name: 'ResourceList',
  components: {
    Pagination,
    ResourceDialog
  },
  filters: {
    datetimeFilter(val: string) {
      if (!val) {
        return ''
      }
      const date = new Date(val)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Mixins(DataListMiXin, HttpProxyMiXin) {
  public dataFilter = new GetResourcesInput()

  private showEditDialog = false
  private editResource = new Resource()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return this.pagedRequest<Resource>({
      service: service,
      controller: controller,
      action: 'GetListAsync',
      params: filter
    })
  }

  private handleCreate() {
    this.editResource = new Resource()
    this.showEditDialog = true
  }

  private handleModify(resource: Resource) {
    this.editResource = resource
    this.showEditDialog = true
  }

  private handleDelete(resource: Resource) {
    this.$confirm(this.l('LocalizationManagement.WillDeleteResource', { 0: resource.displayName ?? resource.name }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            this.request<void>({
              service: service,
              controller: controller,
              action: 'DeleteAsync',
              params: {
                id: resource.id
              }
            }).then(() => {
              this.$message.success(this.l('global.successful'))
              this.refreshPagedData()
            })
          }
        }
      })
  }
}
</script>

<style scoped>
.create-new {
  margin-right: 10px;
  width: 200px;
}
</style>
