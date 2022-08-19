import type { Ref } from 'vue';

import { createVNode, unref } from 'vue';
import { Button } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema, FormActionType } from '/@/components/Form';
import { useModal } from '/@/components/Modal';
import { useValidation } from '/@/hooks/abp/useValidation';
import { useRandomPassword } from '/@/hooks/security/useRandomPassword';
import { usePasswordValidator } from '/@/hooks/security/usePasswordValidator';

export function usePassword(formElRef: Ref<Nullable<FormActionType>>) {
  const { L } = useLocalization('AbpIdentity');
  const { ruleCreator } = useValidation();
  const { validate } = usePasswordValidator();
  const formSchemas: FormSchema[] = [
    {
      field: 'password',
      component: 'InputSearch',
      label: L('Password'),
      colProps: { span: 24 },
      required: true,
      rules: [
        ...ruleCreator.fieldRequired({
          name: 'UserName',
          resourceName: 'AbpIdentity',
          prefix: 'DisplayName',
        }),
        ...ruleCreator.defineValidator({
          trigger: 'change',
          validator: (_, value: any) => {
            if (!value) {
              return Promise.resolve();
            }
            return validate(value)
              .then(() => Promise.resolve())
              .catch((error) => Promise.reject(error));
          },
        }),
      ],
      componentProps: ({ formModel }) => {
        return {
          allowClear: false,
          enterButton: createVNode(
            Button,
            {
              type: 'primary',
            },
            () => 'Random',
          ),
          onSearch: () => {
            const formEl = unref(formElRef);
            formEl?.clearValidate();
            formModel.password = generatePassword();
          },
        };
      },
    },
  ];
  const [registerPasswordModal, { openModal }] = useModal();
  const { generatePassword } = useRandomPassword();

  function showPasswordModal(userId: string) {
    openModal(true, userId, true);
  }

  return { formSchemas, registerPasswordModal, showPasswordModal, generatePassword };
}
