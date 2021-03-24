<template>
  <div class="app-container">
    <div class="filter-container">
      <div class="fs-filter">
        <el-select
          v-model="bucket"
          class="fs-bucket"
          @change="onBucketChanged"
        >
          <el-option
            v-for="b in buckets"
            :key="b.name"
            :label="b.name"
            :value="b.name"
          />
        </el-select>
        <el-breadcrumb
          ref="fileSystemBreadCrumb"
          separator-class="el-icon-arrow-right"
          class="fs-breadcrumb"
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
    </div>

    <el-table
      id="ossObjectTable"
      ref="ossObjectTable"
      v-loading="objectLoading"
      v-el-table-lazy-load="handleGetObjects"
      row-key="name"
      :data="objects"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      height="800"
      @row-click="onRowClick"
      @row-dblclick="onRowDoubleClick"
      @contextmenu.native="onContextMenu"
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
            v-if="row.isFolder"
            name="folder"
            class="folder-icon"
          />
          <svg-icon
            v-else
            name="file"
            class="file-icon"
          />
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.creationTime')"
        prop="creationDate"
        sortable
        width="200"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.creationDate | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.lastModificationTime')"
        prop="lastModifiedDate"
        sortable
        width="200"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.lastModifiedDate | dateTimeFilter }}</span>
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
          <span>{{ row.isFolder ? $t('fileSystem.folder') : '标准存储' }}</span>
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
        v-permission="['AbpOssManagement.OssObject']"
        :label="$t('global.operaActions')"
        align="center"
        width="200"
        min-width="200"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="success"
            icon="el-icon-tickets"
            @click="handleShowOssObject(row)"
          />
          <el-button
            v-permission="['AbpOssManagement.OssObject.Delete']"
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="handleDeleteOssObject(row)"
          />
          <el-button
            v-permission="['AbpOssManagement.OssObject.Download']"
            :disabled="row.isFolder"
            size="mini"
            type="info"
            icon="el-icon-download"
            @click="handleDownloadOssObject(row)"
          />
        </template>
      </el-table-column>
    </el-table>

    <OssObjectProfile
      :show-dialog="showOssObject"
      :bucket="bucket"
      :name="selectionOssObject.name"
      :path="selectionOssObject.path"
      @closed="showOssObject=false"
    />

    <OssObjectUploadDialog
      ref="ossObjectUploadDialog"
      :show-dialog="showUploadOss"
      :bucket="bucket"
      :path="uploadPath"
      @closed="showUploadOss=false"
      @onFileUploaded="onFileUploaded"
    />
  </div>
</template>

<script lang="ts">
import { Component, Mixins, Vue } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import OssManagerApi, {
  GetOssContainerRequest,
  GetOssObjectRequest,
  OssContainer,
  OssObject
} from '@/api/oss-manager'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import OssObjectProfile from './components/OssObjectProfile.vue'
import OssObjectUploadDialog from './components/OssObjectUploadDialog.vue'

const kbUnit = 1 * 1024
const mbUnit = kbUnit * 1024
const gbUnit = mbUnit * 1024
const $contextmenu = Vue.prototype.$contextmenu

@Component({
  name: 'OssManagement',
  components: {
    OssObjectProfile,
    OssObjectUploadDialog
  },
  methods: {
    checkPermission
  },
  filters: {
    dateTimeFilter(datetime: string) {
      if (!datetime) {
        return ''
      }
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
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
  }
})
export default class OssManagement extends Mixins(LocalizationMiXin) {
  private bucket = ''
  private ossObjectEnd = false
  private objectLoading = false
  private showOssObject = false
  private selectionOssObject = new OssObject()
  private buckets = new Array<OssContainer>()
  private objects = new Array<OssObject>()
  private fileSystemRoot = new Array<string>()

  private showUploadOss = false
  private uploadPath = ''

  private getObjectRequest = new GetOssObjectRequest()
  private getBucketRequest = new GetOssContainerRequest()

  mounted() {
    this.fileSystemRoot.push(this.$t('fileSystem.root').toString())
    this.handleGetBuckets()
  }

  private navigationToFilePath() {
    const fileSystemPathArray = this.fileSystemRoot.slice(1)
    const fileSystemPath = fileSystemPathArray.join('')
    this.getObjectRequest.prefix = fileSystemPath
    this.handleClearObjects()
    this.handleGetObjects()
  }

  private handleGoToLastFolder() {
    if (this.fileSystemRoot.length > 1) {
      this.fileSystemRoot.splice(this.fileSystemRoot.length - 1)
      this.navigationToFilePath()
    }
  }

  private onRowClick(row: any) {
    const table = this.$refs.ossObjectTable as any
    table.toggleRowSelection(row)
  }

  private onRowDoubleClick(row: OssObject) {
    if (row.isFolder) {
      // if (row.name.length > 1 && row.name.endsWith('/')) {
      //   this.fileSystemRoot.push(row.name.substring(0, row.name.length - 1))
      // } else {
      //   this.fileSystemRoot.push(row.name)
      // }
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
      this.handleClearObjects()
      this.handleGetObjects()
    }
  }

  private onBucketChanged(bucket: string) {
    this.bucket = bucket
    this.fileSystemRoot.length = 1
    this.getObjectRequest.prefix = ''
    this.handleClearObjects()
    this.handleGetObjects()
  }

  private handleGetBuckets() {
    if (this.getBucketRequest.skipCount < this.getBucketRequest.maxResultCount) {
      this.handleClearObjects()
      this.bucket = ''
      OssManagerApi
        .getBuckets(this.getBucketRequest)
        .then(result => {
          this.buckets = result.containers
          if (result.containers.length === 0) {
            // 控制已在最后一页
            this.getBucketRequest.skipCount = this.getBucketRequest.maxResultCount
          }
        })
    }
  }

  private handleGetObjects() {
    if (this.bucket && !this.ossObjectEnd) {
      this.objectLoading = true
      this.getObjectRequest.bucket = this.bucket
      OssManagerApi
        .getObjects(this.getObjectRequest)
        .then(result => {
          this.objects = this.objects.concat(result.objects)
          this.getObjectRequest.prefix = result.prefix ?? ''
          this.getObjectRequest.delimiter = result.delimiter ?? ''
          this.getObjectRequest.marker = result.nextMarker ?? ''
          if (!result.nextMarker || result.objects.length === 0) {
            this.ossObjectEnd = true
          }
        })
        .finally(() => {
          this.objectLoading = false
        })
    }
  }

  private handleDeleteOssObject(oss: OssObject) {
    this.$confirm(this.l('global.whetherDeleteData', { name: oss.name }),
      this.l('global.questingDeleteByMessage',
        { message: oss.isFolder ? this.l('fileSystem.folder') : this.l('fileSystem.file') }), {
        callback: (action) => {
          if (action === 'confirm') {
            OssManagerApi
              .deleteObject(this.bucket, oss.name, oss.path)
              .then(() => {
                this.$notify.success(this.l('global.dataHasBeenDeleted', { name: oss.name }))
                this.handleClearObjects()
                this.handleGetObjects()
              })
          }
        }
      })
  }

  private handleShowOssObject(oss: OssObject) {
    this.selectionOssObject = oss
    this.showOssObject = true
  }

  private handleDownloadOssObject(oss: OssObject) {
    if (!oss.isFolder) {
      const link = document.createElement('a')
      link.style.display = 'none'
      link.href = OssManagerApi.generateDownloadUrl(this.bucket, oss.name, oss.path)
      link.setAttribute('download', oss.name)
      document.body.appendChild(link)
      link.click()
    }
  }

  private handleClearObjects() {
    this.objects.length = 0
    this.ossObjectEnd = false
    this.objectLoading = false
  }

  private onContextMenu(event: any) {
    event.preventDefault()
    $contextmenu({
      items: [
        {
          label: '创建容器',
          disabled: !checkPermission(['AbpOssManagement.Container.Create']),
          onClick: () => {
            this.$prompt(this.$t('global.pleaseInputBy', { key: this.$t('fileSystem.name') }).toString(),
              '创建容器', {
                showInput: true,
                inputValidator: (val) => {
                  return !(!val || val.length === 0)
                },
                inputErrorMessage: '名称必须输入',
                inputPlaceholder: this.$t('global.pleaseInputBy', { key: this.$t('fileSystem.name') }).toString()
              }).then((val: any) => {
              const name = val.value + '/'
              OssManagerApi
                .createBucket(name.replace('//', '/'))
                .then(() => {
                  this.$message.success(this.l('successful'))
                  this.handleGetBuckets()
                  this.handleGetObjects()
                })
            }).catch(_ => _)
          }
        },
        {
          label: this.$t('fileSystem.addFolder'),
          disabled: !checkPermission(['AbpOssManagement.OssObject.Create']),
          onClick: () => {
            this.$prompt(this.$t('global.pleaseInputBy', { key: this.$t('fileSystem.name') }).toString(),
              this.$t('fileSystem.addFolder').toString(), {
                showInput: true,
                inputValidator: (val) => {
                  return !(!val || val.length === 0)
                },
                inputErrorMessage: this.$t('fileSystem.folderNameIsRequired').toString(),
                inputPlaceholder: this.$t('global.pleaseInputBy', { key: this.$t('fileSystem.name') }).toString()
              }).then((val: any) => {
              const name = val.value + '/'
              OssManagerApi
                .createFolder(this.bucket, name.replace('//', '/'), this.getCurrentPath())
                .then(res => {
                  this.$message.success(this.$t('fileSystem.folderCreateSuccess', { name: res.name }).toString())
                  this.handleClearObjects()
                  this.handleGetObjects()
                })
            }).catch(_ => _)
          },
          divided: true
        },
        {
          label: this.$t('fileSystem.upload'),
          disabled: !checkPermission(['AbpOssManagement.OssObject.Create']),
          onClick: () => {
            this.uploadPath = this.getCurrentPath()
            this.showUploadOss = true
          },
          divided: true
        },
        {
          label: this.$t('fileSystem.bacthDelete'),
          disabled: !checkPermission(['AbpOssManagement.OssObject.Delete']),
          onClick: () => {
            // 未公布批量删除接口
            const table = this.$refs.ossObjectTable as any
            console.log(table)
            OssManagerApi
              .bulkDeleteObject(this.bucket, table.selection.map((x: any) => x.name), this.getCurrentPath())
              .then(() => {
                this.$message.success(this.l('successful'))
                this.handleClearObjects()
                this.handleGetObjects()
              })
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

  private onFileUploaded() {
    this.$message.success(this.l('fileSystem.uploadSuccess'))
    this.handleClearObjects()
    this.handleGetObjects()
  }

  private getCurrentPath() {
    let path = ''
    if (this.fileSystemRoot.length > 1) {
      path = this.fileSystemRoot.slice(1).join('')
    }
    return path
  }
}
</script>

<style lang="scss">
.el-table .folder-row {
  cursor: pointer;
  background-color:rgb(245, 235, 226);
}
.el-table .file-row {
  cursor: pointer;
  background-color: rgb(210, 219, 235);
}
.fs-filter {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: stretch;
  .fs-bucket {
    width: 300px;
  }
  .fs-breadcrumb {
    padding-left: 10px;
    padding-top: 10px;
  }
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
