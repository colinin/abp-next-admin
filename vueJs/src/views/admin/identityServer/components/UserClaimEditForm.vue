<template>
  <el-transfer
    :value="rightClaims"
    class="transfer-scope"
    :data="userClaimTypes"
    :titles="[$t('AbpIdentityServer.Assigned'), $t('AbpIdentityServer.Available')]"
    :props="{
      key: 'type',
      label: 'value'
    }"
    @change="onChanged"
  />
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import ClaimTypeApiService from '@/api/cliam-type'
import { UserClaim } from '@/api/identity-server4'
import { Claim } from '@/api/types'

@Component({
  name: 'UserClaimEditForm',
  model: {
    prop: 'userCliams',
    event: 'change'
  }
})
export default class extends Vue {
  @Prop({ default: () => { return new Array<UserClaim>() } })
  private userCliams!: UserClaim[]

  private userClaimTypes = new Array<Claim>()

  get rightClaims() {
    return this.userCliams.map(claim => claim.type)
  }

  mounted() {
    this.handleGetUserClaimTypes()
  }

  private handleGetUserClaimTypes() {
    ClaimTypeApiService
      .getActivedClaimTypes()
      .then(res => {
        res.items.map(claim => {
          this.userClaimTypes.push(new Claim(claim.name, claim.name))
        })
      })
  }

  private onChanged(value: string[]) {
    const claims = this.userClaimTypes.filter(claim => value.some(key => claim.type === key))
    this.$emit('change', claims.map(claim => new UserClaim(claim.type)))
  }
}
</script>

<style scoped>

</style>
