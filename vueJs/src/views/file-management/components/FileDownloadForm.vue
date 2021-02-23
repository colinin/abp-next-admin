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
            :disabled="row.downloading"
            @click="handleDownload(row)"
          />
          <el-button
            size="mini"
            type="info"
            :disabled="row.pause"
            @click="handlePause(row)"
          >
            <svg-icon name="pause" />
          </el-button>
          <el-button
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleRemove(row)"
          />
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
  pause = true
  blobs = new Array<BlobPart>()
  type!: string
  downloading!: boolean
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

  private handlePause(fileInfo: FileInfo) {
    fileInfo.pause = true
    fileInfo.downloading = false
    this.$emit('onFilePaused', fileInfo)
  }

  private handleRemove(fileInfo: FileInfo) {
    fileInfo.pause = true
    fileInfo.downloading = false
    fileInfo.blobs.length = 0
    this.$emit('onFileRemoved', fileInfo)
  }

  private handleDownload(fileInfo: FileInfo) {
    if (!fileInfo.downloading) {
      fileInfo.pause = false
      fileInfo.downloading = true
      if (this.downloadProgress(fileInfo) >= 100) {
        this.downloadBlob(fileInfo)
      } else {
        this.downlodFile(fileInfo)
      }
    }
  }

  private downlodFile(fileInfo: FileInfo) {
    if (fileInfo.pause) {
      return
    }
    FileSystemService
      .downlodFle(fileInfo.name, fileInfo.path, fileInfo.progress)
      .then((res: any) => {
        fileInfo.type = res.headers['content-type']
        // 获取当前下载字节大小
        const downloadByte = res.data.size
        // 当前下载分块入栈
        fileInfo.blobs.push(res.data)
        fileInfo.progress += downloadByte
        if (fileInfo.size > fileInfo.progress) {
          this.downlodFile(fileInfo)
        } else {
          // 合并下载文件
          this.downloadBlob(fileInfo)
        }
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

    fileInfo.pause = true
    fileInfo.downloading = false
    this.$emit('onFileDownloaded', fileInfo)
  }

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
