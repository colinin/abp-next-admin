import { createVNode } from 'vue';
import { Button } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';
import { useModal } from '/@/components/Modal';
import { useRandomPassword } from '/@/hooks/security/useRandomPassword';

export function usePassword() {
  const { L } = useLocalization('AbpIdentity');
  const formSchemas: FormSchema[] = [
    {
      field: 'password',
      component: 'InputSearch',
      label: L('Password'),
      colProps: { span: 24 },
      required: true,
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
