import { Component, Vue } from 'vue-property-decorator'

@Component
export default class LocalizationMiXin extends Vue {
  /**
   * 本地化接口
   * @param name 本地化名称
   * @param values 参数
   */
   public l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}