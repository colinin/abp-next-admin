<template>
  <el-form
    ref="formEditRole"
    label-width="110px"
    :model="role"
    :rules="roleRules"
  >
    <el-tabs v-model="roleTabItem">
      <el-tab-pane
        :label="$t('roles.basic')"
        name="basic"
      >
        <el-form-item
          prop="name"
          :label="$t('roles.name')"
        >
          <el-input
            v-model="role.name"
            :disabled="role.isStatic"
            :placeholder="$t('global.pleaseInputBy', {key: $t('roles.name')})"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('roles.organizationUnits')"
        name="organizationUnits"
      >
        <organization-unit-tree
          :checked-organization-units="roleOrganizationUnits"
          @onOrganizationUnitsChanged="onOrganizationUnitsChanged"
        />
      </el-tab-pane>
      <el-tab-pane
        v-if="rolePermissionLoaded"
        :label="$t('roles.permission')"
        name="permissions"
      >
        <permission-tree
          ref="permissionTree"
          :expanded="false"
          :readonly="!checkPermission(['AbpIdentity.Roles.ManagePermissions'])"
          :permission="rolePermission"
          @onPermissionChanged="onPermissionChanged"
        />
      </el-tab-pane>
    </el-tabs>
    <el-form-item>
      <el-button
        class="cancel"
        style="width:100px"
        @click="onCancel"
      >
        {{ $t('global.cancel') }}
      </el-button>
      <el-button
        class="confirm"
        type="primary"
        style="width:100px"
        @click="onConfirm"
      >
        {{ $t('global.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import { IPermission } from '@/api/types'
import { checkPermission } from '@/utils/permission'
import { Component, Prop, Watch, Vue } from 'vue-property-decorator'
import RoleService, { RoleDto, UpdateRoleDto } from '@/api/roles'
import PermissionTree from '@/components/PermissionTree/index.vue'
import OrganizationUnitTree from '@/components/OrganizationUnitTree/index.vue'
import PermissionService, { PermissionDto, UpdatePermissionsDto } from '@/api/permission'
import { ChangeUserOrganizationUnitDto } from '@/api/users'

@Component({
  name: 'RoleEditForm',
  components: {
    PermissionTree,
    OrganizationUnitTree
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private roleId!: string

  private roleTabItem = 'basic'
  private role = new RoleDto()
  /** 是否加载用户权限 */
  private rolePermissionLoaded = false
  private roleOrganizationUnitChanged = false
  private roleOrganizationUnits = new Array<string>()

  /** 角色权限数据 */
  private rolePermission = new PermissionDto()
  /** 角色权限已变更 */
  private rolePermissionChanged = false
  /** 变更角色权限数据 */
  private editRolePermissions = new Array<IPermission>()

  private roleRules = {
    name: [
      { required: true, message: this.l('global.pleaseInputBy', { key: this.l('roles.name') }), trigger: 'blur' },
      { min: 3, max: 20, message: this.l('global.charLengthRange', { min: 3, max: 20 }), trigger: 'blur' }
    ]
  }

  @Watch('roleId', { immediate: true })
  private onRoleIdChanged() {
    if (this.roleId) {
      RoleService.getRoleById(this.roleId).then(role => {
        this.role = role
        this.handledGetRoleOrganizationUnits(role.id)
        this.handleGetRolePermissions(role.name)
      })
    }
    this.roleOrganizationUnitChanged = false
    this.roleOrganizationUnits = new Array<string>()
  }

  private handledGetRoleOrganizationUnits(roleId: string) {
    RoleService.getRoleOrganizationUnits(roleId).then(res => {
      this.roleOrganizationUnits = res.items.map(ou => ou.id)
    })
  }

  private handleGetRolePermissions(roleName: string) {
    PermissionService.getPermissionsByKey('R', roleName).then(permission => {
      this.rolePermission = permission
      this.rolePermissionLoaded = true
    })
  }

  private onOrganizationUnitsChanged(checkedKeys: string[]) {
    this.roleOrganizationUnitChanged = true
    this.roleOrganizationUnits = checkedKeys
  }

  private onPermissionChanged(permissions: IPermission[]) {
    this.rolePermissionChanged = true
    this.editRolePermissions = permissions
  }

  private onConfirm() {
    const frmRole = this.$refs.formEditRole as any
    frmRole.validate(async(valid: boolean) => {
      if (valid) {
        const roleUpdateDto = new UpdateRoleDto()
        roleUpdateDto.name = this.role.name
        roleUpdateDto.isPublic = this.role.isPublic
        roleUpdateDto.isDefault = this.role.isDefault
        roleUpdateDto.concurrencyStamp = this.role.concurrencyStamp
        if (this.rolePermissionChanged) {
          const setRolePermissions = new UpdatePermissionsDto()
          setRolePermissions.permissions = this.editRolePermissions
          await PermissionService.setPermissionsByKey('R', this.rolePermission.entityDisplayName, setRolePermissions)
        }
        if (this.roleOrganizationUnitChanged) {
          const roleOrganizationUnitDto = new ChangeUserOrganizationUnitDto()
          roleOrganizationUnitDto.organizationUnitIds = this.roleOrganizationUnits
          await RoleService.changeRoleOrganizationUnits(this.roleId, roleOrganizationUnitDto)
        }
        RoleService.updateRole(this.roleId, roleUpdateDto).then(role => {
          this.$message.success(this.l('roles.updateRoleSuccess', { name: role.name }))
          this.onCancel()
        })
      }
    })
  }

  private onCancel() {
    this.rolePermissionLoaded = false
    this.roleTabItem = 'basic'
    this.$emit('onClosed')
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
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
