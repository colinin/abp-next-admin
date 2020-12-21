<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form inline>
        <el-form-item
          label-width="100px"
          :label="$t('AppPlatform.DisplayName:Filter')"
        >
          <el-input
            v-model="dataQueryFilter.filter"
          />
        </el-form-item>
        <el-form-item
          label-width="100px"
          :label="$t('AppPlatform.DisplayName:PlatformType')"
        >
          <el-select
            v-model="dataQueryFilter.platformType"
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
        <el-form-item
          label-width="100px"
          :label="$t('AppPlatform.DisplayName:Layout')"
        >
          <el-select
            v-model="dataQueryFilter.layoutId"
            class="filter-item"
            clearable
            :placeholder="$t('pleaseSelectBy', {name: $t('AppPlatform.DisplayName:Layout')})"
          >
            <el-option
              v-for="layout in layouts"
              :key="layout.id"
              :label="layout.displayName"
              :value="layout.id"
            />
          </el-select>
        </el-form-item>
        <el-button
          class="filter-item"
          style="width: 150px; margin-left: 10px;"
          type="primary"
          @click="resetList"
        >
          <i class="el-icon-search" />
          {{ $t('AppPlatform.DisplayName:SecrchMenu') }}
        </el-button>
        <el-button
          class="filter-item"
          style="width: 150px; margin-left: 10px;"
          type="success"
          @click="handleAddMenu('')"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          {{ $t('AppPlatform.Menu:AddNew') }}
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
        :label="$t('AppPlatform.DisplayName:Component')"
        prop="component"
        sortable
        width="250px"
      >
        <template slot-scope="{row}">
          <el-tag>
            {{ row.component }}
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
        width="180px"
      >
        <template slot-scope="{row}">
          <span>{{ row.redirect }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        min-width="200px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['Platform.Menu.Create'])"
            size="mini"
            type="success"
            @click="handleAddMenu(row.id)"
          >
            <i class="ivu-icon ivu-icon-md-add" />
          </el-button>
          <el-button
            :disabled="!checkPermission(['Platform.Menu.Update'])"
            size="mini"
            type="primary"
            icon="el-icon-edit"
            @click="handleEditMenu(row.id)"
          />
          <el-button
            :disabled="!checkPermission(['Platform.Menu.Delete'])"
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleRemoveMenu(row)"
          />
        </template>
      </el-table-column>
    </el-table>

    <create-or-update-menu-dialog
      :show-dialog="showEditDialog"
      :menu-id="editMenuId"
      :parent-id="parentMenuId"
      @closed="onMenuEditDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, generateTree } from '@/utils'
import { checkPermission } from '@/utils/permission'
import LayoutService, { PlatformTypes, Layout } from '@/api/layout'
import MenuService, { Menu, GetAllMenu } from '@/api/menu'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import CreateOrUpdateMenuDialog from './components/CreateOrUpdateMenuDialog.vue'

@Component({
  name: 'Menus',
  components: {
    Pagination,
    CreateOrUpdateMenuDialog
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
export default class extends mixins(DataListMiXin) {
  public dataQueryFilter = new GetAllMenu()
  private showEditDialog = false
  private editMenuId = ''
  private parentMenuId = ''
  private layouts = new Array<Layout>()

  private platformTypes = PlatformTypes

  mounted() {
    this.handleGetLayouts()
    this.refreshData()
  }

  protected refreshData() {
    this.dataLoading = true
    MenuService
      .getAll(this.dataQueryFilter)
      .then(res => {
        this.dataList = generateTree(res.items)
        this.onDataLoadCompleted()
      })
      .finally(() => {
        this.dataLoading = false
      })
  }

  private handleGetLayouts() {
    LayoutService
      .getAllList()
      .then(res => {
        this.layouts = res.items
      })
  }

  private handleRemoveMenu(menu: Menu) {
    this.$confirm(this.l('questingDeleteByMessage', { message: menu.displayName }),
      this.l('AppPlatform.Menu:Delete'), {
        callback: (action) => {
          if (action === 'confirm') {
            MenuService
              .delete(menu.id)
              .then(() => {
                this.$message.success(this.l('successful'))
                this.refreshData()
              })
          }
        }
      })
  }

  private handleAddMenu(parentId: string) {
    this.editMenuId = ''
    this.parentMenuId = parentId
    this.showEditDialog = true
  }

  private handleEditMenu(id: string) {
    this.editMenuId = id
    this.showEditDialog = true
  }

  private onMenuEditDialogClosed(changed: boolean) {
    this.showEditDialog = false
    if (changed) {
      this.refreshData()
    }
  }
}
</script>

<style lang="scss" scoped>
.data-filter-collapse-title {
  font-size: 15px;
}
</style>
