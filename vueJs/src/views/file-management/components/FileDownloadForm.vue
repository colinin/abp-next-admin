<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('fileSystem.downloadTask')"
    @close="onFormClosed"
  >
    <el-table
      :data="files"
      stripe
      style="width: 100%"
    >
      <el-table-column
        prop="name"
        :label="$t('fileSystem.name')"
        width="180"
      />
      <el-table-column
        prop="size"
        :label="$t('fileSystem.size')"
        width="180"
      />
      <el-table-column
        :label="$t('fileSystem.progress')"
        width="180"
      >
        <template slot-scope="scope">
          <el-progress
            :text-inside="true"
            :stroke-width="26"
            :status="downloadStatus(scope.row)"
            :percentage="downloadProgress(scope.row)"
          />
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.operaActions')"
        align="center"
        width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="success"
            icon="el-icon-caret-right"
            @click="handleDownloadFile(row)"
          >
            {{ $t('fileSystem.begin') }}
          </el-button>
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleRemoveFile(row)"
          >
            {{ $t('fileSystem.remove') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import FileSystemService from '@/api/filemanagement'

export class FileInfo {
  name!: string
  path!: string
  size!: number
  progress!: number
  pause!: boolean
  blobs = new Array<BlobPart>()
  type!: string
}

@Component({
  name: 'FileDownloadForm'
})
export default class FileDownloadForm extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private files!: FileInfo[]

  get downloadProgress() {
    return (fileInfo: FileInfo) => {
      return Math.round(fileInfo.progress / fileInfo.size * 10000) / 100
    }
  }

  get downloadStatus() {
    return (fileInfo: FileInfo) => {
      const progress = Math.round(fileInfo.progress / fileInfo.size * 10000) / 100
      if (progress >= 50 && progress < 100) {
        return 'warning'
      }
      if (progress >= 100) {
        return 'success'
      }
    }
  }

  private handleRemoveFile(fileInfo: FileInfo) {
    fileInfo.pause = true
    fileInfo.blobs.length = 0
    this.$emit('onFileRemoved', fileInfo)
  }

  private handleDownloadFile(fileInfo: FileInfo) {
    fileInfo.pause = false
    if (fileInfo.progress >= 100) {
      this.downloadBlob(fileInfo)
    } else {
      this.downlodFile(fileInfo, (downloadSize: number) => {
        if (downloadSize >= fileInfo.size) {
          this.$emit('onFileDownloaded', fileInfo)
        }
      })
    }
  }

  private downlodFile(fileInfo: FileInfo, callback: Function) {
    if (fileInfo.pause) {
      return
    }
    FileSystemService.downlodFle(fileInfo.name, fileInfo.path, fileInfo.progress).then((res: any) => {
      fileInfo.type = res.headers['content-type']
      // 获取当前下载字节大小
      const downloadByte = res.data.size
      // 当前下载分块入栈
      fileInfo.blobs.push(res.data)
      fileInfo.progress += downloadByte
      if (fileInfo.size > fileInfo.progress) {
        this.downlodFile(fileInfo, callback)
      } else {
        // 合并下载文件
        this.downloadBlob(fileInfo)
        // 下载完成后的回调
        callback(fileInfo.size)
      }
    }).catch(() => {
      callback(fileInfo.size)
    })
  }

  private downloadBlob(fileInfo: FileInfo) {
    const url = window.URL.createObjectURL(new Blob(fileInfo.blobs, { type: fileInfo.type }))
    const link = document.createElement('a')
    link.style.display = 'none'
    link.href = url
    link.setAttribute('download', fileInfo.name)
    document.body.appendChild(link)
    link.click()
    URL.revokeObjectURL(url)
  }

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
