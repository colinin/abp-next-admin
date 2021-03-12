<template>
  <div class="app-container">
    <h4>{{ $t('AbpAccount.ManageYourProfile') }}</h4>
    <el-divider />
    <el-form
      v-model="myProfile"
      label-width="180px"
    >
      <el-row>
        <el-col :span="16">
          <el-form-item
            :label="$t('AbpAccount.DisplayName:UserName')"
            prop="userName"
          >
            <el-input v-model="myProfile.userName" />
          </el-form-item>
          <el-form-item
            :label="$t('AbpAccount.DisplayName:Name')"
            prop="name"
          >
            <el-input v-model="myProfile.name" />
          </el-form-item>
          <el-form-item
            :label="$t('AbpAccount.DisplayName:Surname')"
            prop="surname"
          >
            <el-input v-model="myProfile.surname" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <div class="user-avatar">
            <div class="box-center">
              <el-row>
                <pan-thumb
                  :image="myAvatar"
                  :height="'100px'"
                  :width="'100px'"
                  :hoverable="false"
                />
              </el-row>
              <el-row>
                <el-button
                  @click="handleShowImageUpload"
                >
                  <i class="el-icon-upload2" />
                  更换头像
                </el-button>
                <avatar-upload
                  v-model="showImageUpload"
                  field="file"
                  :width="300"
                  :height="300"
                  :params="params"
                  :headers="headers"
                  :url="uploadUrl"
                  @close="showImageUpload=false"
                  @src-file-set="handleSrcFileSet"
                  @crop-upload-fail="handleUploadFail"
                  @crop-upload-success="onCropUploadSuccess"
                />
              </el-row>
            </div>
          </div>
        </el-col>
      </el-row>
      <el-form-item
        :label="$t('AbpAccount.DisplayName:Email')"
        prop="email"
      >
        <el-input v-model="myProfile.email" />
      </el-form-item>
      <el-form-item
        :label="$t('AbpAccount.DisplayName:PhoneNumber')"
        prop="phoneNumber"
      >
        <el-input v-model="myProfile.phoneNumber" />
      </el-form-item>
      <el-form-item>
        <el-button
          type="primary"
          @click="handleUpdateMyProfile"
        >
          更新个人资料
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts">
import MyProfileService, { MyProfile, UpdateMyProfile } from '@/api/profile'
import OssManagerApi from '@/api/oss-manager'
import { Component, Vue } from 'vue-property-decorator'
import { UserModule } from '@/store/modules/user'
import AvatarUpload from '@/components/AvatarUpload/index.vue'
import PanThumb from '@/components/PanThumb/index.vue'

@Component({
  name: 'MyProfile',
  components: {
    PanThumb,
    AvatarUpload
  }
})
export default class extends Vue {
  private myProfile = new MyProfile()

  private showImageUpload = false
  private headers: any = {}
  private params: any = {}
  private uploadUrl = ''

  get myAvatar() {
    if (this.myProfile.extraProperties) {
      const avatar = this.myProfile.extraProperties.AvatarUrl
      if (avatar) {
        // 处理下图片与组件大小一致
        return avatar + '/w_300,h_300'
      }
    }
    return ''
  }

  set myAvatar(avatar: string) {
    if (this.myProfile.extraProperties) {
      this.myProfile.extraProperties.AvatarUrl = avatar
    }
  }

  mounted() {
    MyProfileService.getMyProfile().then(profile => {
      this.myProfile = profile
    })
  }

  private handleShowImageUpload() {
    this.showImageUpload = true
    this.headers.Authorization = UserModule.token
  }

  private handleSrcFileSet(fileName: string) {
    this.uploadUrl = OssManagerApi.generateOssUrl('user-account', fileName, UserModule.userName + '/', '/api')
  }

  private handleUploadFail(status: any, field: any) {
    console.log(status)
    console.log(field)
  }

  private onCropUploadSuccess() {
    this.showImageUpload = false
    this.myAvatar = this.uploadUrl
  }

  private handleUpdateMyProfile() {
    this.$confirm(this.$t('AbpAccount.ManageYourProfile').toString(),
      this.$t('AbpAccount.AreYouSure').toString(), {
        callback: (action) => {
          if (action === 'confirm') {
            const updateProfile = new UpdateMyProfile(
              this.myProfile.name,
              this.myProfile.email,
              this.myProfile.userName,
              this.myProfile.surname,
              this.myProfile.phoneNumber
            )
            updateProfile.extraProperties = this.myProfile.extraProperties
            MyProfileService.updateMyProfile(updateProfile).then(profile => {
              this.myProfile = profile
              this.$message.success(this.$t('AbpAccount.PersonalSettingsSaved').toString())
            })
          }
        }
      })
  }
}
</script>

<style lang="scss" scoped>
.box-center {
  margin: 0 auto;
  display: table;
}
.user-avatar {
  .box-center {
    padding-top: 10px;
  }
}
</style>
