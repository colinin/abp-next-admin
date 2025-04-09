<script setup lang="ts">
import type { PermissionDefinitionDto } from '../../types/definitions';
import type { PermissionGroupDefinitionDto } from '../../types/groups';

import { computed, onMounted, ref } from 'vue';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { Checkbox, FormItemRest, TreeSelect } from 'ant-design-vue';

import { usePermissionDefinitionsApi } from '../../api/usePermissionDefinitionsApi';
import { usePermissionGroupDefinitionsApi } from '../../api/usePermissionGroupDefinitionsApi';

interface TreeData {
  checkable?: boolean;
  children: TreeData[];
  displayName: string;
  groupName: string;
  name: string;
}

const emits = defineEmits(['blur', 'change']);

const { getListApi: getPermissionsApi } = usePermissionDefinitionsApi();
const { getListApi: getPermissionGroupsApi } =
  usePermissionGroupDefinitionsApi();
const { deserialize } = useLocalizationSerializer();
const { Lr } = useLocalization();

const permissions = ref<PermissionDefinitionDto[]>([]);
const permissionGroups = ref<PermissionGroupDefinitionDto[]>([]);
const modelValue = defineModel<{
  permissions: string[];
  requiresAll: boolean;
}>({
  default: {
    permissions: [],
    requiresAll: false,
  },
});
const getPermissionOptions = computed(() => {
  const permissionOptions: TreeData[] = [];
  permissionGroups.value.forEach((group) => {
    permissionOptions.push({
      checkable: false,
      displayName: group.displayName,
      groupName: group.name,
      name: group.name,
      children: listToTree(
        permissions.value.filter((x) => x.groupName === group.name),
        {
          id: 'name',
          pid: 'parentName',
        },
      ),
    });
  });
  return permissionOptions;
});
const getRequiredPermissions = computed(() => {
  const permissionNames = modelValue.value?.permissions ?? [];
  return permissions.value
    .filter((permission) => permissionNames.includes(permission.name))
    .map((permission) => {
      return {
        label: permission.displayName,
        value: permission.name,
      };
    });
});

function onChange() {
  emits('change', modelValue.value);
}

function onPermissionsChange(value: any[]) {
  modelValue.value.permissions = value.map((item) => item.value);
  emits('change', modelValue.value);
}

async function onInit() {
  const [permissionGroupRes, permissionsRes] = await Promise.all([
    getPermissionGroupsApi(),
    getPermissionsApi(),
  ]);
  permissionGroups.value = permissionGroupRes.items.map((item) => {
    const displayName = deserialize(item.displayName);
    return {
      ...item,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
  permissions.value = permissionsRes.items.map((item) => {
    const displayName = deserialize(item.displayName);
    return {
      ...item,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
}

onMounted(onInit);
</script>

<template>
  <div class="flex w-full flex-col gap-5">
    <FormItemRest>
      <Checkbox
        v-model:checked="modelValue.requiresAll"
        @blur="emits('blur')"
        @change="onChange"
      >
        {{
          $t('component.simple_state_checking.requirePermissions.requiresAll')
        }}
      </Checkbox>
      <TreeSelect
        :tree-data="getPermissionOptions"
        allow-clear
        tree-checkable
        tree-check-strictly
        :field-names="{ label: 'displayName', value: 'name' }"
        :value="getRequiredPermissions"
        @blur="emits('blur')"
        @change="onPermissionsChange"
      />
    </FormItemRest>
  </div>
</template>

<style scoped></style>
