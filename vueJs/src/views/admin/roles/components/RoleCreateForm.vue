<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentity.NewRole')"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="roleCreateForm"
      label-width="120px"
      :model="createRole"
    >
      <el-form-item
        prop="name"
        :label="$t('AbpIdentity.RoleName')"
      >
        <el-input
          v-model="createRole.name"
        />
      </el-form-item>
      <el-form-item
        prop="isDefault"
        :label="$t('AbpIdentity.DisplayName:IsDefault')"
      >
        <el-switch
          v-model="createRole.isDefault"
        />
      </el-form-item>
      <el-form-item
        prop="isPublic"
        :label="$t('AbpIdentity.DisplayName:IsPublic')"
      >
        <el-switch
          v-model="createRole.isPublic"
        />
      </el-form-item>
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
import { Form } from 'element-ui'
import { Component, Mixins, Prop } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import RoleApiService, { CreateRoleDto } from '@/api/roles'

@Component({
  name: 'RoleCreateForm'
})
export default class RoleCreateForm extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  private createRole = new CreateRoleDto()

  private onFormClosed(changed: boolean) {
    const roleCreateForm = this.$refs.roleCreateForm as Form
    roleCreateForm.resetFields()
    this.$emit('closed', changed)
  }

  private onSave() {
    const roleCreateForm = this.$refs.roleCreateForm as Form
    roleCreateForm.validate(valid => {
      if (valid) {
        RoleApiService.createRole(this.createRole).then(res => {
          const message = this.l('roles.createRoleSuccess', { name: res.name })
          this.$message.success(message)
          this.onFormClosed(true)
        })
      }
    })
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
