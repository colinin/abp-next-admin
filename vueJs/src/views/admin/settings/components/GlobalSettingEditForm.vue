<template>
  <div>
    <el-form
      ref="formGlobalSetting"
      v-model="globalSetting"
      label-width="180px"
      style="width: 96%"
    >
      <el-tabs>
        <el-tab-pane :label="$t('settings.systemSetting')">
          <el-form-item
            v-if="hasSettingExistsed('Abp.Localization.DefaultLanguage')"
            v-popover:DefaultLanguage
            prop="globalSetting['Abp.Localization.DefaultLanguage'].value"
          >
            <el-popover
              ref="DefaultLanguage"
              trigger="hover"
              :title="globalSetting['Abp.Localization.DefaultLanguage'].displayName"
              :content="globalSetting['Abp.Localization.DefaultLanguage'].description"
            />
            <span
              slot="label"
              v-popover:DefaultLanguage
            >{{ globalSetting['Abp.Localization.DefaultLanguage'].displayName }}</span>
            <el-select
              v-model="globalSetting['Abp.Localization.DefaultLanguage'].value"
              style="width: 100%;"
              @change="(value) => handleSettingValueChanged('Abp.Localization.DefaultLanguage', value)"
            >
              <el-option
                v-for="language in definedLanguages"
                :key="language"
                :label="language"
                :value="language"
                :disabled="language===globalSetting['Abp.Localization.DefaultLanguage'].value"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Timing.TimeZone')"
            prop="Abp.Timing.TimeZone'].value"
          >
            <el-popover
              ref="TimeZone"
              trigger="hover"
              :title="globalSetting['Abp.Timing.TimeZone'].displayName"
              :content="globalSetting['Abp.Timing.TimeZone'].description"
            />
            <span
              slot="label"
              v-popover:TimeZone
            >{{ globalSetting['Abp.Timing.TimeZone'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Timing.TimeZone'].value"
              :placeholder="globalSetting['Abp.Timing.TimeZone'].description"
              @input="(value) => handleSettingValueChanged('Abp.Timing.TimeZone', value)"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          :label="$t('settings.passwordSecurity')"
        >
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequiredLength')"
          >
            <el-popover
              ref="PasswordRequiredLength"
              trigger="hover"
              :title="globalSetting['Abp.Identity.Password.RequiredLength'].displayName"
              :content="globalSetting['Abp.Identity.Password.RequiredLength'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequiredLength
            >{{ globalSetting['Abp.Identity.Password.RequiredLength'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Identity.Password.RequiredLength'].value"
              :placeholder="globalSetting['Abp.Identity.Password.RequiredLength'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequiredLength', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequiredUniqueChars')"
          >
            <el-popover
              ref="PasswordRequiredUniqueChars"
              trigger="hover"
              :title="globalSetting['Abp.Identity.Password.RequiredUniqueChars'].displayName"
              :content="globalSetting['Abp.Identity.Password.RequiredUniqueChars'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequiredUniqueChars
            >{{ globalSetting['Abp.Identity.Password.RequiredUniqueChars'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Identity.Password.RequiredUniqueChars'].value"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequiredUniqueChars', value)"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.Password.RequireNonAlphanumeric')"
              >
                <el-popover
                  ref="PasswordRequireNonAlphanumeric"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.Password.RequireNonAlphanumeric'].displayName"
                  :content="globalSetting['Abp.Identity.Password.RequireNonAlphanumeric'].description"
                />
                <span
                  slot="label"
                  v-popover:PasswordRequireNonAlphanumeric
                >{{ globalSetting['Abp.Identity.Password.RequireNonAlphanumeric'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireNonAlphanumeric'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireNonAlphanumeric', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.Password.RequireLowercase')"
              >
                <el-popover
                  ref="PasswordRequireLowercase"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.Password.RequireLowercase'].displayName"
                  :content="globalSetting['Abp.Identity.Password.RequireLowercase'].description"
                />
                <span
                  slot="label"
                  v-popover:PasswordRequireLowercase
                >{{ globalSetting['Abp.Identity.Password.RequireLowercase'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireLowercase'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireLowercase', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.Password.RequireUppercase')"
              >
                <el-popover
                  ref="PasswordRequireUppercase"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.Password.RequireUppercase'].displayName"
                  :content="globalSetting['Abp.Identity.Password.RequireUppercase'].description"
                />
                <span
                  slot="label"
                  v-popover:PasswordRequireUppercase
                >{{ globalSetting['Abp.Identity.Password.RequireUppercase'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireUppercase'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireUppercase', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.Password.RequireDigit')"
              >
                <el-popover
                  ref="PasswordRequireDigit"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.Password.RequireDigit'].displayName"
                  :content="globalSetting['Abp.Identity.Password.RequireDigit'].description"
                />
                <span
                  slot="label"
                  v-popover:PasswordRequireDigit
                >{{ globalSetting['Abp.Identity.Password.RequireDigit'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireDigit'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireDigit', value)"
                />
              </el-form-item>
            </el-col>
            <el-form-item
              v-if="hasSettingExistsed('Abp.Identity.Lockout.AllowedForNewUsers')"
            >
              <el-popover
                ref="LockoutAllowedForNewUsers"
                trigger="hover"
                :title="globalSetting['Abp.Identity.Lockout.AllowedForNewUsers'].displayName"
                :content="globalSetting['Abp.Identity.Lockout.AllowedForNewUsers'].description"
              />
              <span
                slot="label"
                v-popover:LockoutAllowedForNewUsers
              >{{ globalSetting['Abp.Identity.Lockout.AllowedForNewUsers'].displayName }}</span>
              <el-switch
                v-model="globalSetting['Abp.Identity.Lockout.AllowedForNewUsers'].value"
                @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.AllowedForNewUsers', value)"
              />
            </el-form-item>
            <el-form-item
              v-if="hasSettingExistsed('Abp.Identity.Lockout.MaxFailedAccessAttempts')"
            >
              <el-popover
                ref="LockoutMaxFailedAccessAttempts"
                trigger="hover"
                :title="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].displayName"
                :content="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].description"
              />
              <span
                slot="label"
                v-popover:LockoutMaxFailedAccessAttempts
              >{{ globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].displayName }}</span>
              <el-input
                v-model="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].value"
                :placeholder="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].description"
                type="number"
                @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.MaxFailedAccessAttempts', value)"
              />
            </el-form-item>
            <el-form-item
              v-if="hasSettingExistsed('Abp.Identity.Lockout.LockoutDuration')"
            >
              <el-popover
                ref="LockoutLockoutDuration"
                trigger="hover"
                :title="globalSetting['Abp.Identity.Lockout.LockoutDuration'].displayName"
                :content="globalSetting['Abp.Identity.Lockout.LockoutDuration'].description"
              />
              <span
                slot="label"
                v-popover:LockoutLockoutDuration
              >{{ globalSetting['Abp.Identity.Lockout.LockoutDuration'].displayName }}</span>
              <el-input
                v-model="globalSetting['Abp.Identity.Lockout.LockoutDuration'].value"
                :placeholder="globalSetting['Abp.Identity.Lockout.LockoutDuration'].description"
                type="number"
                @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.LockoutDuration', value)"
              />
            </el-form-item>
          </el-row>
        </el-tab-pane>
        <el-tab-pane
          :label="$t('settings.userAccount')"
        >
          <el-row>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.SignIn.RequireConfirmedEmail')"
              >
                <el-popover
                  ref="SignInRequireConfirmedEmail"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.SignIn.RequireConfirmedEmail'].displayName"
                  :content="globalSetting['Abp.Identity.SignIn.RequireConfirmedEmail'].description"
                />
                <span
                  slot="label"
                  v-popover:SignInRequireConfirmedEmail
                >{{ globalSetting['Abp.Identity.SignIn.RequireConfirmedEmail'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.SignIn.RequireConfirmedEmail'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.RequireConfirmedEmail', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.SignIn.EnablePhoneNumberConfirmation')"
              >
                <el-popover
                  ref="SignInEnablePhoneNumberConfirmation"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].displayName"
                  :content="globalSetting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].description"
                />
                <span
                  slot="label"
                  v-popover:SignInEnablePhoneNumberConfirmation
                >{{ globalSetting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.EnablePhoneNumberConfirmation', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.SignIn.RequireConfirmedPhoneNumber')"
              >
                <el-popover
                  ref="SignInRequireConfirmedPhoneNumber"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].displayName"
                  :content="globalSetting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].description"
                />
                <span
                  slot="label"
                  v-popover:SignInRequireConfirmedPhoneNumber
                >{{ globalSetting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.RequireConfirmedPhoneNumber', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Identity.User.IsUserNameUpdateEnabled')"
              >
                <el-popover
                  ref="UserIsUserNameUpdateEnabled"
                  trigger="hover"
                  :title="globalSetting['Abp.Identity.User.IsUserNameUpdateEnabled'].displayName"
                  :content="globalSetting['Abp.Identity.User.IsUserNameUpdateEnabled'].description"
                />
                <span
                  slot="label"
                  v-popover:UserIsUserNameUpdateEnabled
                >{{ globalSetting['Abp.Identity.User.IsUserNameUpdateEnabled'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Identity.User.IsUserNameUpdateEnabled'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.User.IsUserNameUpdateEnabled', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.User.IsEmailUpdateEnabled')"
          >
            <el-popover
              ref="UserIsEmailUpdateEnabled"
              trigger="hover"
              :title="globalSetting['Abp.Identity.User.IsEmailUpdateEnabled'].displayName"
              :content="globalSetting['Abp.Identity.User.IsEmailUpdateEnabled'].description"
            />
            <span
              slot="label"
              v-popover:UserIsEmailUpdateEnabled
            >{{ globalSetting['Abp.Identity.User.IsEmailUpdateEnabled'].displayName }}</span>
            <el-switch
              v-model="globalSetting['Abp.Identity.User.IsEmailUpdateEnabled'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.User.IsEmailUpdateEnabled', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.OrganizationUnit.MaxUserMembershipCount')"
          >
            <el-popover
              ref="OrganizationUnitMaxUserMembershipCount"
              trigger="hover"
              :title="globalSetting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].displayName"
              :content="globalSetting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].description"
            />
            <span
              slot="label"
              v-popover:OrganizationUnitMaxUserMembershipCount
            >{{ globalSetting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].value"
              :placeholder="globalSetting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].description"
              @input="(value) => handleSettingValueChanged('Abp.Identity.OrganizationUnit.MaxUserMembershipCount', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.SmsRegisterTemplateCode')"
          >
            <el-popover
              ref="AccountSmsRegisterTemplateCode"
              trigger="hover"
              :title="globalSetting['Abp.Account.SmsRegisterTemplateCode'].displayName"
              :content="globalSetting['Abp.Account.SmsRegisterTemplateCode'].description"
            />
            <span
              slot="label"
              v-popover:AccountSmsRegisterTemplateCode
            >{{ globalSetting['Abp.Account.SmsRegisterTemplateCode'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Account.SmsRegisterTemplateCode'].value"
              :placeholder="globalSetting['Abp.Account.SmsRegisterTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsRegisterTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.SmsSigninTemplateCode')"
          >
            <el-popover
              ref="AccountSmsSigninTemplateCode"
              trigger="hover"
              :title="globalSetting['Abp.Account.SmsSigninTemplateCode'].displayName"
              :content="globalSetting['Abp.Account.SmsSigninTemplateCode'].description"
            />
            <span
              slot="label"
              v-popover:AccountSmsSigninTemplateCode
            >{{ globalSetting['Abp.Account.SmsSigninTemplateCode'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Account.SmsSigninTemplateCode'].value"
              :placeholder="globalSetting['Abp.Account.SmsSigninTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsSigninTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.SmsResetPasswordTemplateCode')"
          >
            <el-popover
              ref="AccountSmsResetPasswordTemplateCode"
              trigger="hover"
              :title="globalSetting['Abp.Account.SmsResetPasswordTemplateCode'].displayName"
              :content="globalSetting['Abp.Account.SmsResetPasswordTemplateCode'].description"
            />
            <span
              slot="label"
              v-popover:AccountSmsResetPasswordTemplateCode
            >{{ globalSetting['Abp.Account.SmsResetPasswordTemplateCode'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Account.SmsResetPasswordTemplateCode'].value"
              :placeholder="globalSetting['Abp.Account.SmsResetPasswordTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsResetPasswordTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.PhoneVerifyCodeExpiration')"
          >
            <el-popover
              ref="AccountPhoneVerifyCodeExpiration"
              trigger="hover"
              :title="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].displayName"
              :content="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].description"
            />
            <span
              slot="label"
              v-popover:AccountPhoneVerifyCodeExpiration
            >{{ globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].displayName }}</span>
            <el-input
              v-model="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].value"
              :placeholder="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.PhoneVerifyCodeExpiration', value)"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Account.IsSelfRegistrationEnabled')"
              >
                <el-popover
                  ref="AccountIsSelfRegistrationEnabled"
                  trigger="hover"
                  :title="globalSetting['Abp.Account.IsSelfRegistrationEnabled'].displayName"
                  :content="globalSetting['Abp.Account.IsSelfRegistrationEnabled'].description"
                />
                <span
                  slot="label"
                  v-popover:AccountIsSelfRegistrationEnabled
                >{{ globalSetting['Abp.Account.IsSelfRegistrationEnabled'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Account.IsSelfRegistrationEnabled'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Account.IsSelfRegistrationEnabled', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item
                v-if="hasSettingExistsed('Abp.Account.EnableLocalLogin')"
              >
                <el-popover
                  ref="AccountEnableLocalLogin"
                  trigger="hover"
                  :title="globalSetting['Abp.Account.EnableLocalLogin'].displayName"
                  :content="globalSetting['Abp.Account.EnableLocalLogin'].description"
                />
                <span
                  slot="label"
                  v-popover:AccountEnableLocalLogin
                >{{ globalSetting['Abp.Account.EnableLocalLogin'].displayName }}</span>
                <el-switch
                  v-model="globalSetting['Abp.Account.EnableLocalLogin'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Account.EnableLocalLogin', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
        </el-tab-pane>
      </el-tabs>

      <el-form-item>
        <el-button
          type="primary"
          style="width:200px;margin:inherit;"
          @click="onSaveGlobalSetting"
        >
          {{ $t('global.confirm') }}
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { AbpConfigurationModule } from '@/store/modules/abp'
import SettingService, { Setting, SettingUpdate, SettingsUpdate } from '@/api/settings'

const booleanStrings = ['True', 'true', 'False', 'false']

@Component({
  name: 'GlobalSettingEditForm'
})
export default class extends Vue {
  private globalSettingLoaded = false
  private globalSetting: {[key: string]: Setting} = {}
  private globalSettingChangeKeys = new Array<string>()

  get definedLanguages() {
    const languages = AbpConfigurationModule.configuration.localization.languages.map((lang: any) => {
      return lang.cultureName
    })
    return languages
  }

  private hasSettingExistsed(key: string) {
    if (this.globalSetting[key]) {
      return true
    }
    return false
  }

  mounted() {
    this.handleGetGlobalSettings()
  }

  private handleSettingValueChanged(key: string, value: any) {
    if (!this.globalSettingChangeKeys.includes(key)) {
      this.globalSettingChangeKeys.push(key)
    }
    this.$set(this.globalSetting[key], 'value', value)
    this.$forceUpdate()
  }

  private handleGetGlobalSettings() {
    SettingService.getSettings('G', '').then(settings => {
      settings.items.forEach(setting => {
        if (setting.value) {
          const value = setting.value.toLowerCase()
          if (booleanStrings.includes(value)) {
            setting.value = value === 'true'
          }
        } else {
          const defaultValue = setting.defaultValue.toLowerCase()
          if (booleanStrings.includes(defaultValue)) {
            setting.value = defaultValue === 'true'
          }
        }
        this.globalSetting[setting.name] = setting
      })
      this.globalSettingLoaded = true
      this.$forceUpdate()
    })
  }

  private onSaveGlobalSetting() {
    const updateSettings = new SettingsUpdate()
    this.globalSettingChangeKeys.forEach(key => {
      const updateSetting = new SettingUpdate()
      updateSetting.name = key
      updateSetting.value = this.globalSetting[key].value
      updateSettings.settings.push(updateSetting)
    })
    if (updateSettings.settings.length > 0) {
      SettingService.setSettings('G', '', updateSettings).then(() => {
        this.$message.success(this.$t('AbpSettingManagement.SuccessfullySaved').toString())
      })
    }
  }
}
</script>
