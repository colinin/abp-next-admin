<template>
  <div class="app-container">
    <div class="filter-container">
      <el-page-header
        @back="handleGoToLastFolder"
      >
        <template
          slot="content"
          class="file-system-page"
        >
          <el-breadcrumb
            ref="fileSystemBreadCrumb"
            separator-class="el-icon-arrow-right"
          >
            <el-breadcrumb-item
              v-for="(fileRoot, index) in fileSystemRoot"
              :key="index"
              class="file-system-breadcrumb"
              @click.native="(event) => handleBreadCrumbClick(event, index)"
            >
              {{ fileRoot }}
            </el-breadcrumb-item>
          </el-breadcrumb>
        </template>
      </el-page-header>
    </div>

    <el-table
      ref="fileSystemTable"
      v-loading="fileSystemListLoading"
      row-key="name"
      :data="fileSystemList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      :row-class-name="tableRowClassName"
      @selection-change="handleSelectionChanged"
      @row-click="handleRowClick"
      @row-dblclick="handleRowDoubleClick"
    >
      <el-table-column
        type="selection"
        width="50"
        align="center"
      />
      <el-table-column
        :label="$t('fileSystem.name')"
        prop="name"
        sortable
        width="350"
        align="left"
      >
        <template slot-scope="{row}">
          <svg-icon
            :name="row.type | fileSystemTypeIconFilter"
            :class="row.type | fileSystemTypeIconClassFilter"
          />
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.creationTime')"
        prop="creationTime"
        sortable
        width="200"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.creationTime | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.lastModificationTime')"
        prop="lastModificationTime"
        sortable
        width="200"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.lastModificationTime | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.type')"
        prop="type"
        sortable
        width="150"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.type === 0 ? $t('fileSystem.folder') : $t('fileSystem.fileType', {exten: row.extension}) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.size')"
        prop="size"
        sortable
        width="200"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.size | fileSystemSizeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.operaActions')"
        align="center"
        width="250"
        min-width="250"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="row.type === 0 ? !checkPermission(['AbpFileManagement.FileSystem.Delete']) : !checkPermission(['AbpFileManagement.FileSystem.FileManager.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteFolderOrFile(row)"
          >
            {{ row.type === 0 ? $t('fileSystem.deleteFolder') : $t('fileSystem.deleteFile') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleFileSystemCommand"
          >
            <el-button
              v-permission="['AbpFileManagement.FileSystem']"
              size="mini"
              type="info"
            >
              {{ $t('global.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-permission="['AbpFileManagement.FileSystem.FileManager.Create']"
                :command="{key: 'upload', row}"
              >
                {{ $t('fileSystem.upload') }}
              </el-dropdown-item>
              <el-dropdown-item
                v-permission="['AbpFileManagement.FileSystem.FileManager.Download']"
                divided
                :command="{key: 'download', row}"
                :disabled="downloading || row.type === 0"
              >
                {{ $t('fileSystem.download') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="fileSystemCount>0"
      :total="fileSystemCount"
      :page.sync="fileSystemGetFilter.skipCount"
      :limit.sync="fileSystemGetFilter.maxResultCount"
      @pagination="handleGetFileSystemList"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat } from '@/utils'
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import FileSystemService, { FileSystem, FileSystemGetByPaged, FileSystemType } from '@/api/filemanagement'

const kbUnit = 1 * 1024
const mbUnit = kbUnit * 1024
const gbUnit = mbUnit * 1024

@Component({
  name: 'FileManagement',
  components: {
    Pagination
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    },
    fileSystemTypeIconFilter(type: FileSystemType) {
      if (type === FileSystemType.Folder) {
        return 'folder'
      }
      return 'file'
    },
    fileSystemTypeIconClassFilter(type: FileSystemType) {
      if (type === FileSystemType.Folder) {
        return 'folder-icon'
      }
      return 'file-icon'
    },
    fileSystemTypeFilter(type: FileSystemType) {
      if (type === FileSystemType.Folder) {
        return 'fileSystem.folder'
      }
      return 'fileSystem.fileType'
    },
    fileSystemSizeFilter(size: number) {
      if (size > gbUnit) {
        let gb = Math.round(size / gbUnit)
        if (gb < 1) {
          gb = 1
        }
        return gb + ' GB'
      }
      if (size > mbUnit) {
        let mb = Math.round(size / mbUnit)
        if (mb < 1) {
          mb = 1
        }
        return mb + ' MB'
      }
      let kb = Math.round(size / kbUnit)
      if (kb < 1) {
        kb = 1
      }
      return kb + ' KB'
    }
  },
  methods: {
    checkPermission,
    tableRowClassName(row: any) {
      if (row.row.type === 0) {
        return 'folder-row'
      }
      return 'file-row'
    }
  }
})
export default class extends Vue {
  private downloading!: boolean
  private lastFilePath!: string
  private fileSystemRoot!: string[]
  private fileSystemList?: FileSystem[]
  private fileSystemCount!: number
  private fileSystemListLoading!: boolean
  private fileSystemGetFilter!: FileSystemGetByPaged

  constructor() {
    super()
    this.lastFilePath = ''
    this.fileSystemCount = 0
    this.downloading = false
    this.fileSystemListLoading = false
    this.fileSystemRoot = new Array<string>()
    this.fileSystemList = new Array<FileSystem>()
    this.fileSystemGetFilter = new FileSystemGetByPaged()
  }

  mounted() {
    this.fileSystemRoot.push(this.$t('fileSystem.root').toString())
    this.handleGetFileSystemList()
  }

  private handleGetFileSystemList() {
    this.fileSystemListLoading = true
    FileSystemService.getFileSystemList(this.fileSystemGetFilter).then(res => {
      this.fileSystemCount = res.totalCount
      this.fileSystemList = res.items
    }).finally(() => {
      this.fileSystemListLoading = false
    })
  }

  private handleSelectionChanged(selection: any, row: any) {
    console.log(row)
  }

  private handleRowClick(row: any) {
    const table = this.$refs.fileSystemTable as any
    table.toggleRowSelection(row)
  }

  private handleRowDoubleClick(row: any) {
    if (row.type === FileSystemType.Folder) {
      this.fileSystemRoot.push(row.name)
      this.navigationToFilePath()
    }
  }

  private handleBreadCrumbClick(event: any, index: number) {
    // 如果点击的索引为最后一个,不做响应
    if ((index + 1) < this.fileSystemRoot.length) {
      this.fileSystemRoot.splice(index + 1)
      this.navigationToFilePath()
    }
  }

  private navigationToFilePath() {
    const fileSystemPathArray = this.fileSystemRoot.slice(1)
    const fileSystemPath = fileSystemPathArray.join('/')
    this.fileSystemGetFilter.parent = fileSystemPath
    this.handleGetFileSystemList()
  }

  private handleGoToLastFolder() {
    if (this.fileSystemRoot.length > 1) {
      this.fileSystemRoot.splice(this.fileSystemRoot.length - 1)
      this.navigationToFilePath()
    }
  }

  private handleDeleteFolderOrFile(row: any) {
    this.$confirm(this.l('global.whetherDeleteData', { name: row.name }),
      this.l('global.questingDeleteByMessage',
        { message: row.type === 0 ? this.l('fileSystem.folder') : this.l('fileSystem.file') }), {
        callback: (action) => {
          if (action === 'confirm') {
            if (row.type === 0) {
              let path = row.name
              if (row.parent) {
                path = row.parent + '/' + row.name
              }
              FileSystemService.deleteFolder(path).then(() => {
                this.$notify.success(this.l('global.dataHasBeenDeleted', { name: row.name }))
                this.handleGetFileSystemList()
              })
            } else {
              FileSystemService.deleteFile(row.parent, row.name).then(() => {
                this.$notify.success(this.l('global.dataHasBeenDeleted', { name: row.name }))
                this.handleGetFileSystemList()
              })
            }
          }
        }
      })
  }

  private handleFileSystemCommand(command: { key: string, row: any}) {
    switch (command.key) {
      case 'download':
        this.handleDownloadFile(command.row.parent, command.row.name, command.row.size)
        break
      case 'upload':
        if (command.row.type === 0) {
          this.handleUploadFile(command.row.name)
        } else {
          this.handleUploadFile(command.row.parent)
        }
        break
    }
  }

  private handleUploadFile(path: string) {
    console.log(path)
    // TODO: 创建文件上传组件
  }

  private handleDownloadFile(path: string, fileName: string, size: number) {
    this.downloading = true
    const blobs = new Array<BlobPart>()
    this.downlodFile(path, fileName, size, 0, blobs,
      (downloadSize: number) => {
        if (downloadSize >= size) {
          this.downloading = false
          // 释放空间
          blobs.length = 0
        }
      })
  }

  private downlodFile(path: string, fileName: string, size: number, currentByte: number, blobs: BlobPart[], callback: Function) {
    FileSystemService.downlodFle(fileName, path, currentByte).then((res: any) => {
      // 获取当前下载字节大小
      const downloadByte = res.data.size
      // 当前下载分块入栈
      blobs.push(res.data)
      currentByte += downloadByte
      if (size > currentByte) {
        this.downlodFile(path, fileName, size, currentByte, blobs, callback)
      } else {
        // 合并下载文件
        const url = window.URL.createObjectURL(new Blob(blobs, { type: res.headers['content-type'] }))
        const link = document.createElement('a')
        link.style.display = 'none'
        link.href = url
        link.setAttribute('download', fileName)
        document.body.appendChild(link)
        link.click()
        // 下载完成后的回调
        callback(size)
      }
    })
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style lang="scss" scoped>
.options {
  vertical-align: top;
  margin-left: 20px;
}
.el-dropdown + .el-dropdown {
  margin-left: 15px;
}
.el-icon-arrow-down {
  font-size: 12px;
}
</style>

<style lang="scss">
.el-table .folder-row {
  cursor: pointer;
  background-color:rgb(245, 235, 226);
}
.el-table .file-row {
  cursor: pointer;
  background-color: rgb(210, 219, 235);
}
.file-system-breadcrumb .el-breadcrumb__inner {
  color: rgb(34, 34, 173);
  cursor: pointer;
}
.file-icon {
  margin-left: 0px;
  margin-right: 5px;
  margin-top: 0px;
  margin-bottom: 0px;
  color: rgb(55, 189, 189);
}
.folder-icon {
  margin-left: 0px;
  margin-right: 5px;
  margin-top: 0px;
  margin-bottom: 0px;
  color: rgb(235, 130, 33);
}
</style>
