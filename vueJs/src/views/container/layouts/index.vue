<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form inline>
        <el-form-item
          label-width="100px"
          :label="$t('AppPlatform.DisplayName:Filter')"
        >
          <el-input
            v-model="dataFilter.filter"
          />
        </el-form-item>
        <el-form-item
          label-width="100px"
          :label="$t('AppPlatform.DisplayName:PlatformType')"
        >
          <el-select
            v-model="dataFilter.platformType"
            class="filter-item"
            clearable
            :placeholder="$t('pleaseSelectBy', {name: $t('AppPlatform.DisplayName:PlatformType')})"
          >
            <el-option
              v-for="item in platformTypes"
              :key="item.key"
              :label="item.key"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
        <el-button
          class="filter-item"
          style="width: 150px; margin-left: 10px;"
          type="primary"
          @click="refreshPagedData"
        >
          <i class="el-icon-search" />
          {{ $t('AppPlatform.DisplayName:SecrchLayout') }}
        </el-button>
        <el-button
          class="filter-item"
          style="width: 150px; margin-left: 10px;"
          type="success"
          @click="handleAddLayout"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          {{ $t('AppPlatform.Layout:AddNew') }}
        </el-button>
      </el-form>
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
        :label="$t('AppPlatform.DisplayName:Name')"
        prop="Name"
        sortable
        width="200px"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:Path')"
        prop="path"
        sortable
        width="250px"
      >
        <template slot-scope="{row}">
          <el-tag>
            {{ row.path }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:DisplayName')"
        prop="displayName"
        sortable
        width="250px"
      >
        <template slot-scope="{row}">
          <span>{{ row.displayName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:Description')"
        prop="description"
        sortable
        width="250px"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:Redirect')"
        prop="redirect"
        sortable
        width="250px"
      >
        <template slot-scope="{row}">
          <span>{{ row.redirect }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        min-width="200px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['Platform.Layout.Update'])"
            size="mini"
            type="primary"
            icon="el-icon-edit"
            @click="handleEditLayout(row.id)"
          />
          <el-button
            :disabled="!checkPermission(['Platform.Layout.Delete'])"
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleRemoveLayout(row)"
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
      @sort-change="handleSortChange"
    />

    <create-or-update-layout-dialog
      :show-dialog="showEditDialog"
      :layout-id="editLayoutId"
      @closed="onLayoutEditDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, abpPagerFormat } from '@/utils'
import { checkPermission } from '@/utils/permission'
import LayoutService, { Layout, GetLayoutByPaged, PlatformTypes } from '@/api/layout'
import DataListMiXin from '@/mixins/DataListMiXin'
import { Component, Mixins } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import CreateOrUpdateLayoutDialog from './components/CreateOrUpdateLayoutDialog.vue'

@Component({
  name: 'Layouts',
  components: {
    Pagination,
    CreateOrUpdateLayoutDialog
  },
  filters: {
    dateTimeFormatFilter(dateTime: Date) {
      return dateFormat(new Date(dateTime), 'YYYY-mm-dd HH:MM:SS:NS')
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends Mixins(DataListMiXin) {
  public dataFilter = new GetLayoutByPaged()
  private showEditDialog = false
  private editLayoutId = ''

  private platformTypes = PlatformTypes

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return LayoutService.getList(filter)
  }

  private handleRemoveLayout(layout: Layout) {
    this.$confirm(this.l('questingDeleteByMessage', { message: layout.displayName }),
      this.l('AppPlatform.Layout:Delete'), {
        callback: (action) => {
          if (action === 'confirm') {
            LayoutService
              .delete(layout.id)
              .then(() => {
                this.$message.success(this.l('successful'))
                this.refreshPagedData()
              })
          }
        }
      })
  }

  private handleAddLayout() {
    this.editLayoutId = ''
    this.showEditDialog = true
  }

  private handleEditLayout(id: string) {
    this.editLayoutId = id
    this.showEditDialog = true
  }

  private onLayoutEditDialogClosed(changed: boolean) {
    this.showEditDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }
}
</script>

<style lang="scss" scoped>
.data-filter-collapse-title {
  font-size: 15px;
}
</style>
