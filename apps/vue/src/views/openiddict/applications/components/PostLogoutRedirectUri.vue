<template>
  <UriEditForm
    :title="L('DisplayName:PostLogoutRedirectUris')"
    :schemas="schemas"
    :columns="columns"
    :data-source="dataSource"
    rowKey="uri"
    @create="handleAddNew"
    @delete="handleDelete"
  />
</template>

<script lang="ts" setup>
  import { computed } from 'vue';
  import { FormSchema } from '/@/components/Form';
  import { BasicColumn } from '/@/components/Table';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import UriEditForm from '../../components/Uris/UriForm.vue';

  const emits = defineEmits(['create-logout-uri', 'delete-logout-uri']);
  const props = defineProps({
    uris: {
      type: Array as PropType<String[]>,
    },
  });
  const { L } = useLocalization(['AbpOpenIddict']);
  const dataSource = computed(() => {
    if (!props.uris) return [];
    return props.uris.map((uri) => {
      return {
        uri: uri,
      };
    });
  });
  const schemas: FormSchema[] = [
    {
      field: 'uri',
      component: 'Input',
      label: 'Uri',
      colProps: { span: 24 },
    },
  ];
  const columns: BasicColumn[] = [
    {
      dataIndex: 'uri',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];

  function handleAddNew(record) {
    emits('create-logout-uri', record.uri);
  }

  function handleDelete(record) {
    emits('delete-logout-uri', record.uri);
  }
</script>
