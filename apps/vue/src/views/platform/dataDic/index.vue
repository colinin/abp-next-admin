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
  import { defineComponent, ref } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';

  import DataTree from './components/DataTree.vue';
  import DataItemTable from './components/DataItemTable.vue';
  import { Row, Col } from 'ant-design-vue';

  export default defineComponent({
    name: 'DataDictionary',
    components: {
      DataTree,
      DataItemTable,
      Row,
      Col,
    },
    setup() {
      const { t } = useI18n();
      const dataId = ref('');

      return {
        t,
        dataId,
      };
    },
    methods: {
      handleDataChange(id: string) {
        this.dataId = id;
      },
    },
  });
</script>
