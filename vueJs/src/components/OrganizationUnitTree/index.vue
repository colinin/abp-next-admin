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
import itemVue from '../MessageView/item.vue'

class OrganizationUnitItem {
  id?: string
  parentId?: string
  code!: string
  displayName!: string
  isLeaf!: boolean
  children = new Array<OrganizationUnitItem>()

  constructor() {
    this.isLeaf = false
  }

  public createChildren(chidlren: OrganizationUnitItem) {
    this.children.push(chidlren)
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
export default class OrganizationUnitTree extends Vue {
  @Prop({ default: () => { return new Array<string>() } })
  private checkedOrganizationUnits!: string[]

  private organizationUnits = new Array<OrganizationUnitItem>()
  private selectionOrganizationUnits = new Array<string>()

  @Watch('checkedOrganizationUnits')
  private onOrganizationUnitsChanged() {
    const elTree = this.$refs.organizationUnitTree as any
    elTree.setCheckedKeys(this.checkedOrganizationUnits)
  }

  mounted() {
    this.handleGetOrganizationUnits()
  }

  private handleGetOrganizationUnits() {
    this.organizationUnits = new Array<OrganizationUnitItem>()
    OrganizationUnitService.getAllOrganizationUnits()
      .then(res => {
        const rootOrganizationUnits = res.items.filter(item => !item.parentId)
        rootOrganizationUnits.forEach(item => {
          const organizationUnit = new OrganizationUnitItem()
          organizationUnit.id = item.id
          organizationUnit.parentId = item.parentId
          organizationUnit.code = item.code
          organizationUnit.displayName = item.displayName
          const subOrganizationUnits = rootOrganizationUnits.filter(p => p.parentId === item.id)
          this.generateOrganizationUnit(organizationUnit, subOrganizationUnits)
          this.organizationUnits.push(organizationUnit)
        })
      })
  }

  private generateOrganizationUnit(organizationUnit: OrganizationUnitItem, organizationUnits: OrganizationUnit[]) {
    const subOrganizationUnit = organizationUnits.filter(ou => ou.parentId !== organizationUnit.id)
    organizationUnits = organizationUnits.filter(ou => ou.parentId === organizationUnit.id)
    organizationUnits.forEach(ou => {
      const children = new OrganizationUnitItem()
      children.id = ou.id
      children.parentId = ou.parentId
      children.code = ou.code
      children.displayName = ou.displayName
      const itemSubOrganizationUnit = subOrganizationUnit.filter(sou => sou.parentId === ou.id)
      if (itemSubOrganizationUnit.length > 0) {
        this.generateOrganizationUnit(children, itemSubOrganizationUnit)
      }
      organizationUnit.createChildren(children)
    })
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
      const rootOrganizationUnit = new OrganizationUnitItem()
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
      const organizationUnits = new Array<OrganizationUnitItem>()
      organizationUnitItems.items.map((item) => {
        const organizationUnit = new OrganizationUnitItem()
        organizationUnit.id = item.id
        organizationUnit.parentId = item.parentId
        organizationUnit.code = item.code
        organizationUnit.displayName = item.displayName
        organizationUnits.push(organizationUnit)
        const children = node.data.children as OrganizationUnitItem[]
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
