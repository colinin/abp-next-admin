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
          @click.native="handleBreadCrumbClick"
        >
          {{ fileRoot }}
        </el-breadcrumb-item>
      </el-breadcrumb>
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
      @row-click="handleRowClick"
      @row-dblclick="handleRowDoubleClick"
    >
      <el-table-column
        type="selection"
        width="50px"
        align="center"
      />
      <el-table-column
        :label="$t('fileSystem.name')"
        prop="name"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('fileSystem.creationTime')"
        prop="creationTime"
        sortable
        width="150px"
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
        width="150px"
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
        width="150px"
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
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.size | fileSystemSizeFilter }}</span>
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
        return gb + 'GB'
      }
      if (size > mbUnit) {
        let mb = Math.round(size / mbUnit)
        if (mb < 1) {
          mb = 1
        }
        return mb + 'MB'
      }
      let kb = Math.round(size / kbUnit)
      if (kb < 1) {
        kb = 1
      }
      return kb + 'KB'
    }
  }
})
export default class extends Vue {
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
    this.fileSystemListLoading = false
    this.fileSystemRoot = new Array<string>()
    this.fileSystemList = new Array<FileSystem>()
    this.fileSystemGetFilter = new FileSystemGetByPaged()
  }

  mounted() {
    console.log(this.$route.params)
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

  private handleRowClick(row: any) {
    console.log(row)
    const table = this.$refs.fileSystemTable as any
    table.setCurrentRow(row)
  }

  private handleRowDoubleClick(row: any) {
    if (row.type === FileSystemType.Folder) {
      this.fileSystemRoot.push(row.name)
      this.navigationToFilePath()
    }
  }

  private handleBreadCrumbClick(event: any) {
    const nodeIndex = this.fileSystemRoot.findIndex(f => f === event.target.textContent)
    this.fileSystemRoot.splice(nodeIndex)
    this.navigationToFilePath()
  }

  private navigationToFilePath() {
    const fileSystemPathArray = this.fileSystemRoot.slice(1)
    const fileSystemPath = fileSystemPathArray.join('\\')
    console.log(fileSystemPath)
    this.fileSystemGetFilter.parent = fileSystemPath
    this.handleGetFileSystemList()
  }
}
</script>

<style lang="scss">
.file-system-breadcrumb .el-breadcrumb__inner {
  color: rgb(34, 34, 173);
  cursor: pointer;
}
</style>
