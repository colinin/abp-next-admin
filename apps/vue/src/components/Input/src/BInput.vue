<script lang="ts">
  import { defineComponent, h } from 'vue';
  import { Input } from 'ant-design-vue';

  /** 封装为全局禁用自动完成input组件 */
  export default defineComponent({
    name: 'BInput',
    render() {
      return h(Input, {
        ...this.$attrs,
        autocomplete: 'off',
      });
    },
  });
</script>
