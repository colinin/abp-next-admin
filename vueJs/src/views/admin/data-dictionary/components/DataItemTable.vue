<template>
  <div>
    <el-table
      row-key="id"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      :data="dataItems"
    >
      <el-table-column
        :label="$t('AppPlatform.DisplayName:Name')"
        prop="name"
        sortable
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:DisplayName')"
        prop="displayName"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.displayName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:Description')"
        prop="description"
        sortable
        min-width="180"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:DefaultValue')"
        prop="defaultValue"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.defaultValue }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:AllowBeNull')"
        prop="allowBeNull"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.allowBeNull"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AppPlatform.DisplayName:ValueType')"
        prop="valueType"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.valueType | valueTypeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        v-if="checkPermission(['Platform.DataDictionary.ManageItems'])"
        :label="$t('AbpUi.Actions')"
        align="center"
        min-width="90px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="primary"
            icon="el-icon-edit"
            @click="handleEditItem(row)"
          />
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleDeleteItem(row)"
          />
        </template>
      </el-table-column>
    </el-table>

    <create-or-update-data-item-dialog
      :show-dialog="showEditItemDialog"
      :data-item="editDataItem"
      :data-id="dataId"
      @closed="onEditItemDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import Component, { mixins } from 'vue-class-component'
import { Prop, Watch } from 'vue-property-decorator'

import { checkPermission } from '@/utils/permission'
import { dateFormat } from '@/utils'

import DataDictionaryService, { DataItem, ValueType } from '@/api/data-dictionary'

import CreateOrUpdateDataItemDialog from './CreateOrUpdateDataItemDialog.vue'

@Component({
  name: 'DataItemTable',
  components: {
    CreateOrUpdateDataItemDialog
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    },
    valueTypeFilter(valueType: ValueType) {
      switch (valueType) {
        case ValueType.Numeic:
          return 'Numeic'
        case ValueType.Boolean:
          return 'Boolean'
        case ValueType.Date:
          return 'Date'
        case ValueType.DateTime:
          return 'DateTime'
        case ValueType.Array:
          return 'Array'
        case ValueType.Object:
          return 'Object'
        default:
        case ValueType.String:
          return 'String'
      }
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(EventBusMiXin, LocalizationMiXin) {
  @Prop({ default: '' })
  private dataId!: string

  private dataItems = new Array<DataItem>()
  private showEditItemDialog = false
  private editDataItem = new DataItem()

  @Watch('dataId', { immediate: true })
  private onDataIdChanged() {
    this.handleGetDataItems()
  }

  mounted() {
    this.subscribe('onDataIdChanged', this.handleGetDataItems)
    this.subscribe('onCreateNewDataItem', this.handleCreateNewItem)
  }

  destroyed() {
    this.unSubscribe('onDataIdChanged')
    this.unSubscribe('onCreateNewDataItem')
  }

  private handleGetDataItems() {
    if (this.dataId) {
      DataDictionaryService
        .get(this.dataId)
        .then(res => {
          this.dataItems = res.items
        })
    } else {
      this.dataItems = new Array<DataItem>()
    }
  }

  private handleCreateNewItem() {
    this.editDataItem = new DataItem()
    this.showEditItemDialog = true
  }

  private handleEditItem(row: any) {
    const dataItem = new DataItem()
    dataItem.id = row.id
    dataItem.name = row.name
    dataItem.displayName = row.displayName
    dataItem.description = row.description
    dataItem.defaultValue = row.defaultValue
    dataItem.valueType = row.valueType
    dataItem.allowBeNull = row.allowBeNull
    this.editDataItem = dataItem
    this.showEditItemDialog = true
  }

  private handleDeleteItem(row: any) {
    this.$confirm(this.l('AppPlatform.Data:WillDelete', { 0: row.displayName }),
      this.l('AppPlatform.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            DataDictionaryService
              .removeItem(this.dataId, row.name)
              .then(() => {
                this.handleGetDataItems()
              })
          }
        }
      })
  }

  private onEditItemDialogClosed(changed: boolean) {
    this.showEditItemDialog = false
    if (changed) {
      this.handleGetDataItems()
    }
  }
}
</script>

<style lang="scss" scoped>

</style>
