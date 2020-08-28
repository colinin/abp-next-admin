<template>
  <div>
    <el-form
      ref="formsetting"
      v-model="setting"
      label-width="180px"
      style="width: 96%"
    >
      <el-tabs>
        <el-tab-pane
          v-if="hasSettingExistsed('Abp.Localization.DefaultLanguage')"
          :label="$t('settings.systemSetting')"
        >
          <el-form-item
            v-if="hasSettingExistsed('Abp.Localization.DefaultLanguage')"
            v-popover:DefaultLanguage
            prop="setting['Abp.Localization.DefaultLanguage'].value"
          >
            <el-popover
              ref="DefaultLanguage"
              trigger="hover"
              :title="setting['Abp.Localization.DefaultLanguage'].displayName"
              :content="setting['Abp.Localization.DefaultLanguage'].description"
            />
            <span
              slot="label"
              v-popover:DefaultLanguage
            >{{ setting['Abp.Localization.DefaultLanguage'].displayName }}</span>
            <el-select
              v-model="setting['Abp.Localization.DefaultLanguage'].value"
              style="width: 100%;"
              @change="(value) => handleSettingValueChanged('Abp.Localization.DefaultLanguage', value)"
            >
              <el-option
                v-for="language in definedLanguages"
                :key="language"
                :label="language"
                :value="language"
                :disabled="language===setting['Abp.Localization.DefaultLanguage'].value"
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
              :title="setting['Abp.Timing.TimeZone'].displayName"
              :content="setting['Abp.Timing.TimeZone'].description"
            />
            <span
              slot="label"
              v-popover:TimeZone
            >{{ setting['Abp.Timing.TimeZone'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Timing.TimeZone'].value"
              :placeholder="setting['Abp.Timing.TimeZone'].description"
              @input="(value) => handleSettingValueChanged('Abp.Timing.TimeZone', value)"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          v-if="hasSettingExistsed('Abp.Identity.Password.RequiredLength')"
          :label="$t('settings.passwordSecurity')"
        >
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequiredLength')"
          >
            <el-popover
              ref="PasswordRequiredLength"
              trigger="hover"
              :title="setting['Abp.Identity.Password.RequiredLength'].displayName"
              :content="setting['Abp.Identity.Password.RequiredLength'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequiredLength
            >{{ setting['Abp.Identity.Password.RequiredLength'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Identity.Password.RequiredLength'].value"
              :placeholder="setting['Abp.Identity.Password.RequiredLength'].description"
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
              :title="setting['Abp.Identity.Password.RequiredUniqueChars'].displayName"
              :content="setting['Abp.Identity.Password.RequiredUniqueChars'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequiredUniqueChars
            >{{ setting['Abp.Identity.Password.RequiredUniqueChars'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Identity.Password.RequiredUniqueChars'].value"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequiredUniqueChars', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequireNonAlphanumeric')"
          >
            <el-popover
              ref="PasswordRequireNonAlphanumeric"
              trigger="hover"
              :title="setting['Abp.Identity.Password.RequireNonAlphanumeric'].displayName"
              :content="setting['Abp.Identity.Password.RequireNonAlphanumeric'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequireNonAlphanumeric
            >{{ setting['Abp.Identity.Password.RequireNonAlphanumeric'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.Password.RequireNonAlphanumeric'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireNonAlphanumeric', value)"
            />
          </el-form-item>

          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequireLowercase')"
          >
            <el-popover
              ref="PasswordRequireLowercase"
              trigger="hover"
              :title="setting['Abp.Identity.Password.RequireLowercase'].displayName"
              :content="setting['Abp.Identity.Password.RequireLowercase'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequireLowercase
            >{{ setting['Abp.Identity.Password.RequireLowercase'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.Password.RequireLowercase'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireLowercase', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequireUppercase')"
          >
            <el-popover
              ref="PasswordRequireUppercase"
              trigger="hover"
              :title="setting['Abp.Identity.Password.RequireUppercase'].displayName"
              :content="setting['Abp.Identity.Password.RequireUppercase'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequireUppercase
            >{{ setting['Abp.Identity.Password.RequireUppercase'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.Password.RequireUppercase'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireUppercase', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Password.RequireDigit')"
          >
            <el-popover
              ref="PasswordRequireDigit"
              trigger="hover"
              :title="setting['Abp.Identity.Password.RequireDigit'].displayName"
              :content="setting['Abp.Identity.Password.RequireDigit'].description"
            />
            <span
              slot="label"
              v-popover:PasswordRequireDigit
            >{{ setting['Abp.Identity.Password.RequireDigit'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.Password.RequireDigit'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Password.RequireDigit', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Lockout.AllowedForNewUsers')"
          >
            <el-popover
              ref="LockoutAllowedForNewUsers"
              trigger="hover"
              :title="setting['Abp.Identity.Lockout.AllowedForNewUsers'].displayName"
              :content="setting['Abp.Identity.Lockout.AllowedForNewUsers'].description"
            />
            <span
              slot="label"
              v-popover:LockoutAllowedForNewUsers
            >{{ setting['Abp.Identity.Lockout.AllowedForNewUsers'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.Lockout.AllowedForNewUsers'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.AllowedForNewUsers', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Lockout.LockoutDuration')"
          >
            <el-popover
              ref="LockoutLockoutDuration"
              trigger="hover"
              :title="setting['Abp.Identity.Lockout.LockoutDuration'].displayName"
              :content="setting['Abp.Identity.Lockout.LockoutDuration'].description"
            />
            <span
              slot="label"
              v-popover:LockoutLockoutDuration
            >{{ setting['Abp.Identity.Lockout.LockoutDuration'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Identity.Lockout.LockoutDuration'].value"
              :placeholder="setting['Abp.Identity.Lockout.LockoutDuration'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.LockoutDuration', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.Lockout.MaxFailedAccessAttempts')"
          >
            <el-popover
              ref="LockoutMaxFailedAccessAttempts"
              trigger="hover"
              :title="setting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].displayName"
              :content="setting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].description"
            />
            <span
              slot="label"
              v-popover:LockoutMaxFailedAccessAttempts
            >{{ setting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].value"
              :placeholder="setting['Abp.Identity.Lockout.MaxFailedAccessAttempts'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.Lockout.MaxFailedAccessAttempts', value)"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          v-if="hasSettingExistsed('Abp.Identity.SignIn.RequireConfirmedEmail')"
          :label="$t('settings.userAccount')"
        >
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.SignIn.RequireConfirmedEmail')"
          >
            <el-popover
              ref="SignInRequireConfirmedEmail"
              trigger="hover"
              :title="setting['Abp.Identity.SignIn.RequireConfirmedEmail'].displayName"
              :content="setting['Abp.Identity.SignIn.RequireConfirmedEmail'].description"
            />
            <span
              slot="label"
              v-popover:SignInRequireConfirmedEmail
            >{{ setting['Abp.Identity.SignIn.RequireConfirmedEmail'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.SignIn.RequireConfirmedEmail'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.RequireConfirmedEmail', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.SignIn.EnablePhoneNumberConfirmation')"
          >
            <el-popover
              ref="SignInEnablePhoneNumberConfirmation"
              trigger="hover"
              :title="setting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].displayName"
              :content="setting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].description"
            />
            <span
              slot="label"
              v-popover:SignInEnablePhoneNumberConfirmation
            >{{ setting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.SignIn.EnablePhoneNumberConfirmation'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.EnablePhoneNumberConfirmation', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.SignIn.RequireConfirmedPhoneNumber')"
          >
            <el-popover
              ref="SignInRequireConfirmedPhoneNumber"
              trigger="hover"
              :title="setting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].displayName"
              :content="setting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].description"
            />
            <span
              slot="label"
              v-popover:SignInRequireConfirmedPhoneNumber
            >{{ setting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.SignIn.RequireConfirmedPhoneNumber'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.SignIn.RequireConfirmedPhoneNumber', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.User.IsUserNameUpdateEnabled')"
          >
            <el-popover
              ref="UserIsUserNameUpdateEnabled"
              trigger="hover"
              :title="setting['Abp.Identity.User.IsUserNameUpdateEnabled'].displayName"
              :content="setting['Abp.Identity.User.IsUserNameUpdateEnabled'].description"
            />
            <span
              slot="label"
              v-popover:UserIsUserNameUpdateEnabled
            >{{ setting['Abp.Identity.User.IsUserNameUpdateEnabled'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.User.IsUserNameUpdateEnabled'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.User.IsUserNameUpdateEnabled', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.User.IsEmailUpdateEnabled')"
          >
            <el-popover
              ref="UserIsEmailUpdateEnabled"
              trigger="hover"
              :title="setting['Abp.Identity.User.IsEmailUpdateEnabled'].displayName"
              :content="setting['Abp.Identity.User.IsEmailUpdateEnabled'].description"
            />
            <span
              slot="label"
              v-popover:UserIsEmailUpdateEnabled
            >{{ setting['Abp.Identity.User.IsEmailUpdateEnabled'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Identity.User.IsEmailUpdateEnabled'].value"
              @input="(value) => handleSettingValueChanged('Abp.Identity.User.IsEmailUpdateEnabled', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Identity.OrganizationUnit.MaxUserMembershipCount')"
          >
            <el-popover
              ref="OrganizationUnitMaxUserMembershipCount"
              trigger="hover"
              :title="setting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].displayName"
              :content="setting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].description"
            />
            <span
              slot="label"
              v-popover:OrganizationUnitMaxUserMembershipCount
            >{{ setting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].value"
              :placeholder="setting['Abp.Identity.OrganizationUnit.MaxUserMembershipCount'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Identity.OrganizationUnit.MaxUserMembershipCount', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.SmsRegisterTemplateCode')"
          >
            <el-popover
              ref="AccountSmsRegisterTemplateCode"
              trigger="hover"
              :title="setting['Abp.Account.SmsRegisterTemplateCode'].displayName"
              :content="setting['Abp.Account.SmsRegisterTemplateCode'].description"
            />
            <span
              slot="label"
              v-popover:AccountSmsRegisterTemplateCode
            >{{ setting['Abp.Account.SmsRegisterTemplateCode'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Account.SmsRegisterTemplateCode'].value"
              :placeholder="setting['Abp.Account.SmsRegisterTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsRegisterTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.SmsSigninTemplateCode')"
          >
            <el-popover
              ref="AccountSmsSigninTemplateCode"
              trigger="hover"
              :title="setting['Abp.Account.SmsSigninTemplateCode'].displayName"
              :content="setting['Abp.Account.SmsSigninTemplateCode'].description"
            />
            <span
              slot="label"
              v-popover:AccountSmsSigninTemplateCode
            >{{ setting['Abp.Account.SmsSigninTemplateCode'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Account.SmsSigninTemplateCode'].value"
              :placeholder="setting['Abp.Account.SmsSigninTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsSigninTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.SmsResetPasswordTemplateCode')"
          >
            <el-popover
              ref="AccountSmsResetPasswordTemplateCode"
              trigger="hover"
              :title="setting['Abp.Account.SmsResetPasswordTemplateCode'].displayName"
              :content="setting['Abp.Account.SmsResetPasswordTemplateCode'].description"
            />
            <span
              slot="label"
              v-popover:AccountSmsResetPasswordTemplateCode
            >{{ setting['Abp.Account.SmsResetPasswordTemplateCode'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Account.SmsResetPasswordTemplateCode'].value"
              :placeholder="setting['Abp.Account.SmsResetPasswordTemplateCode'].description"
              @input="(value) => handleSettingValueChanged('Abp.Account.SmsResetPasswordTemplateCode', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.PhoneVerifyCodeExpiration')"
          >
            <el-popover
              ref="AccountPhoneVerifyCodeExpiration"
              trigger="hover"
              :title="setting['Abp.Account.PhoneVerifyCodeExpiration'].displayName"
              :content="setting['Abp.Account.PhoneVerifyCodeExpiration'].description"
            />
            <span
              slot="label"
              v-popover:AccountPhoneVerifyCodeExpiration
            >{{ setting['Abp.Account.PhoneVerifyCodeExpiration'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Account.PhoneVerifyCodeExpiration'].value"
              :placeholder="setting['Abp.Account.PhoneVerifyCodeExpiration'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Account.PhoneVerifyCodeExpiration', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.IsSelfRegistrationEnabled')"
          >
            <el-popover
              ref="AccountIsSelfRegistrationEnabled"
              trigger="hover"
              :title="setting['Abp.Account.IsSelfRegistrationEnabled'].displayName"
              :content="setting['Abp.Account.IsSelfRegistrationEnabled'].description"
            />
            <span
              slot="label"
              v-popover:AccountIsSelfRegistrationEnabled
            >{{ setting['Abp.Account.IsSelfRegistrationEnabled'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Account.IsSelfRegistrationEnabled'].value"
              @input="(value) => handleSettingValueChanged('Abp.Account.IsSelfRegistrationEnabled', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Account.EnableLocalLogin')"
          >
            <el-popover
              ref="AccountEnableLocalLogin"
              trigger="hover"
              :title="setting['Abp.Account.EnableLocalLogin'].displayName"
              :content="setting['Abp.Account.EnableLocalLogin'].description"
            />
            <span
              slot="label"
              v-popover:AccountEnableLocalLogin
            >{{ setting['Abp.Account.EnableLocalLogin'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Account.EnableLocalLogin'].value"
              @input="(value) => handleSettingValueChanged('Abp.Account.EnableLocalLogin', value)"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          v-if="hasSettingExistsed('Abp.Mailing.Smtp.Host')"
          :label="$t('settings.mailing')"
        >
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.Host')"
          >
            <el-popover
              ref="SmtpHost"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.Host'].displayName"
              :content="setting['Abp.Mailing.Smtp.Host'].description"
            />
            <span
              slot="label"
              v-popover:SmtpHost
            >{{ setting['Abp.Mailing.Smtp.Host'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.Smtp.Host'].value"
              :placeholder="setting['Abp.Mailing.Smtp.Host'].description"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.Host', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.Port')"
          >
            <el-popover
              ref="SmtpPort"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.Port'].displayName"
              :content="setting['Abp.Mailing.Smtp.Port'].description"
            />
            <span
              slot="label"
              v-popover:SmtpPort
            >{{ setting['Abp.Mailing.Smtp.Port'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.Smtp.Port'].value"
              :placeholder="setting['Abp.Mailing.Smtp.Port'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.Port', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.UserName')"
          >
            <el-popover
              ref="SmtpUserName"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.UserName'].displayName"
              :content="setting['Abp.Mailing.Smtp.UserName'].description"
            />
            <span
              slot="label"
              v-popover:SmtpUserName
            >{{ setting['Abp.Mailing.Smtp.UserName'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.Smtp.UserName'].value"
              :placeholder="setting['Abp.Mailing.Smtp.UserName'].description"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.UserName', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.Password')"
          >
            <el-popover
              ref="SmtpPassword"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.Password'].displayName"
              :content="setting['Abp.Mailing.Smtp.Password'].description"
            />
            <span
              slot="label"
              v-popover:SmtpPassword
            >{{ setting['Abp.Mailing.Smtp.Password'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.Smtp.Password'].value"
              :placeholder="setting['Abp.Mailing.Smtp.Password'].description"
              type="password"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.Password', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.Domain')"
          >
            <el-popover
              ref="SmtpDomain"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.Domain'].displayName"
              :content="setting['Abp.Mailing.Smtp.Domain'].description"
            />
            <span
              slot="label"
              v-popover:SmtpDomain
            >{{ setting['Abp.Mailing.Smtp.Domain'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.Smtp.Domain'].value"
              :placeholder="setting['Abp.Mailing.Smtp.Domain'].description"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.Domain', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.EnableSsl')"
          >
            <el-popover
              ref="SmtpEnableSsl"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.EnableSsl'].displayName"
              :content="setting['Abp.Mailing.Smtp.EnableSsl'].description"
            />
            <span
              slot="label"
              v-popover:SmtpEnableSsl
            >{{ setting['Abp.Mailing.Smtp.EnableSsl'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Mailing.Smtp.EnableSsl'].value"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.EnableSsl', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.Smtp.UseDefaultCredentials')"
          >
            <el-popover
              ref="SmtpUseDefaultCredentials"
              trigger="hover"
              :title="setting['Abp.Mailing.Smtp.UseDefaultCredentials'].displayName"
              :content="setting['Abp.Mailing.Smtp.UseDefaultCredentials'].description"
            />
            <span
              slot="label"
              v-popover:SmtpUseDefaultCredentials
            >{{ setting['Abp.Mailing.Smtp.UseDefaultCredentials'].displayName }}</span>
            <el-switch
              v-model="setting['Abp.Mailing.Smtp.UseDefaultCredentials'].value"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.Smtp.UseDefaultCredentials', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.DefaultFromAddress')"
          >
            <el-popover
              ref="MailingDefaultFromAddress"
              trigger="hover"
              :title="setting['Abp.Mailing.DefaultFromAddress'].displayName"
              :content="setting['Abp.Mailing.DefaultFromAddress'].description"
            />
            <span
              slot="label"
              v-popover:MailingDefaultFromAddress
            >{{ setting['Abp.Mailing.DefaultFromAddress'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.DefaultFromAddress'].value"
              :placeholder="setting['Abp.Mailing.DefaultFromAddress'].description"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.DefaultFromAddress', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.Mailing.DefaultFromDisplayName')"
          >
            <el-popover
              ref="MailingDefaultFromDisplayName"
              trigger="hover"
              :title="setting['Abp.Mailing.DefaultFromDisplayName'].displayName"
              :content="setting['Abp.Mailing.DefaultFromDisplayName'].description"
            />
            <span
              slot="label"
              v-popover:MailingDefaultFromDisplayName
            >{{ setting['Abp.Mailing.DefaultFromDisplayName'].displayName }}</span>
            <el-input
              v-model="setting['Abp.Mailing.DefaultFromDisplayName'].value"
              :placeholder="setting['Abp.Mailing.DefaultFromDisplayName'].description"
              @input="(value) => handleSettingValueChanged('Abp.Mailing.DefaultFromDisplayName', value)"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          v-if="hasSettingExistsed('Abp.FileManagement.FileLimitLength')"
          :label="$t('fileSystem.setting')"
        >
          <el-form-item
            v-if="hasSettingExistsed('Abp.FileManagement.FileLimitLength')"
            prop="Abp.FileManagement.FileLimitLength'].value"
          >
            <el-popover
              ref="FileLimitLength"
              trigger="hover"
              :title="setting['Abp.FileManagement.FileLimitLength'].displayName"
              :content="setting['Abp.FileManagement.FileLimitLength'].description"
            />
            <span
              slot="label"
              v-popover:FileLimitLength
            >{{ setting['Abp.FileManagement.FileLimitLength'].displayName }}</span>
            <el-input
              v-model="setting['Abp.FileManagement.FileLimitLength'].value"
              :placeholder="setting['Abp.FileManagement.FileLimitLength'].description"
              type="number"
              @input="(value) => handleSettingValueChanged('Abp.FileManagement.FileLimitLength', value)"
            />
          </el-form-item>
          <el-form-item
            v-if="hasSettingExistsed('Abp.FileManagement.AllowFileExtensions')"
            prop="Abp.FileManagement.AllowFileExtensions'].value"
          >
            <el-popover
              ref="AllowFileExtensions"
              trigger="hover"
              :title="setting['Abp.FileManagement.AllowFileExtensions'].displayName"
              :content="setting['Abp.FileManagement.AllowFileExtensions'].description"
            />
            <span
              slot="label"
              v-popover:AllowFileExtensions
            >{{ setting['Abp.FileManagement.AllowFileExtensions'].displayName }}</span>
            <el-input
              v-model="setting['Abp.FileManagement.AllowFileExtensions'].value"
              :placeholder="setting['Abp.FileManagement.AllowFileExtensions'].description"
              @input="(value) => handleSettingValueChanged('Abp.FileManagement.AllowFileExtensions', value)"
            />
          </el-form-item>
        </el-tab-pane>
      </el-tabs>

      <el-form-item
        v-if="settings.length>0"
      >
        <el-button
          type="primary"
          style="width:200px;margin:inherit;"
          @click="onSavesetting"
        >
          {{ $t('global.confirm') }}
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Watch, Vue } from 'vue-property-decorator'
import { AbpModule } from '@/store/modules/abp'
import { Setting, SettingUpdate, SettingsUpdate } from '@/api/settings'

const booleanStrings = ['True', 'true', 'False', 'false']

@Component({
  name: 'SettingEditForm'
})
export default class extends Vue {
  @Prop({ default: () => { return Array<Setting>() } })
  private settings!: Setting[]

  private setting: {[key: string]: Setting} = {}

  private settingChangeKeys = new Array<string>()

  get definedLanguages() {
    const languages = AbpModule.configuration.localization.languages.map((lang: any) => {
      return lang.cultureName
    })
    return languages
  }

  @Watch('settings', { immediate: true })
  private onSettingChanged() {
    if (this.settings.length > 0) {
      this.settings.forEach(setting => {
        if (setting.value) {
          const value = setting.value.toLowerCase()
          if (booleanStrings.includes(value)) {
            setting.value = (value === 'true')
          }
        } else {
          if (setting.defaultValue) {
            const defaultValue = setting.defaultValue.toLowerCase()
            if (booleanStrings.includes(defaultValue)) {
              setting.value = (defaultValue === 'true')
            }
          }
        }
        this.setting[setting.name] = setting
      })
      this.$forceUpdate()
    }
  }

  private hasSettingExistsed(key: string) {
    if (this.setting[key]) {
      return true
    }
    return false
  }

  private handleSettingValueChanged(key: string, value: any) {
    if (!this.settingChangeKeys.includes(key)) {
      this.settingChangeKeys.push(key)
    }
    this.$set(this.setting[key], 'value', value)
    this.$forceUpdate()
  }

  private onSavesetting() {
    const updateSettings = new SettingsUpdate()
    this.settingChangeKeys.forEach(key => {
      const updateSetting = new SettingUpdate()
      updateSetting.name = key
      updateSetting.value = this.setting[key].value
      updateSettings.settings.push(updateSetting)
    })
    if (updateSettings.settings.length > 0) {
      this.$emit('onSettingSaving', updateSettings)
    }
  }
}
</script>
