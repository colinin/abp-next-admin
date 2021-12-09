<template>
  <ChatContainer />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import ChatContainer from './components/ChatContainer.vue';

  export default defineComponent({
    name: 'Chat',
    components: { ChatContainer },
    setup() {},
  });
</script>
