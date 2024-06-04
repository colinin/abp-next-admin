<template>
  <div style="margin-top: 10px">
    <template v-for="(org, index) in _value">
      <Tag class="org-item" closable @close="removeOrgItem(index)">
        <template #icon>
          <InfoOutlined v-if="org.type !== 'dept'" />
        </template>
        {{ org.name }}
      </Tag>
    </template>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue';
  import { Tag } from 'ant-design-vue';
  import { InfoOutlined } from '@ant-design/icons-vue';

  const emits = defineEmits(['input']);
  const props = defineProps({
    value: {
      type: Array as PropType<any[]>,
      default: () => {
        return [];
      },
    },
  });

  const _value = computed({
    get: () => props.value,
    set: (val) => {
      emits('input', val);
    },
  });

  function removeOrgItem(index) {
    _value.value.splice(index, 1);
  }
</script>

<style scoped>
  .org-item {
    margin: 5px;
  }
</style>
