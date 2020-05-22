<template>
  <el-form
    ref="profile"
    label-width="110px"
    :model="userProfile"
    :rules="userProfileRules"
  >
    <el-tabs v-model="activedTabPane">
      <el-tab-pane
        :label="$t('userProfile.basic')"
        name="basic"
      >
        <el-form-item
          prop="userName"
          :label="$t('users.userName')"
        >
          <el-input
            v-model="userProfile.userName"
            :placeholder="$t('userProfile.pleaseInputUserName')"
          />
        </el-form-item>
        <el-form-item
          prop="name"
          :label="$t('users.name')"
        >
          <el-input
            v-model="userProfile.name"
            :placeholder="$t('userProfile.pleaseInputName')"
          />
        </el-form-item>
        <el-form-item
          prop="surname"
          :label="$t('users.surname')"
        >
          <el-input
            v-model="userProfile.surname"
            :placeholder="$t('userProfile.pleaseInputSurname')"
          />
        </el-form-item>
        <el-form-item
          prop="phoneNumber"
          :label="$t('users.phoneNumber')"
        >
          <el-input
            v-model="userProfile.phoneNumber"
            :placeholder="$t('userProfile.pleaseInputPhoneNumber')"
          />
        </el-form-item>
        <el-form-item
          prop="email"
          :label="$t('users.email')"
        >
          <el-input
            v-model="userProfile.email"
            :placeholder="$t('userProfile.pleaseInputEmail')"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('userProfile.security')"
        name="security"
      >
        <el-form-item
          v-if="!hasEditUser"
          prop="password"
          label-width="100px"
          :label="$t('userProfile.password')"
        >
          <el-input
            v-model="userPassword"
            type="password"
            :placeholder="$t('userProfile.pleaseInputPassword')"
          />
        </el-form-item>
        <el-form-item
          prop="twoFactorEnabled"
          label-width="100px"
          :label="$t('users.twoFactorEnabled')"
        >
          <el-switch v-model="userProfile.twoFactorEnabled" />
        </el-form-item>
        <el-form-item
          prop="lockoutEnabled"
          label-width="100px"
          :label="$t('users.lockoutEnabled')"
        >
          <el-switch v-model="userProfile.lockoutEnabled" />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('userProfile.roles')"
        name="roles"
      >
        <el-transfer
          v-model="userRoles"
          :titles="[$t('userProfile.roleList'),$t('userProfile.hasRoles')]"
          :data="roleList"
        />
      </el-tab-pane>
      <el-tab-pane
        v-if="allowedChangePermissions() && hasLoadPermission"
        :label="$t('userProfile.permission')"
      >
        <PermissionTree
          ref="permissionTree"
          :expanded="false"
          :readonly="!checkPermission(['AbpIdentity.Users.ManagePermissions'])"
          :permission="userPermission"
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
        {{ $t('table.cancel') }}
      </el-button>
      <el-button
        class="confirm"
        type="primary"
        style="width:100px"
        @click="onSubmit('profile')"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import UserApiService, {
  UserDataDto,
  UserUpdateDto,
  UserCreateDto
} from '@/api/users'
import RoleService from '@/api/roles'
import PermissionService, { PermissionDto, UpdatePermissionsDto } from '@/api/permission'
import PermissionTree from '@/components/PermissionTree/index.vue'
import { IRoleData, IPermission } from '@/api/types'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'UserProfile',
  components: {
    PermissionTree
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: '' }) private userId!: string
  private roleList: {key: string, label: string, disabled: boolean}[]
  /** 用户组 */
  private userRoles: string[]
  private userPassword: string
  private hasEditUser: boolean
  private userRolesChanged: boolean
  private userProfile: UserDataDto
  /** 是否加载用户权限 */
  private hasLoadPermission: boolean
  /** 用户权限数据 */
  private userPermission: PermissionDto
  /** 用户权限已变更 */
  private userPermissionChanged: boolean
  /** 变更用户权限数据 */
  private editUserPermissions: IPermission[]

  private activedTabPane: string

  constructor() {
    super()
    this.activedTabPane = 'basic'
    this.userPassword = ''
    this.hasEditUser = false
    this.userRolesChanged = false
    this.hasLoadPermission = false
    this.userPermissionChanged = false
    this.userRoles = new Array<string>()
    this.userProfile = new UserDataDto()
    this.userPermission = new PermissionDto()
    this.editUserPermissions = new Array<IPermission>()
    this.roleList = new Array<{key: string, label: string, disabled: boolean}>()
  }

  @Watch('userId', { immediate: true })
  onUserIdChanged(userId: string) {
    if (userId) {
      this.handleGetUserProfile()
    }
  }

  onUserRolesChanged(val: string[], oldVal: string[]) {
    if (val.length !== oldVal.length) {
      this.userRolesChanged = true
    }
  }

  private validatePhoneNumberValue = (rule: any, value: string, callback: any) => {
    const phoneReg = /^1[34578]\d{9}$/
    if (!value || !phoneReg.test(value)) {
      callback(new Error(this.l('global.pleaseInputBy', { key: this.l('global.correctPhoneNumber') })))
    } else {
      callback()
    }
  }

  private userProfileRules = {
    userName: [
      { required: true, message: this.l('userProfile.pleaseInputUserName'), trigger: 'blur' },
      { min: 3, max: 20, message: this.l('global.charLengthRange', { min: 3, max: 20 }), trigger: 'blur' }
    ],
    name: [
      { required: true, message: this.l('userProfile.pleaseInputName'), trigger: 'blur' },
      { min: 3, max: 50, message: this.l('global.charLengthRange', { min: 3, max: 50 }), trigger: 'blur' }
    ],
    email: [
      { required: true, message: this.l('userProfile.pleaseInputEmail'), trigger: 'blur' },
      { type: 'email', message: this.l('global.pleaseInputBy', { key: this.l('global.correctEmailAddress') }), trigger: ['blur', 'change'] }
    ],
    password: [
      { required: true, message: this.l('userProfile.pleaseInputPassword'), trigger: 'blur' },
      { min: 6, max: 15, message: this.l('global.charLengthRange', { min: 6, max: 15 }), trigger: 'blur' }
    ],
    phoneNumber: [
      { required: true, validator: this.validatePhoneNumberValue, trigger: 'blur' }
    ]
  }

  mounted() {
    this.handleGetRoles()
    if (this.userId) {
      this.handleGetUserProfile()
    }
  }

  /** 允许变更权限 */
  private allowedChangePermissions() {
    return checkPermission(['AbpIdentity.Users.ManagePermissions'])
  }

  private handleGetUserProfile() {
    this.userRoles = new Array<string>()
    this.userPermission = new PermissionDto()
    UserApiService.getUserById(this.userId).then(user => {
      this.userProfile = user
      this.handleGetUserRoles(this.userId)
      if (this.allowedChangePermissions()) {
        this.handleGetUserPermissions(this.userId)
      }
      this.hasEditUser = true
    })
  }

  private handleGetRoles() {
    RoleService.getRoles().then(data => {
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

  private async handleGetUserRoles(userId: string) {
    const userRoleDto = await UserApiService.getUserRoles(userId)
    this.userRoles = userRoleDto.items.map(r => r.name)
    // 监听用户组变化
    this.$watch('userRoles', this.onUserRolesChanged)
    // const data = await getSettings('U', UserModule.id)
    // console.log(data)
  }

  private async handleGetUserPermissions(id: string) {
    PermissionService.getPermissionsByKey('U', id).then(permission => {
      this.userPermission = permission
      this.hasLoadPermission = true
    })
  }

  private onPermissionChanged(permissions: IPermission[]) {
    this.userPermissionChanged = true
    this.editUserPermissions = permissions
  }

  private onSubmit(formName: string) {
    const userProfileForm = this.$refs[formName] as any
    userProfileForm.validate(async(valid: boolean) => {
      if (valid) {
        if (!this.hasEditUser) {
          const createUserInput = this.createAddUserDto()
          this.userProfile = await UserApiService.createUser(createUserInput)
          this.$message.success(this.$t('users.createUserSuccess', { name: this.userProfile.name }).toString())
        } else {
          const updateUserInput = this.createEditUserDto()
          const user = await UserApiService.updateUser(this.userProfile.id, updateUserInput)
          this.userProfile = user
          this.$message.success(this.$t('users.updateUserSuccess', { name: this.userProfile.name }).toString())
        }
        if (this.userRolesChanged) {
          await UserApiService.setUserRoles(this.userProfile.id, this.userRoles)
        }
        if (this.userPermissionChanged) {
          const setUserPermissions = new UpdatePermissionsDto()
          setUserPermissions.permissions = this.editUserPermissions
          await PermissionService.setPermissionsByKey('U', this.userProfile.id, setUserPermissions)
        }
        userProfileForm.resetFields()
        this.$emit('onUserProfileChanged', this.userProfile.id)
        this.onCancel()
      } else {
        return false
      }
    })
  }

  private onCancel() {
    this.resetForm()
    this.$emit('onClose')
  }

  private resetForm() {
    this.activedTabPane = 'basic'
    this.userRoles = new Array<string>()
    const userProfileForm = this.$refs.profile as any
    userProfileForm.resetFields()
    if (this.hasLoadPermission) {
      const userPermission = this.$refs.permissionTree as PermissionTree
      userPermission.resetPermissions()
      this.hasLoadPermission = false
    }
  }

  private createAddUserDto() {
    const createUserInput = new UserCreateDto()
    createUserInput.name = this.userProfile.name
    createUserInput.userName = this.userProfile.userName
    createUserInput.password = this.userPassword
    createUserInput.surname = this.userProfile.surname
    createUserInput.email = this.userProfile.email
    createUserInput.phoneNumber = this.userProfile.phoneNumber
    createUserInput.twoFactorEnabled = this.userProfile.twoFactorEnabled
    createUserInput.lockoutEnabled = this.userProfile.lockoutEnabled
    createUserInput.roleNames = this.userRoles
    return createUserInput
  }

  private createEditUserDto() {
    const updateUserInput = new UserUpdateDto()
    updateUserInput.name = this.userProfile.name
    updateUserInput.userName = this.userProfile.userName
    updateUserInput.surname = this.userProfile.surname
    updateUserInput.email = this.userProfile.email
    updateUserInput.phoneNumber = this.userProfile.phoneNumber
    updateUserInput.twoFactorEnabled = this.userProfile.twoFactorEnabled
    updateUserInput.lockoutEnabled = this.userProfile.lockoutEnabled
    updateUserInput.roles = this.userRoles
    updateUserInput.concurrencyStamp = this.userProfile.concurrencyStamp
    return updateUserInput
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
