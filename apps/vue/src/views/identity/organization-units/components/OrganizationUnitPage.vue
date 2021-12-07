<template>
  <Row :gutter="16" style="margin-right: 12px; margin-left: 12px">
    <Col :span="10">
      <OrganizationUnitTree @select="handleSelect" />
    </Col>
    <Col :span="14">
      <Card>
        <Tabs v-model="activeKey">
          <TabPane key="members" :tab="L('Users')">
            <MemberTable :ou-id="ouIdRef" />
          </TabPane>
          <TabPane key="roles" :tab="L('Roles')" :forceRender="true">
            <RoleTable :ou-id="ouIdRef" />
          </TabPane>
        </Tabs>
      </Card>
    </Col>
  </Row>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Card, Row, Col, Tabs } from 'ant-design-vue';
  import OrganizationUnitTree from './OrganizationUnitTree.vue';
  import MemberTable from './MemberTable.vue';
  import RoleTable from './RoleTable.vue';

  export default defineComponent({
    name: 'OrganizationUnitPage',
    components: {
      OrganizationUnitTree,
      Card,
      Col,
      MemberTable,
      Row,
      RoleTable,
      Tabs,
      TabPane: Tabs.TabPane,
    },
    setup() {
      const { L } = useLocalization('AbpIdentity');
      const activeKey = ref('members');
      const ouIdRef = ref('');

      function handleSelect(key) {
        ouIdRef.value = key;
      }

      return {
        L,
        activeKey,
        ouIdRef,
        handleSelect,
      };
    },
  });
</script>
