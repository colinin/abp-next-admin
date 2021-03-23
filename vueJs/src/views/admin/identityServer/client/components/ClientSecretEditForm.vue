<template>
  <div>
    <el-form-item
      prop="requireClientSecret"
      label-width="0"
    >
      <el-checkbox
        v-model="client.requireClientSecret"
        class="label-title"
      >
        {{ $t('AbpIdentityServer.Client:RequiredClientSecret') }}
      </el-checkbox>
    </el-form-item>
    <SecretEditForm
      v-model="client.clientSecrets"
      :allowed-create-secret="checkPermission(['AbpIdentityServer.Clients.ManageSecrets'])"
      :allowed-delete-secret="checkPermission(['AbpIdentityServer.Clients.ManageSecrets'])"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { checkPermission } from '@/utils/permission'
import { Client } from '@/api/clients'

import SecretEditForm from '../../components/SecretEditForm.vue'

@Component({
  name: 'ClientSecretEditForm',
  components: {
    SecretEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: () => { return {} } })
  private client!: Client
}
</script>
