<template>
  <div class="app-container">
    <el-form
      ref="frmFeature"
      :model="featureGroups"
      label-width="130px"
    >
      <el-tabs v-model="selectTab">
        <el-tab-pane
          v-for="(group, gi) in featureGroups.groups"
          :key="group.name"
          :label="group.displayName"
          :name="group.name"
        >
          <div
            v-for="(feature, fi) in group.features"
            :key="feature.name"
          >
            <el-form-item
              v-if="feature.valueType !== null"
              :label="feature.displayName"
              :prop="'groups.' + gi + '.features.' + fi + '.value'"
              :rules="feature.valueType.validator | inputRuleFilter(localizer)"
            >
              <el-popover
                :ref="'popover_' + gi + '_' + fi"
                trigger="hover"
                :title="feature.displayName"
                :content="feature.description"
              />
              <span
                slot="label"
                v-popover="'popover_' + gi + '_' + fi"
              >{{ feature.displayName }}</span>
              <div
                v-if="feature.valueType.name === 'ToggleStringValueType'"
              >
                <el-switch
                  :value="getBooleanValue(feature.value)"
                  @change="(value) => handleValueChanged(feature, value)"
                />
              </div>
              <div
                v-else-if="feature.valueType.name === 'FreeTextStringValueType'"
              >
                <el-input
                  v-if="feature.valueType.validator.name === 'NUMERIC'"
                  v-model.number="feature.value"
                  @change="(value) => handleValueChanged(feature, value)"
                />
                <el-input
                  v-else
                  v-model="feature.value"
                  type="text"
                />
              </div>
              <div
                v-else-if="feature.valueType.name === 'SelectionStringValueType'"
              >
                <el-select
                  v-model="feature.value"
                  style="width: 100%;"
                >
                  <el-option
                    v-for="valueItem in feature.valueType.itemSource.items"
                    :key="valueItem.value"
                    :label="localizer(valueItem.displayText.resourceName + '.' + valueItem.displayText.name)"
                    :value="valueItem.value"
                  />
                </el-select>
              </div>
            </el-form-item>
          </div>
        </el-tab-pane>
      </el-tabs>
      <el-button
        type="primary"
        class="confirm"
        @click="onSave"
      >
        {{ $t('AbpFeatureManagement.Submit') }}
      </el-button>
      <el-button
        class="cancel"
        @click="onClosed"
      >
        {{ $t('AbpFeatureManagement.Cancel') }}
      </el-button>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import FeatureManagementService, { Feature, FeatureGroups, Features } from '@/api/feature-management'
import { ElForm } from 'element-ui/types/form'

@Component({
  name: 'FeatureManagement',
  computed: {
    getBooleanValue() {
      return (value: any) => {
        if (value === 'true') {
          return true
        }
        return false
      }
    },
    getNumericValue() {
      return (value: any) => {
        return Number(value)
      }
    }
  },
  filters: {
    /**
     * 动态处理功能表单验证
     * @validator 后端传递的验证规则
     * @localizer 本地化method
     */
    inputRuleFilter(validator: any, localizer: any) {
      const featureRules: {[key: string]: any}[] = new Array<{[key: string]: any}>()
      if (validator.name === 'NUMERIC' && validator.properties) {
        const ruleRang: {[key: string]: any} = {}
        // TODO: 需要动态验证数值范围
        // 暂时不提交
        // 可行性一、对返回数据使用计算属性，在获取数据之后就组装好验证类，传递给对应的form-item的rules
        // 可行性二、TODO...
        ruleRang.pattern = RegExp('^[^' + validator.properties.MinValue + ']\\d{0,4}$|^' + validator.properties.MaxValue + '$')
        // ruleRang.pattern = ^\d{0,11}$
        // ruleRang.type = 'number'
        // ruleRang.min = validator.properties.MinValue
        // ruleRang.max = validator.properties.MaxValue
        ruleRang.trigger = 'blur'
        ruleRang.message = localizer('AbpFeatureManagement.ThisFieldMustBeBetween{0}And{1}', { 0: validator.properties.MinValue, 1: validator.properties.MaxValue })
        featureRules.push(ruleRang)
      } else if (validator.name === 'STRING' && validator.properties) {
        if (validator.properties.AllowNull && validator.properties.AllowNull.toLowerCase() === 'true') {
          const ruleRequired: {[key: string]: any} = {}
          ruleRequired.required = true
          ruleRequired.trigger = 'blur'
          ruleRequired.messahe = localizer('AbpFeatureManagement.ThisFieldIsRequired')
          featureRules.push(ruleRequired)
        }
        const ruleString: {[key: string]: any} = {}
        ruleString.min = validator.properties.MinLength
        ruleString.max = validator.properties.MaxLength
        ruleString.trigger = 'blur'
        ruleString.message = localizer('AbpFeatureManagement.ThisFieldMustBeBetween{0}And{1}', { 0: validator.properties.MinValue, 1: validator.properties.MaxValue })
        featureRules.push(ruleString)
      }
      return featureRules
    }
  },
  methods: {
    /**
     * 本地化method
     */
    localizer(name: string, values?: any[]) {
      return this.$t(name, values)
    },
    validatorNumberRange(rule: any, value: any, callback: any) {
      if (value) {
        const num = Number(value)
        if (num < rule.min ||
            num > rule.max) {
          callback(new Error(this.$t('AbpFeatureManagement.ThisFieldMustBeBetween{0}And{1}', { 0: rule.min, 1: rule.max }).toString()))
        }
      }
    }
  }
})
export default class extends Vue {
  /**
   * 功能提供者名称
   */
  @Prop({ default: '' })
  private providerName!: string

  /**
   * 功能提供者标识
   */
  @Prop({ default: '' })
  private providerKey!: string

  @Prop({ default: false })
  private loadFeature!: boolean

  /**
   * 默认选择tab选项卡
   * 如果不定义的话,动态组合的表单需要手动点击一次才会显示?
   */
  private selectTab = ''
  /**
   * 用于拼接动态表单的功能数据,需要把abp返回的数据做一次调整
   */
  private featureGroups = new FeatureGroups()

  mounted() {
    this.handleGetFeatures()
  }

  @Watch('providerKey')
  onProviderKeyChanged() {
    this.handleGetFeatures()
  }

  private handleValueChanged(feature: Feature, value: any) {
    feature.value = String(value)
  }

  /**
   * 重置表单数据
   */
  public resetFeature() {
    const frmFeature = this.$refs.frmFeature as ElForm
    frmFeature.resetFields()
  }

  /**
   * 获取功能列表
   */
  private handleGetFeatures() {
    if (this.loadFeature) {
      FeatureManagementService
        .getFeatures(this.providerName, this.providerKey)
        .then(res => {
          this.featureGroups = res
          if (this.featureGroups.groups.length > 0) {
            this.selectTab = this.featureGroups.groups[0].name
          }
        })
    }
  }

  /**
   * 保存变更
   */
  private onSave() {
    if (this.featureGroups.groups.length > 0) {
      const frmFeature = this.$refs.frmFeature as any
      frmFeature.validate((valid: boolean) => {
        if (valid) {
          const updateFeatures = new Features()
          this.featureGroups.groups.forEach(group => {
            group.features.forEach(feature => {
              if (feature.valueType != null) {
                updateFeatures.features.push(new Feature(feature.name, feature.value))
              }
            })
          })
          FeatureManagementService
            .updateFeatures(this.providerName, this.providerKey, updateFeatures)
            .then(() => {
              this.$message.success(this.$t('global.successful').toString())
              this.onClosed()
            })
        }
      })
    }
  }

  /**
   * 关闭模态窗口
   */
  private onClosed() {
    this.resetFeature()
    this.$emit('closed')
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  width: 120px;
  position: absolute;
  right: 180px;
}
.cancel {
  width: 120px;
  position: absolute;
  right: 40px;
}
</style>
