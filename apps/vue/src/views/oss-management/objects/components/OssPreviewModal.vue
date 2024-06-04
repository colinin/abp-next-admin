<template>
  <BasicModal
    @register="registerModal"
    :title="L('Objects:Preview')"
    :width="966"
    :min-height="466"
  >
    <ImagePreview :image-list="previewImages" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ImagePreview } from '/@/components/Preview';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { OssObject } from '/@/api/oss-management/model/ossModel';
  import { generateOssUrl } from '/@/api/oss-management/objects';
  import { useUserStoreWithOut } from '/@/store/modules/user';

  const { L } = useLocalization('AbpOssManagement');
  const bucket = ref('');
  const objects = ref<OssObject[]>([]);
  const [registerModal] = useModalInner((data) => {
    bucket.value = data.bucket;
    objects.value = data.objects;
  });
  const previewImages = computed(() => {
    const userStore = useUserStoreWithOut();
    return objects.value.map((obj) => {
      return (
        generateOssUrl(unref(bucket), obj.path, obj.name) + '?access_token=' + userStore.getToken
      );
    });
  });
</script>
