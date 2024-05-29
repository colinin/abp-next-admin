<template>
  <div>
    <FormItem
      :name="['stateChecker', 'model', 'featureNames']"
      :label="t('component.simple_state_checking.requirePermissions.requiresAll')"
      :extra="t('component.simple_state_checking.requirePermissions.requiresAllDesc')"
    >
      <Checkbox :checked="state.stateChecker.model.requiresAll" @change="handleChangeRequiresAll">
        {{ t('component.simple_state_checking.requirePermissions.requiresAll') }}
      </Checkbox>
    </FormItem>
    <FormItem
      :name="['stateChecker', 'model', 'permissions']"
      required
      :label="t('component.simple_state_checking.requirePermissions.permissions')"
    >
      <TreeSelect
        allow-clear
        tree-checkable
        tree-check-strictly
        :tree-data="state.treeData"
        :field-names="{
          label: 'displayName',
          value: 'name',
          children: 'children',
        }"
        :value="getRequiredPermissions"
        @change="handleChangePermissions"
      />
    </FormItem>
  </div>
</template>

<script setup lang="ts">
  import type { CheckboxChangeEvent } from 'ant-design-vue/lib/checkbox/interface';
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, onMounted, watchEffect } from 'vue';
  import { Checkbox, Form, TreeSelect } from 'ant-design-vue';
  import { GetListAsyncByInput } from '/@/api/permission-management/definitions/permissions';
  import { PermissionDefinitionDto } from '/@/api/permission-management/definitions/permissions/model';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { groupBy } from '/@/utils/array';

  const FormItem = Form.Item;
  interface TreeData {
    name: string;
    groupName: string;
    displayName: string;
    children: TreeData[];
  }
  interface StateCheckerModel {
    requiresAll: boolean;
    permissions: string[];
  }
  interface StateChecker {
    name: string;
    model: StateCheckerModel;
  }
  interface State {
    permissions: PermissionDefinitionDto[];
    treeData: TreeData[];
    stateChecker: StateChecker;
  }

  const emits = defineEmits(['change', 'update:value']);
  const props = defineProps({
    value: {
      type: Object as PropType<StateChecker>,
      required: true,
    },
  });

  const { t } = useI18n();
  const { Lr } = useLocalization();
  const { deserialize } = useLocalizationSerializer();
  const state = reactive<State>({
    treeData: [],
    permissions: [],
    stateChecker: {
      name: 'P',
      model: {
        requiresAll: true,
        permissions: [],
      },
    },
  });
  const getRequiredPermissions = computed(() => {
    return state.permissions
      .filter((permission) => state.stateChecker.model.permissions.includes(permission.name))
      .map((permission) => {
        return {
          label: permission.displayName,
          value: permission.name,
        };
      });
  });

  watchEffect(() => {
    state.stateChecker = props.value;
  });
  onMounted(fetchFeatures);

  function fetchFeatures() {
    GetListAsyncByInput({}).then((res) => {
      state.permissions = res.items;
      formatDisplayName(state.permissions);
      const permissionGroup = groupBy(cloneDeep(res.items), 'groupName');
      const permissionGroupTreeData: TreeData[] = [];
      Object.keys(permissionGroup).forEach((gk) => {
        const featureTree = listToTree(permissionGroup[gk], {
          id: 'name',
          pid: 'parentName',
        });
        permissionGroupTreeData.push(...featureTree);
      });
      state.treeData = permissionGroupTreeData;
    });
  }

  function formatDisplayName(list: any[]) {
    if (list && Array.isArray(list)) {
      list.forEach((item) => {
        if (Reflect.has(item, 'displayName')) {
          const info = deserialize(item.displayName);
          item.displayName = Lr(info.resourceName, info.name);
        }
        if (Reflect.has(item, 'children')) {
          formatDisplayName(item.children);
        }
      });
    }
  }

  function handleChangeRequiresAll(e: CheckboxChangeEvent) {
    state.stateChecker.model.requiresAll = e.target.checked;
    emits('change', state.stateChecker);
    emits('update:value', state.stateChecker);
  }

  function handleChangePermissions(value: any[]) {
    state.stateChecker.model.permissions = value.map((val) => val.value);
    emits('change', state.stateChecker);
    emits('update:value', state.stateChecker);
  }
</script>

<style scoped></style>
