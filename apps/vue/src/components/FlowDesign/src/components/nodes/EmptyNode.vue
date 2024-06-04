<template>
  <Node :show="false" @insertNode="(type) => $emit('insertNode', type)" />
</template>

<script lang="ts">
  export default {
    name: 'Empty',
  };
</script>

<script setup lang="ts">
  import Node from './Node.vue';

  defineEmits(['insertNode']);
</script>

<style scoped></style>
