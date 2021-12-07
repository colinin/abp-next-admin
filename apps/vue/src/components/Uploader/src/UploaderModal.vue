<template>
  <Card>
    <template #title>
      <Button id="addFile">添加文件</Button>
    </template>
  </Card>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Button, Card } from 'ant-design-vue';
  import Uploader from 'simple-uploader.js';
  import { basicProps } from './props';

  export default defineComponent({
    name: 'UploaderModal',
    components: { Button, Card },
    props: basicProps,
    setup(props) {
      const uploader = new Uploader(props);

      return {
        uploader,
      };
    },
  });
</script>
