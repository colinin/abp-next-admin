<template>
  <el-dialog
    width="800px"
    title="Oss对象"
    :visible="showDialog"
    custom-class="modal-form"
    @close="onFormClosed"
  >
    <div class="app-container">
      <el-form
        ref="formOssObject"
        label-width="130px"
      >
        <el-form-item label-width="0px">
          <el-carousel
            :interval="5000"
            arrow="always"
            class="oss-preview-div"
          >
            <el-carousel-item>
              <el-image
                :src="ossPreviewUrl"
                fit="contain"
                class="oss-preview-img"
              >
                <div
                  slot="error"
                  class="image-slot"
                >
                  <el-alert
                    title="当前格式不支持预览"
                    type="warning"
                    center
                    show-icon
                    :closable="false"
                  />
                </div>
              </el-image>
            </el-carousel-item>
          </el-carousel>
        </el-form-item>
        <el-form-item
          label="名称"
        >
          <el-input
            :value="oss.name"
            readonly
          />
        </el-form-item>
        <el-form-item
          label="路径"
        >
          <el-input
            :value="oss.path"
            readonly
          />
        </el-form-item>
        <el-form-item
          label="大小"
        >
          <el-input
            :value="oss.size"
            readonly
          />
        </el-form-item>
        <el-form-item
          label="创建时间"
        >
          <el-input
            :value="oss.creationDate | dateTimeFilter"
            readonly
          />
        </el-form-item>
      </el-form>
    </div>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Prop, Watch, Vue } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import OssManagerApi, { OssObject } from '@/api/oss-manager'

// 暂时支持预览的文件
const supportFileTypes = ['jpg', 'png', 'gif', 'bmp', 'jpeg']

@Component({
  name: 'OssObjectProfile',
  filters: {
    dateTimeFilter(datetime: string) {
      if (datetime) {
        const date = new Date(datetime)
        return dateFormat(date, 'YYYY-mm-dd HH:MM:SS')
      }
      return ''
    }
  }
})
export default class OssObjectProfile extends Vue {
  @Prop({ default: '' })
  private name!: string

  @Prop({ default: '' })
  private bucket!: string

  @Prop({ default: '' })
  private path?: string

  @Prop({ default: false })
  private showDialog!: string

  private notSupport = false
  private ossPreviewUrl = ''
  private oss = new OssObject()

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetOssObject()
  }

  private handleGetOssObject() {
    if (this.name && this.showDialog) {
      this.ossPreviewUrl = ''
      OssManagerApi
        .getObject(this.bucket, this.name, this.path)
        .then(res => {
          this.oss = res
          this.handleGetOssObjectBase64()
        })
    }
  }

  private handleGetOssObjectBase64() {
    if (this.oss.name &&
        supportFileTypes.some(x => this.oss.name.toLowerCase().endsWith(x))) { // 仅下载支持预览的文件
      OssManagerApi
        .getObjectData(this.bucket, this.oss.name, this.oss.path)
        .then(res => {
          const reader = new FileReader()
          reader.onload = (e) => {
            if (e.target?.result) {
              this.ossPreviewUrl = e.target.result.toString()
            }
          }
          reader.readAsDataURL(res)
        })
    }
  }

  private onFormClosed() {
    this.ossPreviewUrl = ''
    this.oss = new OssObject()
    this.$emit('closed')
  }
}
</script>

<style scoped>
.oss-preview-img {
  width: 100%;
  height: 100%;
}
</style>
