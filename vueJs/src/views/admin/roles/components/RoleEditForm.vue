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
    >
      <el-form-item
        prop="name"
        :label="$t('AbpIdentity.DisplayName:RoleName')"
        :rules="{
          required: true,
          message: $t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:RoleName')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="role.name"
          :disabled="role.isStatic"
          :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:RoleName')})"
        />
      </el-form-item>
      <el-form-item>
        <el-button
          class="cancel"
          style="width:100px"
          type="info"
          @click="onFormClosed(false)"
        >
          {{ $t('AbpIdentity.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          style="width:100px"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpIdentity.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import RoleService, { RoleDto, UpdateRoleDto } from '@/api/roles'
import { Form } from 'element-ui'

@Component({
  name: 'RoleEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private roleId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private role = new RoleDto()

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetRole()
  }

  private handleGetRole() {
    if (this.showDialog && this.roleId) {
      RoleService.getRoleById(this.roleId).then(role => {
        this.role = role
      })
    }
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
        RoleService.updateRole(this.roleId, roleUpdateDto).then(() => {
          this.$message.success(this.l('global.successful'))
          this.onFormClosed(true)
        })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    const roleEditForm = this.$refs.roleEditForm as Form
    roleEditForm.resetFields()
    this.$emit('closed', changed)
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
