<script lang="ts" setup>
import { onMounted, ref } from 'vue';

import { VbenAvatar } from '@vben-core/shadcn-ui';

import { NotificationReadState, useMyNotifilersApi } from '@abp/notifications';

interface Props {
  avatar?: string;
  notifierCount?: number;
  text?: string;
}

defineOptions({
  name: 'WorkbenchHeader',
});

withDefaults(defineProps<Props>(), {
  avatar: '',
  text: '',
  notifierCount: 0,
});
const unReadNotifilerCount = ref(0);
const { getMyNotifilersApi } = useMyNotifilersApi();

async function onInit() {
  const { totalCount } = await getMyNotifilersApi({
    maxResultCount: 1,
    readState: NotificationReadState.UnRead,
  });
  unReadNotifilerCount.value = totalCount;
}

onMounted(onInit);
</script>
<template>
  <div class="card-box p-4 py-6 lg:flex">
    <VbenAvatar :alt="text" :src="avatar" class="size-20" />
    <div
      v-if="$slots.title || $slots.description"
      class="flex flex-col justify-center md:ml-6 md:mt-0"
    >
      <h1 v-if="$slots.title" class="text-md font-semibold md:text-xl">
        <slot name="title"></slot>
      </h1>
      <span v-if="$slots.description" class="text-foreground/80 mt-1">
        <slot name="description"></slot>
      </span>
    </div>
    <div class="mt-4 flex flex-1 justify-end md:mt-0">
      <div class="flex flex-col justify-center text-right">
        <span class="text-foreground/80">
          {{ $t('workbench.header.notifier.title') }}
        </span>
        <a class="text-2xl">{{
          $t('workbench.header.notifier.count', [notifierCount])
        }}</a>
      </div>
    </div>
  </div>
</template>
