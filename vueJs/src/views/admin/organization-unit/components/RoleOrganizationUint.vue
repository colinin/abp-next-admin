<template>
  <div>
    <el-table
      row-key="id"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      :data="organizationUnitRoles"
    >
      <el-table-column
        :label="$t('roles.name')"
        prop="name"
        sortable
        width="350px"
        min-width="350px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>

          <el-tag
            v-if="row.isDefault"
            type="success"
          >
            {{ $t('roles.isDefault') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.isPublic')"
        prop="isPublic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isPublic ? 'success' : 'warning'"
          >
            {{ row.isPublic ? $t('roles.isPublic') : $t('roles.isPrivate') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.type')"
        prop="isStatic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isStatic ? 'info' : 'success'"
          >
            {{ row.isStatic ? $t('roles.system') : $t('roles.custom') }}
          </el-tag>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="organizationUnitRoleCount>0"
      :total="organizationUnitRoleCount"
      :page.sync="organizationUnitRoleFilter.skipCount"
      :limit.sync="organizationUnitRoleFilter.maxResultCount"
      @pagination="handleGetOrganizationUnitRoles"
    />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Watch, Vue } from 'vue-property-decorator'
import OrganizationUnitService, { OrganizationUnitGetRoleByPaged } from '@/api/organizationunit'
import { RoleDto } from '@/api/roles'
import Pagination from '@/components/Pagination/index.vue'

@Component({
  name: 'RoleOrganizationUint',
  components: {
    Pagination
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private organizationUnitId?: string

  private organizationUnitRoleFilter: OrganizationUnitGetRoleByPaged
  private organizationUnitRoleCount: number
  private organizationUnitRoles: RoleDto[]

  constructor() {
    super()
    this.organizationUnitRoleCount = 0
    this.organizationUnitRoles = new Array<RoleDto>()
    this.organizationUnitRoleFilter = new OrganizationUnitGetRoleByPaged()
  }

  @Watch('organizationUnitId', { immediate: true })
  private onOrganizationUnitIdChanged() {
    this.organizationUnitRoles = new Array<RoleDto>()
    if (this.organizationUnitId) {
      this.handleGetOrganizationUnitRoles()
    }
  }

  private handleGetOrganizationUnitRoles() {
    if (this.organizationUnitId) {
      this.organizationUnitRoleFilter.id = this.organizationUnitId
      OrganizationUnitService.organizationUnitGetRoles(this.organizationUnitRoleFilter).then(res => {
        this.organizationUnitRoles = res.items
      })
    }
  }
}
</script>

<style lang="scss" scoped>

</style>
