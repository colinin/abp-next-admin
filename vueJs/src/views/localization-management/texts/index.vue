<template>
  <div class="app-container">
    <div class="filter-container">
      <el-card>
        <div style="margin-top: 15px;">
          <el-form
            ref="formFilter"
            :model="dataFilter"
            label-width="120px"
          >
            <el-row>
              <el-col :span="6">
                <el-form-item
                  :label="$t('LocalizationManagement.DisplayName:CultureName')"
                  prop="cultureName"
                  :rules="{
                    required: true,
                    message: $t('pleaseSelectBy', {key: $t('LocalizationManagement.DisplayName:CultureName')}),
                    trigger: 'blur'
                  }"
                >
                  <el-select
                    v-model="dataFilter.cultureName"
                    style="width: 100%;"
                  >
                    <el-option
                      v-for="language in languages"
                      :key="language.cultureName"
                      :label="language.displayName"
                      :value="language.cultureName"
                    />
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="6">
                <el-form-item
                  :label="$t('LocalizationManagement.DisplayName:TargetCultureName')"
                  prop="targetCultureName"
                  :rules="{
                    required: true,
                    message: $t('pleaseSelectBy', {key: $t('LocalizationManagement.DisplayName:TargetCultureName')}),
                    trigger: 'blur'
                  }"
                >
                  <el-select
                    v-model="dataFilter.targetCultureName"
                    style="width: 100%;"
                  >
                    <el-option
                      v-for="language in languages"
                      :key="language.cultureName"
                      :label="language.displayName"
                      :value="language.cultureName"
                    />
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="6">
                <el-form-item
                  :label="$t('LocalizationManagement.DisplayName:ResourceName')"
                  prop="resourceName"
                >
                  <el-select
                    v-model="dataFilter.resourceName"
                    style="width: 100%;"
                    clearable
                  >
                    <el-option
                      v-for="resource in resources"
                      :key="resource.name"
                      :label="resource.displayName"
                      :value="resource.name"
                    />
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="6">
                <el-form-item
                  :label="$t('LocalizationManagement.DisplayName:TargetValue')"
                  prop="onlyNull"
                >
                  <el-select
                    v-model="dataFilter.onlyNull"
                    style="width: 100%;"
                    clearable
                  >
                    <el-option
                      :label="$t('LocalizationManagement.DisplayName:Any')"
                      :value="false"
                    />
                    <el-option
                      :label="$t('LocalizationManagement.DisplayName:OnlyNull')"
                      :value="true"
                    />
                  </el-select>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="20">
                <el-form-item
                  :label="$t('LocalizationManagement.Filter')"
                  prop="filter"
                >
                  <el-input
                    v-model="dataFilter.filter"
                    :placeholder="$t('LocalizationManagement.SearchFilter')"
                  >
                    <el-button
                      slot="append"
                      icon="el-icon-search"
                      @click="handleGetTexts(1)"
                    />
                  </el-input>
                </el-form-item>
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
                  {{ $t('LocalizationManagement.Text:AddNew') }}
                </el-button>
              </el-col>
            </el-row>
          </el-form>
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
          :label="$t('LocalizationManagement.DisplayName:Key')"
          prop="key"
          sortable
          width="250px"
        >
          <template slot-scope="{row}">
            <span>{{ row.key }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:Value')"
          prop="value"
          sortable
          width="300px"
        >
          <template slot-scope="{row}">
            <span>{{ row.value }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:TargetValue')"
          prop="targetValue"
          sortable
          width="300px"
        >
          <template slot-scope="{row}">
            <span>{{ row.targetValue }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:ResourceName')"
          prop="resourceName"
          sortable
          width="250px"
        >
          <template slot-scope="{row}">
            <span>{{ row.resourceName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LocalizationManagement.DisplayName:Description')"
          prop="description"
          sortable
          class-name="full-description"
        >
          <template slot-scope="{row}">
            <span>{{ row.description }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('operaActions')"
          align="center"
          width="180px"
          min-width="180px"
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
        @pagination="handleGetTexts(currentPage)"
        @sort-change="handleSortChange"
      />
    </el-card>

    <TextDialog
      :text-id="editText.id"
      :show-dialog="showEditDialog"
      :languages="languages"
      :resources="resources"
      @closed="showEditDialog=false"
    />
  </div>
</template>

<script lang="ts">
import { Component, Mixins } from 'vue-property-decorator'
import DataListMiXin from '@/mixins/DataListMiXin'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'
import Pagination from '@/components/Pagination/index.vue'
import TextDialog from './components/TextDialog.vue'

import {
  service as TextService,
  controller as TextController,
  Text,
  GetTextsInput
} from './types'
import {
  service as LanguageService,
  controller as LanguageController,
  Language
} from '../languages/types'
import {
  service as ResourceService,
  controller as ResourceController,
  Resource
} from '../resources/types'

import { dateFormat, abpPagerFormat } from '@/utils/index'
import { Form } from 'element-ui'

@Component({
  name: 'TextList',
  components: {
    Pagination,
    TextDialog
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
  public dataFilter = new GetTextsInput()

  private showEditDialog = false
  private editText = new Text()

  private languages = new Array<Language>()
  private resources = new Array<Resource>()

  mounted() {
    this.handleGetLanguages()
    this.handleGetResources()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return this.pagedRequest<Text>({
      service: TextService,
      controller: TextController,
      action: 'GetListAsync',
      params: {
        input: filter
      }
    })
  }

  private handleGetTexts(pageNumber: number) {
    const formFilter = this.$refs.formFilter as Form
    formFilter.validate(valid => {
      if (valid) {
        this.currentPage = pageNumber
        this.refreshPagedData()
      }
    })
  }

  private handleGetLanguages() {
    this.listRequest<Language>({
      service: LanguageService,
      controller: LanguageController,
      action: 'GetAllAsync'
    }).then(res => {
      this.languages = res.items
    })
  }

  private handleGetResources() {
    this.listRequest<Resource>({
      service: ResourceService,
      controller: ResourceController,
      action: 'GetAllAsync'
    }).then(res => {
      this.resources = res.items
    })
  }

  private handleCreate() {
    this.editText = new Text()
    this.showEditDialog = true
  }

  private handleModify(text: Text) {
    this.editText = text
    this.showEditDialog = true
  }

  private handleDelete(text: Text) {
    this.$confirm(this.l('LocalizationManagement.WillDeleteText', { 0: text.key }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            this.request<void>({
              service: TextService,
              controller: TextController,
              action: 'DeleteAsync',
              params: {
                id: text.id
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
.full-description {
  width: 100%;
}
</style>
