<template>
  <div>
    <el-card class="box-card">
      <div
        slot="header"
        class="clearfix"
      >
        <span>组织机构</span>
      </div>
      <div>
        <el-tree
          ref="organizationUnitTree"
          node-key="id"
          :props="organizationProps"
          :load="loadOrganizationUnit"
          lazy
          draggable
          highlight-current
          :allow-drag="handleAllowDrag"
          :allow-drop="handleAllowDrop"
          @node-drop="handleNodeDroped"
          @node-click="handleNodeClick"
        >
          <span
            slot-scope="{node, data}"
            class="custom-tree-node"
          >
            <span>{{ node.label }}</span>
            <el-dropdown @command="handleChangeOrganizationUint">
              <span class="el-dropdown-link">
                操作方法
                <i class="el-icon-arrow-down el-icon--right" />
              </span>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item :command="{key: 'append', node: node, data: data}">新增机构</el-dropdown-item>
                <el-dropdown-item :command="{key: 'remove', node: node, data: data}">删除机构</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
          </span>
        </el-tree>
      </div>
    </el-card>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { ListResultDto } from '@/api/types'
import OrganizationUnitService, { OrganizationUnitCreate, OrganizationUnit } from '@/api/organizationunit'

class OrganizationUnitTree {
  id?: string
  parentId?: string
  code!: string
  displayName!: string
  isLeaf!: boolean
  children?: OrganizationUnitTree[]

  constructor() {
    this.isLeaf = false
    this.children = new Array<OrganizationUnitTree>()
  }
}

@Component({
  name: 'EditOrganizationUint',
  data() {
    return {
      organizationProps: {
        label: 'displayName',
        isLeaf: 'isLeaf',
        children: 'children'
      }
    }
  }
})
export default class extends Vue {
  private showUserReferenceDialog = false
  private async loadOrganizationUnit(node: any, resolve: any) {
    if (node.level === 0) {
      const rootOrganizationUnit = new OrganizationUnitTree()
      rootOrganizationUnit.id = undefined
      rootOrganizationUnit.parentId = undefined
      rootOrganizationUnit.code = 'root'
      rootOrganizationUnit.displayName = '组织机构'
      return resolve([rootOrganizationUnit])
    }
    let organizationUnitItems = new ListResultDto<OrganizationUnit>()
    if (node.data.id === undefined) {
      // 根节点
      organizationUnitItems = await OrganizationUnitService.getRootOrganizationUnits()
    } else {
      // 子节点
      organizationUnitItems = await OrganizationUnitService.findOrganizationUnitChildren(node.data.id, undefined)
    }
    if (organizationUnitItems.items.length !== 0) {
      const organizationUnits = new Array<OrganizationUnitTree>()
      organizationUnitItems.items.map((item) => {
        const organizationUnit = new OrganizationUnitTree()
        organizationUnit.id = item.id
        organizationUnit.parentId = item.parentId
        organizationUnit.code = item.code
        organizationUnit.displayName = item.displayName
        organizationUnits.push(organizationUnit)
        const children = node.data.children as OrganizationUnitTree[]
        if (!children.every(x => x.id === item.id)) {
          children.push(organizationUnit)
        }
      })
      return resolve(organizationUnits)
    }
    return resolve([])
  }

  private handleChangeOrganizationUint(command: { key: string, node: any, data: any}) {
    switch (command.key) {
      case 'append' :
        this.handleAppendOrganizationUnit(command.node, command.data)
        break
      case 'remove' :
        this.handleRemoveOrganizationUnit(command.node, command.data)
        break
      default: break
    }
  }

  private handleAppendOrganizationUnit(node: any, data: any) {
    this.$prompt('请输入组织机构名称',
      '新增组织机构', {
        showInput: true,
        inputValidator: (val) => {
          return !(!val || val.length === 0)
        },
        inputErrorMessage: '组织机构名称不能为空',
        inputPlaceholder: '请输入机构名称'
      }).then((val: any) => {
      const organizationUnit = new OrganizationUnitCreate()
      organizationUnit.parentId = data.id
      organizationUnit.displayName = val.value
      OrganizationUnitService.createOrganizationUnit(organizationUnit).then(res => {
        const organizationUnit = new OrganizationUnitTree()
        organizationUnit.id = res.id
        organizationUnit.parentId = res.parentId
        organizationUnit.code = res.code
        organizationUnit.displayName = res.displayName
        data.children.push(organizationUnit)
      })
    }).catch(_ => _)
  }

  private handleRemoveOrganizationUnit(node: any, data: any) {
    this.$confirm('删除组织机构',
      '是否要删除组织机构 ' + data.displayName, {
        callback: (action) => {
          if (action === 'confirm') {
            OrganizationUnitService.deleteOrganizationUnit(data.id).then(() => {
              this.$message.success('组织机构 ' + data.displayName + ' 已删除')
              const parent = node.parent
              const children = parent.data.children as OrganizationUnitTree[]
              const index = children.findIndex(d => d.id === data.id)
              children.splice(index, 1)
              parent.childNodes.splice(index, 1)
            })
          }
        }
      })
  }

  private handleAllowDrag(draggingNode: any) {
    return draggingNode.data.parentId !== undefined && draggingNode.data.parentId !== null
  }

  private handleAllowDrop(draggingNode: any, dropNode: any) {
    return dropNode.code !== 'root'
  }

  private handleNodeDroped(draggingNode: any, dropNode: any) {
    OrganizationUnitService.moveOrganizationUnit(draggingNode.data.id, dropNode.data.id).then(res => {
      const organizationUnit = new OrganizationUnitTree()
      organizationUnit.id = res.id
      organizationUnit.parentId = res.parentId
      organizationUnit.code = res.code
      organizationUnit.displayName = res.displayName
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
