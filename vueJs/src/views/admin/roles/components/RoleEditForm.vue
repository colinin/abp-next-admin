<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentity.RoleSubject', {0: role.name})"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="roleEditForm"
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
      </el-tabs>
      <el-form-item>
        <el-button
          class="cancel"
          style="width:100px"
          @click="onFormClosed(false)"
        >
          {{ $t('global.cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          style="width:100px"
          @click="onSave"
        >
          {{ $t('global.confirm') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Prop, Watch, Vue } from 'vue-property-decorator'
import RoleService, { RoleDto, UpdateRoleDto } from '@/api/roles'
import OrganizationUnitTree from '@/components/OrganizationUnitTree/index.vue'
import { ChangeUserOrganizationUnitDto } from '@/api/users'
import { Form } from 'element-ui'

@Component({
  name: 'RoleEditForm',
  components: {
    OrganizationUnitTree
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private roleId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private roleTabItem = 'basic'
  private role = new RoleDto()
  /** 是否加载用户权限 */
  private rolePermissionLoaded = false
  private roleOrganizationUnitChanged = false
  private roleOrganizationUnits = new Array<string>()

  private roleRules = {
    name: [
      { required: true, message: this.l('global.pleaseInputBy', { key: this.l('roles.name') }), trigger: 'blur' },
      { min: 3, max: 20, message: this.l('global.charLengthRange', { min: 3, max: 20 }), trigger: 'blur' }
    ]
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetRole()
  }

  private handleGetRole() {
    if (this.showDialog && this.roleId) {
      RoleService.getRoleById(this.roleId).then(role => {
        this.role = role
        this.handledGetRoleOrganizationUnits(role.id)
      })
      this.roleOrganizationUnitChanged = false
      this.roleOrganizationUnits = new Array<string>()
    }
  }

  private handledGetRoleOrganizationUnits(roleId: string) {
    RoleService.getRoleOrganizationUnits(roleId).then(res => {
      this.roleOrganizationUnits = res.items.map(ou => ou.id)
    })
  }

  private onOrganizationUnitsChanged(checkedKeys: string[]) {
    this.roleOrganizationUnitChanged = true
    this.roleOrganizationUnits = checkedKeys
  }

  private onSave() {
    const roleEditForm = this.$refs.roleEditForm as any
    roleEditForm.validate(async(valid: boolean) => {
      if (valid) {
        const roleUpdateDto = new UpdateRoleDto()
        roleUpdateDto.name = this.role.name
        roleUpdateDto.isPublic = this.role.isPublic
        roleUpdateDto.isDefault = this.role.isDefault
        roleUpdateDto.concurrencyStamp = this.role.concurrencyStamp
        if (this.roleOrganizationUnitChanged) {
          const roleOrganizationUnitDto = new ChangeUserOrganizationUnitDto()
          roleOrganizationUnitDto.organizationUnitIds = this.roleOrganizationUnits
          await RoleService.changeRoleOrganizationUnits(this.roleId, roleOrganizationUnitDto)
        }
        RoleService.updateRole(this.roleId, roleUpdateDto).then(role => {
          this.$message.success(this.l('roles.updateRoleSuccess', { name: role.name }))
          this.onFormClosed(true)
        })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    this.roleTabItem = 'basic'
    this.rolePermissionLoaded = false
    const roleEditForm = this.$refs.roleEditForm as Form
    roleEditForm.resetFields()
    this.$emit('closed', changed)
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
