<template>
  <div>
    <p class="desc">选择能发起该审批的人员/部门，不选则默认开放给所有人</p>
    <Button size="small" @click="selectOrg" type="primary" round>请选择</Button>
    <OrgItems v-model="select"/>
  </div>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { Button } from 'ant-design-vue';
  import OrgItems from "../OrgItems.vue";

  const props = defineProps({
    config:{
      type: Object,
      default: ()=>{
        return {}
      }
    },
  });
  const select = computed(() => {
    return props.config.assignedUser;
  });
  const state = reactive({
    showOrgSelect: false,
  });

  function selectOrg() {
    console.log(selectOrg);
  }

  function selected(sel: any) {
    select.value.length = 0;
    select.value.forEach(val => sel.push(val));
  }

  function removeOrgItem(index: number){
    select.value.splice(index, 1);
  }
</script>

<style scoped>

</style>