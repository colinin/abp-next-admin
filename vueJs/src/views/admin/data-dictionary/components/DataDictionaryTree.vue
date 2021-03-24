<template>
  <div>
    <el-card class="box-card">
      <div
        slot="header"
        class="clearfix"
      >
        <span>{{ $t('AppPlatform.DisplayName:DataDictionary') }}</span>
        <el-button
          style="float: right;"
          type="primary"
          icon="ivu-icon ivu-icon-md-add"
          @click="handleEditData('')"
        >
          {{ $t('AppPlatform.Data:AddNew') }}
        </el-button>
      </div>
      <div>
        <el-tree
          ref="dataTree"
          node-key="id"
          :props="dataProps"
          :data="datas"
          draggable
          highlight-current
          default-expand-all
          :expand-on-click-node="false"
          icon-class="el-icon-arrow-right"
          :allow-drag="handleAllowDrag"
          :allow-drop="handleAllowDrop"
          @node-drop="handleNodeDroped"
          @node-click="handleNodeClick"
          @node-contextmenu="onContextMenu"
        />
      </div>
    </el-card>

    <create-or-update-data-dialog
      :is-edit="isEditData"
      :title="editDataTitle"
      :show-dialog="showDataDialog"
      :data-id="editDataId"
      @closed="onDataDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { generateTree } from '@/utils/index'

import { Component, Mixins, Vue } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import DataDictionaryService, { Data } from '@/api/data-dictionary'

import CreateOrUpdateDataDialog from './CreateOrUpdateDataDialog.vue'

const $contextmenu = Vue.prototype.$contextmenu

@Component({
  name: 'DataDictionaryTree',
  components: {
    CreateOrUpdateDataDialog
  },
  data() {
    return {
      dataProps: {
        label: 'displayName',
        children: 'children'
      }
    }
  }
})
export default class DataDictionaryTree extends Mixins(LocalizationMiXin) {
  private showDataDialog = false
  private isEditData = false
  private editDataId = ''
  private editDataTitle = ''
  private datas = []

  private currentEditNode = {}

  mounted() {
    this.handleGetDatas()
  }

  private handleGetDatas() {
    DataDictionaryService
      .getAll()
      .then(res => {
        this.datas = generateTree(res.items)
      })
  }

  private onContextMenu(event: any, data: Data) {
    $contextmenu({
      items: [
        {
          label: this.l('AppPlatform.Data:Edit'),
          icon: 'el-icon-edit',
          disabled: !checkPermission(['Platform.DataDictionary.Update']),
          onClick: () => {
            this.handleEditData(data.id)
          }
        },
        {
          label: this.l('AppPlatform.Data:AddNew'),
          icon: 'ivu-icon ivu-icon-md-add',
          disabled: !checkPermission(['Platform.DataDictionary.Create']),
          onClick: () => {
            this.handleEditData('')
          }
        },
        {
          label: this.l('AppPlatform.Data:AppendItem'),
          disabled: !checkPermission(['Platform.DataDictionary.ManageItems']),
          onClick: () => {
            this.$emit('onDataChecked', data.id)
            this.$events.emit('onCreateNewDataItem')
          }
        },
        {
          label: this.l('AppPlatform.Data:Delete'),
          icon: 'el-icon-delete',
          disabled: !checkPermission(['Platform.DataDictionary.Delete']),
          onClick: () => {
            this.$confirm(this.l('AppPlatform.Data:WillDelete', { 0: data.displayName }),
              this.l('AppPlatform.AreYouSure'), {
                callback: (action) => {
                  if (action === 'confirm') {
                    DataDictionaryService
                      .delete(data.id)
                      .then(() => {
                        this.handleGetDatas()
                      })
                  }
                }
              })
          }
        }
      ],
      event,
      customClass: 'context-menu',
      zIndex: 2,
      minWidth: 150
    })
  }

  private onDataDialogClosed(changed: boolean) {
    this.showDataDialog = false
    if (changed) {
      this.handleGetDatas()
    }
  }

  private handleEditData(dataId: string) {
    this.editDataTitle = this.l('AppPlatform.Data:AddNew')
    this.isEditData = false
    if (dataId) {
      this.editDataId = dataId
      this.isEditData = true
      this.editDataTitle = this.l('AppPlatform.Data:Edit')
    } else {
      this.editDataId = ''
    }
    this.showDataDialog = true
  }

  private handleAllowDrag(draggingNode: any) {
    return draggingNode.data.parentId !== undefined && draggingNode.data.parentId !== null
  }

  private handleAllowDrop(draggingNode: any, dropNode: any) {
    console.log(dropNode)
  }

  private handleNodeDroped(draggingNode: any, dropNode: any) {
    console.log(dropNode)
  }

  private handleNodeClick(data: any) {
    if (data.id !== undefined) {
      this.$emit('onDataChecked', data.id)
    }
  }
}
</script>

<style lang="scss" scoped>
  .custom-tree-node {
    flex: 1;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 14px;
    padding-right: 8px;
  }
  .el-dropdown-link {
    cursor: pointer;
    color: #409EFF;
  }
  .el-icon-arrow-down {
    font-size: 12px;
  }
</style>
