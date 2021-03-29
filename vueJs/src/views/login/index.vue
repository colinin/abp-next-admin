<template>
  <div class="login-container">
    <el-form
      ref="formLogin"
      :model="loginForm"
      :rules="loginFormRules"
      label-position="left"
      label-width="0px"
      class="demo-ruleForm login-page"
    >
      <div class="title-container">
        <h3 class="title">
          {{ $t('login.title') }}
        </h3>
        <lang-select class="set-language" />
      </div>
      <el-form-item label-width="0px">
        <tenant-box
          v-if="isMultiEnabled"
          v-model="loginForm.tenantName"
        />
      </el-form-item>
      <el-tabs
        stretch
        @tab-click="handleLoginTabChanged"
      >
        <el-tab-pane :label="$t('login.userLogin')">
          <div v-if="loginType === 'password'">
            <el-form-item
              prop="username"
            >
              <el-input
                v-model="loginForm.username"
                prefix-icon="el-icon-user"
                type="text"
                auto-complete="off"
                tabindex="1"
                :placeholder="$t('global.pleaseInputBy', {key: $t('AbpAccount.UserNameOrEmailAddress')})"
              />
            </el-form-item>
            <el-form-item
              prop="password"
            >
              <el-input
                :key="passwordType"
                ref="password"
                v-model="loginForm.password"
                prefix-icon="el-icon-lock"
                :type="passwordType"
                :placeholder="$t('global.pleaseInputBy', {key: $t('AbpAccount.Password')})"
                name="password"
                tabindex="2"
                @keyup.enter.native="handleUserLogin"
              />
              <span
                class="show-pwd"
                @click="showPwd"
              >
                <svg-icon :name="passwordType === 'password' ? 'eye-off' : 'eye-on'" />
              </span>
            </el-form-item>
          </div>
        </el-tab-pane>
        <el-tab-pane :label="$t('login.phoneLogin')">
          <div v-if="loginType === 'phone'">
            <el-form-item
              prop="phoneNumber"
            >
              <el-input
                ref="loginItemPhone"
                v-model="loginForm.phoneNumber"
                prefix-icon="el-icon-mobile-phone"
                type="text"
                maxlength="11"
                auto-complete="off"
                :placeholder="$t('global.pleaseInputBy', {key: $t('login.phoneNumber')})"
              />
            </el-form-item>
            <el-form-item
              prop="verifyCode"
            >
              <el-row>
                <el-col :span="16">
                  <el-input
                    v-model="loginForm.verifyCode"
                    auto-complete="off"
                    :placeholder="$t('global.pleaseInputBy', {key: $t('login.phoneVerifyCode')})"
                    prefix-icon="el-icon-key"
                    style="margin:-right: 10px;"
                  />
                </el-col>
                <el-col :span="8">
                  <el-button
                    ref="sendButton"
                    style="margin-left: 10px;width: 132px;"
                    :disabled="sending"
                    @click="handleSendPhoneVerifyCode"
                  >
                    {{ sendButtonName }}
                  </el-button>
                </el-col>
              </el-row>
            </el-form-item>
          </div>
        </el-tab-pane>
      </el-tabs>

      <el-row>
        <el-col :span="12">
          <el-form-item
            v-show="selfRegistration"
            label-width="100px"
            :label="$t('AbpAccount.AreYouANewUser')"
          >
            <el-link
              type="success"
              @click="handleRedirectRegister"
            >
              {{ $t('AbpAccount.Register') }}
            </el-link>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item
            label-width="100px"
            :label="$t('AbpAccount.ForgotPassword')"
          >
            <el-link
              type="info"
              @click="handleRedirectResetPassword"
            >
              {{ $t('AbpAccount.ResetPassword') }}
            </el-link>
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item style="width:100%;">
        <el-button
          type="primary"
          style="width:100%;"
          :loading="logining"
          @click="handleUserLogin"
        >
          {{ $t('AbpAccount.Login') }}
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Input } from 'element-ui'
import { Route } from 'vue-router'
import { UserModule } from '@/store/modules/user'
import { Dictionary } from 'vue-router/types/router'
import TenantBox from '@/components/TenantBox/index.vue'
import LangSelect from '@/components/LangSelect/index.vue'
import { Component, Mixins, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import UserService from '@/api/users'
import { AbpModule } from '@/store/modules/abp'

@Component({
  name: 'Login',
  components: {
    LangSelect,
    TenantBox
  }
})
export default class extends Mixins(LocalizationMiXin) {
  private loginType = 'password'
  private passwordType = 'password'
  private redirect?: string

  private sendTimer: any
  private sending = false
  private sendButtonName = this.l('login.sendVerifyCode')
  private logining = false
  private loginForm = {
    tenantName: AbpModule.configuration?.currentTenant?.name,
    username: '',
    password: '',
    phoneNumber: '',
    verifyCode: ''
  }

  get isMultiEnabled() {
    return AbpModule.configuration?.multiTenancy?.isEnabled
  }

  get selfRegistration() {
    const settingValues = AbpModule.configuration?.setting?.values
    if (settingValues && settingValues['Abp.Account.IsSelfRegistrationEnabled'] === true) {
      return true
    }
    return false
  }

  private validatePhoneNumberValue = (rule: any, value: string, callback: any) => {
    const phoneReg = /^1[34578]\d{9}$/
    if (!value || !phoneReg.test(value)) {
      callback(new Error(this.l('global.pleaseInputBy', { key: this.l('global.correctPhoneNumber') })))
    } else {
      callback()
    }
  }

  private loginFormRules = {
    username: [
      {
        required: true, message: this.l('global.pleaseInputBy', { key: this.l('AbpAccount.UserNameOrEmailAddress') }), trigger: 'blur'
      }
    ],
    password: [
      {
        required: true, message: this.l('global.pleaseInputBy', { key: this.l('AbpAccount.Password') }), trigger: 'blur'
      }
    ],
    phoneNumber: [
      {
        required: true, validator: this.validatePhoneNumberValue, trigger: 'blur'
      }
    ],
    verifyCode: [
      {
        required: true, message: this.l('global.pleaseInputBy', { key: this.l('login.phoneVerifyCode') }), trigger: 'blur'
      }
    ]
  }

  destroyed() {
    if (this.sendTimer) {
      clearInterval(this.sendTimer)
    }
  }

  @Watch('$route', { immediate: true })
  private onRouteChange(route: Route) {
    // TODO: remove the "as Dictionary<string>" hack after v4 release for vue-router
    // See https://github.com/vuejs/vue-router/pull/2050 for details
    const query = route.query as Dictionary<string>
    if (query) {
      this.redirect = query.redirect
    }
  }

  private showPwd() {
    if (this.passwordType === 'password') {
      this.passwordType = ''
    } else {
      this.passwordType = 'password'
    }
    this.$nextTick(() => {
      (this.$refs.password as Input).focus()
    })
  }

  private handleRedirectRegister() {
    this.$router.replace('register')
  }

  private handleRedirectResetPassword() {
    this.$router.replace('reset-password')
  }

  private handleUserLogin() {
    const frmLogin = this.$refs.formLogin as any
    frmLogin.validate((valid: boolean) => {
      if (valid) {
        this.logining = true
        if (this.loginType === 'password') {
          const userLogin = {
            username: this.loginForm.username,
            password: this.loginForm.password
          }
          UserModule.Login(userLogin)
            .then(() => {
              this.$router.push({
                path: this.redirect || '/'
              })
            })
            .finally(() => {
              this.resetLoginButton()
            })
        } else {
          const phoneLogin = {
            phoneNumber: this.loginForm.phoneNumber,
            verifyCode: this.loginForm.verifyCode
          }
          UserModule.PhoneLogin(phoneLogin)
            .then(() => {
              console.log(this.$router)
              this.$router.push({
                path: this.redirect || '/'
              })
            })
            .finally(() => {
              this.resetLoginButton()
            })
        }
      }
    })
  }

  private handleSendPhoneVerifyCode() {
    const frmLogin = this.$refs.formLogin as any
    frmLogin.validateField('phoneNumber', (errorMsg: string) => {
      if (!errorMsg) {
        this.sending = true
        UserService.sendSmsSigninCode(this.loginForm.phoneNumber).then(() => {
          let interValTime = 60
          const sendingName = this.l('login.afterSendVerifyCode')
          const sendedName = this.l('login.sendVerifyCode')
          this.sendTimer = setInterval(() => {
            this.sendButtonName = interValTime + sendingName
            --interValTime
            if (interValTime < 0) {
              this.sendButtonName = sendedName
              this.sending = false
              clearInterval(this.sendTimer)
            }
          }, 1000)
        }).catch(() => {
          this.sending = false
        })
      }
    })
  }

  private handleLoginTabChanged(tab: any) {
    this.loginType = tab.paneName === '1' ? 'phone' : 'password'
  }

  private resetLoginButton() {
    setTimeout(() => {
      this.logining = false
    }, 0.5 * 1000)
  }
}
</script>

<style lang="scss" scoped>
.login-container {
    width: 100%;
    height: 100%;
    overflow-x: hidden;
    overflow-y: auto;
    background-color: $loginBg;

    .svg-container {
      padding: 6px 5px 6px 15px;
      color: $darkGray;
      vertical-align: middle;
      width: 30px;
      display: inline-block;
    }

    .title-container {
      position: relative;

    .title {
      font-size: 26px;
      margin: 0px auto 20px auto;
      text-align: center;
      font-weight: bold;
    }

    .tips {
      font-size: 14px;
      color: #fff;
      margin-bottom: 10px;

      span {
        &:first-of-type {
          margin-right: 16px;
        }
      }
    }

    .set-language {
      position: absolute;
      top: 3px;
      font-size: 18px;
      right: 0px;
      cursor: pointer;
    }
  }

  .show-pwd {
    position: absolute;
    right: 10px;
    font-size: 16px;
    color: $darkGray;
    cursor: pointer;
    user-select: none;
  }
}

.login-page {
    -webkit-border-radius: 5px;
    border-radius: 5px;
    margin: 130px auto;
    width: 500px;
    padding: 35px 35px 15px;
    border: 1px solid #8c9494;
    box-shadow: 0 0 25px #454646;
    background-color:rgb(247, 255, 255);

    .loginTab.el-tabs__item {
      width: 180px;
    }
}

label.el-checkbox.rememberme {
    margin: 0px 0px 15px;
    text-align: left;
}
</style>
