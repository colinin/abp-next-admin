<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpPermissionManagement.Permissions') + '-' + entityDisplayName"
    custom-class="modal-form"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    :show-close="false"
    @close="onFormClosed"
  >
    <el-form>
      <el-checkbox
        :disabled="readonly"
        :value="grantAllCheckBoxCheckAll"
        :indeterminate="grantAllCheckBoxForward"
        @change="onGrantAllClicked"
      >
        {{ $t('AbpPermissionManagement.SelectAllInAllTabs') }}
      </el-checkbox>
      <el-divider />
      <el-tabs
        v-model="activeTabPane"
        tab-position="left"
        type="card"
      >
        <el-tab-pane
          v-for="group in permissionGroups"
          :key="group.name"
          :label="group.displayName + ' (' + grantedCount(group) + ')'"
          :name="group.name"
        >
          <el-card shadow="never">
            <div
              slot="header"
              class="clearfix"
            >
              <h3>{{ group.displayName }}</h3>
            </div>
            <el-checkbox
              :disabled="readonly"
              :value="scopeCheckBoxCheckAll(group)"
              :indeterminate="scopeCheckBoxForward(group)"
              @change="(checked) => onCheckScopeAllClicked(checked, group, 'permissionTree-' + group.name)"
            >
              {{ $t('AbpPermissionManagement.SelectAllInThisTab') }}
            </el-checkbox>
            <el-divider />
            <el-tree
              :ref="'permissionTree-' + group.name"
              show-checkbox
              :check-strictly="true"
              node-key="id"
              :data="group.permissions"
              :default-checked-keys="grantedPermissionKeys(group)"
              @check-change="(permission, checked) => onPermissionTreeNodeCheckChanged(permission, checked, group, 'permissionTree-' + group.name)"
            />
          </el-card>
        </el-tab-pane>
      </el-tabs>
      <el-divider />
      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          style="width:100px"
          @click="onFormClosed"
        >
          {{ $t('AbpPermissionManagement.Cancel') }}
        </el-button>
        <el-button
          :disabled="readonly"
          class="confirm"
          type="primary"
          style="width:100px"
          icon="el-icon-check"
          :loading="confirmButtonBusy"
          @click="onSave"
        >
          {{ confirmButtonTitle }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import PermissionApiService, { Permission, UpdatePermissionsDto } from '@/api/permission'
import { Tree } from 'element-ui'

/** element权限树 */
export class PermissionItem {
  /** 权限标识 */
  id = ''
  /** 显示名称 */
  label = ''
  /** 是否授权 */
  isGrant = false
  /** 是否禁用 */
  disabled = false
  /** 子节点 */
  children = new Array<PermissionItem>()
  /** 父节点 */
  parent?: PermissionItem

  constructor(
    id: string,
    label: string,
    isGrant: boolean
  ) {
    this.id = id
    this.label = label
    this.isGrant = isGrant
  }

  public createChildren(permission: PermissionItem) {
    permission.parent = this
    this.children.push(permission)
  }

  public setGrant(grant: boolean) {
    this.isGrant = grant
    // fix bug: 会无限的追踪到跟节点,来进行全部取消授权
    if (this.parent && !this.parent.isGrant) {
      this.parent.setGrant(grant)
    }
  }

  public static setPermissionGrant(grant: boolean, permission: PermissionItem) {
    permission.setGrant(grant)
    if (!grant) {
      permission.children.map(p => {
        PermissionItem.setPermissionGrant(false, p)
      })
    }
  }

  public static setAllPermissionGrant(grant: boolean, permission: PermissionItem) {
    permission.setGrant(grant)
    permission.children.map(p => {
      PermissionItem.setAllPermissionGrant(grant, p)
    })
  }
}

export class PermissionGroup {
  name = ''
  displayName = ''
  permissions = new Array<PermissionItem>()

  constructor(
    name: string,
    displayName: string
  ) {
    this.name = name
    this.displayName = displayName
  }

  public permissionCount() {
    let count = 0
    count += this.deepPermissionCount(this.permissions)
    return count
  }

  public addPermission(permission: PermissionItem) {
    this.permissions.push(permission)
  }

  public setAllGrant(grant: boolean) {
    this.permissions.map(p => {
      PermissionItem.setAllPermissionGrant(grant, p)
    })
  }

  public grantedPermissionKeys() {
    const keys = new Array<string>()
    this.deepGrantedPermissionKeys(keys, this.permissions)
    return keys
  }

  public grantedCount() {
    let count = 0
    count += this.deepGrantedCount(this.permissions)
    return count
  }

  private deepGrantedCount(permissions: PermissionItem[]) {
    let count = 0
    count += permissions.filter(p => p.isGrant).length
    permissions.forEach(p => {
      count += this.deepGrantedCount(p.children)
    })
    return count
  }

  private deepGrantedPermissionKeys(keys: string[], permissions: PermissionItem[]) {
    permissions.forEach(p => {
      if (p.isGrant) {
        keys.push(p.id)
      }
      this.deepGrantedPermissionKeys(keys, p.children)
    })
  }

  private deepPermissionCount(permissions: PermissionItem[]) {
    let count = 0
    count += permissions.length
    permissions.forEach(p => {
      count += this.deepPermissionCount(p.children)
    })
    return count
  }
}

/**
 * 权限编辑组件
 * 大量的计算属性与事件响应,还能再优化
 */
@Component({
  name: 'PermissionForm'
})
export default class PermissionForm extends Vue {
  /** 权限提供者名称 */
  @Prop({ default: '' })
  private providerName!: string

  /** 权限提供者标识 */
  @Prop({ default: '' })
  private providerKey!: string

  /** 是否展示权限编辑组件 */
  @Prop({ default: false })
  private showDialog!: boolean

  /** 权限节点是否只读 */
  @Prop({ default: false })
  private readonly!: boolean

  /** 激活tab页 */
  private activeTabPane = ''
  /** 确认按钮忙碌状态 */
  private confirmButtonBusy = false
  /** 当前编辑权限实体名称 */
  private entityDisplayName = ''
  /** 得到的权限组集合 */
  private permissionGroups = new Array<PermissionGroup>()

  /** 某个权限组已授权数量
   * 用于显示已授权数量
   */
  get grantedCount() {
    return (group: PermissionGroup) => {
      return group.grantedCount()
    }
  }

  /**
   * 所有已授权数量
   */
  get grantAllCount() {
    let count = 0
    this.permissionGroups.forEach(group => {
      count += group.grantedCount()
    })
    return count
  }

  /** 某个权限组已授权节点
   * 用于勾选TreeNode
   */
  get grantedPermissionKeys() {
    return (group: PermissionGroup) => {
      return group.grantedPermissionKeys()
    }
  }

  /**
   * 某个权限组权限数量
   * 用于设定单个Tree的全选CheckBox状态
   */
  get permissionCount() {
    return (group: PermissionGroup) => {
      return group.permissionCount()
    }
  }

  /**
   * 所有权限数量
   */
  get permissionAllCount() {
    let count = 0
    this.permissionGroups.forEach(group => {
      count += group.permissionCount()
    })
    return count
  }

  /**
   * 单个Tree的全选CheckBox是否为选中状态
   */
  get scopeCheckBoxCheckAll() {
    return (group: PermissionGroup) => {
      const grantCount = group.grantedCount()
      return grantCount === group.permissionCount()
    }
  }

  /**
   * 单个Tree的全选CheckBox状态是否为预选状态
   */
  get scopeCheckBoxForward() {
    return (group: PermissionGroup) => {
      const grantCount = group.grantedCount()
      return grantCount > 0 && grantCount < group.permissionCount()
    }
  }

  /**
   * 授权所有CheckBox是否为选中状态
   */
  get grantAllCheckBoxCheckAll() {
    return this.grantAllCount === this.permissionAllCount
  }

  /**
   * 授权所有CheckBox状态是否为预选状态
   */
  get grantAllCheckBoxForward() {
    const grantCount = this.grantAllCount
    return grantCount > 0 && grantCount < this.permissionAllCount
  }

  /**
   * 确认按钮标题
   */
  get confirmButtonTitle() {
    if (this.confirmButtonBusy) {
      return this.$t('AbpPermissionManagement.SavingWithThreeDot')
    }
    return this.$t('AbpPermissionManagement.Save')
  }

  /**
   * 响应组件可视事件
   */
  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetPermissions()
  }

  /**
   * 获取权限集合
   */
  private handleGetPermissions() {
    this.activeTabPane = ''
    this.permissionGroups.length = 0
    if (this.showDialog && this.providerName) {
      PermissionApiService.getPermissionsByKey(this.providerName, this.providerKey).then(res => {
        this.entityDisplayName = res.entityDisplayName
        res.groups.map(g => {
          const group = new PermissionGroup(g.name, g.displayName)
          const parents = g.permissions.filter(p => p.parentName === null)
          parents.forEach(parent => {
            const permission = new PermissionItem(parent.name, parent.displayName, parent.isGranted)
            permission.disabled = this.readonly
            const subPermissions = g.permissions.filter(p => p.parentName?.startsWith(parent.name))
            this.generatePermission(permission, subPermissions)
            group.addPermission(permission)
          })
          this.permissionGroups.push(group)
        })
        if (this.permissionGroups.length > 0) {
          this.activeTabPane = this.permissionGroups[0].name
        }
      })
    }
  }

  /** 递归生成子节点
   * @param permissionTree 二级权限树
   * @param permissions 权限列表
   */
  private generatePermission(permission: PermissionItem, permissions: Permission[]) {
    const subPermissions = permissions.filter(p => p.parentName !== permission.id)
    permissions = permissions.filter(p => p.parentName === permission.id)
    permissions.forEach(p => {
      const children = new PermissionItem(p.name, p.displayName, p.isGranted)
      children.disabled = this.readonly
      const itemSubPermissions = subPermissions.filter(sp => sp.parentName === p.name)
      if (itemSubPermissions.length > 0) {
        this.generatePermission(children, itemSubPermissions)
      }
      permission.createChildren(children)
    })
  }

  /**
   * 保存权限
   */
  private onSave() {
    const updatePermission = new UpdatePermissionsDto()
    this.permissionGroups.forEach(group => {
      this.updatePermissionByInput(updatePermission, group.permissions)
    })
    this.confirmButtonBusy = true
    PermissionApiService
      .setPermissionsByKey(this.providerName, this.providerKey, updatePermission)
      .then(() => {
        this.$message.success(this.$t('global.successful').toString())
      })
      .finally(() => {
        this.confirmButtonBusy = false
      })
  }

  private updatePermissionByInput(permissions: UpdatePermissionsDto, items: PermissionItem[]) {
    items.forEach(p => {
      permissions.addPermission(p.id, p.isGrant)
      this.updatePermissionByInput(permissions, p.children)
    })
  }

  /**
   * 窗口关闭事件
   */
  private onFormClosed() {
    this.$emit('closed')
  }

  /**
   * 授予所有权限 按钮事件
   */
  private onGrantAllClicked(checked: boolean) {
    this.permissionGroups.forEach(group => {
      group.setAllGrant(checked)
      const trees = this.$refs['permissionTree-' + group.name] as Tree[]
      trees[0].setCheckedKeys(this.grantedPermissionKeys(group))
    })
  }

  /**
   * Permission Tree 全选按钮事件
   */
  private onCheckScopeAllClicked(checked: boolean, group: PermissionGroup, treeRef: any) {
    group.setAllGrant(checked)
    const trees = this.$refs[treeRef] as Tree[]
    trees[0].setCheckedKeys(this.grantedPermissionKeys(group))
  }

  /**
   * Permission TreeNode 变更事件
   */
  private onPermissionTreeNodeCheckChanged(permission: PermissionItem, checked: boolean, group: PermissionGroup, treeRef: any) {
    PermissionItem.setPermissionGrant(checked, permission)
    if (permission.children.length > 0) {
      const trees = this.$refs[treeRef] as Tree[]
      trees[0].setCheckedKeys(this.grantedPermissionKeys(group))
    }
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
