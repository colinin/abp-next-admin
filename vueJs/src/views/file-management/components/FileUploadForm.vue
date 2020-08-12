<template>
  <div>
    <uploader
      ref="uploader"
      :options="options"
      :auto-start="false"
      class="uploader-pane"
      @file-added="onFileAdded"
      @file-error="onFileError"
      @file-complete="onFileUploadCompleted"
    >
      <uploader-unsupport />
      <uploader-drop>
        <uploader-btn :attrs="attrs">
          {{ $t('fileSystem.addFile') }}
        </uploader-btn>
      </uploader-drop>
      <uploader-list />
    </uploader>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import FileManagementAppService, { FileUploadUrl } from '@/api/filemanagement'

export class UploadOptions {
  target!: string
  chunkSize!: number
  testChunks!: boolean
  fileParameterName!: string
  maxChunkRetries!: number
  headers?: any
  query?: any
  permanentErrors?: number[]
  successStatuses?: number[]

  constructor() {
    this.query = {}
    this.headers = {}
    this.testChunks = false
    this.successStatuses = new Array<number>()
    this.permanentErrors = new Array<number>()
  }
}

@Component({
  name: 'FileUploadForm'
})
export default class extends Vue {
  @Prop({ default: () => new UploadOptions() })
  private options!: UploadOptions

  @Prop({ default: '' })
  private path!: string

  private attrs!: {[key: string]: any}
  private files?: any = {}

  constructor() {
    super()
    this.attrs = {
      accept: ['image/*', 'document/*', 'video/*', 'audio/*']
    }
    this.options.target = FileUploadUrl
    this.options.successStatuses = [200, 201, 202, 204, 205]
    this.options.permanentErrors = [400, 401, 403, 404, 415, 500, 501]
  }

  public close() {
    this.files = {}
    const uploadControl = this.$refs.uploader as any
    uploadControl.files = []
    uploadControl.fileList = []
    uploadControl.uploader.cancel()
  }

  @Watch('path', { immediate: true })
  private onPathChanged() {
    this.options.query.path = this.path
  }

  private onFileAdded(file: any) {
    this.files[file.name] = file.chunkSize
    file.uploader.fileStatusText.error = this.$t('fileSystem.uploadError')
    file.uploader.fileStatusText.paused = this.$t('fileSystem.paused')
    file.uploader.fileStatusText.success = this.$t('fileSystem.uploadSuccess')
    file.uploader.fileStatusText.waiting = this.$t('fileSystem.waitingUpload')
    file.uploader.fileStatusText.uploading = this.$t('fileSystem.uploading')
  }

  private onFileError(rootFile: any, file: any, message: any) {
    const abpResponse = JSON.parse(message)
    this.$message.error(abpResponse.error.message)
  }

  private onFileUploadCompleted(file: any) {
    console.log(file)
    this.$emit('onFileUploaded')
  }
}
</script>

<style lang="scss" scoped>

</style>
