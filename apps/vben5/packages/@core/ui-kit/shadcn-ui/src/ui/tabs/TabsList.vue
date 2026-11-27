<script setup lang="ts">
import type { TabsListProps } from 'reka-ui';

import type { HTMLAttributes } from 'vue';

import { computed } from 'vue';

import { cn } from '@vben-core/shared/utils';

import { reactiveOmit } from '@vueuse/core';
import { TabsList } from 'reka-ui';

const props = withDefaults(
  defineProps<
    TabsListProps & {
      class?: HTMLAttributes['class'];
      variant?: 'default' | 'line';
    }
  >(),
  {
    variant: 'default',
  },
);

const delegatedProps = reactiveOmit(props, 'class', 'variant');

const variantClass = computed(() =>
  props.variant === 'line'
    ? 'gap-1 rounded-none bg-transparent'
    : 'rounded-md bg-muted',
);
</script>

<template>
  <TabsList
    data-slot="tabs-list"
    :data-variant="variant"
    v-bind="delegatedProps"
    :class="
      cn(
        'group/tabs-list text-muted-foreground inline-flex h-9 w-fit items-center justify-center p-1',
        variantClass,
        props.class,
      )
    "
  >
    <slot></slot>
  </TabsList>
</template>
