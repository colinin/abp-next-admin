<script lang="ts" setup>
import type { Component } from 'vue';

import { onMounted, ref } from 'vue';

import { AbpAbout } from '@abp/ui';

import { useSystemInfoApi } from '#/api/core/useSystemInfoApi';

defineOptions({ name: 'AbpAbout' });

interface AboutComponent {
  content: Component | string;
  title: string;
}

interface AboutItem {
  items?: AboutComponent[];
  title: string;
}

const aboutItems = ref<AboutItem[]>([]);
const { getSystemInfoApi } = useSystemInfoApi();

async function onInit() {
  const items: AboutItem[] = [];
  const res = await getSystemInfoApi();
  res.components.forEach((component) => {
    const components: AboutComponent[] = [];
    component.keys.forEach((key) => {
      components.push({
        title: key.displayName,
        content: component.details[key.name],
      });
    });
    items.push({
      title: component.name,
      items: components,
    });
  });
  aboutItems.value = items;
}

onMounted(onInit);
</script>

<template>
  <AbpAbout :custom-items="aboutItems" />
</template>
