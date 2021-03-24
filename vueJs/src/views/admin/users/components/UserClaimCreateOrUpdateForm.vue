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
      ref="userClaimForm"
      :model="editUserClaim"
      label-width="120px"
      :rules="userClaimRules"
    >
      <el-form-item
        prop="claimType"
        :label="$t('AbpIdentity.DisplayName:ClaimType')"
      >
        <el-select
          v-model="editUserClaim.claimType"
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
          v-if="hasStringValueType(editUserClaim.claimType)"
          v-model="editUserClaim.claimValue"
          type="text"
        />
        <el-input
          v-else-if="hasIntegerValueType(editUserClaim.claimType)"
          v-model="editUserClaim.claimValue"
          type="number"
        />
        <el-switch
          v-else-if="hasBooleanValueType(editUserClaim.claimType)"
          v-model="editUserClaim.claimValue"
        />
        <el-date-picker
          v-else-if="hasDateTimeValueType(editUserClaim.claimType)"
          v-model="editUserClaim.claimValue"
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
      :data="userClaims"
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
            :disabled="!checkPermission(['AbpIdentity.Users.ManageClaims'])"
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
import UserService, { UserClaim, UserClaimCreateOrUpdate, UserClaimDelete } from '@/api/users'
import ClaimTypeApiService, { IdentityClaimType, IdentityClaimValueType } from '@/api/cliam-type'
import { Form } from 'element-ui'

@Component({
  name: 'UserClaimCreateOrUpdateForm',
  methods: {
    checkPermission
  }
})
export default class UserClaimCreateOrUpdateForm extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private userId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private editUserClaim = new UserClaimCreateOrUpdate()
  private userClaims = new Array<UserClaim>()
  private claimTypes = new Array<IdentityClaimType>()

  private userClaimRules = {}

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
    this.handleGetUserClaims()
  }

  mounted() {
    this.handleGetClaimTypes()
    this.handleGetUserClaims()
    this.userClaimRules = {
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

  private handleGetUserClaims() {
    if (this.userId && this.showDialog) {
      UserService.getUserClaims(this.userId).then(res => {
        this.userClaims = res.items
      })
    }
  }

  private handleDeleteUserClaim(claim: UserClaim) {
    this.$confirm(this.l('AbpIdentity.DeleteClaim'),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            const deleteClaim = new UserClaimDelete()
            deleteClaim.claimType = claim.claimType
            deleteClaim.claimValue = claim.claimValue
            UserService.deleteUserClaim(this.userId, deleteClaim).then(() => {
              this.$message.success(this.l('global.successful'))
              const claimIndex = this.userClaims.findIndex(uc => uc.id === claim.id)
              this.userClaims.splice(claimIndex, 1)
            })
          }
        }
      })
  }

  // TODO: 可以简化为一个组件,通过组件来实现用户/客户端等的声明类型
  private onClaimTypeChanged() {
    const valueType = this.cliamType(this.editUserClaim.claimType)
    switch (valueType) {
      case IdentityClaimValueType.Int :
        this.editUserClaim.claimValue = '0'
        break
      case IdentityClaimValueType.String :
        this.editUserClaim.claimValue = ''
        break
      case IdentityClaimValueType.Boolean :
        this.editUserClaim.claimValue = 'false'
        break
      case IdentityClaimValueType.DateTime :
        this.editUserClaim.claimValue = ''
        break
    }
  }

  private onSave() {
    const userClaimForm = this.$refs.userClaimForm as Form
    userClaimForm.validate(valid => {
      if (valid) {
        UserService.addUserClaim(this.userId, this.editUserClaim).then(() => {
          this.$message.success(this.l('global.successful'))
          userClaimForm.resetFields()
          this.handleGetUserClaims()
        })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    const userClaimForm = this.$refs.userClaimForm as Form
    userClaimForm.resetFields()
    this.$emit('closed', changed)
  }
}
</script>
