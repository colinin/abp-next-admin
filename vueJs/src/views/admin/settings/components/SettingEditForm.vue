<template>
  <div>
    <el-form
      ref="formsetting"
      label-width="180px"
      style="width: 96%"
    >
      <el-tabs :tab-position="tabPosition">
        <el-tab-pane
          v-for="group in settingGroups"
          :key="group.displayName"
          :label="group.displayName"
        >
          <el-card
            v-for="setting in group.settings"
            :key="setting.displayName"
          >
            <div
              slot="header"
              class="clearfix"
            >
              <span>{{ setting.displayName }}</span>
            </div>
            <el-form-item
              v-for="detail in setting.details"
              :key="detail.name"
              :label="detail.displayName"
              label-width="200px"
            >
              <el-popover
                :ref="detail.name"
                trigger="hover"
                :title="detail.displayName"
                :content="detail.description"
              />
              <span
                slot="label"
                v-popover="detail.name"
              >{{ detail.displayName }}</span>
              <el-input
                v-if="detail.valueType===0"
                v-model="detail.value"
                :placeholder="detail.description"
                :type="detail.isEncrypted ? 'password' : 'text'"
                :show-password="detail.isEncrypted"
                @input="(value) => handleSettingValueChanged(detail.name, value)"
              />
              <el-input
                v-if="detail.valueType===1"
                v-model="detail.value"
                :placeholder="detail.description"
                type="number"
                @input="(value) => handleSettingValueChanged(detail.name, value)"
              />
              <el-switch
                v-if="detail.valueType===2"
                :value="detail.value==='true'"
                @change="(value) => {
                  handleSettingValueChanged(detail.name, value)
                  detail.value = value.toString()
                }"
              />
              <el-select
                v-if="detail.valueType===5"
                v-model="detail.value"
                style="width: 100%;"
                @change="(value) => handleSettingValueChanged(detail.name, value)"
              >
                <el-option
                  v-for="option in detail.options"
                  :key="option.name"
                  :label="option.name"
                  :value="option.value"
                  :disabled="option.value===detail.value"
                />
              </el-select>
            </el-form-item>
          </el-card>
        </el-tab-pane>
      </el-tabs>

      <el-form-item
        v-if="changeSetting.settings.length>0"
      >
        <el-button
          type="primary"
          class="save-button"
          @click="onSavesetting"
        >
          {{ $t('global.confirm') }}
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import { SettingGroup, SettingUpdate, SettingsUpdate } from '@/api/settings'
import { isUndefined } from 'lodash'

@Component({
  name: 'SettingEditForm'
})
export default class extends Vue {
  @Prop({ default: () => { return Array<SettingGroup>() } })
  private settingGroups!: SettingGroup[]

  @Prop({ default: 'top' })
  private tabPosition!: 'left' | 'right' | 'top' | 'bottom'

  private changeSetting = new SettingsUpdate()

  private handleSettingValueChanged(key: string, value: any) {
    const setting = this.changeSetting.settings.find(setting => setting.name === key)
    if (isUndefined(setting)) {
      const setting = new SettingUpdate()
      setting.name = key
      setting.value = value
      this.changeSetting.settings.push(setting)
    } else {
      setting.value = value
    }
  }

  private onSavesetting() {
    if (this.changeSetting.settings.length > 0) {
      this.$emit('onSettingSaving', this.changeSetting)
    }
  }
}
</script>

<style scoped>
.save-button {
  width: 200px;
  margin-top: 30px;
}
</style>
