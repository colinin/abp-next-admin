<template>
  <div>
    <el-row>
      <el-col
        :span="8"
      >
        <organization-unit-tree
          @onOrganizationUnitChecked="handleOrganizationUnitChecked"
        />
      </el-col>
      <el-col :span="16">
        <el-card>
          <div
            slot="header"
          >
            <span>{{ $t('AbpIdentity.OrganizationUnit:Members') }}</span>
          </div>
          <div>
            <el-tabs>
              <el-tab-pane :label="$t('AbpIdentity.Roles')">
                <role-organization-uint :organization-unit-id="checkedOrganizationUintId" />
              </el-tab-pane>
              <el-tab-pane :label="$t('AbpIdentity.Users')">
                <user-organization-uint :organization-unit-id="checkedOrganizationUintId" />
              </el-tab-pane>
            </el-tabs>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import OrganizationUnitTree from './components/OrganizationUnitTree.vue'
import UserOrganizationUint from './components/UserOrganizationUint.vue'
import RoleOrganizationUint from './components/RoleOrganizationUint.vue'

@Component({
  name: 'OrganizationUint',
  components: {
    OrganizationUnitTree,
    UserOrganizationUint,
    RoleOrganizationUint
  }
})
export default class extends Vue {
  private checkedOrganizationUintId = ''

  private handleOrganizationUnitChecked(id: string) {
    this.checkedOrganizationUintId = id
  }
}
</script>

<style lang="scss" scoped>
</style>
