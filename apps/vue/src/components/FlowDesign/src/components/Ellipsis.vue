<template>
  <div
    :class="{ line: row === 1, lines: row > 1 }"
    :title="hoverTip ? content : ''"
    :style="{ '--row': row }"
  >
    <slot name="pre"></slot>
    {{ content }}
  </div>
</template>

<script setup lang="ts">
  defineProps({
    row: {
      type: Number,
      default: 1,
    },
    hoverTip: {
      type: Boolean,
      default: false,
    },
    content: {
      type: String,
      default: '',
    },
  });
</script>

<style lang="less" scoped>
  .line {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .lines {
    display: -webkit-box;
    word-break: break-all;
    overflow: hidden;
    text-overflow: ellipsis;
    -webkit-line-clamp: var(--row);
    -webkit-box-orient: vertical;
  }
</style>
