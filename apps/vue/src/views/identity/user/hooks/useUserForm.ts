import type { Ref } from 'vue';

import { cloneDeep } from 'lodash-es';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';
import { computed, reactive, ref, unref, onMounted } from 'vue';
import { CreateUser, UpdateUser } from '/@/api/identity/model/userModel';
import { create, getById, getRoleList, update } from '/@/api/identity/user';
import { getAssignableRoles } from '/@/api/identity/user';
import { Role } from '/@/api/identity/model/roleModel';

interface UseUserFormContext {
  userRef: Ref<Recordable>;
  formElRef: Ref<any>;
}

export function useUserForm({ userRef, formElRef }: UseUserFormContext) {
  const { L } = useLocalization('AbpIdentity');
  const { ruleCreator } = useValidation();
  const assignableRoles = ref<Role[]>([]);

  onMounted(() => {
    getAssignableRoles().then((res) => {
      assignableRoles.value = res.items;
    });
  });

  const formRules = reactive({
    userName: ruleCreator.fieldRequired({
      name: 'UserName',
      resourceName: 'AbpIdentity',
      trigger: 'blur',
    }),
    password: ruleCreator.fieldRequired({
      name: 'Password',
      resourceName: 'AbpIdentity',
      trigger: 'blur',
    }),
    email: ruleCreator.fieldRequired({
      name: 'Email',
      prefix: 'DisplayName',
      resourceName: 'AbpIdentity',
      trigger: 'blur',
    }),
  });

  const roleDataSource = computed(() => {
    const roles = unref(assignableRoles);
    return roles.map((role) => {
      return {
        key: role.name,
        title: role.name,
        description: role.name,
        disabled: false,
      };
    });
  });

  const userTitle = computed(() => {
    if (unref(userRef)?.id) {
      return L('Edit');
    }
    return L('NewUser');
  });

  async function getUser(id) {
    userRef.value = {
      roleNames: [],
    };
    if (id) {
      const user = await getById(id);
      const userRoles = await getRoleList(id);
      const roleNames = userRoles.items.map((role) => role.name);
      user.roleNames = roleNames;
      userRef.value = user;
    }
  }

  function handleRoleChange(_, direction, moveRoles: string[]) {
    const user = unref(userRef);
    user.roleNames = user.roleNames ?? [];
    switch (direction) {
      case 'left':
        moveRoles.forEach((key) => {
          const index = user.roleNames.findIndex((role) => role === key);
          if (index >= 0) {
            user.roleNames.splice(index, 1);
          }
        });
        break;
      case 'right':
        moveRoles.forEach((key) => {
          user.roleNames.push(key);
        });
        break;
    }
  }

  function handleSubmit() {
    return new Promise((resolve, reject) => {
      const formEl = unref(formElRef);
      formEl
        .validate()
        .then(() => {
          const user = unref(userRef);
          const api = user.id
            ? update(user.id, cloneDeep(user) as UpdateUser)
            : create(cloneDeep(user) as CreateUser);
          api
            .then((res) => {
              userRef.value = {};
              resolve(res);
            })
            .catch((error) => {
              reject(error);
            });
        })
        .catch((error) => {
          reject(error);
        });
    });
  }

  return {
    userTitle,
    formRules,
    roleDataSource,
    getUser,
    handleRoleChange,
    handleSubmit,
  };
}
