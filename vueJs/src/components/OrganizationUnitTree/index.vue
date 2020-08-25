<template>
  <el-tree
    ref="organizationUnitTree"
    show-checkbox
    node-key="id"
    :props="organizationUnitProps"
    :load="loadOrganizationUnit"
    lazy
    draggable
    highlight-current
    :default-checked-keys="checkedOrganizationUnits"
    @check="onOrganizationUnitsChecked"
  />
</template>

<script lang="ts">
import { Component, Prop, Watch, Vue } from 'vue-property-decorator'
import { ListResultDto } from '@/api/types'
import OrganizationUnitService, { OrganizationUnit } from '@/api/organizationunit'

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
  name: 'OrganizationUnitTree',
  data() {
    return {
      organizationUnitProps: {
        label: 'displayName',
        isLeaf: 'isLeaf',
        children: 'children'
      }
    }
  }
})
export default class extends Vue {
  @Prop({ default: () => { return new Array<string>() } })
  private checkedOrganizationUnits!: string[]

  private selectionOrganizationUnits = new Array<string>()

  @Watch('checkedOrganizationUnits')
  private onOrganizationUnitsChanged() {
    const elTree = this.$refs.organizationUnitTree as any
    elTree.setCheckedKeys(this.checkedOrganizationUnits)
  }

  private onOrganizationUnitsChecked(data: any, treeCheckData: any) {
    const checkKeys = treeCheckData.checkedKeys
    const valiadOuId = checkKeys.findIndex((key: string) => key === undefined)
    if (valiadOuId !== -1) {
      checkKeys.splice(valiadOuId, 1)
    }
    this.$emit('onOrganizationUnitsChanged', checkKeys)
  }

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
}
</script>

<style lang="scss" scoped>

</style>
