<template>
  <{{ model.table_name }} />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import {{ model.table_name }} from './components/{{ model.table_name }}.vue';

  export default defineComponent({
    name: '{{ model.application }}',
    components: { {{ model.table_name }} },
  });
</script>
