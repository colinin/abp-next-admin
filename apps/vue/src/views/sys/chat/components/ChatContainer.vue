<template>
  <div class="im-container">
    <div class="im-sider">
      <ChatSider @switch="handleSwitchComp" :defaultKey="switchComp" />
    </div>
    <div class="im-main">
      <component v-if="switchComp !== ''" :is="switchComp" />
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import ChatSider from './ChatSider.vue';
  import ChatMessage from './ChatMessage.vue';

  export default defineComponent({
    name: 'ChatContainer',
    components: {
      ChatSider,
      ChatMessage,
    },
    setup() {
      const switchComp = ref('chat-message');

      function handleSwitchComp(key: string) {
        switchComp.value = key;
      }

      return {
        switchComp,
        handleSwitchComp,
      };
    },
  });
</script>

<style lang="less" scoped>
  .im-container {
    display: flex;
    flex-direction: row;
    margin-top: 5px;
    height: 100%;
    overflow: hidden;

    .im-sider {
      width: 60px;
      height: 100%;
    }

    .im-main {
      width: 100%;
    }
  }
</style>
