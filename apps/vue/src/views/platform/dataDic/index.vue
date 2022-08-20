<template>
  <Row type="flex">
    <Col :span="8">
      <DataTree @change="handleDataChange" />
    </Col>
    <Col :span="16">
      <DataItemTable :data-id="dataId" @reload="handleDataChange(dataId)" />
    </Col>
  </Row>
</template>

<script lang="ts">
  export default {
    name: 'DataDictionary',
  };
</script>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { Row, Col } from 'ant-design-vue';
  import DataTree from './components/DataTree.vue';
  import DataItemTable from './components/DataItemTable.vue';

  const dataId = ref('');

  function handleDataChange(id: string) {
    dataId.value = id;
  }
</script>
