<template>
  <div v-if="globalSettingLoaded">
    <el-form
      ref="formGlobalSetting"
      v-model="globalSetting"
      label-width="180px"
      style="width: 96%"
    >
      <el-tabs>
        <el-tab-pane :label="$t('settings.systemSetting')">
          <el-form-item
            prop="globalSetting['Abp.Localization.DefaultLanguage'].value"
            :label="globalSetting['Abp.Localization.DefaultLanguage'].displayName"
          >
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
            <!-- <el-input
              v-model="globalSetting['Abp.Localization.DefaultLanguage'].value"
              :placeholder="globalSetting['Abp.Localization.DefaultLanguage'].description"
              @input="(value) => handleSettingValueChanged('Abp.Localization.DefaultLanguage', value)"
            /> -->
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          v-if="allowIdentitySetting"
          :label="$t('settings.passwordSecurity')"
        >
          <el-form-item :label="globalSetting['Abp.Identity.Password.RequiredLength'].displayName">
            <el-input
              v-model="globalSetting['Abp.Identity.Password.RequiredLength'].value"
              :placeholder="globalSetting['Abp.Identity.Password.RequiredLength'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequiredLength', value)"
            />
          </el-form-item>
          <el-form-item :label="globalSetting['Abp.Identity.Password.RequiredUniqueChars'].displayName">
            <el-input
              v-model="globalSetting['Abp.Identity.Password.RequiredUniqueChars'].value"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequiredUniqueChars', value)"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.Password.RequireNonAlphanumeric'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireNonAlphanumeric'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireNonAlphanumeric', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.Password.RequireLowercase'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireLowercase'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireLowercase', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.Password.RequireUppercase'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireUppercase'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireUppercase', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.Password.RequireDigit'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.Password.RequireDigit'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireDigit', value)"
                />
              </el-form-item>
            </el-col>
            <el-form-item :label="globalSetting['Abp.Identity.Lockout.AllowedForNewUsers'].displayName">
              <el-switch
                v-model="globalSetting['Abp.Identity.Lockout.AllowedForNewUsers'].value"
                @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.AllowedForNewUsers', value)"
              />
            </el-form-item>
            <el-form-item :label="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].displayName">
              <el-input
                v-model="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].value"
                :placeholder="globalSetting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].description"
                type="number"
                @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.MaxFailedAccessAttempts', value)"
              />
            </el-form-item>
            <el-form-item :label="globalSetting['Abp.Identity.Lockout.LockoutDuration'].displayName">
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
          v-if="allowAccountSetting"
          :label="$t('settings.userAccount')"
        >
          <el-row>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.SignIn.RequireConfirmedEmail'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.SignIn.RequireConfirmedEmail'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.RequireConfirmedEmail', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.EnablePhoneNumberConfirmation', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.RequireConfirmedPhoneNumber', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Identity.User.IsUserNameUpdateEnabled'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Identity.User.IsUserNameUpdateEnabled'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Identity.User.IsUserNameUpdateEnabled', value)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item :label="globalSetting['Abp.Identity.User.IsEmailUpdateEnabled'].displayName">
            <el-switch
              v-model="globalSetting['Abp.Identity.User.IsEmailUpdateEnabled'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.User.IsEmailUpdateEnabled', value)"
            />
          </el-form-item>
          <el-form-item :label="globalSetting['Abp.Account.SmsRegisterTemplateCode'].displayName">
            <el-input
              v-model="globalSetting['Abp.Account.SmsRegisterTemplateCode'].value"
              :placeholder="globalSetting['Abp.Account.SmsRegisterTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsRegisterTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item :label="globalSetting['Abp.Account.SmsSigninTemplateCode'].displayName">
            <el-input
              v-model="globalSetting['Abp.Account.SmsSigninTemplateCode'].value"
              :placeholder="globalSetting['Abp.Account.SmsSigninTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsSigninTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item :label="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].displayName">
            <el-input
              v-model="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].value"
              :placeholder="globalSetting['Abp.Account.PhoneVerifyCodeExpiration'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.PhoneVerifyCodeExpiration', value)"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item :label="globalSetting['Abp.Account.IsSelfRegistrationEnabled'].displayName">
                <el-switch
                  v-model="globalSetting['Abp.Account.IsSelfRegistrationEnabled'].value"
                  @input="(value) => handleSettingValueChanged('Abp.Account.IsSelfRegistrationEnabled', value)"
                />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item :label="globalSetting['Abp.Account.EnableLocalLogin'].displayName">
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

  get allowIdentitySetting() {
    if (this.globalSetting['Abp.Identity.Password.RequiredLength']) {
      return true
    }
    return false
  }

  get allowAccountSetting() {
    if (this.globalSetting['Abp.Account.EnableLocalLogin']) {
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
