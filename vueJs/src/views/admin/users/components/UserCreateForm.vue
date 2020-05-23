<template>
  <el-form
    ref="formCreateUser"
    label-width="110px"
    :model="createUser"
    :rules="createUserRules"
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
            v-model="createUser.userName"
            :placeholder="$t('userProfile.pleaseInputUserName')"
          />
        </el-form-item>
        <el-form-item
          prop="name"
          :label="$t('users.name')"
        >
          <el-input
            v-model="createUser.name"
            :placeholder="$t('userProfile.pleaseInputName')"
          />
        </el-form-item>
        <el-form-item
          prop="surname"
          :label="$t('users.surname')"
        >
          <el-input
            v-model="createUser.surname"
            :placeholder="$t('userProfile.pleaseInputSurname')"
          />
        </el-form-item>
        <el-form-item
          prop="phoneNumber"
          :label="$t('users.phoneNumber')"
        >
          <el-input
            v-model="createUser.phoneNumber"
            :placeholder="$t('userProfile.pleaseInputPhoneNumber')"
          />
        </el-form-item>
        <el-form-item
          prop="email"
          :label="$t('users.email')"
        >
          <el-input
            v-model="createUser.email"
            :placeholder="$t('userProfile.pleaseInputEmail')"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('userProfile.security')"
        name="security"
      >
        <el-form-item
          prop="password"
          label-width="100px"
          :label="$t('userProfile.password')"
        >
          <el-input
            v-model="createUser.password"
            type="password"
            :placeholder="$t('userProfile.pleaseInputPassword')"
          />
        </el-form-item>
        <el-form-item
          prop="twoFactorEnabled"
          label-width="100px"
          :label="$t('users.twoFactorEnabled')"
        >
          <el-switch v-model="createUser.twoFactorEnabled" />
        </el-form-item>
        <el-form-item
          prop="lockoutEnabled"
          label-width="100px"
          :label="$t('users.lockoutEnabled')"
        >
          <el-switch v-model="createUser.lockoutEnabled" />
        </el-form-item>
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
        @click="onSubmit"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import UserApiService, { UserCreateDto } from '@/api/users'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'UserProfile',
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  private createUser: UserCreateDto

  private activedTabPane: string

  constructor() {
    super()
    this.activedTabPane = 'basic'
    this.createUser = new UserCreateDto()
  }

  private validatePhoneNumberValue = (rule: any, value: string, callback: any) => {
    const phoneReg = /^1[34578]\d{9}$/
    if (!value || !phoneReg.test(value)) {
      callback(new Error(this.l('global.pleaseInputBy', { key: this.l('global.correctPhoneNumber') })))
    } else {
      callback()
    }
  }

  private createUserRules = {
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

  private onSubmit() {
    const frmCreateUser = this.$refs.formCreateUser as any
    frmCreateUser.validate(async(valid: boolean) => {
      if (valid) {
        UserApiService.createUser(this.createUser).then(user => {
          this.$message.success(this.l('users.createUserSuccess', { name: user.name }))
          this.$emit('onUserProfileChanged', user.id)
          this.resetForm()
          this.onCancel()
        })
      }
    })
  }

  private onCancel() {
    this.resetForm()
    this.$emit('onClose')
  }

  private resetForm() {
    this.activedTabPane = 'basic'
    this.createUser = new UserCreateDto()
    const frmCreateUser = this.$refs.formCreateUser as any
    frmCreateUser.resetFields()
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
