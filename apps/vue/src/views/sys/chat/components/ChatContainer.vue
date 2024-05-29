<template>
  <div class="im-container">
    <div class="im-sider">
      <ChatSider @switch="handleSwitchComp" :defaultKey="switchComp" />
    </div>
    <div class="im-main">
      <component v-if="switchComp !== ''" :is="componentsRef[switchComp]" />
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { ref, shallowRef } from 'vue';
  import ChatSider from './ChatSider.vue';
  import ChatMessage from './ChatMessage.vue';

  const componentsRef = shallowRef({
    ChatMessage: ChatMessage,
  });

  const switchComp = ref('ChatMessage');

  function handleSwitchComp(key: string) {
    switchComp.value = key;
  }
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
