import type { Ref } from 'vue';
import { computed, ref, onMounted, watchEffect } from 'vue';
import { getAll as getAllOrganizationUnits } from '/@/api/identity/organization-units';
import { OrganizationUnit } from '/@/api/identity/organization-units/model';
import { getOrganizationUnits } from '/@/api/identity/users';
import { listToTree } from '/@/utils/helper/treeHelper';

interface UseOrganizationUnit {
    userRef: Ref<Recordable | undefined>;
}

export function useOrganizationUnit({ userRef }: UseOrganizationUnit) {
    const organizationUnits = ref<OrganizationUnit[]>([]);
    const hasInOrganizationUnitKeys = ref<string[]>([]);

    const getOrganizationUnitsTree = computed(() => {
        const ouTree = listToTree(organizationUnits.value, {
            id: 'id',
            pid: 'parentId',
            children: 'children',
        });
        return ouTree;
    });

    async function fetchOrganizationUnits() {
        const { items } = await getAllOrganizationUnits();
        organizationUnits.value = items;
    }

    async function fetchUserOrganizationUnits(userId: string) {
        const { items } = await getOrganizationUnits(userId);
        hasInOrganizationUnitKeys.value = items.map((item) => item.code);
    }

    onMounted(fetchOrganizationUnits);
    watchEffect(() => {
        userRef.value?.id && fetchUserOrganizationUnits(userRef.value.id);
    });

    return {
        getOrganizationUnitsTree,
        hasInOrganizationUnitKeys,
    };
}