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

<script lang="ts">
  import { defineComponent, ref, unref, watch } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ImagePreview } from '/@/components/Preview';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { OssObject } from '/@/api/oss-management/model/ossModel';
  import { generateOssUrl } from '/@/api/oss-management/oss';
  import { useUserStoreWithOut } from '/@/store/modules/user';

  export default defineComponent({
    name: 'OssPreviewModal',
    components: { BasicModal, ImagePreview },
    setup() {
      const { L } = useLocalization('AbpOssManagement');
      const bucket = ref('');
      const objects = ref<OssObject[]>([]);
      const previewImages = ref<any[]>([]);
      const [registerModal] = useModalInner((data) => {
        bucket.value = data.bucket;
        objects.value = data.objects;
      });
      const userStore = useUserStoreWithOut();

      watch(
        () => unref(objects),
        (objs) => {
          previewImages.value = objs.map(x => {
            return generateOssUrl(unref(bucket), x.path, x.name) + "?access_token=" + userStore.getToken;
          });
        },
      );

      return {
        L,
        previewImages,
        registerModal,
      };
    },
  });
</script>
