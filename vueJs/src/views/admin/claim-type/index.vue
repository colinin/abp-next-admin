<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:0;"
      >{{ $t('global.queryFilter') }}</label>
      <el-input
        v-model="dataFilter.filter"
        :placeholder="$t('global.filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="refreshPagedData"
      >
        {{ $t('global.searchList') }}
      </el-button>
      <el-button
        v-permission="['AbpIdentity.IdentityClaimTypes.Create']"
        class="filter-item"
        type="primary"
        @click="handleCreateClaimType"
      >
        {{ $t('AbpIdentity.AddClaim') }}
      </el-button>
    </div>

    <el-table
      v-loading="dataLoading"
      row-key="id"
      :data="dataList"
      stripe
      border
      fit
      highlight-current-row
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('AbpIdentity.IdentityClaim:Name')"
        prop="name"
        sortable
        width="300px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.IdentityClaim:ValueType')"
        prop="valueType"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.valueType | claimValueTypeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.IdentityClaim:Description')"
        prop="description"
        sortable
        min-width="100%"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.IdentityClaim:Regex')"
        prop="regex"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.regex }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.IdentityClaim:Required')"
        prop="required"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.required"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.IdentityClaim:IsStatic')"
        prop="isStatic"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.isStatic"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        v-if="checkPermission(['AbpIdentity.IdentityClaimTypes.Update', 'AbpIdentity.IdentityClaimTypes.Delete'])"
        :label="$t('operaActions')"
        align="center"
        width="260px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="row.isStatic"
            size="mini"
            type="primary"
            @click="handleUpdateClaimType(row)"
          >
            {{ $t('AbpIdentity.UpdateClaim') }}
          </el-button>
          <el-button
            :disabled="row.isStatic"
            size="mini"
            type="danger"
            @click="handleDeleteClaimType(row)"
          >
            {{ $t('AbpIdentity.DeleteClaim') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
    />

    <create-or-update-cliam-type-form
      :title="editClaimTypeTitle"
      :claim-type-id="editClaimTypeId"
      :show-dialog="showClaimTypeDialog"
      @closed="onClaimTypeDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { abpPagerFormat } from '@/utils'
import Pagination from '@/components/Pagination/index.vue'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import CreateOrUpdateCliamTypeForm from './components/CreateOrUpdateCliamTypeForm.vue'
import ClaimTypeApiService, { IdentityClaimType, IdentityClaimTypeGetByPaged, IdentityClaimValueType } from '@/api/cliam-type'

const valueTypeMap: { [key: number]: string } = {
  [IdentityClaimValueType.String]: 'String',
  [IdentityClaimValueType.Boolean]: 'Boolean',
  [IdentityClaimValueType.DateTime]: 'DateTime',
  [IdentityClaimValueType.Int]: 'Int'
}

@Component({
  name: 'ClaimType',
  components: {
    Pagination,
    CreateOrUpdateCliamTypeForm
  },
  filters: {
    claimValueTypeFilter(valueType: IdentityClaimValueType) {
      return valueTypeMap[valueType]
    }
  },
  methods: {
    checkPermission
  }
})
export default class ClaimType extends mixins(DataListMiXin) {
  private editClaimTypeId = ''
  private editClaimTypeTitle = ''
  private showClaimTypeDialog = false
  public dataFilter = new IdentityClaimTypeGetByPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList() {
    return ClaimTypeApiService.getClaimTypes(this.dataFilter)
  }

  private handleCreateClaimType() {
    this.editClaimTypeId = ''
    this.editClaimTypeTitle = this.l('AbpIdentity.IdentityClaim:New')
    this.showClaimTypeDialog = true
  }

  private handleUpdateClaimType(claimType: IdentityClaimType) {
    this.editClaimTypeId = claimType.id
    this.editClaimTypeTitle = this.l('AbpIdentity.ClaimSubject', { 0: claimType.name })
    this.showClaimTypeDialog = true
  }

  private handleDeleteClaimType(claimType: IdentityClaimType) {
    this.$confirm(this.l('AbpIdentity.WillDeleteClaim', { 0: claimType.name }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            ClaimTypeApiService.deleteClaimType(claimType.id).then(() => {
              this.$message.success(this.l('global.successful'))
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private onClaimTypeDialogClosed(changed: boolean) {
    this.showClaimTypeDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }
}
</script>
