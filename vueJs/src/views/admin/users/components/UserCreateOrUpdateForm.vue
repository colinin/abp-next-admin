<template>
  <el-dialog
    :visible="showDialog"
    :title="title"
    width="800px"
    custom-class="modal-form"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    :show-close="false"
    @close="onFormClosed"
  >
    <el-form
      ref="editUserForm"
      label-width="110px"
      :model="currentUser"
      :rules="currentUserRules"
    >
      <el-tabs v-model="activedTabPane">
        <el-tab-pane
          :label="$t('AbpIdentity.UserInformations')"
          name="information"
        >
          <el-form-item
            prop="userName"
            :label="$t('AbpIdentity.DisplayName:UserName')"
          >
            <el-input
              v-model="currentUser.userName"
              :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:UserName')})"
            />
          </el-form-item>
          <el-form-item
            prop="name"
            :label="$t('AbpIdentity.DisplayName:Name')"
          >
            <el-input
              v-model="currentUser.name"
              :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:Name')})"
            />
          </el-form-item>
          <el-form-item
            prop="surname"
            :label="$t('AbpIdentity.DisplayName:Surname')"
          >
            <el-input
              v-model="currentUser.surname"
              :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:Surname')})"
            />
          </el-form-item>
          <el-form-item
            prop="phoneNumber"
            :label="$t('AbpIdentity.DisplayName:PhoneNumber')"
          >
            <el-input
              v-model="currentUser.phoneNumber"
              :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:PhoneNumber')})"
            />
          </el-form-item>
          <el-form-item
            prop="email"
            :label="$t('AbpIdentity.DisplayName:Email')"
          >
            <el-input
              v-model="currentUser.email"
              :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:Email')})"
            />
          </el-form-item>
          <el-form-item
            v-if="!isEditUser"
            prop="password"
            :label="$t('AbpIdentity.DisplayName:Password')"
          >
            <el-input
              v-model="currentUser.password"
              type="password"
              :placeholder="$t('global.pleaseInputBy', {key: $t('AbpIdentity.DisplayName:Password')})"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item
                prop="twoFactorEnabled"
                label-width="120px"
                :label="$t('AbpIdentity.DisplayName:TwoFactorEnabled')"
              >
                <el-switch v-model="currentUser.twoFactorEnabled" />
              </el-form-item>
            </el-col>
            <el-col :span="16">
              <el-form-item
                prop="lockoutEnabled"
                label-width="180px"
                :label="$t('AbpIdentity.LockoutEnabled')"
              >
                <el-switch v-model="currentUser.lockoutEnabled" />
              </el-form-item>
            </el-col>
          </el-row>
        </el-tab-pane>
        <el-tab-pane
          :label="$t('AbpIdentity.Roles')"
          name="roles"
        >
          <el-transfer
            v-model="currentUserRoles"
            :titles="[$t('AbpIdentity.RoleList'),$t('AbpIdentity.HasRoles')]"
            :data="roleList"
          />
        </el-tab-pane>
      </el-tabs>
      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed"
        >
          {{ $t('AbpIdentity.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
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
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import { checkPermission } from '@/utils/permission'
import { AbpModule } from '@/store/modules/abp'
import EventBusMiXin from '@/mixins/EventBusMiXin'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

import RoleService from '@/api/roles'
import { IRoleData } from '@/api/types'
import UserApiService, { UserCreate, UserUpdate, User, UserCreateOrUpdate } from '@/api/users'

@Component({
  name: 'UserCreateOrUpdateForm',
  methods: {
    checkPermission
  }
})
export default class extends Mixins(EventBusMiXin, LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private editUserId!: string

  private title = ''
  private activedTabPane = 'information'
  private editUser = new User()
  private currentUser = new UserCreateOrUpdate()
  private currentUserRoles = new Array<string>()
  private roleList = new Array<{key: string, label: string, disabled: boolean}>()

  private currentUserRoleChanged = false

  private currentUserRules = {
    userName: [
      { required: true, message: this.l('global.pleaseInputBy', { key: this.l('AbpIdentity.DisplayName:UserName') }), trigger: 'blur' }
    ],
    email: [
      { required: true, message: this.l('global.pleaseInputBy', { key: this.l('AbpIdentity.DisplayName:Email') }), trigger: 'blur' },
      { type: 'email', message: this.l('AbpValidation.ThisFieldIsNotAValidEmailAddress'), trigger: ['blur', 'change'] }
    ],
    password: [
      { required: true, message: this.l('global.pleaseInputBy', { key: this.l('AbpIdentity.DisplayName:Password') }), trigger: 'blur' },
      { min: this.requiredPasswordLength, message: this.l('AbpValidation.ThisFieldMustBeAStringWithAMinimumLengthOf', { 0: this.requiredPasswordLength }), trigger: 'blur' }
    ]
  }

  get requiredPasswordLength() {
    if (AbpModule.configuration) {
      const setting = AbpModule.configuration.setting.values['Abp.Identity.Password.RequiredLength']
      if (setting) {
        return Number(setting)
      }
    }
    return 3
  }

  get isEditUser() {
    if (this.editUserId) {
      return true
    }
    return false
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetUser()
    if (this.editUserId) {
      this.title = this.l('AbpIdentity.Edit')
    } else {
      this.title = this.l('AbpIdentity.NewUser')
    }
  }

  @Watch('currentUserRoles')
  onUserRolesChanged(val: string[], oldVal: string[]) {
    if (val.length !== oldVal.length) {
      this.currentUserRoleChanged = true
    }
  }

  mounted() {
    this.handleGetRoles()
  }

  private handleGetRoles() {
    RoleService.getAllRoles().then(data => {
      const roles = data.items.map((item: IRoleData) => {
        return {
          key: item.name,
          label: item.name,
          disabled: !item.isPublic
        }
      })
      this.roleList = roles
    })
  }

  private handleGetUser() {
    this.activedTabPane = 'information'
    this.editUser = new User()
    this.currentUser = new UserCreateOrUpdate()
    this.currentUserRoles = new Array<string>()
    if (this.showDialog && this.editUserId) {
      UserApiService.getUserById(this.editUserId).then(user => {
        this.editUser = user
        this.currentUser.name = user.name
        this.currentUser.userName = user.userName
        this.currentUser.surname = user.surname
        this.currentUser.email = user.email
        this.currentUser.phoneNumber = user.phoneNumber
        this.currentUser.lockoutEnabled = user.lockoutEnabled
      })
      UserApiService.getUserRoles(this.editUserId).then(roles => {
        this.currentUserRoles = roles.items.map(role => role.name)
      })
    }
  }

  private onSave() {
    const editUserForm = this.$refs.editUserForm as any
    editUserForm.validate(async(valid: boolean) => {
      if (valid) {
        if (this.isEditUser) {
          const updateUser = new UserUpdate()
          this.updateUserByInput(updateUser)
          updateUser.concurrencyStamp = this.editUser.concurrencyStamp
          updateUser.password = null
          UserApiService.updateUser(this.editUserId, updateUser)
            .then(user => {
              this.trigger('userChanged', user)
              this.onFormClosed()
            })
        } else {
          const createUser = new UserCreate()
          this.updateUserByInput(createUser)
          createUser.password = this.currentUser.password
          UserApiService.createUser(createUser)
            .then(user => {
              this.trigger('userChanged', user)
              this.onFormClosed()
            })
        }
      }
    })
  }

  private updateUserByInput(user: UserCreateOrUpdate) {
    user.name = this.currentUser.name
    user.userName = this.currentUser.userName
    user.surname = this.currentUser.surname
    user.email = this.currentUser.email
    user.phoneNumber = this.currentUser.phoneNumber
    user.lockoutEnabled = this.currentUser.lockoutEnabled
    if (this.currentUserRoleChanged) {
      user.roleNames = this.currentUserRoles
    } else {
      user.roleNames = null
    }
  }

  private onFormClosed() {
    this.resetForm()
    this.$emit('closed')
  }

  private resetForm() {
    this.activedTabPane = 'infomation'
    const editUserForm = this.$refs.editUserForm as any
    editUserForm.resetFields()
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  position: absolute;
  right: 10px;
  width: 100px
}
.cancel {
  position: absolute;
  right: 120px;
  width: 100px
}
</style>
