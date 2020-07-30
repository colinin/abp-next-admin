<template>
  <div>
    <el-row>
      <el-col
        :span="8"
      >
        <edit-organization-uint
          @onOrganizationUnitChecked="handleOrganizationUnitChecked"
        />
      </el-col>
      <el-col :span="16">
        <el-card>
          <div
            slot="header"
          >
            <span>机构成员</span>
          </div>
          <div>
            <el-tabs>
              <el-tab-pane label="角色列表">
                <role-organization-uint :organization-unit-id="checkedOrganizationUintId" />
              </el-tab-pane>
              <el-tab-pane label="用户列表">
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
import EditOrganizationUint from './components/EditOrganizationUint.vue'
import UserOrganizationUint from './components/UserOrganizationUint.vue'
import RoleOrganizationUint from './components/RoleOrganizationUint.vue'

@Component({
  name: 'OrganizationUint',
  components: {
    EditOrganizationUint,
    UserOrganizationUint,
    RoleOrganizationUint
  }
})
export default class extends Vue {
  private checkedOrganizationUintId?: string = ''

  private handleOrganizationUnitChecked(id: string) {
    this.checkedOrganizationUintId = id
  }
}
</script>

<style lang="scss" scoped>
</style>
