<template>
  <div class="app-container">
    <div class="filter-container">
      <el-breadcrumb
        ref="fileSystemBreadCrumb"
        separator-class="el-icon-arrow-right"
      >
        <el-breadcrumb-item
          v-for="(fileRoot, index) in fileSystemRoot"
          :key="index"
          class="file-system-breadcrumb"
          @click.native="(event) => onBreadCrumbClick(event, index)"
        >
          {{ fileRoot }}
        </el-breadcrumb-item>
      </el-breadcrumb>
    </div>

    <el-table
      ref="fileSystemTable"
      v-loading="dataLoading"
      row-key="name"
      :data="dataList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @row-click="onRowClick"
      @row-dblclick="onRowDoubleClick"
      @contextmenu.native="onContextMenu"
      @sort-change="handleSortChange"
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
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
    />

    <el-dialog
      :visible.sync="showFileUploadDialog"
      :title="$t('fileSystem.upload')"
      :show-close="false"
      @closed="onFileUploadFormClosed"
    >
      <file-upload-form
        ref="fileUploadForm"
        :path="lastFilePath"
        @onFileUploaded="onFileUploaded"
      />
    </el-dialog>

    <file-download-form
      :show-dialog="showDownloadDialog"
      :files="downloadFiles"
      @closed="showDownloadDialog=false"
      @onFileRemoved="onFileRemoved"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat } from '@/utils'
import { checkPermission } from '@/utils/permission'
import { Vue } from 'vue-property-decorator'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import FileUploadForm from './components/FileUploadForm.vue'
import FileDownloadForm, { FileInfo } from './components/FileDownloadForm.vue'
import Pagination from '@/components/Pagination/index.vue'
import FileSystemService, { FileSystemGetByPaged, FileSystemType } from '@/api/filemanagement'

const kbUnit = 1 * 1024
const mbUnit = kbUnit * 1024
const gbUnit = mbUnit * 1024
const $contextmenu = Vue.prototype.$contextmenu

@Component({
  name: 'FileManagement',
  components: {
    Pagination,
    FileUploadForm,
    FileDownloadForm
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
export default class extends mixins(DataListMiXin) {
  private showFileUploadDialog = false
  private downloading = false
  private lastFilePath = ''
  private fileSystemRoot = new Array<string>()

  private showDownloadDialog = false
  private downloadFiles = new Array<FileInfo>()

  public dataFilter = new FileSystemGetByPaged()

  mounted() {
    this.fileSystemRoot.push(this.$t('fileSystem.root').toString())
    this.refreshPagedData()
  }

  protected getPagedList(filter: any) {
    return FileSystemService.getFileSystemList(filter)
  }

  private navigationToFilePath() {
    const fileSystemPathArray = this.fileSystemRoot.slice(1)
    const fileSystemPath = fileSystemPathArray.join('/')
    this.dataFilter.parent = fileSystemPath
    this.refreshPagedData()
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
                this.refreshPagedData()
              })
            } else {
              FileSystemService.deleteFile(row.parent, row.name).then(() => {
                this.$notify.success(this.l('global.dataHasBeenDeleted', { name: row.name }))
                this.refreshPagedData()
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
          let path = command.row.name
          if (command.row.parent) {
            path = command.row.parent + '/' + path
          }
          this.handleUploadFile(path)
        } else {
          this.handleUploadFile(command.row.parent)
        }
        break
    }
  }

  private handleUploadFile(path: string) {
    this.lastFilePath = path
    this.showFileUploadDialog = true
  }

  private handleDownloadFile(path: string, fileName: string, size: number) {
    if (!this.downloadFiles.some(x => x.name === fileName && x.path === path)) {
      const file = new FileInfo()
      file.name = fileName
      file.path = path
      file.size = size
      file.progress = 0
      this.downloadFiles.push(file)
    }
    this.showDownloadDialog = true
  }

  private onFileRemoved(fileInfo: FileInfo) {
    const index = this.downloadFiles.findIndex(x => x.path === fileInfo.path && x.name === fileInfo.name)
    this.downloadFiles.splice(index, 1)
  }

  private onRowClick(row: any) {
    if (row.type === FileSystemType.Folder) {
      let path = row.name
      if (row.parent) {
        path = row.parent + '/' + row.name
      }
      this.lastFilePath = path
    }
    const table = this.$refs.fileSystemTable as any
    table.toggleRowSelection(row)
  }

  private onRowDoubleClick(row: any) {
    if (row.type === FileSystemType.Folder) {
      this.fileSystemRoot.push(row.name)
      this.navigationToFilePath()
    }
  }

  private onBreadCrumbClick(event: any, index: number) {
    // 如果点击的索引为最后一个,不做响应
    if ((index + 1) < this.fileSystemRoot.length) {
      this.fileSystemRoot.splice(index + 1)
      this.navigationToFilePath()
    } else {
      this.refreshPagedData()
    }
  }

  private onFileUploaded() {
    this.refreshPagedData()
  }

  private onFileUploadFormClosed() {
    this.showFileUploadDialog = false
    const frmUpload = this.$refs.fileUploadForm as any
    frmUpload.close()
  }

  private onContextMenu(event: any) {
    event.preventDefault()
    $contextmenu({
      items: [
        {
          label: this.$t('fileSystem.addFolder'),
          disabled: !checkPermission(['AbpFileManagement.FileSystem.Create']),
          onClick: () => {
            let parent = ''
            // 在根目录下
            if (this.fileSystemRoot.length > 1) {
              parent = this.fileSystemRoot.slice(1).join('/')
            }
            this.$prompt(this.$t('global.pleaseInputBy', { key: this.$t('fileSystem.name') }).toString(),
              this.$t('fileSystem.addFolder').toString(), {
                showInput: true,
                inputValidator: (val) => {
                  return !(!val || val.length === 0)
                },
                inputErrorMessage: this.$t('fileSystem.folderNameIsRequired').toString(),
                inputPlaceholder: this.$t('global.pleaseInputBy', { key: this.$t('fileSystem.name') }).toString()
              }).then((val: any) => {
              FileSystemService.createFolder(val.value, parent).then(() => {
                this.$message.success(this.$t('fileSystem.folderCreateSuccess', { name: val.value }).toString())
                this.refreshPagedData()
              })
            }).catch(_ => _)
          },
          divided: true
        },
        {
          label: this.$t('fileSystem.upload'),
          disabled: !checkPermission(['AbpFileManagement.FileSystem.FileManager.Create']),
          onClick: () => {
            let path = ''
            if (this.fileSystemRoot.length > 1) {
              path = this.fileSystemRoot.slice(1).join('/')
            }
            this.lastFilePath = path
            this.showFileUploadDialog = true
          },
          divided: true
        },
        {
          label: this.$t('fileSystem.bacthDownload'),
          disabled: !checkPermission(['AbpFileManagement.FileSystem.FileManager.Download']),
          onClick: () => {
            const table = this.$refs.fileSystemTable as any
            // 过滤未添加到下载列表的文件
            const selectFiles = table.selection.filter((x: any) => x.type === 1 && !this.downloadFiles.some(f => f.name === x.name && f.path === x.path))
            // 格式化文件列表别添加到下载列表
            this.downloadFiles.push(...selectFiles.map((f: any) => {
              const file = new FileInfo()
              file.name = f.name
              file.path = f.parent
              file.size = f.size
              file.progress = 0
              return file
            }))
            // 显示下载列表
            this.showDownloadDialog = true
          }
        },
        {
          label: this.$t('fileSystem.bacthDelete'),
          disabled: true, // !checkPermission(['AbpFileManagement.FileSystem.FileManager.Delete']),
          onClick: () => {
            // 未公布批量删除接口
            // const table = this.$refs.fileSystemTable as any
            // 过滤类型为文件的选中项
            // const selectFiles = table.selection.filter((x: any) => x.type === 1)
          }
        }
      ],
      event,
      customClass: 'context-menu',
      zIndex: 3,
      minWidth: 150
    })
    return false
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
