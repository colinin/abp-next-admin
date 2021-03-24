<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentity.ManageClaim')"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="true"
    :close-on-press-escape="true"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="roleClaimForm"
      :model="editRoleClaim"
      label-width="120px"
      :rules="roleClaimRules"
    >
      <el-form-item
        prop="claimType"
        :label="$t('AbpIdentity.DisplayName:ClaimType')"
      >
        <el-select
          v-model="editRoleClaim.claimType"
          style="width: 100%"
          @change="onClaimTypeChanged"
        >
          <el-option
            v-for="claim in claimTypes"
            :key="claim.id"
            :label="claim.name"
            :value="claim.name"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        prop="claimValue"
        :label="$t('AbpIdentity.DisplayName:ClaimValue')"
      >
        <el-input
          v-if="hasStringValueType(editRoleClaim.claimType)"
          v-model="editRoleClaim.claimValue"
          type="text"
        />
        <el-input
          v-else-if="hasIntegerValueType(editRoleClaim.claimType)"
          v-model="editRoleClaim.claimValue"
          type="number"
        />
        <el-switch
          v-else-if="hasBooleanValueType(editRoleClaim.claimType)"
          v-model="editRoleClaim.claimValue"
        />
        <el-date-picker
          v-else-if="hasDateTimeValueType(editRoleClaim.claimType)"
          v-model="editRoleClaim.claimValue"
          type="datetime"
          style="width: 100%"
        />
      </el-form-item>
      <el-form-item
        style="text-align: center;"
        label-width="0px"
      >
        <el-button
          type="primary"
          style="width:180px"
          @click="onSave"
        >
          {{ $t('AbpIdentity.AddClaim') }}
        </el-button>
      </el-form-item>
    </el-form>
    <el-table
      row-key="id"
      :data="roleClaims"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:ClaimType')"
        prop="claimType"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.claimType }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:ClaimValue')"
        prop="claimValue"
        sortable
        min-width="100%"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ claimValue(row.claimType, row.claimValue) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="150px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentity.Roles.ManageClaims'])"
            size="mini"
            type="danger"
            @click="handleDeleteUserClaim(row)"
          >
            {{ $t('AbpIdentity.DeleteClaim') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import RoleApiService, { RoleClaim, RoleClaimCreateOrUpdate, RoleClaimDelete } from '@/api/roles'
import ClaimTypeApiService, { IdentityClaimType, IdentityClaimValueType } from '@/api/cliam-type'
import { Form } from 'element-ui'

@Component({
  name: 'RoleClaimCreateOrUpdateForm',
  methods: {
    checkPermission
  }
})
export default class UserClaimCreateOrUpdateForm extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private roleId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private editRoleClaim = new RoleClaimCreateOrUpdate()
  private roleClaims = new Array<RoleClaim>()
  private claimTypes = new Array<IdentityClaimType>()

  private roleClaimRules = {}

  get cliamType() {
    return (claimName: string) => {
      const claimIndex = this.claimTypes.findIndex(cliam => cliam.name === claimName)
      if (claimIndex >= 0) {
        return this.claimTypes[claimIndex].valueType
      }
      return IdentityClaimValueType.String
    }
  }

  get claimValue() {
    return (type: string, value: string) => {
      const valueType = this.cliamType(type)
      switch (valueType) {
        case IdentityClaimValueType.Int :
        case IdentityClaimValueType.String :
          return value
        case IdentityClaimValueType.Boolean :
          return value.toLowerCase() === 'true'
        case IdentityClaimValueType.DateTime :
          return dateFormat(new Date(value), 'YYYY-mm-dd HH:MM:SS')
      }
    }
  }

  get hasStringValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.String
    }
  }

  get hasBooleanValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.Boolean
    }
  }

  get hasDateTimeValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.DateTime
    }
  }

  get hasIntegerValueType() {
    return (claimName: string) => {
      return this.cliamType(claimName) === IdentityClaimValueType.Int
    }
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetRoleClaims()
  }

  mounted() {
    this.handleGetClaimTypes()
    this.handleGetRoleClaims()
    this.roleClaimRules = {
      claimType: [
        { required: true, message: this.l('pleaseSelectBy', { key: this.l('AbpIdentity.DisplayName:ClaimType') }), trigger: 'blur' }
      ],
      claimValue: [
        { required: true, message: this.l('pleaseInputBy', { key: this.l('AbpIdentity.DisplayName:ClaimValue') }), trigger: 'blur' }
      ]
    }
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService.getActivedClaimTypes().then(res => {
      this.claimTypes = res.items
    })
  }

  private handleGetRoleClaims() {
    if (this.showDialog && this.roleId) {
      RoleApiService.getRoleClaims(this.roleId).then(res => {
        this.roleClaims = res.items
      })
    }
  }

  private handleDeleteUserClaim(claim: RoleClaim) {
    this.$confirm(this.l('AbpIdentity.DeleteClaim'),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            const deleteClaim = new RoleClaimDelete()
            deleteClaim.claimType = claim.claimType
            deleteClaim.claimValue = claim.claimValue
            RoleApiService.deleteRoleClaim(this.roleId, deleteClaim).then(() => {
              this.$message.success(this.l('global.successful'))
              const claimIndex = this.roleClaims.findIndex(uc => uc.id === claim.id)
              this.roleClaims.splice(claimIndex, 1)
            })
          }
        }
      })
  }

  // TODO: 可以简化为一个组件,通过组件来实现用户/客户端等的声明类型
  private onClaimTypeChanged() {
    const valueType = this.cliamType(this.editRoleClaim.claimType)
    switch (valueType) {
      case IdentityClaimValueType.Int :
        this.editRoleClaim.claimValue = '0'
        break
      case IdentityClaimValueType.String :
        this.editRoleClaim.claimValue = ''
        break
      case IdentityClaimValueType.Boolean :
        this.editRoleClaim.claimValue = 'false'
        break
      case IdentityClaimValueType.DateTime :
        this.editRoleClaim.claimValue = ''
        break
    }
  }

  private onSave() {
    const roleClaimForm = this.$refs.roleClaimForm as Form
    roleClaimForm.validate(valid => {
      if (valid) {
        RoleApiService.addRoleClaim(this.roleId, this.editRoleClaim).then(() => {
          this.$message.success(this.$t('global.successful').toString())
          roleClaimForm.resetFields()
          this.handleGetRoleClaims()
        })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    const roleClaimForm = this.$refs.roleClaimForm as Form
    roleClaimForm.resetFields()
    this.$emit('closed', changed)
  }
}
</script>
