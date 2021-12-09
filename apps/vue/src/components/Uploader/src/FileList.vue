<script lang="ts">
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'FileList',
    props: {
      uploader: {
        type: Object as PropType<object>,
        required: true,
      },
    },
    setup() {},
  });
</script>
