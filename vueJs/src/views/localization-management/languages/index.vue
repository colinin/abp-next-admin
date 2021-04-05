<template>
  <div class="app-container">
    <div class="filter-container">
      <el-card>
        <div style="margin-top: 15px;">
          <el-row style="width: 100%;">
            <el-col :span="20">
              <el-input
                v-model="dataFilter.filter"
                :placeholder="$t('LocalizationManagement.SearchFilter')"
              >
                <el-button
                  slot="append"
                  icon="el-icon-search"
                  @click="refreshPagedData"
                />
              </el-input>
            </el-col>
            <el-col
              :span="4"
              style="text-align: right;"
            >
              <el-button
                class="create-new"
                type="success"
                @click="handleCreate"
              >
                <i class="ivu-icon ivu-icon-md-add" />
                {{ $t('LocalizationManagement.Language:AddNew') }}
              </el-button>
            </el-col>
          </el-row>
        </div>
      </el-card>
    </div>

    <el-card>
      <el-table
        v-loading="dataLoading"
        row-key="id"
        :data="dataList"
        border
        fit
        highlight-current-row
        style="width: 100%;"
        @sort-change="handleSortChange"
      >
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:Enable')"
          prop="enable"
          sortable
          width="100px"
          align="center"
        >
          <template slot-scope="{row}">
            <el-switch
              v-model="row.enable"
              disabled
              active-color="#13ce66"
              inactive-color="#ff4949"
            />
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:CultureName')"
          prop="cultureName"
          sortable
          width="150px"
        >
          <template slot-scope="{row}">
            <span>{{ row.cultureName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:UiCultureName')"
          prop="uiCultureName"
          sortable
          width="150px"
        >
          <template slot-scope="{row}">
            <span>{{ row.uiCultureName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:DisplayName')"
          prop="displayName"
          sortable
          width="200px"
        >
          <template slot-scope="{row}">
            <span>{{ row.displayName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:CreationTime')"
          prop="creationTime"
          sortable
          width="200px"
        >
          <template slot-scope="{row}">
            <span>{{ row.creationTime | datetimeFilter }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:LastModificationTime')"
          prop="lastModificationTime"
          sortable
          width="200px"
        >
          <template slot-scope="{row}">
            <span>{{ row.lastModificationTime | datetimeFilter }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('operaActions')"
          align="center"
          width="180px"
        >
          <template slot-scope="{row}">
            <el-tooltip
              effect="dark"
              :content="$t('LocalizationManagement.Edit')"
              placement="top"
            >
              <el-button
                size="mini"
                type="primary"
                icon="el-icon-edit"
                @click="handleModify(row)"
              />
            </el-tooltip>
            <el-tooltip
              effect="dark"
              :content="$t('LocalizationManagement.Delete')"
              placement="top"
            >
              <el-button
                size="mini"
                type="danger"
                icon="el-icon-delete"
                @click="handleDelete(row)"
              />
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>

      <Pagination
        v-show="dataTotal>0"
        :total="dataTotal"
        :page.sync="currentPage"
        :limit.sync="pageSize"
        @pagination="refreshPagedData"
        @sort-change="handleSortChange"
      />
    </el-card>

    <LanguageDialog
      :language-id="editLanguage.id"
      :show-dialog="showEditDialog"
      @closed="showEditDialog=false"
    />
  </div>
</template>

<script lang="ts">
import { Component, Mixins } from 'vue-property-decorator'
import DataListMiXin from '@/mixins/DataListMiXin'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'
import Pagination from '@/components/Pagination/index.vue'
import LanguageDialog from './components/LanguageDialog.vue'

import {
  service,
  controller,
  Language,
  GetLanguagesInput
} from './types'

import { dateFormat, abpPagerFormat } from '@/utils/index'

@Component({
  name: 'LanguageList',
  components: {
    Pagination,
    LanguageDialog
  },
  filters: {
    datetimeFilter(val: string) {
      if (!val) {
        return ''
      }
      const date = new Date(val)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Mixins(DataListMiXin, HttpProxyMiXin) {
  public dataFilter = new GetLanguagesInput()

  private showEditDialog = false
  private editLanguage = new Language()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return this.pagedRequest<Language>({
      service: service,
      controller: controller,
      action: 'GetListAsync',
      params: filter
    })
  }

  private handleCreate() {
    this.editLanguage = new Language()
    this.showEditDialog = true
  }

  private handleModify(language: Language) {
    this.editLanguage = language
    this.showEditDialog = true
  }

  private handleDelete(language: Language) {
    this.$confirm(this.l('LocalizationManagement.WillDeleteLanguage', { 0: language.displayName ?? language.cultureName }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            this.request<void>({
              service: service,
              controller: controller,
              action: 'DeleteAsync',
              params: {
                id: language.id
              }
            }).then(() => {
              this.$message.success(this.l('global.successful'))
              this.refreshPagedData()
            })
          }
        }
      })
  }
}
</script>

<style scoped>
.create-new {
  margin-right: 10px;
  width: 200px;
}
</style>
