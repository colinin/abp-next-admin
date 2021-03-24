<template>
  <div>
    <el-card class="box-card">
      <div
        slot="header"
        class="clearfix"
      >
        <span>{{ $t('AbpIdentity.OrganizationUnit:Tree') }}</span>
        <el-button
          style="float: right;"
          type="primary"
          icon="ivu-icon ivu-icon-md-add"
          @click="handleCreateRootOrganizationUnit(null)"
        >
          {{ $t('AbpIdentity.OrganizationUnit:AddRoot') }}
        </el-button>
      </div>
      <div>
        <el-tree
          ref="organizationUnitTree"
          node-key="id"
          :props="organizationProps"
          :data="organizationUnits"
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

    <user-reference
      :organization-unit-id="editOrganizationUnitId"
      :show-dialog="showUserReferenceDialog"
      @closed="() => {
        showUserReferenceDialog = false
      }"
    />

    <role-reference
      :organization-unit-id="editOrganizationUnitId"
      :show-dialog="showRoleReferenceDialog"
      @closed="() => {
        showRoleReferenceDialog = false
      }"
    />

    <create-or-update-organization-unit
      :is-edit="isEditOrganizationUnit"
      :title="editOrganizationUnitTitle"
      :show-dialog="showOrganizationUnitDialog"
      :organization-unit-id="editOrganizationUnitId"
      :on-organization-unit-changed="onOrganizationUnitChanged"
      @closed="onOrganizationUnitDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'

import { Component, Mixins, Vue } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import OrganizationUnitService, { OrganizationUnit } from '@/api/organizationunit'

import { Tree } from 'element-ui'
import UserReference from './UserReference.vue'
import RoleReference from './RoleReference.vue'
import CreateOrUpdateOrganizationUnit from './CreateOrUpdateOrganizationUnit.vue'

const $contextmenu = Vue.prototype.$contextmenu
class OrganizationUnitItem {
  id!: string
  parentId?: string
  code!: string
  displayName!: string
  children = new Array<OrganizationUnitItem>()

  constructor(
    id: string,
    code: string,
    displayName: string,
    parentId?: string
  ) {
    this.id = id
    this.code = code
    this.displayName = displayName
    this.parentId = parentId
  }

  public createChildren(chidlren: OrganizationUnitItem) {
    this.children.push(chidlren)
  }
}

@Component({
  name: 'OrganizationUnitTree',
  data() {
    return {
      organizationProps: {
        label: 'displayName',
        children: 'children'
      }
    }
  },
  components: {
    UserReference,
    RoleReference,
    CreateOrUpdateOrganizationUnit
  }
})
export default class OrganizationUnitTree extends Mixins(LocalizationMiXin) {
  private showUserReferenceDialog = false
  private showRoleReferenceDialog = false

  private showOrganizationUnitDialog = false
  private isEditOrganizationUnit = false
  private editOrganizationUnitId = ''
  private editOrganizationUnitTitle = ''
  private onOrganizationUnitChanged = (ou: OrganizationUnitItem) => { console.log(ou) }
  private organizationUnits = new Array<OrganizationUnitItem>()

  private currentEditNode = {}

  mounted() {
    this.handleGetOrganizationUnits()
  }

  private handleGetOrganizationUnits() {
    this.organizationUnits = new Array<OrganizationUnitItem>()
    OrganizationUnitService.getAllOrganizationUnits()
      .then(res => {
        const rootOrganizationUnits = res.items.filter(item => !item.parentId)
        rootOrganizationUnits.forEach(item => {
          const organizationUnit = new OrganizationUnitItem(item.id, item.code, item.displayName, item.parentId)
          const subOrganizationUnits = res.items.filter(p => p.code.startsWith(item.code))
          this.generateOrganizationUnit(organizationUnit, subOrganizationUnits)
          this.organizationUnits.push(organizationUnit)
        })
      })
  }

  private generateOrganizationUnit(organizationUnit: OrganizationUnitItem, organizationUnits: OrganizationUnit[]) {
    const subOrganizationUnit = organizationUnits.filter(ou => ou.parentId !== organizationUnit.id)
    organizationUnits = organizationUnits.filter(ou => ou.parentId === organizationUnit.id)
    organizationUnits.forEach(ou => {
      const children = new OrganizationUnitItem(ou.id, ou.code, ou.displayName, ou.parentId)
      const itemSubOrganizationUnit = subOrganizationUnit.filter(sou => sou.parentId === ou.id)
      if (itemSubOrganizationUnit.length > 0) {
        this.generateOrganizationUnit(children, itemSubOrganizationUnit)
      }
      organizationUnit.createChildren(children)
    })
  }

  private onContextMenu(event: any, ou: OrganizationUnitItem) {
    const organizationUnitTree = this.$refs.organizationUnitTree as Tree
    $contextmenu({
      items: [
        {
          label: this.l('AbpIdentity.Edit'),
          icon: 'el-icon-edit',
          disabled: !checkPermission(['AbpIdentity.OrganizationUnits.Update']),
          onClick: () => {
            this.editOrganizationUnitTitle = this.l('AbpIdentity.Edit')
            this.isEditOrganizationUnit = true
            this.editOrganizationUnitId = ou.id
            this.showOrganizationUnitDialog = true
            this.onOrganizationUnitChanged = (res) => {
              ou.displayName = res.displayName
            }
          }
        },
        {
          label: this.l('AbpIdentity.OrganizationUnit:AddChildren'),
          icon: 'ivu-icon ivu-icon-md-add',
          disabled: !checkPermission(['AbpIdentity.OrganizationUnits.Create']),
          onClick: () => {
            this.handleCreateRootOrganizationUnit(ou)
          }
        },
        {
          label: this.$t('AbpIdentity.OrganizationUnit:AddMember'),
          disabled: !checkPermission(['AbpIdentity.OrganizationUnits.ManageUsers']),
          onClick: () => {
            this.editOrganizationUnitId = ou.id
            this.showUserReferenceDialog = true
          }
        },
        {
          label: this.$t('AbpIdentity.OrganizationUnit:AddRole'),
          disabled: !checkPermission(['AbpIdentity.OrganizationUnits.ManageRoles']),
          onClick: () => {
            this.editOrganizationUnitId = ou.id
            this.showRoleReferenceDialog = true
          }
        },
        {
          label: this.$t('AbpIdentity.Delete'),
          icon: 'el-icon-delete',
          disabled: !checkPermission(['AbpIdentity.OrganizationUnits.Delete']),
          onClick: () => {
            this.$confirm(this.l('AbpIdentity.OrganizationUnit:WillDelete', { 0: ou.displayName }),
              this.l('AbpIdentity.AreYouSure'), {
                callback: (action) => {
                  if (action === 'confirm') {
                    OrganizationUnitService
                      .deleteOrganizationUnit(ou.id)
                      .then(() => {
                        organizationUnitTree.remove(ou)
                        // 所有根节点已经被删除
                        if (organizationUnitTree.data.length === 0) {
                          this.$emit('onOrganizationUnitChecked', '')
                        }
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

  private onOrganizationUnitDialogClosed() {
    this.showOrganizationUnitDialog = false
  }

  private handleCreateRootOrganizationUnit(ou: OrganizationUnitItem) {
    const organizationUnitTree = this.$refs.organizationUnitTree as Tree
    this.editOrganizationUnitTitle = this.l('AbpIdentity.OrganizationUnit:AddChildren')
    this.isEditOrganizationUnit = false
    if (ou) {
      this.editOrganizationUnitId = ou.id
      this.onOrganizationUnitChanged = (res) => {
        organizationUnitTree.append(res, ou)
      }
    } else {
      this.editOrganizationUnitId = ''
      this.onOrganizationUnitChanged = (res) => {
        if (this.organizationUnits.length > 0) {
          const lastNode = organizationUnitTree.getNode(this.organizationUnits[this.organizationUnits.length - 1].id)
          organizationUnitTree.insertAfter(res, lastNode)
        } else {
          this.handleGetOrganizationUnits()
        }
      }
    }
    this.showOrganizationUnitDialog = true
  }

  private handleAllowDrag(draggingNode: any) {
    return draggingNode.data.parentId !== undefined && draggingNode.data.parentId !== null
  }

  private handleAllowDrop(draggingNode: any, dropNode: any) {
    return dropNode.code !== 'root'
  }

  private handleNodeDroped(draggingNode: any, dropNode: any) {
    OrganizationUnitService
      .moveOrganizationUnit(draggingNode.data.id, dropNode.data.id)
      .then(() => {
        this.handleGetOrganizationUnits()
      })
  }

  private handleNodeClick(data: any) {
    if (data.id !== undefined) {
      this.$emit('onOrganizationUnitChecked', data.id)
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
